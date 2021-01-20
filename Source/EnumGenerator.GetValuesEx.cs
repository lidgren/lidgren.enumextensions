using System;
using System.Text;
using Lidgren.CoreInl;

namespace Lidgren.EnumExtensions
{
	public sealed partial class EnumExGenerator
	{
		private void GenerateGetValuesEx(ref ValueStringBuilder strb, in EnumInstance ins)
		{
			strb.AppendLine($"\t\tprivate static readonly {ins.EnumName}[] s_valuesFor{ins.EnumName} = new {ins.EnumName}[]");
			strb.AppendLine("\t\t{");
			for (int i = 0; i < ins.MemberNames.Count; i++)
			{
				var mbrName = ins.MemberNames[i];
				var mbrNum = ins.MemberNumericValues[i].ToString();
				strb.AppendLine($"\t\t\t{ins.EnumName}.{mbrName}, // {mbrNum}");
			}
			strb.AppendLine("\t\t};");

			strb.Append($"\t\tprivate static readonly {ins.UnderlyingType}[] s_numericValuesFor{ins.EnumName} = new {ins.UnderlyingType}[] {{ ");
			for (int i = 0; i < ins.MemberNames.Count; i++)
			{
				var mbrNum = ins.MemberNumericValues[i].ToString();
				strb.Append(mbrNum + ", ");
			}
			strb.AppendLine(" };");

			strb.AppendLine($"\t\tpublic static ReadOnlySpan<{ins.EnumName}> GetValuesEx(this {ins.EnumName} unused) => s_valuesFor{ins.EnumName};");
			strb.AppendLine();
		}
	}
}
