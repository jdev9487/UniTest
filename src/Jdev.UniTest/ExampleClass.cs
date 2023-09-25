﻿namespace Jdev.UniTest;

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
        _foo.DoFoo();
        var x = _bar.DoBar();
    }

    public void DoOtherStuff()
    {
        _foo.DoFoo();
        _foo.DoFoo();
        _foo.DoFoo();
        _bar.DoBar();
        _bar.DoBar();
    }
}