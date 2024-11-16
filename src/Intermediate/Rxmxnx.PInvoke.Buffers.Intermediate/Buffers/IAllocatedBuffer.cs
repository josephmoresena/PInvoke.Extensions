namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if NET6_0
[RequiresPreviewFeatures]
#endif
public interface IAllocatedBuffer<T>
{
	/// <summary>
	/// Indicates whether current type is binary space.
	/// </summary>
	internal static abstract Boolean IsBinary { get; }
	/// <summary>
	/// Buffer metadata.
	/// </summary>
	private protected static abstract IBufferTypeMetadata<T> Metadata { get; }
	/// <summary>
	/// Current type components.
	/// </summary>
	internal static abstract IBufferTypeMetadata<T>[] Components { get; }

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> components);
	/// <summary>
	/// Appends a composed buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the composed buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void Append<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> components)
		where TBuffer : struct, IAllocatedBuffer<T>;

	/// <summary>
	/// Retrieves the <see cref="IBufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="IBufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static IBufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		=> TBuffer.Metadata;

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> components)
		where TBuffer : struct, IAllocatedBuffer<T>
	{
		if (TBuffer.IsBinary)
			IAllocatedBuffer<T>.AppendComponent(TBuffer.Metadata, components);
	}

	/// <summary>
	/// Appends all components from <paramref name="component"/> instance.
	/// </summary>
	/// <param name="component">A <see cref="IBufferTypeMetadata{T}"/> instance.</param>
	/// <param name="components">A dictionary of components.</param>
	private static void AppendComponent(IBufferTypeMetadata<T> component,
		IDictionary<UInt16, IBufferTypeMetadata<T>> components)
	{
		if (!components.TryAdd(component.Size, component)) return;
		foreach (IBufferTypeMetadata<T> metadataComponent in component.Components.AsSpan())
			IAllocatedBuffer<T>.AppendComponent(metadataComponent, components);
	}
}