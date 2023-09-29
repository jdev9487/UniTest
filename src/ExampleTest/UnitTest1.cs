namespace ExampleTest;

using Jdev.UniTest;
using Jdev.UniTest.Extensions;

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
    }

    [Test]
    public void Test1()
    {
        // Cut.DoStuff();
        MockOf<IFoo>().WithMethod(x => x.DoFoo).IsGiven(new Guid());
    }
}