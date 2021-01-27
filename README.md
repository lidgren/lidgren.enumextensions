# lidgren.enumextensions
Performance oriented extension methods for enums generated at compile time using roslyn source generator.

```
MyEnum value;

value.ToStringEx();
value.TryParseEx("MemberName");
value.IsDefinedEx();
value.GetNamesEx();
value.GetValuesEx();
```

Mostly exactly matches the regular enum methods; except that GetNames, GetValues and ToString() does not allocate any memory, but returns a readonly span to preallocated data.
Supports any kind of enum, including flags enums.

[Benchmarks here](https://github.com/lidgren/lidgren.enumextensions/wiki)
