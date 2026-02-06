using View;
using ViewTests.Stubs;


namespace ViewTests;

public class AsyncEnvironmentTests
{
    [Fact]
    public async Task UnmanagedTypes_Uncorrupted_When_CapturedScope_SameAs_AccessedScope()
    {
        var unmanagedStruct = new UnmanagedStruct(5, 8);
        var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

        await Task.Run(() => { Task.Delay(1000); });

        var retrievedStruct = view.Read();
        var result = Equals(retrievedStruct, unmanagedStruct);

        Assert.True(result);
    }

    [Fact]
    public async Task ManagedTypes_Uncorrupted_When_CapturedScope_SameAs_AccessedScope()
    {
        var managedStruct = new ManagedStruct(5, new StubClass(8));
        var view = View<ManagedStruct>.Of(ref managedStruct);

        await Task.Run(() => { Task.Delay(1000); });

        var retrievedStruct = view.Read();
        var result = Equals(retrievedStruct, managedStruct);

        Assert.True(result);
    }

    [Fact]
    public async Task ReferenceTypes_UncorruptedData_When_CapturedScope_SameAs_AccessedScope()
    {
        var refType = new StubClass(8);
        var view = View<StubClass>.Of(ref refType);

        await Task.Run(() => { Task.Delay(1000); });

        var retrievedClass = view.Read();
        var result = StubClass.Equals(retrievedClass, refType);

        Assert.True(result);
    }

    [Fact]
    public async Task ReferenceTypes_UncorruptedReference_When_CapturedScope_SameAs_AccessedScope()
    {
        var refType = new StubClass(8);
        var view = View<StubClass>.Of(ref refType);

        await Task.Run(() => { Task.Delay(1000); });

        var retrievedClass = view.Read();
        var result = ReferenceEquals(retrievedClass, refType);

        Assert.True(result);
    }


    [Fact]
    public async Task UnmanagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        (int a, int b) = (5, 8);

        var escapedView = await Task.Run(() =>
        {
            var unmanagedStruct = new UnmanagedStruct(a, b);
            var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

            return view;
        });

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new UnmanagedStruct(a, b));

        Assert.False(result);
    }

    [Fact]
    public async Task ManagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        (int a, int b) = (5, 8);

        var escapedView = await Task.Run(() =>
        {
            var managedStruct = new ManagedStruct(a, new StubClass(b));
            var view = View<ManagedStruct>.Of(ref managedStruct);

            return view;
        });

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new ManagedStruct(a, new StubClass(b)));

        Assert.False(result);
    }

    [Fact]
    public async Task ReferenceTypes_CorruptedData_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        int a = 5;

        var escapedView = await Task.Run(async () =>
        {
            var refType = new StubClass(a);
            var view = View<StubClass>.Of(ref refType);
            await Task.Delay(1000);

            return view;
        });

        var retrievedClass = escapedView.Read();
        Assert.ThrowsAny<Exception>(() => retrievedClass.Number);
    }


    [Fact]
    public async Task UnmanagedTypes_Uncorrupted_When_PassedTo_And_HandledIn_AsyncMethods()
    {
        var a = 5;
        var unmanagedStruct = new UnmanagedStruct(a, 8);
        var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

        var retrievedNumber = await StubExecution.AsyncExecution(view);

        Assert.Equal(retrievedNumber, a);
    }

    [Fact]
    public async Task ManagedTypes_Uncorrupted_When_PassedTo_And_HandledIn_AsyncMethods()
    {
        var stubClass = new StubClass(8);
        var managedStruct = new ManagedStruct(5, stubClass);
        var view = View<ManagedStruct>.Of(ref managedStruct);

        var retrievedClass = await StubExecution.AsyncExecution(view);

        var result = ReferenceEquals(stubClass, retrievedClass);

        Assert.True(result);
    }

    [Fact]
    public async Task ReferenceTypes_UncorruptedData_When_PassedTo_And_HandledIn_AsyncMethods()
    {
        int a = 5;
        var stubClass = new StubClass(a);
        var view = View<StubClass>.Of(ref stubClass);

        var retrievedNumber = await StubExecution.AsyncExecution(view);

        Assert.Equal(retrievedNumber, stubClass.Number);
    }
}