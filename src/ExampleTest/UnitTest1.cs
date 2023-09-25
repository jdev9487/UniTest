namespace ExampleTest;

using Jdev.UniTest;
using Moq;

public class Tests : TestOf<ExampleClass>
{
    protected override void SetUp()
    {
        MockOf<IBar>()
            .Setup(x => x.DoBar())
            .Returns(1);
    }

    protected override void TearDown()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test1()
    {
        Cut.DoStuff();
        MockOf<IFoo>().Verify(x => x.DoFoo(), Times.Once);
    }
}