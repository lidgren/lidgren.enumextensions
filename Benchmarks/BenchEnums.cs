using System;

namespace Benchmarks
{
	public enum SimpleEnum
	{
		Apple,
		Banana,
		Coconut,
		Dodo,
		Eiffel,
	}

	public enum OffsetDiscontinuousEnum
	{
		apple = 5,
		book = 10,
		Dodo = 16,
		coconut = 20,
	}

	[Flags]
	public enum FlagsEnum
	{
		None = 0,
		Apple = 1 << 0,
		Book = 1 << 1,

		AppleAndBook = Apple | Book,

		Coconut = 1 << 2,
		Dodo = 1 << 3,

		All = Apple | Book | Coconut | Dodo
	}
}
