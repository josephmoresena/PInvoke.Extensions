﻿namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8ConcatenationHelper<T>
{
    /// <summary>
    /// Delegate. Writes UTF-8 bytes in current instance.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private delegate void BinaryWriteDelegate(ReadOnlySpan<Byte> value);
    /// <summary>
    /// Delegate. Writes values' UTF-8 bytes in current instance.
    /// </summary>
    /// <param name="value">Value to write.</param>
    private delegate void WriteDelegate(T? value);
    /// <summary>
    /// Delegate. Asynchronously writes values' UTF-8 bytes in current instance.
    /// </summary>
    /// <param name="value">Value to write.</param>
    /// <returns>A taks that represents the asynchronous write operation.</returns>
    private delegate Task WriteAsyncDelegate(T? value);

    /// <summary>
    /// Internal memory stream.
    /// </summary>
    private readonly MemoryStream _mem;
    /// <summary>
    /// Internal stream writer.
    /// </summary>
    private readonly StreamWriter _writer;
    /// <summary>
    /// Separator.
    /// </summary>
    private readonly T? _separator;
    /// <summary>
    /// Delegate for check values.
    /// </summary>
    private readonly IsEmptyDelegate _isEmpty;

    /// <summary>
    /// Current binary write delegate.
    /// </summary>
    private BinaryWriteDelegate _binaryWrite = default!;
    /// <summary>
    /// Current value write delegate.
    /// </summary>
    private WriteDelegate _write = default!;
    /// <summary>
    /// Current value asynchronous write delegate.
    /// </summary>
    private WriteAsyncDelegate _writeAsync = default!;
    /// <summary>
    /// Indicates whether current instance has been disposed.
    /// </summary>
    private Boolean _disposedValue = false;

    /// <summary>
    /// Initialize current instance delegate.
    /// </summary>
    private void InitializeDelegates()
    {
        if (!this._isEmpty(this._separator))
        {
            this._binaryWrite = InitialWrite;
            this._write = InitialWrite;
            this._writeAsync = InitialWriteAsync;
        }
        else
        {
            this._binaryWrite = FinalWrite;
            this._write = FinalWrite;
            this._writeAsync = FinalWriteAsync;
        }
    }


    /// <summary>
    /// Writes <paramref name="value"/> in current instance.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private void WriteValue(ReadOnlySpan<Byte> value) => this._mem.Write(value);

    /// <summary>
    /// Retrieves a copy of binary data into the buffer.
    /// </summary>
    /// <param name="nullTerminated">Indicates whether the UTF-8 text must be null-terminated.</param>
    /// <returns>Copy of binary data into the buffer.</returns>
    private Byte[]? GetBinaryData(Boolean nullTerminated)
    {
        ReadOnlySpan<Byte> span = PrepareUtf8Text(this._mem.GetBuffer());
        if (!span.IsEmpty)
        {
            Int32 resultLength = span.Length + (nullTerminated ? 1 : 0);
            Byte[] result = new Byte[resultLength];
            span.CopyTo(result);
            return result;
        }
        return default;
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance and
    /// sets the delegates for separator writing.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private void InitialWrite(ReadOnlySpan<Byte> value)
    {
        if (!value.IsEmpty)
        {
            this.WriteValue(value);
            this._binaryWrite = this.WriteWithSeparator;
            this._write = this.WriteWithSeparator;
            this._writeAsync = this.WriteWithSeparatorAsync;
        }
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance and
    /// sets the delegates for separator writing.
    /// </summary>
    /// <param name="value">Value to write.</param>
    private void InitialWrite(T? value)
    {
        if (!this._isEmpty(value))
        {
            this.WriteValue(value);
            this._binaryWrite = this.WriteWithSeparator;
            this._write = this.WriteWithSeparator;
            this._writeAsync = this.WriteWithSeparatorAsync;
        }
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance preceding by
    /// the separator.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private void WriteWithSeparator(ReadOnlySpan<Byte> value)
    {
        if (!value.IsEmpty)
        {
            this.WriteValue(this._separator!);
            this.WriteValue(value);
        }
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance preceding by
    /// the separator.
    /// </summary>
    /// <param name="value">Value to write.</param>
    private void WriteWithSeparator(T? value)
    {
        if (!this._isEmpty(value))
        {
            this.WriteValue(this._separator!);
            this.WriteValue(value);
        }
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private void FinalWrite(ReadOnlySpan<Byte> value)
    {
        if (!value.IsEmpty)
            this.WriteValue(value);
    }

    /// <summary>
    /// Writes <paramref name="value"/> in current instance.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    private void FinalWrite(T? value)
    {
        if (!this._isEmpty(value))
            this.WriteValue(value);
    }

    /// <summary>
    /// Asynchronously writes <paramref name="value"/> in current instance and
    /// sets the delegates for separator writing.
    /// </summary>
    /// <param name="value">Value to write.</param>
    /// <returns>A taks that represents the asynchronous write operation.</returns>
    private async Task InitialWriteAsync(T? value)
    {
        if (!this._isEmpty(value))
        {
            await this.WriteValueAsync(value);
            this._binaryWrite = this.WriteWithSeparator;
            this._write = this.WriteWithSeparator;
            this._writeAsync = this.WriteWithSeparatorAsync;
        }
    }

    /// <summary>
    /// Asynchronously writes <paramref name="value"/> in current instance preceding by
    /// the separator.
    /// </summary>
    /// <param name="value">Value to write.</param>
    /// <returns>A taks that represents the asynchronous write operation.</returns>
    private async Task WriteWithSeparatorAsync(T? value)
    {
        if (!this._isEmpty(value))
        {
            await this.WriteValueAsync(this._separator!);
            if (this._isEmpty(value))
                await this.WriteValueAsync(value);
        }
    }

    /// <summary>
    /// Asynchronously writes <paramref name="value"/> in current instance.
    /// </summary>
    /// <param name="value">UTF-8 bytes to write.</param>
    /// <returns></returns>
    private async Task FinalWriteAsync(T? value)
    {
        if (!this._isEmpty(value))
            await this.WriteValueAsync(value);
    }
}

