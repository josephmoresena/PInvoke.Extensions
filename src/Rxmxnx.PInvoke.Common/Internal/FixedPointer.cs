﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class from memory pointer block fixing.
/// </summary>
internal unsafe abstract class FixedPointer : IFixedPointer, IEquatable<FixedPointer>
{
    /// <summary>
    /// Pointer to fixed memory block.
    /// </summary>
    private readonly void* _ptr;
    /// <summary>
    /// Memory block size in bytes.
    /// </summary>
    private readonly Int32 _binaryLength;
    /// <summary>
    /// Indicates whether the memory block is read-only. 
    /// </summary>
    private readonly Boolean _isReadOnly;
    /// <summary>
    /// Indicates whether current instance remains valid.
    /// </summary>
    private readonly IMutableWrapper<Boolean> _isValid;

    /// <summary>
    /// Memory type.
    /// </summary>
    public abstract Type? Type { get; }
    /// <summary>
    /// Memory offset.
    /// </summary>
    public abstract Int32 BinaryOffset { get; }
    /// <summary>
    /// Indicates whether current instance is a function pointer.
    /// </summary>
    public abstract Boolean IsFunction { get; }
    /// <summary>
    /// Memory block size in bytes.
    /// </summary>
    public Int32 BinaryLength => this._binaryLength - this.BinaryOffset;
    /// <summary>
    /// Indicates whether current instance remains valid.
    /// </summary>
    public Boolean IsReadOnly => this._isReadOnly;

