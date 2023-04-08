﻿namespace Rxmxnx.PInvoke.Tests.CStringTests.JoinTests;

public sealed class EmptyTest : JoinTestBase
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptyByteTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        Test(GetByteSeparator(), values);
        Test(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptyByteTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        Test(GetByteSeparator(), values);
        Test(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptyByteTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        Test(GetByteSeparator(), values);
        Test(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task LocalEmptyByteTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        await TestAsync(GetByteSeparator(), values);
        await TestAsync(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task ReferenceEmptyByteTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        await TestAsync(GetByteSeparator(), values);
        await TestAsync(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task NullEmptyByteTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        await TestAsync(GetByteSeparator(), values);
        await TestAsync(GetByteSeparator(), values.ToList());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles) is CString cstr ? cstr.AsSpan() : default;
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles) is CString cstr ? cstr.AsSpan() : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles) is CString cstr ? cstr.AsSpan() : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task LocalEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task ReferenceEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task NullEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = TestSet.GetCString(TestSet.GetIndices(1)[0], handles);
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static void Test(Byte separator, CString?[] values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(Byte separator, IEnumerable<CString?> values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(ReadOnlySpan<Byte> separator, CString?[] values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(CString? separator, CString?[] values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(CString? separator, IEnumerable<CString?> values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static async Task TestAsync(Byte separator, CString?[] values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static async Task TestAsync(Byte separator, IEnumerable<CString?> values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static async Task TestAsync(CString? separator, CString?[] values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static async Task TestAsync(CString? separator, IEnumerable<CString?> values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
}
