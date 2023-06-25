﻿namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Collection of lengths for each text in the buffer. Used for interpreting the buffer content.
	/// </summary>
	private readonly Int32?[] _lengths;
	/// <summary>
	/// Internal buffer representing the combined null-terminated UTF-8 texts.
	/// </summary>
	private readonly String _value;

	/// <summary>
	/// Retrieves the internal buffer as a <see cref="ReadOnlySpan{Char}"/> instance and creates a
	/// <see cref="CString"/> array representing the sequence of texts.
	/// </summary>
	/// <param name="output">
	/// Output <see cref="CString"/> array that represents the sequence of texts.
	/// </param>
	/// <returns>A <see cref="ReadOnlySpan{Char}"/> representing the internal buffer.</returns>
	private unsafe ReadOnlySpan<Char> AsUnsafeSpan(out CString[] output)
	{
		ReadOnlySpan<Char> result = this._value;
		ref Char firstCharRef = ref MemoryMarshal.GetReference(result);
		IntPtr ptr = new(Unsafe.AsPointer(ref firstCharRef));
		output = this.GetValues(ptr).ToArray();
		return this._value;
	}
	/// <summary>
	/// Retrieves a sequence of <see cref="CString"/> based on the buffer and lengths of the texts.
	/// </summary>
	/// <param name="ptr">Pointer to the start of the buffer.</param>
	/// <returns>An <see cref="IEnumerable{CString}"/> representing the sequence of texts.</returns>
	private IEnumerable<CString> GetValues(IntPtr ptr)
	{
		Int32 offset = 0;
		for (Int32 i = 0; i < this._lengths.Length; i++)
		{
			Int32? length = this._lengths[i];
			if (length.HasValue && length.Value > 0)
			{
				yield return CString.CreateUnsafe(ptr + offset, length.Value + 1);
				offset += length.Value + 1;
			}
			else if (length.HasValue)
			{
				yield return CString.Empty;
			}
			else
			{
				yield return CString.Zero;
			}
		}
	}
}