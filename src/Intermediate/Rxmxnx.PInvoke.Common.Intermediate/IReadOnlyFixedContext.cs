﻿namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a context from a read-only block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the read-only fixed memory block.</typeparam>
public interface IReadOnlyFixedContext<T> : IReadOnlyFixedMemory<T> where T : unmanaged
{
	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as
	/// a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual read-only memory from the transformation.</param>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{TDestination}"/>.</returns>
	IReadOnlyFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		where TDestination : unmanaged;
}