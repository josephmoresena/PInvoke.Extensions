﻿namespace Rxmxnx.PInvoke.Tests.Internal.Utf8ComparatorTests;

[ExcludeFromCodeCoverage]
public sealed class InvalidTest
{
	private static readonly CStringUtf8Comparator cstrComparator =
		CStringUtf8Comparator.Create(StringComparison.InvariantCulture);
	private static readonly StringUtf8Comparator strComparator =
		StringUtf8Comparator.Create(StringComparison.InvariantCulture);

	[Theory]
	[InlineData("Hello")]
	[InlineData("Привет")]
	[InlineData("こんにちは")]
	internal void Test(String str)
	{
		CString cstr = (CString)str;
		CString invalidCStr = InvalidTest.GetInvalidCstr(cstr);
		String invalidStr = invalidCStr.ToString();

		Int32 result = String.Compare(str, invalidStr, StringComparison.InvariantCulture);

		Assert.Equal(result, InvalidTest.cstrComparator.Compare(cstr, invalidCStr));
		Assert.Equal(result, InvalidTest.strComparator.Compare(cstr, invalidStr));

		Assert.Equal(-result, InvalidTest.cstrComparator.Compare(invalidCStr, cstr));
		Assert.Equal(-result, InvalidTest.strComparator.Compare(invalidCStr, str));

		Assert.Equal(0, InvalidTest.cstrComparator.Compare(invalidCStr, invalidCStr));
		Assert.Equal(0, InvalidTest.strComparator.Compare(invalidCStr, invalidStr));

		Assert.Equal(result == 0, InvalidTest.cstrComparator.TextEquals(cstr, invalidCStr));
		Assert.Equal(result == 0, InvalidTest.strComparator.TextEquals(cstr, invalidStr));

		Assert.True(InvalidTest.cstrComparator.TextEquals(invalidCStr, invalidCStr));
		Assert.True(InvalidTest.strComparator.TextEquals(invalidCStr, invalidStr));
	}

	private static CString GetInvalidCstr(CString cstr)
	{
		Byte[] utf8Byte = cstr.ToArray();
		for (Int32 i = 0; i < utf8Byte.Length; i++)
			utf8Byte[i] = Random.Shared.Next(0, 2) == 0 ? utf8Byte[i] : InvalidTest.GetInvalidByte();
		return utf8Byte;
	}
	private static Byte GetInvalidByte()
	{
		Int32 result = Random.Shared.Next(0x80, 0xC0);
		return (Byte)result;
	}
}