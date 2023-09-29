namespace Jdev.UniTest;

public class ExampleClass : IExampleClass
{
    private readonly IFoo _foo;
    private readonly IBar _bar;

    public ExampleClass(IFoo foo, IBar bar)
    {
        _foo = foo;
        _bar = bar;
    }

    public void DoStuff()
    {
        var count = _bar.DoBar("hello");
        _foo.DoFoo(new Guid(), count);
    }

    public void DoOtherStuff()
    {
    }
}