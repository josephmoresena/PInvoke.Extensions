namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed unsafe class MultipleAllocTest
{
	[Fact]
	internal void BooleanTest() => MultipleAllocTest.MultipleAlloc<Boolean>();
	[Fact]
	internal void ByteTest() => MultipleAllocTest.MultipleAlloc<Byte>();
	[Fact]
	internal void Int16Test() => MultipleAllocTest.MultipleAlloc<Int16>();
	[Fact]
	internal void Int32Test() => MultipleAllocTest.MultipleAlloc<Int32>();
	[Fact]
	internal void Int64Test() => MultipleAllocTest.MultipleAlloc<Int64>();

	[Fact]
	internal void NullableBooleanTest() => MultipleAllocTest.MultipleAlloc<Boolean?>();
	[Fact]
	internal void NullableByteTest() => MultipleAllocTest.MultipleAlloc<Byte?>();
	[Fact]
	internal void NullableInt16Test() => MultipleAllocTest.MultipleAlloc<Int16?>();
	[Fact]
	internal void NullableInt32Test() => MultipleAllocTest.MultipleAlloc<Int32?>();
	[Fact]
	internal void NullableInt64Test() => MultipleAllocTest.MultipleAlloc<Int64?>();

	[Fact]
	internal void BooleanArrayTest() => MultipleAllocTest.MultipleAlloc<Boolean[]?>();
	[Fact]
	internal void ByteArrayTest() => MultipleAllocTest.MultipleAlloc<Byte[]?>();
	[Fact]
	internal void Int16ArrayTest() => MultipleAllocTest.MultipleAlloc<Int16[]?>();
	[Fact]
	internal void Int32ArrayTest() => MultipleAllocTest.MultipleAlloc<Int32[]?>();
	[Fact]
	internal void Int64ArrayTest() => MultipleAllocTest.MultipleAlloc<Int64[]?>();
	[Fact]
	internal void StringTest() => MultipleAllocTest.MultipleAlloc<String?>();

	[Fact]
	internal void BooleanWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean>>();
	[Fact]
	internal void ByteWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte>>();
	[Fact]
	internal void Int16WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16>>();
	[Fact]
	internal void Int32WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32>>();
	[Fact]
	internal void Int64WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64>>();

	[Fact]
	internal void NullableBooleanWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean?>>();
	[Fact]
	internal void NullableByteWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte?>>();
	[Fact]
	internal void NullableInt16WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16?>>();
	[Fact]
	internal void NullableInt32WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32?>>();
	[Fact]
	internal void NullableInt64WrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64?>>();

	[Fact]
	internal void BooleanArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Boolean[]?>>();
	[Fact]
	internal void ByteArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Byte[]?>>();
	[Fact]
	internal void Int16ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int16[]?>>();
	[Fact]
	internal void Int32ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int32[]?>>();
	[Fact]
	internal void Int64ArrayWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<Int64[]?>>();
	[Fact]
	internal void StringWrapperTest() => MultipleAllocTest.MultipleAlloc<WrapperStruct<String?>>();

	private static void MultipleAlloc<T>()
	{
		UInt16 count = (UInt16)(Math.Pow(2, Random.Shared.Next(2, 4)) - 1);
		Span<IntPtr> span0 = stackalloc IntPtr[5];
		span0[0] = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span0));
		BufferManager.Alloc<T>(count, MultipleAllocTest.Do);
		BufferManager.Alloc<T, IntPtr>(count, in span0[0], MultipleAllocTest.Do);
		Boolean inStack = BufferManager.Alloc<T, Boolean>(count, MultipleAllocTest.Get);
		Assert.Equal(default, BufferManager.Alloc<T, IntPtr, T>(count, in span0[0], MultipleAllocTest.Get));
		Assert.True(inStack);

		Exception exception = new();

		Assert.Equal(exception, Assert.ThrowsAny<Exception>(() => BufferManager.Alloc<T>(count, ThrowDo)));
		Assert.Equal(
			exception,
			Assert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, Exception>(count, exception, ThrowDoEx)));
		Assert.Equal(exception, Assert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, T>(count, ThrowGet)));
		Assert.Equal(
			exception,
			Assert.ThrowsAny<Exception>(() => BufferManager.Alloc<T, Exception, T>(count, exception, ThrowGetEx)));
		return;

		void ThrowDo(ScopedBuffer<T> buffer) => throw exception;
		void ThrowDoEx(ScopedBuffer<T> buffer, in Exception ex) => throw ex;
		T ThrowGet(ScopedBuffer<T> buffer) => throw exception;
		T ThrowGetEx(ScopedBuffer<T> buffer, in Exception ex) => throw ex;
	}
	private static void Do<T>(ScopedBuffer<T> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.InRange(buffer.Span.Length, 2, Math.Pow(2, 4));
		Assert.InRange(buffer.FullLength, 2, Math.Pow(2, 4));
		Assert.Equal(default, buffer.Span[0]);
	}
	private static void Do<T>(ScopedBuffer<T> buffer, in IntPtr ptr)
	{
		Assert.True(buffer.InStack);
		Assert.InRange(buffer.Span.Length, 2, Math.Pow(2, 4));
		Assert.InRange(buffer.FullLength, 2, Math.Pow(2, 4));
		Assert.Equal(default, buffer.Span[0]);
		Assert.True(Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in ptr)) == ptr.ToPointer());
	}
	private static Boolean Get<T>(ScopedBuffer<T> buffer)
	{
		Assert.True(buffer.InStack);
		Assert.InRange(buffer.Span.Length, 2, Math.Pow(2, 4));
		Assert.InRange(buffer.FullLength, 2, Math.Pow(2, 4));
		Assert.Equal(default, buffer.Span[0]);
		return buffer.InStack;
	}
	private static T Get<T>(ScopedBuffer<T> buffer, in IntPtr ptr)
	{
		MultipleAllocTest.Do(buffer, in ptr);
		return buffer.Span[0];
	}
}