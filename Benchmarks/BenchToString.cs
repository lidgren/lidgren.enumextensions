using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	[MemoryDiagnoser]
	[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
	public class BenchToString
	{
		private SimpleEnum m_simpleApple;
		private FlagsEnum m_flagsApple;
		private OffsetDiscontinuousEnum m_offDisApple;

		public BenchToString()
		{
			m_simpleApple = SimpleEnum.Apple;
			m_flagsApple = FlagsEnum.Apple;
			m_offDisApple = OffsetDiscontinuousEnum.apple;
		}

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("SimpleEnum")]
		public int Core_SimpleEnum_ToString()
		{
			return m_simpleApple.ToString().Length;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int EnumsNET_SimpleEnum_ToString()
		{
			return EnumsNET.Enums.GetName<SimpleEnum>(m_simpleApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int FastEnum_SimpleEnum_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<SimpleEnum>(m_simpleApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int Lidgren_SimpleEnum_ToString()
		{
			return m_simpleApple.ToStringEx().Length;
		}

		// offset discontinuous

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Core_OffsetDiscontinuousEnum_ToString()
		{
			return m_offDisApple.ToString().Length;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int EnumsNET_OffsetDiscontinuousEnum_ToString()
		{
			return EnumsNET.Enums.GetName<OffsetDiscontinuousEnum>(m_offDisApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int FastEnum_OffsetDiscontinuousEnum_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<OffsetDiscontinuousEnum>(m_offDisApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Lidgren_OffsetDiscontinuousEnum_ToString()
		{
			return m_offDisApple.ToStringEx().Length;
		}

		// single flag set

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Core_FlagsEnum_Single_ToString()
		{
			return (m_flagsApple).ToString().Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int EnumsNET_FlagsEnum_Single_ToString()
		{
			return EnumsNET.Enums.GetName<FlagsEnum>(m_flagsApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int FastEnum_FlagsEnum_Single_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<FlagsEnum>(m_flagsApple).Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Lidgren_FlagsEnum_Single_ToString()
		{
			return m_flagsApple.ToStringEx().Length;
		}

		// multiple flags set

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int Core_FlagsEnum_Multi_ToString()
		{
			return (FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).ToString().Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int EnumsNET_FlagsEnum_Multi_ToString()
		{
			return EnumsNET.Enums.GetName<FlagsEnum>(FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int FastEnum_FlagsEnum_Multi_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<FlagsEnum>(FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).Length;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Multi")]
		public int Lidgren_FlagsEnum_Multi_ToString()
		{
			return (FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).ToStringEx().Length;
		}
	}
}
