namespace ExampleTest;

using Jdev.UniTest;

public class Tests : TestOf<ExampleClass>
{
    protected override void SetUp()
    {
    }

    [Test]
    public void Test1()
    {
        VerifyThat<IFoo>(x => x.DoFoo)
            .IsGiven(ResultFrom<IBar>(x => x.DoBar))
            .During(() => Cut.DoStuff());
    }
}