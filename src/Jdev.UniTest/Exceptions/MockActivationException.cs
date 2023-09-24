namespace Jdev.UniTest.Exceptions;

public class MockActivationException : Exception
{
    private readonly Type _type;
    
    public MockActivationException(Type type)
    {
        _type = type;
    }

    public override string Message => $"Could not create mock of {_type}";
}