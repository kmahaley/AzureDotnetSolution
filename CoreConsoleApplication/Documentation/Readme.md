# CoreConsoleApplication

## Run Benchmark

- add dependency
`<PackageReference Include="BenchmarkDotNet" Version="0.13.3" />`
- Create Class annotated with `[MemoryDiagnoser]` or `[MemoryDiagnoser(false)]`

```

[MemoryDiagnoser]
public class BechmarkApiDemo
{

```

- Create methods with APIs you want to bechmark. eg. stringBuilder, string Concat, string interpolation etc
- annotate methods with `[Benchmark]`

```

[Benchmark]
public string ConcatStringsUsingStringBuilder() {...}
[Benchmark]
public string ConcatStringsUsingGenericList() {...}

```

- Add runner in main method
```
static void Main(string[] args)
{
   var summary = BenchmarkRunner.Run<BechmarkApiDemo>()
}
```
- Build project, to check any errors
- Bechmark need release mode. Run project as

`dotnet run .\CoreConsoleApplication.csproj -c Release`