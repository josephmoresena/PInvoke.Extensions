﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of mutable <see cref="Input{TValue}"/> object.
/// </summary>
/// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
internal sealed record Reference<TValue> : Input<TValue>, IMutableReference<TValue>
    where TValue : struct
{
    /// <summary>
    /// Internal lock object.
    /// </summary>
    private readonly Object _writeLock = new();

    /// <inheritdoc/>
    internal override void SetInstance(in TValue newValue)
    {
        lock (this._writeLock)
            base._instance = newValue;
    }

    void IMutableWrapper<TValue>.SetInstance(TValue newValue) => this.SetInstance(newValue);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal Reference(in TValue instance) : base(instance) { }
}