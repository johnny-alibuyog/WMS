namespace AmpedBiz.Common.CustomTypes
{

	public struct Prop<T>
	{
		public readonly static Prop<T> Empty = new Prop<T>(default(T));

		private readonly T _value;

		private readonly bool _hasValue;

		private Prop(T value)
		{
			this._hasValue = true;
			this._value = value;
		}

		public T Value(T def)
		{
			return this._hasValue ? this._value : def;
		}

		public static implicit operator Prop<T>(T value)
		{
			return new Prop<T>(value);
		}
	}
}
