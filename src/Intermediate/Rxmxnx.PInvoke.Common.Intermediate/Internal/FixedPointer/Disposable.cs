namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedPointer
{
	/// <summary>
	/// Helper class for managing fixed memory pointer blocks in a disposable context.
	/// </summary>
	/// <typeparam name="TFixed">A <see cref="FixedPointer"/> type.</typeparam>
	protected abstract record Disposable<TFixed> : IWrapper<TFixed>, IDisposable where TFixed : FixedPointer
	{
		/// <summary>
		/// Internal <see cref="IDisposable"/> instance.
		/// </summary>
		private readonly IDisposable? _disposable;
		/// <inheritdoc cref="Disposable{TFixed}.Value"/>
		private readonly TFixed _fixed;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="fixedPointer">Current <typeparamref name="TFixed"/> instance.</param>
		/// <param name="disposable"><see cref="IDisposable"/> instance.</param>
		protected Disposable(TFixed fixedPointer, IDisposable? disposable)
		{
			this._fixed = fixedPointer;
			this._disposable = disposable;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <inheritdoc/>
		public TFixed Value => this._fixed;

		~Disposable() { this.Dispose(false); }

		/// <inheritdoc cref="IDisposable.Dispose()"/>
		/// <param name="disposing">Indicates whether current call is from <see cref="IDisposable.Dispose()"/>.</param>
		protected virtual void Dispose(Boolean disposing)
		{
			if (!this._fixed.IsValid || (!disposing && this._disposable is IFixedPointer.IDisposable)) return;
			this._fixed.Unload();
			this._disposable?.Dispose();
		}
	}
}