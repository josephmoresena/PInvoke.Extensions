﻿using System;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    internal sealed record NativeArrayReference<T>
        where T : unmanaged
    {
        public static readonly NativeArrayReference<T> Empty = new(IntPtr.Zero, default);

        private readonly IntPtr _ptr;
        private readonly Int32 _length;

        public Int32 Length => this._length;

        public NativeArrayReference(IntPtr ptr, Int32 length)
        {
            this._ptr = ptr;
            this._length = length;
        }

        [IndexerName("Position")]
        public ref readonly T this[Int32 index] => ref this.AsSpan()[index];

        public ReadOnlySpan<T>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();
        public T[] ToArray() => this.AsSpan().ToArray();
        public NativeArrayReference<T> Range(Int32 offset, Int32 length)
        {
            if (offset != 0 || length != this.Length)
            {
                this.ValidateRange(offset, length);
                Int32 byteOffset = offset * NativeUtilities.SizeOf<T>();
                IntPtr newPtr = this._ptr + byteOffset;
                return new NativeArrayReference<T>(newPtr, length);
            }
            return this;
        }


        public static implicit operator ReadOnlySpan<T>(in NativeArrayReference<T> externalC) => externalC.AsSpan();

        private ReadOnlySpan<T> AsSpan() => this._ptr.AsReadOnlySpan<T>(this._length);

        private void ValidateRange(Int32 offset, Int32 length)
        {
            if (offset > this._length)
                throw new ArgumentOutOfRangeException(nameof(offset), "");
            if (offset + length > this._length)
                throw new ArgumentOutOfRangeException(nameof(length), "");
        }
    }
}
