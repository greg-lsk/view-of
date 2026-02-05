using System.Runtime.CompilerServices;


namespace View;

public unsafe readonly struct View<T> where T : struct
{
    private readonly void* _referedData;

    private View(ref T referedData) => _referedData = Unsafe.AsPointer(ref referedData);
    public static View<T> Of(ref T referedData) => new(ref referedData);

    public readonly ref T Peek() => ref Unsafe.AsRef<T>(_referedData);
    public readonly ref readonly T Peek() => ref Unsafe.AsRef<T>(_referedData);
    public readonly T Read() => Unsafe.Read<T>(_referedData);
}