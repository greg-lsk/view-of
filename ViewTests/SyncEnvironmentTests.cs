using ViewTests.Stubs;


namespace ViewTests;

public class SyncEnvironmentTests
{
    [Fact]
    public async Task UnmanagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        (int a, int b) = (5, 8);

        var escapedView = StubExecution.SyncExecutionUnmanagedEscape(a, b);

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new UnmanagedStruct(a, b));

        Assert.False(result);
    }

    [Fact]
    public async Task ManagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        (int a, int b) = (5, 8);

        var escapedView = StubExecution.SyncExecutionManagedEscape(a, b);

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new ManagedStruct(a, new StubClass(b)));

        Assert.False(result);
    }

    [Fact]
    public async Task RefTypes_CorruptedData_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        int a = 5;

        var escapedView = StubExecution.SyncExecutionRefTypeEscape(a);

        var retrievedClass = escapedView.Read();
        Assert.NotEqual(a, retrievedClass.Number);
    }
}