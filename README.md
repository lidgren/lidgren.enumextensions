# lidgren.enumextensions
Performance oriented extension methods for enums generated at compile time using roslyn source generator.

```
MyEnum value;

value.ToStringEx();
value.TryParseEx("MemberName");
value.IsDefined();
value.GetNames();
value.GetValues();
```

Mostly exactly matches the regular enum methods; except that GetNames and GetValues does not allocate a new array, but returns a preallocated ReadOnlySpan<string>.
