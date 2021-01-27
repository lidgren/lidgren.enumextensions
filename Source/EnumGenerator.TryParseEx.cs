using System;
using System.Text;
using Lidgren.CoreInl;

namespace Lidgren.EnumExtensions
{
	public sealed partial class EnumExGenerator
	{
		private void GenerateTryParseEx(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			if (ins.IsFlags)
			{
				PerformGenerateTryParse(ref strb, ins, true, false); // generates PartFastTryParse
				PerformGenerateTryParse(ref strb, ins, true, true); // generates PartFastTryParseIgnoreCase

				PerformGenerateFlagsTryParse(ref strb, ins, true); // generates FastTryParse
				PerformGenerateFlagsTryParse(ref strb, ins, false); // generates FastTryParseIgnoreCase
			}
			else
			{
				PerformGenerateTryParse(ref strb, ins, false, false); // generates FastTryParse
				PerformGenerateTryParse(ref strb, ins, false, true); // generates FastTryParseIgnoreCase
			}
		}

		private void PerformGenerateFlagsTryParse(ref ValueStringBuilder strb, in EnumInstance ins, bool ignoreCase)
		{
			if (ignoreCase == false)
				strb.AppendLine($"\t\tpublic static bool TryParseEx(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");
			else
				strb.AppendLine($"\t\tpublic static bool TryParseExIgnoreCase(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");

			strb.AppendLine("\t\t{");
			var partFunc = ignoreCase ? "PartTryParseIgnoreCase" : "PartTryParse";
			strb.AppendLine(
$@"			{ins.EnumName} retval = default;
			{ins.EnumName} add = default;
			for(;;)
			{{
				var idx = str.IndexOf(',');
				if (idx == -1)
				{{
					if (add.{partFunc}(str) == false)
						return false;
					value = retval | add;
					return true;
				}}
				if (add.{partFunc}(str.Slice(0, idx)) == false)
					return false;
				retval |= add;
				str = str.Slice(idx + 1).Trim();
				if (str.Length <= 0)
				{{
					value = retval;
					return true;
				}}
			}}
		}}
");
		}

		private void PerformGenerateTryParse(ref ValueStringBuilder strb, in EnumInstance ins, bool part, bool ignoreCase)
		{
			if (ins.MemberNames.Count < 2)
				strb.AppendLine("\t\t[MethodImpl(MethodImplOptions.AggressiveInlining)]");

			if (part)
			{
				if (ignoreCase == false)
					strb.AppendLine($"\t\tprivate static bool PartTryParse(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");
				else
					strb.AppendLine($"\t\tprivate static bool PartTryParseIgnoreCase(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");
			}
			else
			{
				if (ignoreCase == false)
					strb.AppendLine($"\t\tpublic static bool TryParseEx(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");
				else
					strb.AppendLine($"\t\tpublic static bool TryParseExIgnoreCase(ref this {ins.EnumName} value, ReadOnlySpan<char> str)");
			}

			strb.AppendLine("\t\t{");
			if (ins.MemberNames.Count == 0)
			{
				strb.AppendLine("\t\t\treturn false;");
			}
			else if (ins.MemberNames.Count == 1)
			{
				strb.AppendLine("\t\t\t{");
				if (ignoreCase)
					strb.AppendLine($"\t\t\tbool match0 = str.Equals(\"{ins.MemberNames[0]}\", StringComparison.OrdinalIgnoreCase);");
				else
					strb.AppendLine($"\t\t\tbool match0 = str.Equals(\"{ins.MemberNames[0]}\", StringComparison.Ordinal);");

				strb.AppendLine("\t\t\tif (match0)");
				strb.AppendLine($"\t\t\t\tvalue = {ins.EnumName}.{ins.MemberNames[0]};");
				strb.AppendLine("\t\t\treturn match0;");
				strb.AppendLine("\t\t\t}");
			}
			else
			{
				// 2+ members

				if (ignoreCase)
				{
					strb.AppendLine(
$@"			// FNV1a
			ulong hash = 14695981039346656037ul;
			for (int i = 0; i < str.Length; i++)
				hash = (hash ^ (uint)char.ToLower(str[i])) * 1099511628211ul;

			switch(hash)
			{{");
				}
				else
				{
					strb.AppendLine(
$@"			// FNV1a
			ulong hash = 14695981039346656037ul;
			for (int i = 0; i < str.Length; i++)
				hash = (hash ^ (uint)str[i]) * 1099511628211ul;
			
			switch(hash)
			{{");
				}

				foreach (var mbr in ins.MemberNames)
				{
					string mbrComment;
					if (ignoreCase)
						mbrComment = mbr.ToLower();
					else
						mbrComment = mbr;
					var mh = SyntaxHelper.FNV1a64(mbrComment.AsSpan());
					strb.AppendLine($"\t\t\t\tcase {mh}ul: // {mbrComment}");
					strb.AppendLine($"\t\t\t\t\tvalue = {ins.EnumName}.{mbr};");
					strb.AppendLine($"\t\t\t\t\treturn true;");
				}
				strb.AppendLine($"\t\t\t\tdefault:");
				strb.AppendLine($"\t\t\t\t\treturn false;");
				strb.AppendLine("\t\t\t}"); // switch
			}

			strb.AppendLine("\t\t}"); // tryparse()
			strb.AppendLine();
		}
	}
}
