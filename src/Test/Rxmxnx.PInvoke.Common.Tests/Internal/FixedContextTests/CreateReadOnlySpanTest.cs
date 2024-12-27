﻿namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateReadOnlySpanTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => CreateReadOnlySpanTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => CreateReadOnlySpanTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => CreateReadOnlySpanTest.Test<Int16>();
	[Fact]
	internal void CharTest() => CreateReadOnlySpanTest.Test<Char>();
	[Fact]
	internal void Int32Test() => CreateReadOnlySpanTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => CreateReadOnlySpanTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => CreateReadOnlySpanTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => CreateReadOnlySpanTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => CreateReadOnlySpanTest.Test<Single>();
	[Fact]
	internal void HalfTest() => CreateReadOnlySpanTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => CreateReadOnlySpanTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => CreateReadOnlySpanTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => CreateReadOnlySpanTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => CreateReadOnlySpanTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => CreateReadOnlySpanTest.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => CreateReadOnlySpanTest.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => CreateReadOnlySpanTest.Test<String>();

	private static void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>().ToArray();
		FixedContextTestsBase.WithFixed(values, CreateReadOnlySpanTest.Test);
		FixedContextTestsBase.WithFixed(values, CreateReadOnlySpanTest.ReadOnlyTest);
	}

	private static void Test<T>(FixedContext<T> ctx, T[] values)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(values.Length);
		Assert.Equal(values.Length, span.Length);
		Assert.Equal(values.Length, ctx.Count);
		Assert.True(Unsafe.AreSame(ref values[0], in span[0]));
		Assert.False(ctx.IsFunction);

		Exception functionException = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}

	private static void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(values.Length);
		Assert.Equal(values.Length, span.Length);
		Assert.Equal(values.Length, ctx.Count);
		Assert.True(Unsafe.AreSame(ref values[0], in span[0]));
		Assert.False(ctx.IsFunction);

		Exception functionException = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);

		ctx.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => ctx.CreateSpan<T>(values.Length));
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);

		Exception functionException2 = Assert.Throws<InvalidOperationException>(ctx.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException2.Message);
	}
}