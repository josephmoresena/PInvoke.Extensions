﻿using System;

namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Private constructor.
    /// </summary>
    /// <param name="bytes">Binary internal information.</param>
    private CString([DisallowNull] Byte[] bytes)
    {
        this._isLocal = true;
        this._data = ValueRegion<Byte>.Create(bytes);
        this._isNullTerminated = bytes.Any() && bytes[^1] == default;
        this._length = bytes.Length - (this._isNullTerminated ? 1 : 0);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate.</param>
    /// <param name="isLiteral">Indicates whether returned span is from UTF-8 literal.</param>
    private CString(ReadOnlySpanFunc<Byte> func, Boolean isLiteral)
    {
        this._isLocal = false;
        this._isFunction = true;
        this._data = ValueRegion<Byte>.Create(func);

        ReadOnlySpan<Byte> data = func();
        Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
        this._isNullTerminated = isLiteral || isNullTerminatedSpan;
        this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">A <see cref="CString"/> value.</param>
    /// <param name="startIndex">Offset for range.</param>
    /// <param name="length">Length of range.</param>
    private CString(CString value, Int32 startIndex, Int32 length)
    {
        this._isLocal = value._isLocal;
        this._isFunction = value._isFunction;
        this._data = ValueRegion<Byte>.Create(value._data, startIndex, value.GetDataLength(startIndex, length));

        ReadOnlySpan<Byte> data = this._data;
        Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
        Boolean isLiteral = value._isFunction && value._isNullTerminated;
        this._isNullTerminated = isLiteral && value._length - startIndex == length || isNullTerminatedSpan;
        this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="utf16Text">UTF-16 text.</param>
    private CString(String utf16Text)
    {
        String utf8String = GetUtf8String(utf16Text);
        this._isLocal = true;
        this._isFunction = true;
        this._data = ValueRegion<Byte>.Create(() => MemoryMarshal.Cast<Char, Byte>(utf8String));
        this._length = utf16Text.Length;
        this._isNullTerminated = true;
    }
}