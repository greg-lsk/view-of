using View;
using ViewTests.Stubs;


namespace ViewTests;

public class SyncEnvironmentTests
{
    [Fact]
    public async Task UnmanagedTypes_Uncorrupted_When_CapturedScope_SameAs_AccessedScope()
    {
        var unmanagedStruct = new UnmanagedStruct(5, 8);
        var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

        StubExecution.SyncExecution();

        var retrievedStruct = view.Read();
        var result = Equals(retrievedStruct, unmanagedStruct);

        Assert.True(result);
    }

    [Fact]
    public async Task ManagedTypes_Uncorrupted_When_CapturedScope_SameAs_AccessedScope()
    {
        var managedStruct = new ManagedStruct(5, new StubClass(8));
        var view = View<ManagedStruct>.Of(ref managedStruct);

        StubExecution.SyncExecution();

        var retrievedStruct = view.Read();
        var result = Equals(retrievedStruct, managedStruct);

        Assert.True(result);
    }

    [Fact]
    public async Task UnmanagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        var escapedView = StubExecution.SyncExecutionUnmanagedEscape(5, 8);

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new UnmanagedStruct(5, 8));

        Assert.False(result);
    }

    [Fact]
    public async Task ManagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        var escapedView = StubExecution.SyncExecutionManagedEscape(5, 8);

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new ManagedStruct(5, new StubClass(8)));

        Assert.False(result);
    }

    [Fact]
    public async Task UnmanagedTypes_Uncorrupted_When_PassedTo_And_HandledIn_SyncMethods()
    {
        var number = 5;
        var unmanagedStruct = new UnmanagedStruct(number, 8);
        var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

        var retrievedNumber = StubExecution.SyncExecution(view);

        Assert.Equal(retrievedNumber, number);
    }

    [Fact]
    public async Task ManagedTypes_Uncorrupted_When_PassedTo_And_HandledIn_SyncMethods()
    {
        var stubClass = new StubClass(8);
        var managedStruct = new ManagedStruct(5, stubClass);
        var view = View<ManagedStruct>.Of(ref managedStruct);

        var retrievedClass = StubExecution.SyncExecution(view);

        var result = ReferenceEquals(stubClass, retrievedClass);

        Assert.True(result);
    }
}