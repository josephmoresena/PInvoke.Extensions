﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Represents text as a sequence of UTF-8 code units.
    /// </summary>
    public sealed class CString : ICloneable
    {
        /// <summary>
        /// Represents the empty UTF-8 string. This field is read-only.
        /// </summary>
#pragma warning disable CS8601 // Possible null reference assignment.
        private static readonly CString Empty = new Byte[] { default };
#pragma warning restore CS8601 // Possible null reference assignment.

        /// <summary>
        /// Internal object data.
        /// </summary>
        private readonly Object _data;

        /// <summary>
        /// Internal <see cref="Byte"/> array for internal <see cref="CString"/>.
        /// </summary>
        private Byte[]? internalData => this._data as Byte[];
        /// <summary>
        /// <see cref="NativeArrayReference{Byte}"/> object for external <see cref="CString"/>.
        /// </summary>
        private NativeArrayReference<Byte> externalData => this._data as NativeArrayReference<Byte> ?? NativeArrayReference<Byte>.Empty;

        /// <summary>
        /// Gets the <see cref="Byte"/> object at a specified position in the current <see cref="CString"/>
        /// object.
        /// </summary>
        /// <param name="index">A position in the current c string.</param>
        /// <returns>The object at position <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
        /// </exception>
        [IndexerName("Position")]
        public Byte this[Int32 index] => this.internalData != default ? this.internalData[index] : this.externalData[index];

        /// <summary>
        /// Gets the number of bytes in the current <see cref="CString"/> object.
        /// </summary>
        /// <returns>
        /// The number of characters in the current string.
        /// </returns>
        public Int32 Length => this.internalData != default ? this.internalData.Length : this.externalData.Length;

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="bytes">Binary internal information.</param>
        private CString([DisallowNull] Byte[] bytes)
        {
            this._data = bytes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// UTF-8 character repeated a specified number of times.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="count"></param>
        public CString(Byte c, Int32 count) : this(Enumerable.Repeat(c, count).ToArray()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CString"/> class to the value indicated by a specified 
        /// pointer to an array of UTF-8 characters and a length.
        /// </summary>
        /// <param name="ptr">A pointer to a array of UTF-8 characters.</param>
        /// <param name="length">The number of <see cref="Byte"/> within value to use.</param>
        public CString(IntPtr ptr, Int32 length)
        {
            this._data = new NativeArrayReference<Byte>(ptr, length);
        }

        /// <summary>
        /// Retrieves an object that can iterate through the individual <see cref="Byte"/> in this 
        /// <see cref="CString"/>.
        /// </summary>
        /// <returns>An enumerator object.</returns>
        public ReadOnlySpan<Byte>.Enumerator GetEnumerator() => this.GetSpan().GetEnumerator();

        /// <summary>
        /// Returns a reference to this instance of <see cref="CString"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Object Clone() => new CString(this.GetSpan().ToArray());

        /// <summary>
        /// Defines an implicit conversion of a given <see cref="Byte"/> array to <see cref="CString"/>.
        /// </summary>
        /// <param name="bytes">A <see cref="Byte"/> array to implicitly convert.</param>
        public static implicit operator CString?(Byte[]? bytes) => bytes != default ? new(bytes) : default;
        /// <summary>
        /// Defines an implicit conversion of a given <see cref="String"/> to <see cref="CString"/>.
        /// </summary>
        /// <param name="str">A <see cref="String"/> to implicitly convert.</param>
#pragma warning disable CS8604 // Possible null reference argument.
        public static implicit operator CString?(String? str) => !String.IsNullOrEmpty(str) ? new(str.AsUtf8().Concat(Empty.internalData).ToArray()) : default;
#pragma warning restore CS8604 // Possible null reference argument.
        /// <summary>
        /// Defines an implicit conversion of a given <see cref="CString"/> to a read-only span of bytes.
        /// </summary>
        /// <param name="cString">A <see cref="CString"/> to implicitly convert.</param>
        public static implicit operator ReadOnlySpan<Byte>(CString? cString) => cString != default ? cString.GetSpan() : default;

        /// <summary>
        /// Retreives the internal or external information as <see cref="ReadOnlySpan{Byte}"/> object.
        /// </summary>
        /// <returns><see cref="ReadOnlySpan{Byte}"/> object.</returns>
        private ReadOnlySpan<Byte> GetSpan() => this.internalData != default ? this.internalData : this.externalData;

        /// <summary>
        /// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/> and advances
        /// the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="strm">
        /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
        /// will be copied.
        /// </param>
        /// <param name="nullTerminated">
        /// Indicates whether the UTF-8 text must be null-terminated into the <see cref="Stream"/>.
        /// </param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public Task WriteAsync(Stream strm, Boolean nullTerminated)
        {
            Int32 bytesToWrite = this.Length;
            if (nullTerminated)
                bytesToWrite--;

            if (this.internalData != default)
                return strm.WriteAsync(this.internalData, 0, bytesToWrite);
            else
                return Task.Run(() =>
                {
                    strm.Write(this.externalData.Range(0, bytesToWrite));
                });
        }
    }
}
