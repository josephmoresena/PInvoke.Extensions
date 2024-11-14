namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This class allows to allocate buffers on stack if possible.
/// </summary>
#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="action">Action to perform with allocated buffer.</param>
	public static void Alloc<T>(UInt16 count, AllocatedBufferAction<T> action)
	{
		try
		{
			if (typeof(T).IsValueType)
				AllocatedBuffer.AllocValue(count, action);
			else
				AllocatedBuffer.AllocObject(count, action);
		}
		catch (Exception)
		{
			AllocatedBuffer.AllocHeap(count, action);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="state">State object.</param>
	/// <param name="action">Action to perform with allocated buffer.</param>
	public static void Alloc<T, TState>(UInt16 count, TState state, AllocatedBufferAction<T, TState> action)
	{
		try
		{
			if (typeof(T).IsValueType)
				AllocatedBuffer.AllocValue(count, state, action);
			else
				AllocatedBuffer.AllocObject(count, state, action);
		}
		catch (Exception)
		{
			AllocatedBuffer.AllocHeap(count, state, action);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="func">Function to execute with allocated buffer.</param>
	public static TResult Alloc<T, TResult>(UInt16 count, AllocatedBufferFunc<T, TResult> func)
	{
		try
		{
			return typeof(T).IsValueType ?
				AllocatedBuffer.AllocValue(count, func) :
				AllocatedBuffer.AllocObject(count, func);
		}
		catch (Exception)
		{
			return AllocatedBuffer.AllocHeap(count, func);
		}
	}
	/// <summary>
	/// Allocates a buffer with <paramref name="count"/> elements and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in allocated buffer.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="count">Number of element in allocated buffer.</param>
	/// <param name="state">State object.</param>
	/// <param name="func">Function to execute with allocated buffer.</param>
	public static TResult Alloc<T, TState, TResult>(UInt16 count, TState state,
		AllocatedBufferFunc<T, TState, TResult> func)
	{
		try
		{
			return typeof(T).IsValueType ?
				AllocatedBuffer.AllocValue(count, state, func) :
				AllocatedBuffer.AllocObject(count, state, func);
		}
		catch (Exception)
		{
			return AllocatedBuffer.AllocHeap(count, state, func);
		}
	}
	/// <summary>
	/// Registers object buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void Register<TBuffer>() where TBuffer : struct, IAllocatedBuffer<Object>
		=> MetadataCache<Object>.RegisterBuffer<TBuffer>();
	/// <summary>
	/// Registers <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
	public static void Register<T, TBuffer>() where TBuffer : struct, IAllocatedBuffer<T> where T : struct
		=> MetadataCache<T>.RegisterBuffer<TBuffer>();
}
#pragma warning restore CA2252

/// <summary>
/// Provides a safe representation of allocated buffer of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public readonly ref struct AllocatedBuffer<T>
{
	/// <summary>
	/// Indicates whether current buffer is heap allocated.
	/// </summary>
	private readonly Boolean _heapAllocated;

	/// <summary>
	/// Current buffer span.
	/// </summary>
	public Span<T> Span { get; }
	/// <summary>
	/// Indicates whether current buffer is stack allocated.
	/// </summary>
	public Boolean InStack => !this._heapAllocated;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="span">Buffer span.</param>
	/// <param name="heapAllocated">Indicates whether current buffer is heap allocated.</param>
	internal AllocatedBuffer(Span<T> span, Boolean heapAllocated = false)
	{
		this.Span = span;
		this._heapAllocated = heapAllocated;
	}
}