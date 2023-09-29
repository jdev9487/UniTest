namespace ExampleTest;

using Jdev.UniTest;
using Jdev.UniTest.Extensions;

public class Tests : TestOf<ExampleClass>
{
    protected override void SetUp()
    {
    }

    [Test]
    public void Test1()
    {
        Cut.DoStuff();
        // var result = ResultFrom<IBar>(x => x.DoBar);
        // MockOf<IFoo>().Setup(x => x.DoFoo(new Guid(), 1));
        VerifyThat<IFoo>(x => x.DoFoo).IsGiven(ResultFrom<IBar>(y => y.DoBar));
    }
}