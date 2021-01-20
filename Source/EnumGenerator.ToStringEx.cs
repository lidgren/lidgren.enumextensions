using System;
using System.Text;
using Lidgren.CoreInl;

namespace Lidgren.EnumExtensions
{
	public sealed partial class EnumExGenerator
	{
		private void GenerateToStringEx(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			// Assumed to run AFTER GenerateGetNamesEx

			strb.AppendLine($"\t\tpublic static string ToStringEx(this {ins.EnumName} value)");
			strb.AppendLine("\t\t{");

			if (ins.IsFlags)
			{
				//var code = s_flagsWriteCode;
				//code = code.Replace("ENUMNAME", ins.EnumName);
				//strb.AppendLine(code);

				strb.AppendLine("\t\t\t// default implementation of flags-to-string is actually pretty good");
				strb.AppendLine("\t\t\treturn value.ToString();");
			}
			else if (ins.IsContinuous == true)
			{
				// continuous values
				strb.AppendLine($"\t\t\treturn s_namesFor{ins.EnumName}[({ins.UnderlyingType})value - ({ins.UnderlyingType}){ins.EnumName}.{ins.MemberNames[0]}];");
			}
			else
			{
				// discontinuous values
				strb.AppendLine("\t\t\tswitch(value)");
				strb.AppendLine("\t\t\t{");
				for(int i=0;i<ins.MemberNames.Count;i++)
				{
					var member = ins.MemberNames[i];
					strb.AppendLine($"\t\t\t\tcase {ins.EnumName}.{member}:");
					strb.AppendLine($"\t\t\t\t\treturn s_namesFor{ins.EnumName}[{i}];");
				}
				strb.AppendLine("\t\t\t\tdefault:");
				strb.AppendLine("\t\t\t\t\tThrowException();");
				strb.AppendLine("\t\t\t\t\treturn null;");
				strb.AppendLine("\t\t\t}");
			}

			strb.AppendLine("\t\t}");
			strb.AppendLine();

			return;
		}

		private static string s_flagsWriteCode =
@$"		// phase one; calculate length of string
		int len = 0;

		var vals = s_valuesForFlagsEnum;
		var names = s_namesForFlagsEnum;
		int numMatches = 0;
		long remaining = (long)value;
		int firstMatch = -1;
		for(int i = vals.Length - 1; i >= 0; i--)
		{{
			var v = (long)vals[i];
			if (v == 0)
			{{
				if (len == 0)
					return names[i];
				break;
			}}
			if ((v & remaining) == v)
			{{
				remaining -= v;
				if (len > 0)
					len += 2; // comma and space
				else
					firstMatch = i;
				len += names[i].Length;
				numMatches++;
			}}
		}}

		if (numMatches == 1)
			return names[firstMatch];

		// phase two; concatenate matches
		return String.Create<ValueTuple<ENUMNAME[], string[]>>(
			len,
			new ValueTuple<ENUMNAME[], string[]>(vals, names),
			(chars, tuple) =>
			{{
				var vals = tuple.Item1;
				var names = tuple.Item2;
				long remaining = (long)value;
				for(int i = vals.Length - 1; i >= 0; i--)
				{{
					var v = (long)vals[i];
					if (v == 0)
						return;
					if ((v & remaining) == v)
					{{
						remaining -= v;

						var name = names[i];
						int nameLen = name.Length;

						// fill from back
						int pos = chars.Length - nameLen;
						name.AsSpan().CopyTo(chars.Slice(pos));
						if (chars.Length <= nameLen)
							return; // done!
						chars[pos - 2] = ',';
						chars[pos - 1] = ' ';
						chars = chars.Slice(0, pos - 2);
					}}
				}}
			}}
		);
";

	}
}






