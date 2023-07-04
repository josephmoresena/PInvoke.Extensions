﻿namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class JoinByteTest
{
	[Fact]
	internal void Test()
	{
		List<GCHandle> handles = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		try
		{
			CString?[] values = new CString[strings.Length];
			for (Int32 i = 0; i < values.Length; i++)
				values[i] = TestSet.GetCString(indices[i], handles);

			JoinByteTest.ArrayTest(JoinByteTest.GetByteSeparator(), strings, values);
			JoinByteTest.ArrayRangeTest(JoinByteTest.GetByteSeparator(), strings, values);
			JoinByteTest.EnumerableTest(JoinByteTest.GetByteSeparator(), strings, values);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	[Fact]
	internal async Task TestAsync()
	{
		List<GCHandle> handles = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
		try
		{
			CString?[] values = new CString[strings.Length];
			for (Int32 i = 0; i < values.Length; i++)
				values[i] = TestSet.GetCString(indices[i], handles);

			await JoinByteTest.ArrayTestAsync(JoinByteTest.GetByteSeparator(), strings, values);
			await JoinByteTest.ArrayRangeTestAsync(JoinByteTest.GetByteSeparator(), strings, values);
			await JoinByteTest.EnumerableTestAsync(JoinByteTest.GetByteSeparator(), strings, values);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	private static void ArrayTest(Byte separator, String?[] strings, CString?[] values)
	{
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		Assert.Same(CString.Empty, CString.Join(separator, Array.Empty<CString?>()));
	}
	private static void EnumerableTest(Byte separator, String?[] strings, IEnumerable<CString?> values)
	{
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		Assert.Same(CString.Empty, CString.Join(separator, Array.Empty<CString?>().ToList()));
	}
	private static void ArrayRangeTest(Byte separator, String?[] strings, CString?[] values)
	{
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = CString.Join(separator, values, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
		Assert.Same(CString.Empty, CString.Join(separator, values, 0, 0));
	}

	private static async Task ArrayTestAsync(Byte separator, String?[] strings, CString?[] values)
	{
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task EnumerableTestAsync(Byte separator, String?[] strings, IEnumerable<CString?> values)
	{
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static async Task ArrayRangeTestAsync(Byte separator, String?[] strings, CString?[] values)
	{
		Int32 startIndex = Random.Shared.Next(0, strings.Length);
		Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
		String strSeparator = Encoding.UTF8.GetString(new[] { separator, });
		String expectedCString = String.Join(strSeparator, strings, startIndex, count);
		Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

		CString resultCString = await CString.JoinAsync(separator, values, startIndex, count);
		String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[..^1]);

		Assert.Equal(expectedCString, resultCStringCString);
		Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[..^1]);
	}
	private static Byte GetByteSeparator()
	{
		Int32 result = Random.Shared.Next(33, 127);
		return (Byte)result;
	}
}