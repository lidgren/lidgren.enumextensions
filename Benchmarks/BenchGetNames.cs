using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	[MemoryDiagnoser]
	[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
	public class BenchGetNames
	{
		private SimpleEnum m_simpleApple;
		private FlagsEnum m_flagsApple;
		private OffsetDiscontinuousEnum m_offDisApple;

		public BenchGetNames()
		{
			m_simpleApple = SimpleEnum.Apple;
			m_flagsApple = FlagsEnum.Apple;
			m_offDisApple = OffsetDiscontinuousEnum.apple;
		}

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("GetNames")]
		public int Core_SimpleEnum_GetNames()
		{
			return Enum.GetNames<SimpleEnum>().Length;
		}

		[Benchmark]
		[BenchmarkCategory("GetNames")]
		public int EnumsNET_SimpleEnum_GetNames()
		{
			return EnumsNET.Enums.GetNames<SimpleEnum>().Count;
		}

		[Benchmark]
		[BenchmarkCategory("GetNames")]
		public int FastEnum_SimpleEnum_GetNames()
		{
			return FastEnumUtility.FastEnum.GetNames<SimpleEnum>().Count;
		}

		[Benchmark]
		[BenchmarkCategory("GetNames")]
		public int Lidgren_SimpleEnum_GetNames()
		{
			return SimpleEnum.Apple.GetNamesEx().Length;
		}

		//
		// GetValues
		//

		[Benchmark(Baseline = true)]
		[BenchmarkCategory("GetValues")]
		public int Core_SimpleEnum_GetValues()
		{
			return Enum.GetValues<SimpleEnum>().Length;
		}

		[Benchmark]
		[BenchmarkCategory("GetValues")]
		public int EnumsNET_SimpleEnum_GetValues()
		{
			return EnumsNET.Enums.GetValues<SimpleEnum>().Count;
		}

		[Benchmark]
		[BenchmarkCategory("GetValues")]
		public int FastEnum_SimpleEnum_GetValues()
		{
			return FastEnumUtility.FastEnum.GetValues<SimpleEnum>().Count;
		}

		[Benchmark]
		[BenchmarkCategory("GetValues")]
		public int Lidgren_SimpleEnum_GetValues()
		{
			return SimpleEnum.Apple.GetValuesEx().Length;
		}
	}
}
