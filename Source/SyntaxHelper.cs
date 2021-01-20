using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Lidgren.EnumExtensions
{
	public static class SyntaxHelper
	{
		public static AttributeSyntax GetAttribute(SyntaxNode syntaxNode, string name)
		{
			foreach (var child in syntaxNode.ChildNodes())
			{
				if (child is AttributeListSyntax attrList)
				{
					foreach (var attr in attrList.Attributes)
					{
						if (attr.Name.GetText().ToString().Equals(name, StringComparison.Ordinal))
							return attr;
					}
				}
			}
			return null;
		}

		public static T FindChild<T>(SyntaxNode parent) where T : SyntaxNode
		{
			foreach (var child in parent.ChildNodes())
			{
				if (child is T retval)
					return retval;
			}
			return null;
		}

		public static bool HasModifier(SyntaxTokenList list, SyntaxKind kind)
		{
			foreach (var token in list)
				if (token.Kind() == kind)
					return true;
			return false;
		}

		public static string GetNamespace(SyntaxNode node)
		{
			// back up to namespace
			while (node != null && (node is NamespaceDeclarationSyntax == false))
				node = node.Parent;
			if (node == null)
				return null;
			return (node as NamespaceDeclarationSyntax).Name.ToString();
		}

		public static string GetModifier(StructDeclarationSyntax node, SyntaxKind kind)
		{
			foreach (var mod in node.Modifiers)
			{
				if (mod.Kind() == kind)
					return mod.ToString();
			}
			return null;
		}

		public static string GetModifier(MemberDeclarationSyntax node, SyntaxKind kind)
		{
			foreach (var mod in node.Modifiers)
			{
				if (mod.Kind() == kind)
					return mod.ToString();
			}
			return null;
		}

		public static Dictionary<GROUPT, List<T>> GroupBy<T, GROUPT>(this List<T> from, Func<T, GROUPT> getGroup)
		{
			var retval = new Dictionary<GROUPT, List<T>>();
			foreach (var item in from)
			{
				var g = getGroup(item);
				if (retval.TryGetValue(g, out var list) == false)
				{
					list = new List<T>();
					retval[g] = list;
				}
				list.Add(item);
			}
			return retval;
		}

		public static List<string> GetUsings(SyntaxNode syntaxNode)
		{
			var retval = new List<string>();

			// back up to compilationunit
			var root = syntaxNode.SyntaxTree.GetRoot();
			foreach (var child in root.ChildNodes())
			{
				if (child is UsingDirectiveSyntax us)
					retval.Add(us.GetText().ToString().Trim());
			}
			return retval;
		}

		public static uint FNV1a(ReadOnlySpan<char> str)
		{
			uint hash = (uint)2166136261u;
			for (int i = 0; i < str.Length; i++)
				hash = (hash ^ (uint)str[i]) * 16777619u;
			return hash;
		}

		public static ulong FNV1a64(ReadOnlySpan<char> str)
		{
			ulong hash = 14695981039346656037ul;
			for (int i = 0; i < str.Length; i++)
				hash = (hash ^ (uint)str[i]) * 1099511628211ul;
			return hash;
		}
	}
}
