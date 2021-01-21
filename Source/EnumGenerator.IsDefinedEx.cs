using System;
using System.Text;
using Lidgren.CoreInl;

namespace Lidgren.EnumExtensions
{
	public sealed partial class EnumExGenerator
	{
		private void GenerateIsDefinedEx(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			if (ins.MemberNumericValues.Length == 0)
			{
				// nothing is ever defined!
				strb.AppendLine($"		public static bool IsDefinedEx(this {ins.EnumName} value) => false;");
				return;
			}

			if (ins.MemberNumericValues.Length == 1)
			{
				strb.AppendLine(
					$"		public static bool IsDefinedEx(this {ins.EnumName} value) => value == {ins.EnumName}.{ins.MemberNames[0]};");
				return;
			}

			if (ins.IsContinuous && !ins.IsFlags)
			{
				// simple check
				strb.AppendLine(
$@"		public static bool IsDefinedEx(this {ins.EnumName} value)
		{{
			return value >= {ins.EnumName}.{ins.MemberNames[0]} && value <= {ins.EnumName}.{ins.MemberNames[ins.MemberNumericValues.Length - 1]};
		}}
");
				return;
			}

			// fall back
			//strb.AppendLine($"		public static bool IsDefined(this {ins.EnumName} value) => Array.BinarySearch<{ins.UnderlyingType}>(s_numericValuesFor{ins.EnumName}, ({ins.UnderlyingType})value) >= 0;");

			/*
			strb.AppendLine(
$@"		public static bool IsDefined(this {ins.EnumName} value)
		{{
			{ins.UnderlyingType} num = ({ins.UnderlyingType})value;
			for(int i=0;i<s_numericValuesFor{ins.EnumName}.Length;i++)
				if (s_numericValuesFor{ins.EnumName}[i] == num)
					return true;
			return false;
		}}
");
			*/
			strb.AppendLine(
$@"		public static bool IsDefinedEx(this {ins.EnumName} value)
		{{
			switch(value) {{");
			foreach (var name in ins.MemberNames)
				strb.AppendLine($"\t\t\t\tcase {ins.EnumName}.{name}:");
			strb.AppendLine(
@"					return true;
				default:
					return false;
			}
		}");
		}
	}
}
