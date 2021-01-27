using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lidgren.CoreInl;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using CodingSeb.ExpressionEvaluator;

namespace Lidgren.EnumExtensions
{
	[Generator]
	public sealed partial class EnumExGenerator : ISourceGenerator
	{
		private static ExpressionEvaluator s_evaluator = new ExpressionEvaluator();

		public void Initialize(GeneratorInitializationContext context)
		{
			context.RegisterForSyntaxNotifications(() => new EnumExSyntaxReceiver());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var receiver = context.SyntaxReceiver as EnumExSyntaxReceiver;
			if (receiver == null)
				return;

			var strb = new ValueStringBuilder(1024);

			var groups = receiver.Instances.GroupBy((x) => x.Namespace);
			foreach (var pair in groups)
			{
				var ns = pair.Key;
				var instances = pair.Value.ToArray();

				strb.Clear();
				strb.AppendLine("using System;");
				strb.AppendLine("using System.Runtime.CompilerServices;");
				strb.AppendLine();

				strb.AppendLine("// preprocessor symbols:");
				foreach (var symbol in context.ParseOptions.PreprocessorSymbolNames)
					strb.AppendLine("// " + symbol);
				strb.AppendLine();

				strb.AppendLine($"namespace {ns}");
				strb.AppendLine("{");

				//				strb.AppendLine(
				//@"		private ref struct ConcatenateContext
				//		{
				//			public int NumFlags;
				//			public Span<int> FlagIndices;
				//			public string[] NamesForFlagsEnum;
				//		}
				//");

				for (int i = 0; i < instances.Length; i++)
				{
					ref var ins = ref instances[i];

					strb.AppendLine($"\tpublic static class {ins.EnumName}_EnumExtensions");
					strb.AppendLine("\t{");

					try
					{
						AnalyzeEnumValues(ref ins);
						GenerateInstance(ref strb, ins);
					}
					catch (Exception ex)
					{
						strb.AppendLine("#error " + ex.ToString());
					}
					strb.AppendLine("\t\tprivate static void ThrowException() { throw new Exception(); }");
					strb.AppendLine("\t}"); // class
				}
				strb.AppendLine("}"); // ns

				context.AddSource(ns + ".EnumExtensions", SourceText.From(strb.ToString(), Encoding.UTF8));
			}
		}

		private void GenerateInstance(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			strb.AppendLine("\t\t//");
			strb.AppendLine("\t\t// " + ins.EnumName);
			strb.AppendLine("\t\t//");
			GenerateGetNamesEx(ref strb, ins);
			GenerateGetValuesEx(ref strb, ins);
			GenerateTryParseEx(ref strb, ins);
			GenerateToStringEx(ref strb, ins);
			GenerateIsDefinedEx(ref strb, ins);
		}

		private void AnalyzeEnumValues(ref EnumInstance ins)
		{
			long nextNumVal = 0;

			ins.MemberNumericValues = new long[ins.MemberNames.Count];
			ins.IsContinuous = true;
			for (int i = 0; i < ins.MemberValues.Count; i++)
			{
				var val = ins.MemberValues[i];

				// make numeric value
				long numVal;
				if (string.IsNullOrWhiteSpace(val))
				{
					numVal = nextNumVal;
					nextNumVal++;
				}
				else
				{
					numVal = ParseValue(val, ins.UnderlyingType, ins);
					nextNumVal = numVal + 1;
				}

				ins.MemberNumericValues[i] = numVal;

				if (val == null)
					continue;

				if (i != 0)
					ins.IsContinuous = false;
			}

			// sort values and names by integer value
			var mapping = new int[ins.MemberNumericValues.Length];
			for (int i = 0; i < mapping.Length; i++)
				mapping[i] = i;

			var insCopy = ins;
			Array.Sort(mapping, (a, b) =>
			{
				return Comparer<Int64>.Default.Compare(insCopy.MemberNumericValues[a], insCopy.MemberNumericValues[b]);
			});

			var newNames = new List<string>(ins.MemberNames.Count);
			var newValues = new List<string>(ins.MemberValues.Count);
			var newNumeric = new List<long>(ins.MemberNumericValues.Length);
			for (int i = 0; i < insCopy.MemberNumericValues.Length; i++)
			{
				newNames.Add(ins.MemberNames[mapping[i]]);
				newValues.Add(ins.MemberValues[mapping[i]]);
				newNumeric.Add(ins.MemberNumericValues[mapping[i]]);
			}
			ins.MemberNames = newNames;
			ins.MemberValues = newValues;
			ins.MemberNumericValues = newNumeric.ToArray();
		}

		private long ParseValue(string value, string underlyingType, in EnumInstance ins)
		{
			// all numeric or not?
			foreach (var c in value.AsSpan().Trim())
				if (char.IsDigit(c) == false)
					return ParseExpression(value, ins);

			switch (underlyingType)
			{
				case "byte":
					return byte.Parse(value);
				case "Int16":
				case "short":
					return Int16.Parse(value);
				case "UInt16":
				case "ushort":
					return UInt16.Parse(value);
				case "Int32":
				case "int":
					return Int32.Parse(value);
				case "UInt32":
				case "uint":
					return UInt32.Parse(value);
				case "Int64":
				case "long":
					return Int64.Parse(value);
				case "UInt64":
				case "ulong":
					return (long)UInt64.Parse(value);
				default:
					throw new Exception("Unsupported underlying type: " + underlyingType);
			}
		}

		private long ParseExpression(string value, in EnumInstance ins)
		{
			// replace all known enum values
			for (int i = 0; i < ins.MemberNames.Count; i++)
			{
				var name = ins.MemberNames[i];
				value = value.Replace(name, ins.MemberNumericValues[i].ToString());
			}

			// run expression
			var retval = s_evaluator.Evaluate(value).ToString();
			return Int64.Parse(retval, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
