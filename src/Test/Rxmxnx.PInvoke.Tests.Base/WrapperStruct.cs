namespace Rxmxnx.PInvoke.Tests;

[StructLayout(LayoutKind.Sequential)]
public readonly struct WrapperStruct<T>
{
	public T Value { get; init; }
}