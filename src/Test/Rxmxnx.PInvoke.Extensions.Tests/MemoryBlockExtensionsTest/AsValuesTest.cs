﻿namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsValuesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => AsValuesTest.Test<Byte>();
	[Fact]
	internal void CharTest() => AsValuesTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => AsValuesTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => AsValuesTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => AsValuesTest.Test<Double>();
	[Fact]
	internal void GuidTest() => AsValuesTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => AsValuesTest.Test<Half>();
	[Fact]
	internal void Int16Test() => AsValuesTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => AsValuesTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => AsValuesTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => AsValuesTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => AsValuesTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => AsValuesTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => AsValuesTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => AsValuesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T[] values = AsValuesTest.fixture.CreateMany<T>(10).ToArray();
		AsValuesTest.Test<T, Byte>(values);
		AsValuesTest.Test<T, Char>(values);
		AsValuesTest.Test<T, DateTime>(values);
		AsValuesTest.Test<T, Decimal>(values);
		AsValuesTest.Test<T, Double>(values);
		AsValuesTest.Test<T, Guid>(values);
		AsValuesTest.Test<T, Half>(values);
		AsValuesTest.Test<T, Int16>(values);
		AsValuesTest.Test<T, Int32>(values);
		AsValuesTest.Test<T, Int64>(values);
		AsValuesTest.Test<T, SByte>(values);
		AsValuesTest.Test<T, Single>(values);
		AsValuesTest.Test<T, UInt16>(values);
		AsValuesTest.Test<T, UInt32>(values);
		AsValuesTest.Test<T, UInt64>(values);
	}
	private static unsafe void Test<T, T2>(T[] values) where T : unmanaged where T2 : unmanaged
	{
		Span<T> span = values;
		ReadOnlySpan<T> readOnlySpan = values;
		Span<Byte> byteSpan = MemoryMarshal.AsBytes(span);
		Span<T2> spanT2 = MemoryMarshal.Cast<Byte, T2>(byteSpan);
		Span<Byte> spanResidual = byteSpan[(spanT2.Length * sizeof(T2))..];

		T2[] valuesT2 = spanT2.ToArray();
		Byte[] resiudalBytes = spanResidual.ToArray();

		Assert.Equal(valuesT2, span.AsValues<T, T2>().ToArray());
		Assert.Equal(valuesT2, readOnlySpan.AsValues<T, T2>().ToArray());
		Assert.Equal(valuesT2, span.AsValues<T, T2>(out Span<Byte> residual).ToArray());
		Assert.Equal(resiudalBytes, residual.ToArray());
		Assert.Equal(valuesT2, span.AsValues<T, T2>(out ReadOnlySpan<Byte> residualRo).ToArray());
		Assert.Equal(resiudalBytes, residualRo.ToArray());
		Assert.Equal(valuesT2, readOnlySpan.AsValues<T, T2>(out ReadOnlySpan<Byte> residualRo2).ToArray());
		Assert.Equal(resiudalBytes, residualRo2.ToArray());

		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual),
		                           ref MemoryMarshal.GetReference(residual)));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual),
		                           ref MemoryMarshal.GetReference(residualRo)));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanResidual),
		                           ref MemoryMarshal.GetReference(residualRo2)));

		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanT2),
		                           ref MemoryMarshal.GetReference(span.AsValues<T, T2>())));
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(spanT2),
		                           ref MemoryMarshal.GetReference(readOnlySpan.AsValues<T, T2>())));
	}
}