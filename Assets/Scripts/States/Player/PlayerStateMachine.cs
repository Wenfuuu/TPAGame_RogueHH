using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance;

    public Camera cam;
    public PathFinding pathfinding;

    private Vector3 lastPosition;
    public LayerMask mask;
    public float moveSpeed = 5f;

    private List<Node> path;
    private List<Node> highlightPath;
    private int currentTargetIndex = 0;
    private bool isClicked = false;
    private bool hitEnemy = false;
    private bool executing = false;
    private GameManager manager;

    public Animator _animator;

    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    private bool _isMoving = false;
    private bool _inBattle = false;
    private bool _isNearEnemy = false;
    private bool _isUsingSkill = false;

    public GameObject sword;
    public TrailRenderer trail;
    public GameObject SwordEffect;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool InBattle { get { return _inBattle; } }// untuk batasin gerak 1 tile
    public bool NearEnemy { get { return _isNearEnemy; } }// untuk cek ready to attack

    public bool IsUsingSkill { get { return _isUsingSkill; } set { _isUsingSkill = value; } }//untuk cek sfx

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _animator = GetComponent<Animator>();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        _currentState.UpdateStates();
        manager = GameManager.Instance;
        if(manager.CheckAggro())
        {
            _inBattle = true;
        }else _inBattle = false;

        //if (_isNearEnemy) Debug.Log("near enemy ready to attack");
        CheckReadyToAttack();

        if (Input.GetKeyDown(KeyCode.Space))// skip turn
        {
            Debug.Log("skipping turn");
            GetComponent<PlayerSkills>().ReduceCooldown();
            GetComponent<PlayerSkills>().ApplyAllBuff();
            GameManager.isEnemyTurn = true;
        }

        if (Input.mousePosition != null && !_isMoving)// highlight path
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = GetMapPosition();

            Node dest = pathfinding.grid.NodeFromWorldPoint(targetPos);
            //Debug.Log(dest.isWalkable);
            if (dest.isWalkable == true)
            {
                highlightPath = pathfinding.FindPath(startPos, targetPos);

                if (highlightPath != null && !isClicked)
                {
                    ChangeColor();
                }
            }
        }
        else if (Input.mousePosition != null && _isMoving)// highlight 1 tile
        {
            ChangeOneColor();
        }

        if (Input.GetMouseButtonDown(0) && _isMoving)
        {
            currentTargetIndex = path.Count;
        }

        if (GameManager.isEnemyTurn) return;
        if (Input.GetMouseButtonDown(0) && !_isMoving)
        {
            Vector3 targetPos = GetMapPosition();

            if (_isNearEnemy)// kalo lagi deket musuh cek tile yang diklik ada musuh atau tidak
            {
                //hitEnemy = false;
                if (executing) return;
                CheckHitEnemy(targetPos);
                if (hitEnemy)
                {
                    hitEnemy = false;
                    return;
                }
            }

            Node dest = pathfinding.grid.NodeFromWorldPoint(targetPos);
            targetPos.y = 1;
            Vector3 currPos = new Vector3();
            currPos.x = Mathf.RoundToInt(transform.position.x);
            currPos.y = Mathf.RoundToInt(transform.position.y);
            currPos.z = Mathf.RoundToInt(transform.position.z);
            targetPos.x = Mathf.RoundToInt(targetPos.x);
            targetPos.y = Mathf.RoundToInt(targetPos.y);
            targetPos.z = Mathf.RoundToInt(targetPos.z);

            if (dest.isWalkable && (currPos != targetPos))
            {
                Vector3 prevPos = transform.position;
                PlayerMoveCommand moveCommand = new PlayerMoveCommand(this, targetPos);
                manager.QueueCommand(moveCommand);

                // kalo ada enemy aggro & player brsn gerak
                if (manager.CheckAggro() && (transform.position != prevPos))
                {
                    GameManager.isEnemyTurn = true;
                    //GetComponent<PlayerSkills>().ReduceCooldown();
                    //Debug.Log("ada enemy aggro");
                }
            }
        }
    }

    void CheckHitEnemy(Vector3 targetPos)
    {
        //cek targetPos yang diklik itu ada enemy atau tidak
        targetPos.y = 1;
        targetPos.x = Mathf.RoundToInt(targetPos.x);
        targetPos.y = Mathf.RoundToInt(targetPos.y);
        targetPos.z = Mathf.RoundToInt(targetPos.z);

        float distance = Vector3.Distance(transform.position, targetPos);
        Debug.Log("distance to hit: " + distance);
        if (distance > 2.2f) return;

        HashSet<EnemyStateMachine> enemies = manager.getAggroEnemies();
        foreach (EnemyStateMachine enemy in enemies)
        {
            Vector3 enemyPos;
            enemyPos.x = Mathf.RoundToInt(enemy.transform.position.x);
            enemyPos.y = Mathf.RoundToInt(enemy.transform.position.y);
            enemyPos.z = Mathf.RoundToInt(enemy.transform.position.z);
            Debug.Log("hitting in " + targetPos);
            Debug.Log("enemy in " + enemyPos);
            if (targetPos == enemyPos)
            {
                //Debug.Log("hitting enemy");// works tinggal tambahin animasi atk buat player
                HandleRotation(targetPos);
                StartCoroutine(HandleAttack(enemy));
                return;
            }
        }
    }

    IEnumerator HandleAttack(EnemyStateMachine enemy)
    {
        executing = true;
        GetComponent<PlayerSkills>().ReduceCooldown();
        GetComponent<PlayerSkills>().ApplyAllBuff();
        GetComponent<PlayerSkills>().CheckActiveSkills();
        sword.SetActive(true);
        yield return StartCoroutine(HitEnemy(enemy));
        sword.SetActive(false);
        GetComponent<PlayerSkills>().CheckActiveSkills();
        executing = false;

        GameManager.isEnemyTurn = true;
    }

    IEnumerator HitEnemy(EnemyStateMachine enemy)
    {
        int attackType = Random.Range(1, 4);
        string attackAnimation = $"IsAttacking{attackType}";

        //Debug.Log("start hitting enemy");
        _animator.SetBool(attackAnimation, true);
        yield return new WaitForSeconds(0.2f);
        //enemy._animator.SetBool("IsHit", true);
        //calculate damage
        int damage = CalculateDamage(enemy);
        //crit
        bool crit = false;
        float critDamage = 100f;
        int randomVal = Random.Range(1, 101);
        if (randomVal <= gameObject.GetComponent<PlayerDamageable>().playerStats.CritRate) crit = true;
        if (crit)
        {
            critDamage = gameObject.GetComponent<PlayerDamageable>().playerStats.CritDamage;
            damage += Mathf.RoundToInt(damage * (critDamage / 100f));
            CameraShake.Instance.Shake();
            SFXManager.Instance.PlayRandomSFX(Sounds.Instance.CriticalSFX, enemy.transform, 1f);
        }
        //kasi damage
        enemy.GetComponent<EnemyDamageable>().DecreaseHealth(damage);
        //kasi sfx
        if (_isUsingSkill)
        {
            SFXManager.Instance.PlaySFX(Sounds.Instance.BashSFX, enemy.transform, 1f);
        }
        else SFXManager.Instance.PlayRandomSFX(Sounds.Instance.SwordSFX, enemy.transform, 1f);
        //kasi damage popup
        DamagePopUpGenerator.Instance.CreatePopUp(damage, crit, enemy.transform.position);
        if (enemy.GetComponent<EnemyDamageable>().enemyStats.CurrentHP > 0)
        {
            enemy._animator.SetBool("IsHit", true);
            SFXManager.Instance.PlayRandomSFX(Sounds.Instance.GruntSFX, enemy.transform, 1f);
            yield return new WaitForSeconds(0.3f);
            enemy._animator.SetBool("IsHit", false);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            SFXManager.Instance.PlayRandomSFX(Sounds.Instance.DeathSFX, enemy.transform, 1f);
            //atur UI
            manager.UpdateEnemyCount.RaiseEvent(manager.GetEnemyCount() - 1);

            enemy._animator.SetBool("IsDead", true);
            enemy.IsDead = true;
            HandleEnemyDrop(enemy);
            yield return new WaitForSeconds(1.2f);
            Node temp = Grid.Instance.NodeFromWorldPoint(enemy.transform.position);
            temp.isWalkable = true;
            HandleEnemyDeath(enemy);
        }
        //Debug.Log("finished hitting enemy");
        _animator.SetBool(attackAnimation, false);
        hitEnemy = true;
    }

    int CalculateDamage(EnemyStateMachine enemy)
    {
        float defenseScalingFactor = Random.Range(50, 101);
        //Debug.Log("def scaling: " + defenseScalingFactor);
        EnemyDamageable damageable = enemy.GetComponent<EnemyDamageable>();
        float defense = damageable.enemyStats.Defense;
        //Debug.Log("defense: " + defense);
        float defenseFactor = 1f - (defense / (defense + defenseScalingFactor));
        //Debug.Log("def factor: " + defenseFactor);
        int atk = gameObject.GetComponent<PlayerDamageable>().playerStats.Attack;
        return Mathf.RoundToInt(atk * defenseFactor);
    }

    void HandleEnemyDrop(EnemyStateMachine enemy)
    {
        int zhenDrop = enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.ZhenDrop;
        int expDrop = enemy.gameObject.GetComponent<EnemyDamageable>().enemyStats.ExpDrop;

        gameObject.GetComponent<PlayerEXP>().IncreaseEXP(expDrop);
        gameObject.GetComponent<PlayerZhen>().IncreaseZhen(zhenDrop);
    }

    void HandleEnemyDeath(EnemyStateMachine enemy)
    {
        manager.EnemyDeath(enemy);
    }

    void Hit()
    {
        Debug.Log("test hit");
    }

    void CheckReadyToAttack()
    {
        if (_isMoving) return;

        _isNearEnemy = false;
        Vector3 currPos = transform.position;

        HashSet<EnemyStateMachine> enemies = manager.getAggroEnemies();
        foreach(EnemyStateMachine enemy in enemies)
        {
            float distance = Vector3.Distance(currPos, enemy.transform.position);

            if (distance <= 2.2f)
            {
                _isNearEnemy = true;
            }
            if(_isNearEnemy) break;
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        Node dest = pathfinding.grid.NodeFromWorldPoint(targetPosition);
        if (dest.isWalkable)
        {
            path = pathfinding.FindPath(transform.position, targetPosition);

            if (path != null && path.Count > 0)
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    void HandleRotation(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = targetRotation;
        }
    }

    IEnumerator FollowPath()
    {
        _isMoving = true;
        currentTargetIndex = 0;

        while (currentTargetIndex < path.Count)
        {
            //Debug.Log("curr idx: " + currentTargetIndex);
            Node targetNode = path[currentTargetIndex];
            Vector3 targetPosition = targetNode.worldPosition;
            targetPosition.y = 1;

            Vector3 currentPosition = transform.position;

            HandleRotation(targetPosition);

            float travelPercent = 0f;
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, travelPercent);
                yield return null;
            }

            GetComponent<PlayerSkills>().ReduceCooldown();
            GetComponent<PlayerSkills>().ApplyAllBuff();
            SFXManager.Instance.PlayRandomSFX(Sounds.Instance.StepSFX, transform, 1f);
            currentTargetIndex++;

            if (_inBattle) break;
        }

        path.Clear(); 
        _isMoving = false;

        isClicked = false;
    }

    void ChangeOneColor()
    {
        //Debug.Log("Changing one tile");
        Vector3 tilePos = GetMapPosition();
        Node tileTarget = pathfinding.grid.NodeFromWorldPoint(tilePos);

        foreach (Node node in pathfinding.grid.grid)
        {
            if (node == null) continue;

            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length == 0) continue; // Skip if no colliders are found.

            GameObject tile = colliders[0].gameObject;
            tile.GetComponentInChildren<Renderer>().material.color = Color.gray;
        }

        Collider[] collide = Physics.OverlapSphere(tileTarget.worldPosition, 0.1f);
        foreach (Collider col in collide)
        {
            GameObject target = col.gameObject;
            target.GetComponentInChildren<Renderer>().material.color = Color.white;
        }
    }

    void ChangeColor()
    {
        foreach (Node node in pathfinding.grid.grid)
        {
            if (node == null) continue;

            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length == 0) continue;

            GameObject tile = colliders[0].gameObject;
            tile.GetComponentInChildren<Renderer>().material.color = Color.gray;
        }

        foreach (Node node in highlightPath)
        {
            Collider[] colliders = Physics.OverlapSphere(node.worldPosition, 0.1f);
            if (colliders.Length > 0)
            {
                GameObject tile = colliders[0].gameObject;
                if (tile != null)
                {
                    tile.GetComponentInChildren<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    public Vector3 GetMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            lastPosition = hit.point;
            lastPosition.y = 0;
        }

        return lastPosition;
    }
}
