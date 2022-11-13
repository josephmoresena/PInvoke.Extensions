﻿using System;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
    internal unsafe sealed class FixedContext<T> : IFixedContext<T>, IReadOnlyFixedContext<T> where T : unmanaged
    {
        /// <summary>
        /// Pointer to fixed memory block.
        /// </summary>
        private readonly void* _ptr;
        /// <summary>
        /// Number of <typeparamref name="T"/> items in memory block.
        /// </summary>
        private readonly Int32 _count;
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
        private Boolean _isValid;

        /// <summary>
        /// Memory block size in bytes.
        /// </summary>
        public Int32 BinaryLength => this._binaryLength;

        Span<T> IFixedContext<T>.Values => this.CreateSpan<T>(this._count);
        Span<Byte> IFixedContext<T>.BinaryValues => this.CreateSpan<Byte>(this._binaryLength);
        ReadOnlySpan<T> IReadOnlyFixedContext<T>.Values => this.CreateReadOnlySpan<T>(this._count);
        ReadOnlySpan<Byte> IReadOnlyFixedContext<T>.BinaryValues => this.CreateReadOnlySpan<Byte>(this._binaryLength);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="count"></param>
        /// <param name="isReadOnly"></param>
        public FixedContext(void* ptr, Int32 count, Boolean isReadOnly = false)
        {
            this._ptr = ptr;
            this._count = count;
            this._binaryLength = count * sizeof(Byte);
            this._isValid = true;
            this._isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Creates a <see cref="Span{TValue}"/> instance over the memory block whose 
        /// length is <paramref name="length"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the objects in the span.</typeparam>
        /// <param name="length">Span length.</param>
        /// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
        public Span<TValue> CreateSpan<TValue>(Int32 length)
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
        public ReadOnlySpan<TValue> CreateReadOnlySpan<TValue>(Int32 length)
        {
            this.ValidateOperation(true);
            return new(this._ptr, length);
        }
        /// <summary>
        /// Invalidates current context.
        /// </summary>
        public void Unload() => this._isValid = false;

        ITransformationContext<T, TDestination> IFixedContext<T>.Transformation<TDestination>() => this.GetTransformation<TDestination>();
        IReadOnlyFixedContext<T> IFixedContext<T>.AsReadOnly() => this;
        IReadOnlyTransformationContext<T, TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>() => this.GetTransformation<TDestination>();

        /// <summary>
        /// Creates a <see cref="TransformationContext{T, TDestination}"/> instance.
        /// </summary>
        /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
        /// <returns>A <see cref="TransformationContext{T, TDestination}"/> instance.</returns>
        private TransformationContext<T, TDestination> GetTransformation<TDestination>() where TDestination : unmanaged => new(this);
        /// <summary>
        /// Validates any operation over the fixed memory block.
        /// </summary>
        /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
        private void ValidateOperation(Boolean isReadOnly = false)
        {
            if (!isReadOnly && this._isReadOnly)
                throw new InvalidOperationException("The current context is read-only.");
            if (!this._isValid)
                throw new InvalidOperationException("The current context is not valid.");
        }
    }
}
