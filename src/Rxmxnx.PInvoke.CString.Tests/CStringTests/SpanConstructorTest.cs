﻿using System.Threading.Tasks;

namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class SpanConstructorTest
{
    [Fact]
    internal async Task TestAsync()
    {
        Int32 lenght = TestSet.Utf16Text.Count;
        CString[,] cstr = new CString[3, lenght];
        Task task1 = Task.Run(() => CreateCStringFromFunction(cstr));
        Task task2 = Task.Run(() => CreateCStringFromBytes(cstr));
        Task task3 = Task.Run(() => CreateCStringFromNullTerminatedBytes(cstr));

        await Task.WhenAll(task1, task2, task3);
        for (Int32 i = 0; i < lenght; i++)
        {
            CString cstr1 = cstr[0, i];
            for (Int32 j = 1; j < 3; j++)
            {
                CString cstr2 = cstr[j, i];
                Assert.Equal(cstr1, cstr2);
                switch (j)
                {
                    case 1:
                        AssertFromBytes(cstr2);
                        break;
                    case 2:
                        AssertFromNullTerminatedBytes(cstr2);
                        break;
                }
                Assert.True(cstr2.Equals(TestSet.Utf16Text[i]));
                Assert.True(cstr2.Equals((Object)TestSet.Utf16Text[i]));
                Assert.True(cstr2.Equals((Object)cstr1));
                Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
                Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
                Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
                Assert.Equal(0, cstr1.CompareTo(cstr2));
            }
            AssertFromFunction(cstr1);
            Assert.True(cstr1.Equals(TestSet.Utf16Text[i]));
            Assert.Equal(0, cstr1.CompareTo(TestSet.Utf16Text[i]));
            Assert.Equal(TestSet.Utf16Text[i].GetHashCode(), cstr1.GetHashCode());
        }
    }

    private void AssertFromFunction(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
    }
    private void AssertFromBytes(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
    }
    private void AssertFromNullTerminatedBytes(CString cstr)
    {
        Assert.False(cstr.IsFunction);
        Assert.True(cstr.IsNullTerminated);
        Assert.False(cstr.IsReference);
        Assert.False(cstr.IsSegmented);
        Assert.False(CString.IsNullOrEmpty(cstr));
        Assert.Equal(cstr.Length + 2, CString.GetBytes(cstr).Length);
    }
    private static void CreateCStringFromFunction(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
            cstr[0, i] = new(TestSet.Utf8Text[i]());
    }
    private static void CreateCStringFromBytes(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
            cstr[1, i] = new(TestSet.Utf8Bytes[i]);
    }
    private static void CreateCStringFromNullTerminatedBytes(CString[,] cstr)
    {
        for (Int32 i = 0; i < TestSet.Utf8NullTerminatedBytes.Count; i++)
            cstr[2, i] = new(TestSet.Utf8NullTerminatedBytes[i]);
    }
}
