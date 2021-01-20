using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.CoreInl;

namespace Lidgren.EnumExtensions
{
	public sealed partial class EnumExGenerator
	{
		private void GenerateGetNamesEx(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			strb.AppendLine($"\t\tprivate static readonly string[] s_namesFor{ins.EnumName} = new string[]");
			strb.AppendLine("\t\t{");
			foreach (var mbr in ins.MemberNames)
				strb.AppendLine("\t\t\t\"" + mbr.Trim() + "\", ");
			strb.AppendLine("\t\t};");
			strb.AppendLine($"\t\tpublic static ReadOnlySpan<string> GetNamesEx(this {ins.EnumName} unused) => s_namesFor{ins.EnumName};");
			strb.AppendLine();
		}
	}
}
