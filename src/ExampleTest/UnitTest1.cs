namespace ExampleTest;

using ExampleClass;
using Jdev.UniTest;

public class Tests : TestOf<ExampleClass>
{
    protected override void SetUp()
    {
        MockOf<IPython>()
            .Setup(x => x.GetJokes())
            .Returns(new[] { "hello" });
    }

    [Test]
    public void Test1()
    {
        VerifyThat<IFoo>(x => x.DoFoo)
            .IsGiven(ResultFrom<IBar>(x => x.DoBar))
            .During(() => Cut.DoStuff());
    }
}