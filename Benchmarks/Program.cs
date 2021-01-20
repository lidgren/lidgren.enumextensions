using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enums.Net version 3.03");
			Console.WriteLine("FastEnum version 1.6.0");
			Console.WriteLine();

#if DEBUG
			BenchSimple.VerifyCorrectness();
#else
			var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchSimple>();
			Console.WriteLine(summary);
#endif
			Console.WriteLine("Done");
		}
	}

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

	[SimpleJob(RuntimeMoniker.NetCoreApp50)]
	[MemoryDiagnoser]
	public class BenchSimple
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Crash()
		{
			throw new Exception("Assertion failed");
		}

		private static void Assert(bool ok)
		{
			if (!ok)
				Crash();
		}

		[Conditional("DEBUG")]
		public static void VerifyCorrectness()
		{
			// SimpleEnum ToString
			Assert(SimpleEnum.Apple.ToString() == "Apple");
			Assert(EnumsNET.Enums.GetName<SimpleEnum>(SimpleEnum.Apple) == "Apple");
			Assert(FastEnumUtility.FastEnum.GetName<SimpleEnum>(SimpleEnum.Apple) == "Apple");
			Assert(SimpleEnum.Apple.ToStringEx() == "Apple");

			// TryParse
			var ok = Enum.TryParse<SimpleEnum>("Apple", out var value);
			Assert(ok && value == SimpleEnum.Apple);

			ok = EnumsNET.Enums.TryParse<SimpleEnum>("Apple", out value);
			Assert(ok && value == SimpleEnum.Apple);

			ok = FastEnumUtility.FastEnum.TryParse<SimpleEnum>("Apple", out value);
			Assert(ok && value == SimpleEnum.Apple);

			value = default;
			ok = value.TryParseEx("Apple");
			Assert(ok && value == SimpleEnum.Apple);

			// GetNames
			var coreNames = Enum.GetNames<SimpleEnum>();
			var enumsNetNames = EnumsNET.Enums.GetNames<SimpleEnum>();
			var fastEnums = FastEnumUtility.FastEnum.GetNames<SimpleEnum>();
			var lidEnums = SimpleEnum.Apple.GetNamesEx();

			for(int i=0;i<coreNames.Length;i++)
			{
				Assert(coreNames[i] == enumsNetNames[i]);
				Assert(coreNames[i] == fastEnums[i]);
				Assert(coreNames[i] == lidEnums[i]);
			}

			// FlagsEnum
			// ToString
			// single flag set

			var testFlagsEnums = new FlagsEnum[]
			{
				FlagsEnum.None,
				FlagsEnum.Apple,
				FlagsEnum.Apple | FlagsEnum.Book,
				FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo,
				FlagsEnum.All,
			};

			foreach (var fe in testFlagsEnums)
			{
				var facitStr = fe.ToString();

				// tostring

				// EnumsNET.Enums fails on bit flags
				//var en = EnumsNET.Enums.GetName<FlagsEnum>(fe);
				//Assert(en == facitStr);

				// FastEnum fails on bit flags
				//Assert(FastEnumUtility.FastEnum.GetName<FlagsEnum>(fe) == facitStr);

				Assert(fe.ToStringEx() == facitStr);

				// tryparse
				bool fok = EnumsNET.Enums.TryParse<FlagsEnum>(facitStr, out var fvalue);
				Assert(fok == true && fvalue == fe);
				//fok = FastEnumUtility.FastEnum.TryParse<FlagsEnum>(facitStr, out fvalue);
				//Assert(fok == true && fvalue == fe);
				fvalue = default;
				fok = fvalue.TryParseEx(facitStr);
				Assert(fok == true && fvalue == fe);
			}
		}

		//
		// SimpleEnum
		//

		//
		// ToString
		//

		[Benchmark]
		public int Core_SimpleEnum_ToString()
		{
			return SimpleEnum.Apple.ToString().Length;
		}

		[Benchmark]
		public int EnumsNet_SimpleEnum_ToString()
		{
			return EnumsNET.Enums.GetName<SimpleEnum>(SimpleEnum.Apple).Length;
		}

		[Benchmark]
		public int FastEnum_SimpleEnum_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<SimpleEnum>(SimpleEnum.Apple).Length;
		}

		[Benchmark]
		public int Lidgren_SimpleEnum_ToString()
		{
			return SimpleEnum.Apple.ToStringEx().Length;
		}

		//
		// TryParse
		//

		[Benchmark]
		public int Core_SimpleEnum_TryParse()
		{
			var ok = Enum.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int EnumsNet_SimpleEnum_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int FastEnum_SimpleEnum_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<SimpleEnum>("Apple", out var value);
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int Lidgren_SimpleEnum_TryParse()
		{
			SimpleEnum value = default;
			var ok = value.TryParseEx("Apple");
			return (ok ? 1 : 0) + (int)value;
		}

		//
		// GetNames
		//

		[Benchmark]
		public int Core_SimpleEnum_GetNames()
		{
			return Enum.GetNames<SimpleEnum>().Length;
		}

		[Benchmark]
		public int EnumsNET_SimpleEnum_GetNames()
		{
			return EnumsNET.Enums.GetNames<SimpleEnum>().Count;
		}

		[Benchmark]
		public int FastEnum_SimpleEnum_GetNames()
		{
			return FastEnumUtility.FastEnum.GetNames<SimpleEnum>().Count;
		}

		[Benchmark]
		public int Lidgren_SimpleEnum_GetNames()
		{
			return SimpleEnum.Apple.GetNamesEx().Length;
		}

		//
		// FlagsEnum
		//

		//
		// ToString
		//

		// single flag set

		[Benchmark]
		public int Core_FlagsEnum_Single_ToString()
		{
			return (FlagsEnum.Apple).ToString().Length;
		}

		[Benchmark]
		public int EnumsNet_FlagsEnum_Single_ToString()
		{
			return EnumsNET.Enums.GetName<FlagsEnum>(FlagsEnum.Apple).Length;
		}

		[Benchmark]
		public int FastEnum_FlagsEnum_Single_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<FlagsEnum>(FlagsEnum.Apple).Length;
		}

		[Benchmark]
		public int Lidgren_FlagsEnum_Single_ToString()
		{
			return (FlagsEnum.Apple).ToStringEx().Length;
		}

		// multiple flags set

		[Benchmark]
		public int Core_FlagsEnum_Multi_ToString()
		{
			return (FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).ToString().Length;
		}

		[Benchmark]
		public int EnumsNet_FlagsEnum_Multi_ToString()
		{
			throw new Exception("Fails");
			//return EnumsNET.Enums.AsString<FlagsEnum>(FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).Length;
			//return EnumsNET.Enums.GetName<FlagsEnum>(FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).Length;
		}

		[Benchmark]
		public int FastEnum_FlagsEnum_Multi_ToString()
		{
			return FastEnumUtility.FastEnum.GetName<FlagsEnum>(FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).Length;
		}

		[Benchmark]
		public int Lidgren_FlagsEnum_Multi_ToString()
		{
			return (FlagsEnum.Apple | FlagsEnum.Dodo | FlagsEnum.Book).ToStringEx().Length;
		}

		//
		// TryParse
		//

		// single

		[Benchmark]
		public int Core_FlagsEnum_Single_TryParse()
		{
			var ok = Enum.TryParse<FlagsEnum>("Coconut", out var value);
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int EnumsNet_FlagsEnum_Single_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<FlagsEnum>("Coconut", out var value);
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int FastEnum_FlagsEnum_Single_TryParse()
		{
			bool ok = FastEnumUtility.FastEnum.TryParse<FlagsEnum>("Coconut", out var value);
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int Lidgren_FlagsEnum_Single_TryParse()
		{
			FlagsEnum value = default;
			var ok = value.TryParseEx("Coconut");
			if (ok == false || value != (FlagsEnum.Coconut))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		// multiple flags set

		[Benchmark]
		public int Core_FlagsEnum_Multi_TryParse()
		{
			var ok = Enum.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int EnumsNet_FlagsEnum_Multi_TryParse()
		{
			bool ok = EnumsNET.Enums.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int FastEnum_FlagsEnum_Multi_TryParse()
		{
			throw new Exception("Fails");
			bool ok = FastEnumUtility.FastEnum.TryParse<FlagsEnum>("Apple, Coconut, Dodo", out var value);
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int Lidgren_FlagsEnum_Multi_TryParse()
		{
			FlagsEnum value = default;
			var ok = value.TryParseEx("Apple, Coconut, Dodo");
			if (ok == false || value != (FlagsEnum.Apple | FlagsEnum.Coconut | FlagsEnum.Dodo))
				Crash();
			return (ok ? 1 : 0) + (int)value;
		}

		[Benchmark]
		public int Core_SimpleEnum_IsDefined()
		{
			var ok = Enum.IsDefined<SimpleEnum>(SimpleEnum.Dodo);
			return ok ? 0 : 1;
		}

		[Benchmark]
		public int EnumsNet_SimpleEnum_IsDefined()
		{
			var ok = EnumsNET.Enums.IsDefined<SimpleEnum>(SimpleEnum.Dodo);
			return ok ? 0 : 1;
		}

		[Benchmark]
		public int FastEnum_SimpleEnum_IsDefined()
		{
			var ok = FastEnumUtility.FastEnum.IsDefined<SimpleEnum>(SimpleEnum.Dodo);
			return ok ? 0 : 1;
		}

		[Benchmark]
		public int Lidgren_SimpleEnum_IsDefined()
		{
			var ok = (SimpleEnum.Dodo).IsDefinedEx();
			return ok ? 0 : 1;
		}

		// discontinuous
		[Benchmark]
		public int Core_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = Enum.IsDefined<OffsetDiscontinuousEnum>(OffsetDiscontinuousEnum.Dodo);
			return ok ? 0 : 1;
		}
		
		[Benchmark]
		public int EnumsNet_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = EnumsNET.Enums.IsDefined<OffsetDiscontinuousEnum>(OffsetDiscontinuousEnum.Dodo);
			return ok ? 0 : 1;
		}

		[Benchmark]
		public int FastEnum_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = FastEnumUtility.FastEnum.IsDefined<OffsetDiscontinuousEnum>(OffsetDiscontinuousEnum.Dodo);
			return ok ? 0 : 1;
		}

		[Benchmark]
		public int Lidgren_OffsetDiscontinuousEnum_IsDefined()
		{
			var ok = (OffsetDiscontinuousEnum.Dodo).IsDefinedEx();
			return ok ? 0 : 1;
		}
	}
}
