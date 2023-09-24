namespace Jdev.UniTest.Exceptions;

public class AmbiguousConstructorException : Exception
{
    private readonly Type _cutType;
    
    public AmbiguousConstructorException(Type cutType)
    {
        _cutType = cutType;
    }

    public override string Message => $"{_cutType} has more than one constructor";
}