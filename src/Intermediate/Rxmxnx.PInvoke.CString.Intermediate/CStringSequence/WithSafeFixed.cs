﻿namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Executes a specified action using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <param name="action">The action to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe void WithSafeFixed(ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			_ = this.AsUnsafeSpan(out CString[] output);
			FixedCStringSequence fseq =
				new(output, CString.CreateUnsafe(new(ptr), this._value.Length * CStringSequence.SizeOfChar, true));
			try
			{
				action(fseq);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified action on the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>,
	/// using an additional parameter passed to the action.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <param name="state">The additional parameter to pass to the action.</param>
	/// <param name="action">The action to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe void WithSafeFixed<TState>(TState state, ReadOnlyFixedListAction<TState> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			_ = this.AsUnsafeSpan(out CString[] output);
			FixedCStringSequence fseq =
				new(output, CString.CreateUnsafe(new(ptr), this._value.Length * CStringSequence.SizeOfChar, true));
			try
			{
				action(fseq, state);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified function using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="func">The function to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe TResult WithSafeFixed<TResult>(ReadOnlyFixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			_ = this.AsUnsafeSpan(out CString[] output);
			FixedCStringSequence fseq =
				new(output, CString.CreateUnsafe(new(ptr), this._value.Length * CStringSequence.SizeOfChar, true));
			try
			{
				return func(fseq);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified function using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>,
	/// and an additional parameter passed to the function.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="state">The additional parameter to pass to the function.</param>
	/// <param name="func">The function to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe TResult WithSafeFixed<TState, TResult>(TState state, ReadOnlyFixedListFunc<TState, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			_ = this.AsUnsafeSpan(out CString[] output);
			FixedCStringSequence fseq =
				new(output, CString.CreateUnsafe(new(ptr), this._value.Length * CStringSequence.SizeOfChar, true));
			try
			{
				return func(fseq, state);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
}