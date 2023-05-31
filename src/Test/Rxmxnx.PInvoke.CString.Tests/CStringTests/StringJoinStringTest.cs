﻿namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class StringJoinStringTest
{
    [Fact]
    internal void Test()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        ArrayTest(GetCStringSeparator(), strings);
        ArrayRangeTest(GetCStringSeparator(), strings);
        EnumerableTest(GetCStringSeparator(), strings);
    }

    [Fact]
    internal async Task TestAsync()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        await ArrayTestAsync(GetCStringSeparator(), strings);
        await ArrayRangeTestAsync(GetCStringSeparator(), strings);
        await EnumerableTestAsync(GetCStringSeparator(), strings);
    }

    private static void ArrayTest(String? separator, String?[] strings)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void EnumerableTest(String? separator, IEnumerable<String?> strings)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void ArrayRangeTest(String? separator, String?[] strings)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }

    private static async Task ArrayTestAsync(String? separator, String?[] strings)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task EnumerableTestAsync(String? separator, IEnumerable<String?> strings)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task ArrayRangeTestAsync(String? separator, String?[] strings)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static String? GetCStringSeparator()
    {
        Int32 result = Random.Shared.Next(-3, TestSet.Utf16Text.Count);
        return TestSet.GetString(result);
    }
}
