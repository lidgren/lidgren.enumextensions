using System;
using System.Collections.Generic;

namespace Lidgren.EnumExtensions
{
	public struct EnumInstance
	{
		public string Namespace;
		public string EnumName;
		public string UnderlyingType;
		public bool IsFlags;
		public List<string> MemberNames;
		public List<string> MemberValues;

		// analyzed values:
		public bool IsContinuous;
		public long[] MemberNumericValues;
	}
}
