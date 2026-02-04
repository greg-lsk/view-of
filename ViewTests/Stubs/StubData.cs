namespace ViewTests.Stubs;

internal class StubClass(int number)
{
    internal int Number { get; private set; } = number;

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