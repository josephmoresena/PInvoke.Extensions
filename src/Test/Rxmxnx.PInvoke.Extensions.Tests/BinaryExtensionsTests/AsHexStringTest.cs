﻿namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class AsHexStringTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void NormalTest()
	{
		Byte[] input = AsHexStringTest.fixture.Create<Byte[]>();
		StringBuilder strBuild = new();
		foreach (Byte value in input)
		{
			Assert.Equal(value.ToString("X2").ToLowerInvariant(), value.AsHexString());
			strBuild.Append(value.ToString("X2"));
		}
		Assert.Equal(strBuild.ToString().ToLowerInvariant(), input.AsHexString());
	}
}