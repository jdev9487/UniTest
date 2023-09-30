namespace Jdev.UniTest.Models;

public class Verification
{
    public Action Action { get; init; } = default!;

    public void During(Action method)
    {
        method();
        Action();
    }
}