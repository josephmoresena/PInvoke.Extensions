namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal sealed class BufferTypeMetadata<TBuffer, T> : BufferTypeMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
	/// <inheritdoc/>
	public override Type BufferType => typeof(TBuffer);

	/// <summary>
	/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
#pragma warning disable CA2252
	public BufferTypeMetadata(Int32 capacity, Boolean isBinary = true) : base(
		isBinary, TBuffer.Components, (UInt16)capacity)
#pragma warning restore CA2252
	{ }

	/// <inheritdoc/>
	internal override BufferTypeMetadata<T>? Compose(BufferTypeMetadata<T> otherMetadata)
		=> otherMetadata.Compose<TBuffer>();
	/// <inheritdoc/>
	internal override BufferTypeMetadata<T>? Compose<TOther>()
	{
		if (!BufferManager.BufferAutoCompositionEnabled || !this.IsBinary ||
		    !IManagedBuffer<T>.GetMetadata<TOther>().IsBinary) return default;
		return BufferManager.MetadataManager<T>.ComposeWithReflection(typeof(TBuffer), typeof(TOther));
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute(ScopedBufferAction<T> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		action(scoped);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TState>(TState state, ScopedBufferAction<T, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		action(scoped, state);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TResult>(ScopedBufferFunc<T, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TState, TResult>(TState state, ScopedBufferFunc<T, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped, state);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TU, TState>(TState state, ScopedBufferAction<TU, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size, this);
		action(scoped, state);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TU, TState, TResult>(TState state, ScopedBufferFunc<TU, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped, state);
	}
}