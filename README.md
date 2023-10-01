# UniTest
Fluent unit testing framework

## Quickstart
1. Install nuget package
   
   `dotnet add package Jdev.UniTest --version <VERSION>`

2.  Inherit test class from `TestOf`:
   
    ```cs
    public class ExampleTest : TestOf<ExampleClass>
    {
        protected override void SetUp()
        {
            MockOf<IPython>()
                .SetUp(x => x.GetJokes())
                .Returns(new [] { "hello" });
        }

        [Test]
        public void ShouldPassResultFromBarToFoo()
        {
            VerifyThat<IFoo>(x => x.DoFoo)
                .IsGiven(ResultFrom<IBar>(x => x.DoBar))
                .During(() => Cut.DoStuff());
        }
    }
    ```
    ### Notes on above
    1. `MockOf` returns a mock of an injected dependency; this is created automatically during the construction of `TestOf`.
    2. The test uses `Moq.Verify` to ensure that the result of the calling `DoBar` on the `IBar` dependency was passed to `IFoo`'s `DoFoo` method.
    3. The `During` call actually calls the method on the class under test (`Cut`).