using System.Collections;

public interface ICommand
{
    void Execute();
    IEnumerator ExecuteCoroutine();
}
