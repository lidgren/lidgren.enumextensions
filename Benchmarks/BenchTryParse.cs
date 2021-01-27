using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	[MemoryDiagnoser]
	[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
	public class BenchTryParse
	{
		private SimpleEnum m_simpleApple;
		private FlagsEnum m_flagsApple;
		private OffsetDiscontinuousEnum m_offDisApple;

		public BenchTryParse()
		{
			m_simpleApple = SimpleEnum.Apple;
			m_flagsApple = FlagsEnum.Apple;
			m_offDisApple = OffsetDiscontinuousEnum.apple;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Crash()
		{
			throw new Exception("Assertion failed");
		}

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("SimpleEnum")]
		public int Core_SimpleEnum_TryParse()
		{
			var ok = Enum.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int EnumsNET_SimpleEnum_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int FastEnum_SimpleEnum_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int Lidgren_SimpleEnum_TryParse()
		{
			SimpleEnum value = default;
			var ok = value.TryParseEx("Apple");
			return (ok ? 1 : 0) + (int)value;
		}

		// offdist

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Core_OffsetDiscontinuousEnum_TryParse()
		{
			var ok = Enum.TryParse<OffsetDiscontinuousEnum>("coconut", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int EnumsNET_OffsetDiscontinuousEnum_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<OffsetDiscontinuousEnum>("coconut", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int FastEnum_OffsetDiscontinuousEnum_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<OffsetDiscontinuousEnum>("coconut", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Lidgren_OffsetDiscontinuousEnum_TryParse()
		{
			OffsetDiscontinuousEnum value = default;
			var ok = value.TryParseEx("coconut");
			return (ok ? 1 : 0) + (int)value;
		}

		// single flag

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Core_FlagsEnum_Single_TryParse()
		{
			var ok = Enum.TryParse<FlagsEnum>("Coconut", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int EnumsNET_FlagsEnum_Single_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<FlagsEnum>("Coconut", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int FastEnum_FlagsEnum_Single_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<FlagsEnum>("Coconut", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Lidgren_FlagsEnum_Single_TryParse()
		{
			FlagsEnum value = default;
			var ok = value.TryParseEx("Coconut");
#if DEBUG
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		// multiple flags set

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int Core_FlagsEnum_Multi_TryParse()
		{
			var ok = Enum.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int EnumsNET_FlagsEnum_Multi_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int FastEnum_FlagsEnum_Multi_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
#if DEBUG
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int Lidgren_FlagsEnum_Multi_TryParse()
		{
			FlagsEnum value = default;
			var ok = value.TryParseEx("Apple, Coconut, Dodo");
#if DEBUG
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
#endif
			return (ok ? 1 : 0) + (int)value;
		}
	}
}
