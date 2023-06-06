﻿namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a mutable reference to a fixed memory location.
/// </summary>
/// <typeparam name="T">Type of the value referenced in memory.</typeparam>
public interface IFixedReference<T> : IReferenceable<T>, IReadOnlyFixedReference<T>, IFixedMemory where T : unmanaged
{
    /// <summary>
    /// Reinterprets the <typeparamref name="T"/> fixed memory reference as a 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
    /// <param name="residual">Output. Residual memory left after the transformation.</param>
    /// <returns>
    /// A <see cref="IFixedReference{TDestination}"/> instance.
    /// </returns>
    IFixedReference<TDestination> Transformation<TDestination>(out IFixedMemory residual) where TDestination : unmanaged;

    /// <summary>
    /// Reinterprets the <typeparamref name="T"/> fixed memory reference as a 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
    /// <param name="residual">Output. Residual read-only memory left after the transformation.</param>
    /// <returns>
    /// A <see cref="IReadOnlyFixedReference{TDestination}"/> instance.
    /// </returns>
    new IFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual) where TDestination : unmanaged;
}
