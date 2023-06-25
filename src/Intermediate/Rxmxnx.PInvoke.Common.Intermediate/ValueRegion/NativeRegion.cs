﻿namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a native memory region in which a sequence of <typeparamref name="T"/> values is stored.
	/// </summary>
	private sealed class NativeRegion : ValueRegion<T>
	{
		/// <summary>
		/// An empty instance of <see cref="NativeRegion"/>.
		/// </summary>
		public static readonly NativeRegion Empty = new(IntPtr.Zero, 0);
		/// <summary>
		/// The length of the <typeparamref name="T"/> sequence.
		/// </summary>
		private readonly Int32 _length;

		/// <summary>
		/// The pointer to the native memory region.
		/// </summary>
		private readonly IntPtr _ptr;

		/// <summary>
		/// Initializes a new instance of the <see cref="NativeRegion"/> class.
		/// </summary>
		/// <param name="ptr">The pointer to the native memory region.</param>
		/// <param name="length">The length of the <typeparamref name="T"/> sequence.</param>
		public NativeRegion(IntPtr ptr, Int32 length)
		{
			this._ptr = ptr;
			this._length = this._ptr != IntPtr.Zero ? length : 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NativeRegion"/> class from a subrange of an existing
		/// <see cref="NativeRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="NativeRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private unsafe NativeRegion(NativeRegion region, Int32 offset, Int32 length)
		{
			T* tPtr = region.GetElementPointer(offset);
			this._ptr = new(tPtr);
			this._length = length;
		}

		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex) => this.Slice(startIndex, this._length - startIndex);
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			ValidationUtilities.ThrowIfInvalidSubregion(this._length, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}

		/// <inheritdoc/>
		internal override unsafe ReadOnlySpan<T> AsSpan()
		{
			void* pointer = this._ptr.ToPointer();
			return new(pointer, this._length);
		}
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> length != 0 ? new(this, startIndex, length) : NativeRegion.Empty;

		/// <summary>
		/// Retrieves the pointer of the element at the given index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The pointer of the element at the given index.</returns>
		private unsafe T* GetElementPointer(Int32 index)
		{
			T* tPtr = (T*)this._ptr.ToPointer();
			tPtr += index;
			return tPtr;
		}
	}
}