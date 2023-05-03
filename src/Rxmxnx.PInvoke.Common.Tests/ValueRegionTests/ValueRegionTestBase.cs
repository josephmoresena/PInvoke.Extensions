﻿namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

public abstract class ValueRegionTestBase
{
    /// <summary>
    /// Fixture instance.
    /// </summary>
    protected static readonly IFixture fixture = new Fixture();

    /// <summary>
    /// Creates a new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.
    /// </summary>
    /// <typeparam name="T">Unmanaged type of the items in the sequence.</typeparam>
    /// <param name="array">A <typeparamref name="T"/> array.</param>
    /// <param name="handles">Collection in which to apend used handle.</param>
    /// <param name="isReference">Indicates whether the created region is a reference.</param>
    /// <returns>A new <see cref="ValueRegion{T}"/> instance from <paramref name="array"/>.</returns>
    protected unsafe static ValueRegion<T> Create<T>(T[] array, ICollection<GCHandle> handles, out Boolean isReference) where T : unmanaged
    {
        isReference = false;
        switch (Random.Shared.Next(default, 7))
        {
            case 0:
            case 1:
                return ValueRegion<T>.Create(array);
            case 2:
            case 4:
                return ValueRegion<T>.Create(() => array);
            default:
                isReference = true;
                handles.Add(GCHandle.Alloc(array, GCHandleType.Pinned));
                fixed (void* ptr = array)
                    return ValueRegion<T>.Create(new IntPtr(ptr), array.Length);
        }
    }
}

