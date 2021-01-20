using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lidgren.EnumExtensions
{
	public sealed class EnumExSyntaxReceiver : ISyntaxReceiver
	{
		public List<EnumInstance> Instances = new List<EnumInstance>();

		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{
			var enumDecl = syntaxNode as EnumDeclarationSyntax;
			if (enumDecl is null)
				return;

			var ins = new EnumInstance();

			ins.EnumName = enumDecl.Identifier.ValueText.ToString();
			ins.Namespace = SyntaxHelper.GetNamespace(enumDecl);

			if (enumDecl.BaseList != null)
			{
				// get underlying type
				foreach (var tpd in enumDecl.BaseList.Types)
				{
					// just get first
					ins.UnderlyingType = tpd.GetText().ToString().Trim();
					break;
				}
			}
			else
			{
				ins.UnderlyingType = "int";
			}

			ins.IsFlags = SyntaxHelper.GetAttribute(enumDecl, "Flags") != null;
			ins.MemberNames = new List<string>();
			ins.MemberValues = new List<string>();

			foreach (var child in enumDecl.ChildNodes())
			{
				if (child is not EnumMemberDeclarationSyntax member)
					continue;
				ins.MemberNames.Add(member.Identifier.ValueText.ToString().Trim());
				if (member.EqualsValue != null)
					ins.MemberValues.Add(member.EqualsValue.Value.GetText().ToString().Trim());
				else
					ins.MemberValues.Add(null);
			}

			Instances.Add(ins);
		}
	}
}
