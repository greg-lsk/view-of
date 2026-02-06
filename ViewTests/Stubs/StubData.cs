using View;

namespace ViewTests.Stubs;

internal class StubClass(int number)
{
    internal int Number { get; private set; } = number;

    public static bool Equals(StubClass x, StubClass y) => x.Number == y.Number;
}

internal readonly struct UnmanagedStruct(int a, int b)
{
    internal int A { get; } = a;
    internal int B { get; } = b;
    
    public static bool Equals(UnmanagedStruct x, UnmanagedStruct y) => x.A == y.A && x.B == y.B;
}

internal readonly struct ManagedStruct(int a, StubClass stubClass)
{
    internal int A { get; } = a;
    internal StubClass Stub { get; } = stubClass;

    public static bool Equals(ManagedStruct x, ManagedStruct y) => x.A == y.A && x.Stub.Number == y.Stub.Number;
}


internal static class StubExecution
{
    internal static async Task<int> AsyncExecution(View<UnmanagedStruct> view)
    {
        await Task.Delay(1000);
        return view.Read().A;
    }
    internal static async Task<StubClass> AsyncExecution(View<ManagedStruct> view)
    {
        await Task.Delay(1000);
        return view.Read().Stub;
    }
    internal static async Task<int> AsyncExecution(View<StubClass> view)
    {
        await Task.Delay(1000);
        return view.Read().Number;
    }


    internal static void SyncExecution() 
    { }
    internal static int SyncExecution(View<UnmanagedStruct> view)
    {
        return view.Read().A;
    }
    internal static StubClass SyncExecution(View<ManagedStruct> view)
    {
        return view.Read().Stub;
    }

    internal static View<ManagedStruct> SyncExecutionManagedEscape(int numberA, int numberB)
    {
        var managedStruct = new ManagedStruct(numberA, new(numberB));
        return View<ManagedStruct>.Of(ref managedStruct);
    }
    internal static View<UnmanagedStruct> SyncExecutionUnmanagedEscape(int numberA, int numberB)
    {
        var unmanagedStruct = new UnmanagedStruct(numberA, numberB);
        return View<UnmanagedStruct>.Of(ref unmanagedStruct);
    }
    internal static View<StubClass> SyncExecutionRefTypeEscape(int numberA)
    {
        var refType = new StubClass(numberA);
        return View<StubClass>.Of(ref refType);
    }
}