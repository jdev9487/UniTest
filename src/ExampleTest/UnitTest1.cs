using Core;
using Moq;

namespace ExampleTest;

public class Tests
{
    [Test]
    public void Test1()
    {
        var asdf = new TestOf<ExampleClass>();
        asdf.Cut.DoStuff();
        asdf.MockOf<IFoo>()
            .Verify(x => x.DoFoo(), Times.Once);
    }
}