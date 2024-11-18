namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Primordial buffer.
/// </summary>
/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public struct Composed<TBufferA, TBufferB, T> : IManagedBuffer<T> where TBufferA : struct, IManagedBuffer<T>
	where TBufferB : struct, IManagedBuffer<T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly ManagedBufferMetadata<T> metadata =
		new BufferTypeMetadata<Composed<TBufferA, TBufferB, T>, T>(TBufferA.Metadata.Size + TBufferB.Metadata.Size);

	/// <summary>
	/// Low buffer.
	/// </summary>
	private TBufferA _buff0;
	/// <summary>
	/// High buffer.
	/// </summary>
	private TBufferB _buff1;

	static Boolean IManagedBuffer<T>.IsPure => TBufferA.IsBinary && typeof(TBufferA) == typeof(TBufferB);
	static Boolean IManagedBuffer<T>.IsBinary => TBufferA.IsBinary && TBufferB.IsBinary;
	static ManagedBufferMetadata<T> IManagedBuffer<T>.Metadata => Composed<TBufferA, TBufferB, T>.metadata;
	static ManagedBufferMetadata<T>[] IManagedBuffer<T>.Components => [TBufferA.Metadata, TBufferB.Metadata,];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, ManagedBufferMetadata<T>> components)
	{
		IManagedBuffer<T>.AppendComponent<TBufferA>(components);
		IManagedBuffer<T>.AppendComponent<TBufferB>(components);
	}
	static void IManagedBuffer<T>.Append<TBuffer>(IDictionary<UInt16, ManagedBufferMetadata<T>> components)
	{
		TBufferB.Append<Composed<TBufferB, TBuffer, T>>(components);
		TBufferB.Append<TBuffer>(components);
		TBufferA.Append<TBufferB>(components);
	}
}
#pragma warning restore CA2252