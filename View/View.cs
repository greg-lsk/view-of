using System.Runtime.CompilerServices;


namespace View;

/// <summary>
/// Represents a view into a block of unmanaged memory that holds a value of a specified 
/// struct type <typeparamref name="T"/>. This struct provides mechanisms to read data directly from the unmanaged memory.
/// </summary>
/// <typeparam name="T">The type of the struct that this view points to. It must be a value type.</typeparam>
public unsafe readonly struct View<T> where T : struct
{
    private readonly void* _referedData;

    private View(ref T referedData) => _referedData = Unsafe.AsPointer(ref referedData);

    /// <summary>
    /// Creates a new instance of the <see cref="View{T}"/> struct from a reference to 
    /// the specified struct type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="referedData">A reference to the data to be viewed.</param>
    /// <returns>A new <see cref="View{T}"/> instance that points to the specified data.</returns>
    public static View<T> Of(ref T referedData) => new(ref referedData);

    /// <summary>
    /// Retrieves a read-only reference to the struct value located at the pointer position.
    /// This allows for access to the data without modifying it.
    /// </summary>
    /// <returns>A read-only reference to the struct value.</returns>
    public readonly ref readonly T Peek() => ref Unsafe.AsRef<T>(_referedData);

    /// <summary>
    /// Reads the struct value from the unmanaged memory directly and returns it.
    /// </summary>
    /// <returns>The struct value read from the unmanaged memory.</returns>
    public readonly T Read() => Unsafe.Read<T>(_referedData);
}