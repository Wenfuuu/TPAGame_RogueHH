using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCommand : ICommand
{
    private PlayerStateMachine _player;
    private Vector3 _targetPosition;

    public PlayerMoveCommand(PlayerStateMachine player, Vector3 targetPosition)
    {
        _player = player;
        _targetPosition = targetPosition;
    }

    public void Execute()
    {
        _player.MoveTo(_targetPosition);
    }
}
