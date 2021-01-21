using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	public enum SingleEntryEnum
	{
		Apple,
	}

	public enum SimpleEnum
	{
		Apple,
		Book,
		Coconut
	}

	public enum OffsetEnum
	{
		Apple = 10,
		Book,
		Coconut
	}

	public enum OffsetDiscontinuousEnum
	{
		apple = 5,
		book = 10,
		coconut = 20,
	}

	[Flags]
	public enum FlagsEnum
	{
		None = 0,
		Apple = 1 << 0,
		Book = 1 << 1,

		Coconut = 1 << 2,
		Dodo = 1 << 3,

		AppleAndBook = Apple | Book,

		All = Apple | Book | Coconut | Dodo
	}

	[TestClass]
	public class UnitTests
	{
		[TestMethod]
		public void TestSimpleEnum()
		{
			var tp = typeof(SimpleEnum);
			var val = default(SimpleEnum);

			// test GetNamesEx()
			var facitNames = Enum.GetNames(tp);
			var exNames = val.GetNamesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitNames.Length; i++)
				Assert.AreEqual(facitNames[i], exNames[i]);

			// test GetValuesEx
			var facitValues = Enum.GetValues<SimpleEnum>();
			var exValues = val.GetValuesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitValues.Length; i++)
				Assert.AreEqual(facitValues[i], exValues[i]);

			// test ToStringEx()
			foreach (var value in facitValues)
				Assert.AreEqual(value.ToString(), value.ToStringEx());

			// test TryParseEx
			foreach(var name in facitNames)
			{
				var facit = Enum.Parse<SimpleEnum>(name);

				val = default;
				bool ok = val.TryParseEx(name);

				Assert.IsTrue(ok);
				Assert.AreEqual(facit, val);
			}

			// test failed tryparse
			var notok = Enum.TryParse<SimpleEnum>("notastringthatexistsinenum", out _);
			var exnotok = val.TryParseEx("notastringthatexistsinenum");
			Assert.AreEqual(notok, exnotok);

			// test isdefined
			val = SimpleEnum.Apple;
			Assert.IsTrue(val.IsDefinedEx());
			val = SimpleEnum.Book;
			Assert.IsTrue(val.IsDefinedEx());
			val = SimpleEnum.Coconut;
			Assert.IsTrue(val.IsDefinedEx());
			val = (SimpleEnum)999;
			Assert.IsFalse(val.IsDefinedEx());
		}

		[TestMethod]
		public void TestOffsetEnum()
		{
			var tp = typeof(OffsetEnum);
			var val = default(OffsetEnum);

			// test GetNamesEx
			var facitNames = Enum.GetNames(tp);
			var exNames = val.GetNamesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitNames.Length; i++)
				Assert.AreEqual(facitNames[i], exNames[i]);

			// test GetValuesEx
			var facitValues = Enum.GetValues<OffsetEnum>();
			var exValues = val.GetValuesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitValues.Length; i++)
				Assert.AreEqual(facitValues[i], exValues[i]);

			// test ToStringEx()
			foreach (var value in facitValues)
				Assert.AreEqual(value.ToString(), value.ToStringEx());

			// test TryParse
			foreach (var name in facitNames)
			{
				var facit = Enum.Parse<OffsetEnum>(name);

				val = default;
				bool ok = val.TryParseEx(name);

				Assert.IsTrue(ok);
				Assert.AreEqual(facit, val);
			}

			// test failed tryparse
			var notok = Enum.TryParse<OffsetEnum>("notastringthatexistsinenum", false, out _);
			var exnotok = val.TryParseEx("notastringthatexistsinenum");
			Assert.AreEqual(notok, exnotok);

			// test isdefined
			val = OffsetEnum.Apple;
			Assert.IsTrue(val.IsDefinedEx());
			val = OffsetEnum.Book;
			Assert.IsTrue(val.IsDefinedEx());
			val = OffsetEnum.Coconut;
			Assert.IsTrue(val.IsDefinedEx());
			val = (OffsetEnum)999;
			Assert.IsFalse(val.IsDefinedEx());
		}

		[TestMethod]
		public void TestDiscontinuousOffsetEnum()
		{
			var tp = typeof(OffsetDiscontinuousEnum);
			var val = default(OffsetDiscontinuousEnum);

			// test GetNamesEx
			var facitNames = Enum.GetNames(tp);
			var exNames = val.GetNamesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitNames.Length; i++)
				Assert.AreEqual(facitNames[i], exNames[i]);

			// test GetValuesEx
			var facitValues = Enum.GetValues<OffsetDiscontinuousEnum>();
			var exValues = val.GetValuesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitValues.Length; i++)
				Assert.AreEqual(facitValues[i], exValues[i]);

			// test ToStringEx
			foreach (var value in facitValues)
				Assert.AreEqual(value.ToString(), value.ToStringEx());
			
			// test TryParseEx
			foreach (var name in facitNames)
			{
				var facit = Enum.Parse<OffsetDiscontinuousEnum>(name);

				val = default;
				bool ok = val.TryParseEx(name);

				Assert.IsTrue(ok);
				Assert.AreEqual(facit, val);
			}

			// test failed TryParseEx
			var notok = Enum.TryParse<OffsetDiscontinuousEnum>("notastringthatexistsinenum", false, out _);
			var exnotok = val.TryParseEx("notastringthatexistsinenum");
			Assert.AreEqual(notok, exnotok);

			// test isdefined
			val = OffsetDiscontinuousEnum.apple;
			Assert.IsTrue(val.IsDefinedEx());
			val = OffsetDiscontinuousEnum.book;
			Assert.IsTrue(val.IsDefinedEx());
			val = OffsetDiscontinuousEnum.coconut;
			Assert.IsTrue(val.IsDefinedEx());
			val = (OffsetDiscontinuousEnum)999;
			Assert.IsFalse(val.IsDefinedEx());
		}

		[TestMethod]
		public void TestFlagsEnum()
		{
			bool ok;
			var tp = typeof(FlagsEnum);
			var val = default(FlagsEnum);

			// test GetNamesEx
			var facitNames = Enum.GetNames(tp);
			var exNames = val.GetNamesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitNames.Length; i++)
				Assert.AreEqual(facitNames[i], exNames[i]);

			// test GetValuesEx
			var facitValues = Enum.GetValues<FlagsEnum>();
			var exValues = val.GetValuesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitValues.Length; i++)
				Assert.AreEqual(facitValues[i], exValues[i]);

			// test ToStringEx
			foreach (var value in facitValues)
				Assert.AreEqual(value.ToString(), value.ToStringEx());

			// test TryParseEx
			foreach (var name in facitNames)
			{
				var facit = Enum.Parse<FlagsEnum>(name);

				val = default;
				ok = val.TryParseEx(name);

				Assert.IsTrue(ok);
				Assert.AreEqual(facit, val);
			}

			// test failed TryParseEx
			var notok = Enum.TryParse<FlagsEnum>("notastringthatexistsinenum", false, out _);
			var exnotok = val.TryParseEx("notastringthatexistsinenum");
			Assert.AreEqual(notok, exnotok);

			// flags specific testing
			var a = FlagsEnum.Apple | FlagsEnum.Book | FlagsEnum.Coconut | FlagsEnum.Dodo;
			var aa = a.ToString();
			var bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			a = FlagsEnum.Apple | FlagsEnum.Book | FlagsEnum.Dodo;
			aa = a.ToString();
			bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			a = FlagsEnum.None;
			aa = a.ToString();
			bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			a = FlagsEnum.Apple;
			aa = a.ToString();
			bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			a = FlagsEnum.AppleAndBook;
			aa = a.ToString();
			bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			a = FlagsEnum.All;
			aa = a.ToString();
			bb = a.ToStringEx();
			Assert.AreEqual(aa, bb);

			// test flags tryparse

			ok = a.TryParseEx("Apple, Book");
			Assert.IsTrue(ok);
			Assert.AreEqual(FlagsEnum.AppleAndBook, a);
			Assert.AreEqual(FlagsEnum.Apple | FlagsEnum.Book, a);

			ok = a.TryParseEx("Apple, ASDkjahsdkjhsd");
			Assert.IsFalse(ok);

			ok = a.TryParseEx("Coconut, Apple");
			Assert.IsTrue(ok);
			Assert.AreEqual(FlagsEnum.Apple | FlagsEnum.Coconut, a);

			ok = a.TryParseEx("adfasdfdafd");
			Assert.IsFalse(ok);

			ok = a.TryParseEx(",,,");
			Assert.IsFalse(ok);

			ok = a.TryParseEx("");
			Assert.IsFalse(ok);

			ok = a.TryParseEx("Apple");
			Assert.IsTrue(ok);
			Assert.AreEqual(FlagsEnum.Apple, a);

			ok = a.TryParseEx("All");
			Assert.IsTrue(ok);
			Assert.AreEqual(FlagsEnum.All, a);

			// test isdefined
			val = FlagsEnum.Apple;
			Assert.IsTrue(val.IsDefinedEx());
			val = FlagsEnum.AppleAndBook;
			Assert.IsTrue(val.IsDefinedEx());
			val = FlagsEnum.All;
			Assert.IsTrue(val.IsDefinedEx());
			val = FlagsEnum.None;
			Assert.IsTrue(val.IsDefinedEx());
			val = (FlagsEnum)999;
			Assert.IsFalse(val.IsDefinedEx());
		}

		[TestMethod]
		public void TestSingleEntryEnum()
		{
			var tp = typeof(SingleEntryEnum);
			var val = default(SingleEntryEnum);

			// test GetNamesEx()
			var facitNames = Enum.GetNames(tp);
			var exNames = val.GetNamesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitNames.Length; i++)
				Assert.AreEqual(facitNames[i], exNames[i]);

			// test GetValuesEx
			var facitValues = Enum.GetValues<SingleEntryEnum>();
			var exValues = val.GetValuesEx();
			Assert.AreEqual(facitNames.Length, exNames.Length);
			for (int i = 0; i < facitValues.Length; i++)
				Assert.AreEqual(facitValues[i], exValues[i]);

			// test ToStringEx()
			foreach (var value in facitValues)
				Assert.AreEqual(value.ToString(), value.ToStringEx());

			// test TryParseEx
			foreach (var name in facitNames)
			{
				var facit = Enum.Parse<SingleEntryEnum>(name);

				val = default;
				bool ok = val.TryParseEx(name);

				Assert.IsTrue(ok);
				Assert.AreEqual(facit, val);
			}

			// test failed tryparse
			var notok = Enum.TryParse<SingleEntryEnum>("notastringthatexistsinenum", out _);
			var exnotok = val.TryParseEx("notastringthatexistsinenum");
			Assert.AreEqual(notok, exnotok);

			// test isdefined
			val = SingleEntryEnum.Apple;
			Assert.IsTrue(val.IsDefinedEx());
			val = (SingleEntryEnum)999;
			Assert.IsFalse(val.IsDefinedEx());
		}
	}
}
