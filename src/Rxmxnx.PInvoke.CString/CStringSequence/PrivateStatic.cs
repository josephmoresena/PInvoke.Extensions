﻿namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Creates a new UTF-8 text sequence with a specific <paramref name="lengths"/> and initializes each
    /// UTF-8 texts into it after creation by using the specified callback.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
    /// <returns>The create UTF-8 text sequence.</returns>
    public static CStringSequence Create<TState>(TState state, CStringSequenceCreationAction<TState> action, params Int32[] lengths)
    {
        Int32 bytesLength = lengths.Where(x => x > 0).Sum(x => x + 1);
        Int32 length = bytesLength / SizeOfChar + (bytesLength % SizeOfChar);
        String buffer = String.Create(length, state, (span, state) =>
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    CreateCStringSequence(new(ptr), lengths, state, action);
            }
        });
        return new(buffer, lengths);
    }

    /// <summary>
    /// Creates the sequence buffer.
    /// </summary>
    /// <param name="lengths">Text length collection.</param>
    /// <param name="values">Text collection.</param>
    /// <returns>
    /// <see cref="String"/> instance that contains the binary information of the UTF-8 text sequence.
    /// </returns>
    private static String CreateBuffer(Int32[] lengths, CString?[] values)
    {
        Int32 totalBytes = 0;
        for (Int32 i = 0; i < lengths.Length; i++)
            if (values[i] is CString value && value.Length > 0)
                totalBytes += value.Length + 1;
        Int32 totalChars = (totalBytes / SizeOfChar) + (totalBytes % SizeOfChar);
        return String.Create(totalChars, values, CopyText);
    }

    /// <summary>
    /// Copy the content of <see cref="CString"/> items in <paramref name="values"/> to
    /// <paramref name="charSpan"/> span.
    /// </summary>
    /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
    /// <param name="values">A enumeration of <see cref="CString"/> items.</param>
    private static void CopyText(Span<Char> charSpan, CString?[] values)
    {
        Int32 position = 0;
        unsafe
        {
            fixed (void* charsPtr = &MemoryMarshal.GetReference(charSpan))
            {
                Span<Byte> byteSpan = new(charsPtr, charSpan.Length * SizeOfChar);
                for (Int32 i = 0; i < values.Length; i++)
                    if (values[i] is CString value && value.Length > 0)
                    {
                        ReadOnlySpan<Byte> valueSpan = value.AsSpan();
                        valueSpan.CopyTo(byteSpan[position..]);
                        position += valueSpan.Length;
                        byteSpan[position] = default;
                        position++;
                    }
            }
        }
    }

    /// <summary>
    /// Copy the content of <paramref name="sequence"/> to <paramref name="charSpan"/>.
    /// </summary>
    /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
    /// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
    private static void CopySequence(Span<Char> charSpan, CStringSequence sequence)
    {
        ReadOnlySpan<Char> chars = sequence._value;
        chars.CopyTo(charSpan);
    }

    /// <summary>
    /// Preforms a binary copy of all non-empty <paramref name="span"/> to 
    /// <paramref name="destination"/> span.
    /// </summary>
    /// <param name="span">A read-only <see cref="CString"/> span instance.</param>
    /// <param name="destination">The destination binary buffer.</param>
    private static void BinaryCopyTo(ReadOnlySpan<CString> span, Byte[] destination)
    {
        Int32 offset = 0;
        foreach (CString value in span)
            if (value.Length > 0)
            {
                ReadOnlySpan<Byte> bytes = value.AsSpan();
                bytes.CopyTo(destination.AsSpan()[offset..bytes.Length]);
                offset += bytes.Length;
            }
    }

    /// <summary>
    /// Performs the creation of the UTF-8 text sequence with a specific <paramref name="lengths"/> and 
    /// whose buffer is referenced by <paramref name="bufferPtr"/>. 
    /// Each UTF-8 text is initialized using the specified callback.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="bufferPtr">Pointer to internal <see cref="CStringSequence"/> buffer.</param>
    /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
    private static unsafe void CreateCStringSequence<TState>(IntPtr bufferPtr, Int32[] lengths, TState state, CStringSequenceCreationAction<TState> action)
    {
        Int32 offset = 0;
        for (Int32 i = 0; i < lengths.Length; i++)
            if (lengths[i] > 0)
            {
                IntPtr currentPtr = bufferPtr + offset;
                Span<Byte> bytes = new(currentPtr.ToPointer(), lengths[i]);
                action(bytes, i, state);
                offset += lengths[i] + 1;
            }
    }
}

