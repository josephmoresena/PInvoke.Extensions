﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class represents an arbitrary memory region in which a sequence of 
/// <typeparamref name="T"/> values is found.
/// </summary>
/// <typeparam name="T">Unmanaged type of sequence item.</typeparam>
internal abstract partial class ValueRegion<T> where T : unmanaged
{
    /// <summary>
    /// Gets an item from the memory region at the specified zero-based <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is less then zero or greater than or equal to 
    /// memory region length.
    /// </exception>
    /// <returns>The element from the memory region.</returns>
    public virtual T this[Int32 index] => this.AsSpan()[index];
    /// <summary>
    /// Indicates whether current memory region is segmented.
    /// </summary>
    public virtual Boolean IsSegmented => false;

    /// <summary>
    /// Copies the contents of this memory region into a new array.
    /// </summary>
    /// <returns>An array containing the data in the current memory region.</returns>
    public virtual T[] ToArray() => this.AsSpan().ToArray();

    /// <summary>
    /// Gets an array from this memory region.
    /// </summary>
    /// <returns>An array containing the data in the current memory region.</returns>
    protected virtual T[]? AsArray() => default;

    /// <summary>
    /// Creates a new read-only span over this memory region.
    /// </summary>
    /// <returns>The read-only span representation of the memory region.</returns>
    internal abstract ReadOnlySpan<T> AsSpan();

    /// <inheritdoc/>
    public static implicit operator ReadOnlySpan<T>(ValueRegion<T> region) => region.AsSpan();
    /// <inheritdoc/>
    public static explicit operator T[]?(ValueRegion<T> region) => region.AsArray();

    /// <summary>
    /// Creates a new <see cref="ValueRegion{T}"/> instance from an array of 
    /// <typeparamref name="T"/> values.
    /// </summary>
    /// <param name="array">Array of <typeparamref name="T"/> values.</param>
    /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
    public static ValueRegion<T> Create([DisallowNull] T[] array) => new ManagedRegion(array);
    /// <summary>
    /// Creates a new <see cref="ValueRegion{T}"/> instance from a pointer to memory region and 
    /// the amount of values in sequence.
    /// </summary>
    /// <param name="ptr">Pointer to memory region.</param>
    /// <param name="length">Amount of values in sequence.</param>
    /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
    public static ValueRegion<T> Create(IntPtr ptr, Int32 length)
        => ptr != IntPtr.Zero && length != default ? new NativeRegion(ptr, length) : NativeRegion.Empty;
    /// <summary>
    /// Creates a new <see cref="ValueRegion{T}"/> instance from a <see cref="ReadOnlySpanFunc{T}"/>
    /// delegate.
    /// </summary>
    /// <param name="func"><see cref="ReadOnlySpanFunc{T}"/> delegate.</param>
    /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
    public static ValueRegion<T> Create(ReadOnlySpanFunc<T> func) => new FuncRegion(func);
    /// <summary>
    /// Creates a new <see cref="ValueRegion{T}"/> instance whose offset is <paramref name="startIndex"/>
    /// and whose length is <paramref name="length"/>.
    /// </summary>
    /// <param name="region">A <see cref="ValueRegion{T}"/> instance.</param>
    /// <param name="startIndex">Offset for range.</param>
    /// <param name="length">Length of range.</param>
    /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ValueRegion<T> Create(ValueRegion<T> region, Int32 startIndex, Int32 length)
    {
        if (region is ManagedRegion managed)
            return new SegmentedManagedRegion(managed, startIndex, length);
        else if (region is SegmentedManagedRegion segmented)
            return new SegmentedManagedRegion(segmented, startIndex, length);
        else if (region is FuncRegion func)
            return new SegmentedFuncRegion(func, startIndex, length);
        else if (region is SegmentedFuncRegion segmentedFunc)
            return new SegmentedFuncRegion(segmentedFunc, startIndex, length);

        ReadOnlySpan<T> regionSpan = region.AsSpan();
        ref T spanRef = ref MemoryMarshal.GetReference(regionSpan);
        IntPtr spanPtr = new(Unsafe.AsPointer(ref spanRef));
        return Create(spanPtr + startIndex, length);
    }
}
