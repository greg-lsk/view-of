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
    public async Task UnmanagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        var escapedView = await Task.Run(() =>
        {
            var unmanagedStruct = new UnmanagedStruct(5, 8);
            var view = View<UnmanagedStruct>.Of(ref unmanagedStruct);

            return view;
        });

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new UnmanagedStruct(5, 8));

        Assert.False(result);
    }

    [Fact]
    public async Task ManagedTypes_Corrupted_When_CapturedScope_NarrowerThan_AccessedScope()
    {
        var escapedView = await Task.Run(() =>
        {
            var managedStruct = new ManagedStruct(5, new StubClass(8));
            var view = View<ManagedStruct>.Of(ref managedStruct);

            return view;
        });

        var retrievedStruct = escapedView.Read();
        var result = Equals(retrievedStruct, new ManagedStruct(5, new StubClass(8)));

        Assert.False(result);
    }
}