    IntPtr IFixedPointer.Pointer => new(this._ptr);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    protected FixedPointer(void* ptr, Int32 binaryLength, Boolean isReadOnly)
    {
        this._ptr = ptr;
        this._binaryLength = binaryLength;
        this._isValid = new MutableWrapper<Boolean>(true);
        this._isReadOnly = isReadOnly;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    /// <param name="isValid">Indicates whether current instance remains valid.</param>
    protected FixedPointer(void* ptr, Int32 binaryLength, Boolean isReadOnly, IMutableWrapper<Boolean> isValid)
    {
        this._ptr = ptr;
        this._binaryLength = binaryLength;
        this._isValid = isValid;
        this._isReadOnly = isReadOnly;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="fptr">Fixed context of memory block.</param>
    protected FixedPointer(FixedPointer fptr)
    {
        this._ptr = fptr._ptr;
        this._binaryLength = fptr._binaryLength;
        this._isValid = fptr._isValid;
        this._isReadOnly = fptr._isReadOnly;
    }

    /// <summary>
    /// Creates a reference of a <typeparamref name="TValue"/> value over the memory block.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <returns>
    /// A reference to a <typeparamref name="TValue"/> value over the memory block.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref TValue CreateReference<TValue>() where TValue : unmanaged
    {
        this.ValidateOperation();
        this.ValidateReferenceSize<TValue>();
        return ref Unsafe.AsRef<TValue>(this._ptr);
    }

    /// <summary>
    /// Creates a read-only reference of a <typeparamref name="TValue"/> value over
    /// the memory block.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    /// <returns>
    /// A read-only reference to a <typeparamref name="TValue"/> value over the memory
    /// block.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref readonly TValue CreateReadOnlyReference<TValue>() where TValue : unmanaged
    {
        this.ValidateOperation(true);
        ValidationUtilities.ThrowIfInvalidRefTypePointer<TValue>(this._binaryLength);
        this.ValidateReferenceSize<TValue>();
        return ref Unsafe.AsRef<TValue>(this._ptr);
    }

    /// <summary>
    /// Creates a <see cref="Span{TValue}"/> instance over the memory block whose 
    /// length is <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the objects in the span.</typeparam>
    /// <param name="length">Span length.</param>
    /// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<TValue> CreateSpan<TValue>(Int32 length) where TValue : unmanaged
    {
        this.ValidateOperation();
        return new(this._ptr, length);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlySpan{TValue}"/> instance over the memory block whose 
    /// length is <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the objects in the read-only span.</typeparam>
    /// <param name="length">Span length.</param>
    /// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<TValue> CreateReadOnlySpan<TValue>(Int32 length) where TValue : unmanaged
    {
        this.ValidateOperation(true);
        return new(this._ptr, length);
    }

    /// <summary>
    /// Creates a <see cref="Span{Byte}"/> instance over the memory block.
    /// </summary>
    /// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<Byte> CreateBinarySpan()
    {
        this.ValidateOperation();
        void* ptr = this.GetMemoryOffset();
        return new(ptr, this.BinaryLength);
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlySpan{Byte}"/> instance over the memory block.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<Byte> CreateReadOnlyBinarySpan()
    {
        this.ValidateOperation(true);
        void* ptr = this.GetMemoryOffset();
        return new(ptr, this.BinaryLength);
    }

    /// <summary>
    /// Creates a <typeparamref name="TDelegate"/> instance over the memory block.
    /// </summary>
    /// <typeparam name="TDelegate">A <see cref="Delegate"/> type.</typeparam>
    /// <returns>A <typeparamref name="TDelegate"/> instance over the memory block.</returns>
    public TDelegate CreateDelegate<TDelegate>() where TDelegate : Delegate
    {
        this.ValidateFunctionOperation();
        return Marshal.GetDelegateForFunctionPointer<TDelegate>(new(this._ptr));
    }

    /// <summary>
    /// Invalidates current context.
    /// </summary>
    public virtual void Unload() => this._isValid.Value = false;

    /// <inheritdoc/>
    public virtual Boolean Equals(FixedPointer? other)
        => other is not null && this.GetMemoryOffset() == other.GetMemoryOffset() &&
        this.BinaryLength == other.BinaryLength && this._isReadOnly == other._isReadOnly;

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override Boolean Equals(Object? obj) => this.Equals(obj as FixedPointer);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override Int32 GetHashCode()
    {
        HashCode result = new();
        result.Add(new IntPtr(this._ptr));
        result.Add(this.BinaryOffset);
        result.Add(this._binaryLength);
        result.Add(this._isReadOnly);
        if (this.Type is not null)
            result.Add(this.Type);
        return result.ToHashCode();
    }

    /// <summary>
    /// Validates any operation over the fixed memory block.
    /// </summary>
    /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ValidateOperation(Boolean isReadOnly = false)
    {
        ValidationUtilities.ThrowIfFunctionPointer(this.IsFunction);
        ValidationUtilities.ThrowIfInvalidPointer(this._isValid);
        ValidationUtilities.ThrowIfReadOnlyPointer(isReadOnly, this._isReadOnly);
    }

    /// <summary>
    /// Retrieves the number of <typeparamref name="TValue"/> items that can be
    /// referenced into the fixed memory block.
    /// </summary>
    /// <typeparam name="TValue">The type of the objects referenced.</typeparam>
    /// <returns>
    /// The number of <typeparamref name="TValue"/> items that can be referenced into the
    /// fixed memory block.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected Int32 GetCount<TValue>() where TValue : unmanaged
        => this._binaryLength / sizeof(TValue);

    /// <summary>
    /// Validates the size of the referenced value type from current instance.
    /// </summary>
    /// <typeparam name="TValue">Type of the referenced value.</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ValidateReferenceSize<TValue>() where TValue : unmanaged
        => ValidationUtilities.ThrowIfInvalidRefTypePointer<TValue>(this._binaryLength);

    /// <summary>
    /// Validates any operation over the fixed function pointer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateFunctionOperation()
    {
        ValidationUtilities.ThrowIfNotFunctionPointer(this.IsFunction);
        ValidationUtilities.ThrowIfInvalidPointer(this._isValid);
    }

    /// <summary>
    /// Retrieves the memory offset for current instance.
    /// </summary>
    /// <returns>Pointer to offset memory.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void* GetMemoryOffset()
    {
        IntPtr ptr = new(this._ptr);
        IntPtr result = ptr + this.BinaryOffset;
        return result.ToPointer();
    }
}
