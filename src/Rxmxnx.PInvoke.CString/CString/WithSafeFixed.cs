﻿namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current instance and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <param name="action">A <see cref="FixedAction"/> delegate.></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithSafeFixed(ReadOnlyFixedAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        ReadOnlySpan<Byte> span = this._data;
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
            {
                FixedContext<Byte> ctx = new(ptr, this._length, true);
                try
                {
                    action(ctx);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current instance and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedAction{TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WithSafeFixed<TArg>(TArg arg, ReadOnlyFixedAction<TArg> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        ReadOnlySpan<Byte> span = this._data;
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
            {
                FixedContext<Byte> ctx = new(ptr, this._length, true);
                try
                {
                    action(ctx, arg);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current instance and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="func">A <see cref="FixedFunc{TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult WithSafeFixed<TResult>(ReadOnlyFixedFunc<TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        ReadOnlySpan<Byte> span = this._data;
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
            {
                FixedContext<Byte> ctx = new(ptr, this._length, true);
                try
                {
                    return func(ctx);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current instance and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="FixedFunc{TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult WithSafeFixed<TArg, TResult>(TArg arg, ReadOnlyFixedFunc<TArg, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        ReadOnlySpan<Byte> span = this._data;
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
            {
                FixedContext<Byte> ctx = new(ptr, this._length, true);
                try
                {
                    return func(ctx, arg);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        }
    }
}