namespace ExampleTest;

using Jdev.UniTest;
using Jdev.UniTest.Extensions;
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
        MockOf<IFoo>().Accepts<IFoo, Guid, int>(x => x.DoFoo, new Guid());
    }
}