namespace ExampleAssembly;

public class ExampleClass : IExampleClass
{
    private readonly IFoo _foo;
    private readonly IBar _bar;
    private readonly IPython _python;

    public ExampleClass(IFoo foo, IBar bar, IPython python)
    {
        _foo = foo;
        _bar = bar;
        _python = python;
    }

    public void DoStuff()
    {
        var jokes = _python.GetJokes();
        var count = _bar.DoBar(jokes.First().ToUpper());
        _foo.DoFoo(new Guid(), count);
    }
}