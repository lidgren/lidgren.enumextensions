using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using Lidgren.Core;

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
			var fmt = "{0,-35} {1,10} {2,10} {3,10} {4,10}";
			StdOut.WriteLine(string.Format(fmt, "Test", "Core", "EnumsNet", "FastEnum", "Lidgren"), ConsoleColor.Magenta);

			Verify<BenchToString>("SimpleEnum_ToString");
			Verify<BenchToString>("OffsetDiscontinuousEnum_ToString");
			Verify<BenchToString>("FlagsEnum_Single_ToString");
			Verify<BenchToString>("FlagsEnum_Multi_ToString");

			Console.WriteLine();
			Verify<BenchTryParse>("SimpleEnum_TryParse");
			Verify<BenchTryParse>("OffsetDiscontinuousEnum_TryParse");
			Verify<BenchTryParse>("FlagsEnum_Single_TryParse");
			Verify<BenchTryParse>("FlagsEnum_Multi_TryParse");

			Console.WriteLine();
			Verify<BenchIsDefined>("SimpleEnum_IsDefined");
			Verify<BenchIsDefined>("OffsetDiscontinuousEnum_IsDefined");
			Verify<BenchIsDefined>("FlagsEnum_Single_IsDefined");
			//Verify<BenchIsDefined>("FlagsEnum_Multi_IsDefined");

			Console.WriteLine();
			Verify<BenchGetNames>("SimpleEnum_GetNames");
			Verify<BenchGetNames>("SimpleEnum_GetValues");

#else
			//Console.WriteLine("ToString");
			//Console.WriteLine(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchToString>());

			var summaries = new List<Summary>();
			Console.WriteLine();
			summaries.Add(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchGetNames>());
			summaries.Add(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchIsDefined>());
			summaries.Add(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchToString>());
			summaries.Add(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchTryParse>());

			foreach (var sum in summaries)
				Console.WriteLine(sum);

			//Console.WriteLine();
			//Console.WriteLine("TryParse");
			//Console.WriteLine(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchTryParse>());
			//
			//Console.WriteLine();
			//Console.WriteLine("GetNames and GetValues");
			//Console.WriteLine(BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchGetNames>());
#endif
		}

		private static void Verify<T>(string postFix) where T : new()
		{
			StdOut.Write(postFix.PadRight(35) + " ", ConsoleColor.Green);
			PrintMethod<T>("Core_", postFix);
			PrintMethod<T>("EnumsNET_", postFix);
			PrintMethod<T>("FastEnum_", postFix);
			PrintMethod<T>("Lidgren_", postFix);
			StdOut.WriteLine();
		}

		private static void PrintMethod<T>(string prefix, string postFix) where T : new()
		{
			T bench = new T();
			var mi = typeof(T).GetMethod(prefix + postFix);
			if (mi == null)
			{
				StdOut.Write("    !EXISTS", ConsoleColor.Red);
				return;
			}
			try
			{
				var res = mi.Invoke(bench, null);
				StdOut.Write(res.ToString().PadLeft(10) + " ");
			}
			catch { StdOut.Write("  EXCEPTION", ConsoleColor.Red); }
		}
	}
}
