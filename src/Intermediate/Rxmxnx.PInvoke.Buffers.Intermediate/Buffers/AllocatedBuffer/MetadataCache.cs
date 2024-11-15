namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	/// <summary>
	/// Static class to store metadata cache for <typeparamref name="T"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	internal static partial class MetadataCache<T>
	{
		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
		public static IBufferTypeMetadata<T>? GetMetadata(UInt16 count)
		{
			IBufferTypeMetadata<T>? result = MetadataCache<T>.GetFundamental(count);
			if (result is null) return result;
			while (count - result.Size > 0)
			{
				IBufferTypeMetadata<T>? aux = MetadataCache<T>.GetMetadata((UInt16)(count - result.Size));
				lock (MetadataCache<T>.cache.LockObject)
				{
					// Auxiliary metadata not found. Use minimal.
					if (aux is null)
						return MetadataCache<T>.cache.GetMinimal(count);
					result = result.Compose(aux);
					if (result is null)
						// Unable to create composed metadata. Use minimal.
						return MetadataCache<T>.cache.GetMinimal(count);
					MetadataCache<T>.cache.Add(result);
				}
			}
			return result;
		}
		/// <summary>
		/// Creates <see cref="IBufferTypeMetadata{T}"/> for <see cref="Composed{TBufferA,TBufferB,T}"/>.
		/// </summary>
		/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
		/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
		/// <returns>
		/// The <see cref="IBufferTypeMetadata{T}"/> for <see cref="Composed{TBufferA,TBufferB,T}"/> buffer.
		/// </returns>
		[UnconditionalSuppressMessage("AOT", "IL2055",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL2060",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL3050",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		public static IBufferTypeMetadata<T>? CreateComposedWithReflection<TBufferA, TBufferB>()
			where TBufferA : struct, IAllocatedBuffer<T> where TBufferB : struct, IAllocatedBuffer<T>
		{
			if (MetadataCache<T>.cache.GetMetadataInfo is null) return default;
			try
			{
				Type genericType =
					AllocatedBuffer.typeofComposed.MakeGenericType(typeof(TBufferA), typeof(TBufferB), typeof(T));
				MethodInfo getGenericMetadataInfo =
					MetadataCache<T>.cache.GetMetadataInfo.MakeGenericMethod(genericType);
				Func<IBufferTypeMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<IBufferTypeMetadata<T>>>();
				IBufferTypeMetadata<T> result = getGenericMetadata();
				MetadataCache<T>.cache.Add(result);
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.cache.MaxSpace) < result.Size)
					MetadataCache<T>.cache.MaxSpace *= 2;
				return result;
			}
			catch (Exception)
			{
				return default;
			}
		}
		/// <summary>
		/// Registers buffer type.
		/// </summary>
		/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
		public static void RegisterBuffer<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		{
			IBufferTypeMetadata<T> metadata = IAllocatedBuffer<T>.GetMetadata<TBuffer>();
			lock (MetadataCache<T>.cache.LockObject)
			{
				if (!MetadataCache<T>.cache.Add(metadata) || !TBuffer.IsBinary) return;
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.cache.MaxSpace) < metadata.Size)
					MetadataCache<T>.cache.MaxSpace *= 2;
				TBuffer.AppendComponent(MetadataCache<T>.cache.Buffers);
			}
		}
	}
}
#pragma warning restore CA2252