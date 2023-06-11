﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory blocks.
/// </summary>
internal unsafe abstract class FixedMemory : ReadOnlyFixedMemory, IFixedMemory, IEquatable<FixedMemory>
{
    /// <summary>
    /// Constructs a new <see cref="FixedMemory"/> instance using a pointer to a memory block, and its size.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    protected FixedMemory(void* ptr, Int32 binaryLength) : base(ptr, binaryLength, false) { }
    /// <summary>
    /// Constructs a new <see cref="FixedMemory"/> instance using a pointer to a memory block, its size,
    /// and a valid status.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    /// <param name="isValid">Indicates whether current instance remains valid.</param>
    protected FixedMemory(void* ptr, Int32 binaryLength, IMutableWrapper<Boolean> isValid) : base(ptr, binaryLength, false, isValid) { }
    /// <summary>
    /// Constructs a new <see cref="FixedMemory"/> instance using another instance as a template.
    /// </summary>
    /// <param name="mem">The <see cref="FixedMemory"/> instance to copy data from.</param>
    protected FixedMemory(FixedMemory mem) : base(mem) { }
    /// <summary>
    /// Constructs a new <see cref="FixedMemory"/> instance using another instance as a template and specifying a
    /// memory offset.
    /// </summary>
    /// <param name="mem">The <see cref="FixedMemory"/> instance to copy data from.</param>
    /// <param name="offset">The offset to be added to the pointer to the memory block.</param>
    protected FixedMemory(FixedMemory mem, Int32 offset) : base(mem, offset) { }

    Span<Byte> IFixedMemory.Bytes => base.CreateBinarySpan();
    ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => base.CreateReadOnlyBinarySpan();
    IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();

    /// <inheritdoc/>
    public virtual Boolean Equals(FixedMemory? other) => this.Equals(other as FixedPointer);
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override Boolean Equals(Object? obj) => base.Equals(obj as FixedMemory);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();
    /// <inheritdoc/>
    public new virtual IFixedContext<Byte> AsBinaryContext() => new FixedContext<Byte>(this.BinaryOffset, this);
}