using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	[MemoryDiagnoser]
	[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
	public class BenchIsDefined
	{
		private SimpleEnum m_simpleApple;
		private FlagsEnum m_flagsApple;
		private OffsetDiscontinuousEnum m_offDisApple;

		public BenchIsDefined()
		{
			m_simpleApple = SimpleEnum.Apple;
			m_flagsApple = FlagsEnum.Apple;
			m_offDisApple = OffsetDiscontinuousEnum.apple;
		}

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("SimpleEnum")]
		public int Core_SimpleEnum_IsDefined()
		{
			var ok = Enum.IsDefined<SimpleEnum>(m_simpleApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int EnumsNET_SimpleEnum_IsDefined()
		{
			var ok = EnumsNET.Enums.IsDefined<SimpleEnum>(m_simpleApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int FastEnum_SimpleEnum_IsDefined()
		{
			var ok = FastEnumUtility.FastEnum.IsDefined<SimpleEnum>(m_simpleApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("SimpleEnum")]
		public int Lidgren_SimpleEnum_IsDefined()
		{
			var ok = m_simpleApple.IsDefinedEx();
			return ok ? 0 : 1;
		}

		// discontinuous

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Core_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = Enum.IsDefined<OffsetDiscontinuousEnum>(m_offDisApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int EnumsNET_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = EnumsNET.Enums.IsDefined<OffsetDiscontinuousEnum>(m_offDisApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int FastEnum_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = FastEnumUtility.FastEnum.IsDefined<OffsetDiscontinuousEnum>(m_offDisApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("OffsetDiscontinuousEnum")]
		public int Lidgren_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = (m_offDisApple).IsDefinedEx();
			return ok ? 0 : 1;
		}

		// flags

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Core_FlagsEnum_Single_IsDefined()
		{
			var ok = Enum.IsDefined<FlagsEnum>(m_flagsApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int EnumsNET_FlagsEnum_Single_IsDefined()
		{
			var ok = EnumsNET.Enums.IsDefined<FlagsEnum>(m_flagsApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int FastEnum_FlagsEnum_Single_IsDefined()
		{
			var ok = FastEnumUtility.FastEnum.IsDefined<FlagsEnum>(m_flagsApple);
			return ok ? 0 : 1;
		}

		[Benchmark]
		[BenchmarkCategory("FlagsEnum_Single")]
		public int Lidgren_FlagsEnum_Single_IsDefined()
		{
			var ok = m_flagsApple.IsDefinedEx();
			return ok ? 0 : 1;
		}
	}
}
