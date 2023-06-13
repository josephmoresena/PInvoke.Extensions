﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides a mechanism for iterating through a sequence of elements of type
/// <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence.</typeparam>
internal sealed class SequenceEnumerator<T> : IEnumerator<T>
{
    /// <summary>
    /// The sequence of elements to iterate through.
    /// </summary>
    private readonly IEnumerableSequence<T> _instance;

    /// <summary>
    /// The current element in the sequence.
    /// </summary>
    private T _current = default!;
    /// <summary>
    /// The current position in the sequence.
    /// </summary>
    private Int32 _index = -1;

    /// <inheritdoc/>
    public T Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ValidationUtilities.ThrowIfInvalidIndexEnumerator(this._index, this._instance.GetSize());
            return this._current;
        }
    }

    Object IEnumerator.Current => this.Current!;

    /// <summary>
    /// Initializes a new instance of the <see cref="SequenceEnumerator{T}"/> class.
    /// </summary>
    /// <param name="instance">The sequence of elements to iterate through.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SequenceEnumerator(IEnumerableSequence<T> instance)
    {
        this._instance = instance;
    }

    void IDisposable.Dispose()
    {
        // No unmanaged resources to dispose.
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean MoveNext()
    {
        this._index++;
        if (this._index < this._instance.GetSize())
        {
            this._current = this._instance.GetItem(this._index);
            return true;
        }
        return false;
    }
    /// <inheritdoc/>
    public void Reset() => this._index = -1;
}

