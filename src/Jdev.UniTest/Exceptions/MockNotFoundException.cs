namespace Jdev.UniTest.Exceptions;

public class MockNotFoundException : Exception
{
    private readonly Type _mockType;

    public MockNotFoundException(Type mockType)
    {
        _mockType = mockType;
    }

    public override string Message => $"Mock of type {_mockType} was not found in dependencies";
}