
# Benchmarking DotNetCore 2.1, 2.2,3.0 and CLR 4.7.2
Following [Stephen Toub](https://devblogs.microsoft.com/dotnet/author/toub/) [post from April 2018 on DotNetCore 2.0 and 2.1 benchmarks](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/), I have decided to update it with latest DotNetCore3.0 preview. Most of the following are taken from his post. The benchmarking library is [benchmarkdotnet.](https://benchmarkdotnet.org/)

# Experimental setup
All benchmarks were run on the same computer (Dell XPS 9550, in flight mode, antivirus off):
``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.3761.0
  Job-BFNISR : .NET Core 2.0.9 (CoreCLR 4.6.26614.01, CoreFX 4.6.26614.01), 64bit RyuJIT
  Job-OXVCGU : .NET Core 2.1.9 (CoreCLR 4.6.27414.06, CoreFX 4.6.27415.01), 64bit RyuJIT
  Job-AQDXUX : .NET Core 2.2.4 (CoreCLR 4.6.27521.02, CoreFX 4.6.27521.01), 64bit RyuJIT
  Job-KTSQXX : .NET Core 3.0.0-preview4-27615-11 (CoreCLR 4.6.27615.73, CoreFX 4.700.19.21213), 64bit RyuJIT
  Clr        : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.3761.0

Platform=X64  Runtime=Clr  

```
# Md5VsSha256

        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);

| Method |     Job |     Toolchain |     Mean | Ratio |
|------- |-------- |-------------- |---------:|------:|
| Sha256 | Default | .NET Core 2.0 | 49.20 us |  0.52 |
| Sha256 | Default | .NET Core 2.1 | 49.18 us |  0.52 |
| Sha256 | Default | .NET Core 2.2 | 48.57 us |  0.52 |
| Sha256 | Default | .NET Core 3.0 | 48.95 us |  0.52 |
| Sha256 |     Clr |       Default | 94.14 us |  1.00 |
|        |         |               |          |       |
|    Md5 | Default | .NET Core 2.0 | 23.01 us |  0.95 |
|    Md5 | Default | .NET Core 2.1 | 23.11 us |  0.95 |
|    Md5 | Default | .NET Core 2.2 | 23.18 us |  0.95 |
|    Md5 | Default | .NET Core 3.0 | 23.15 us |  0.95 |
|    Md5 |     Clr |       Default | 24.35 us |  1.00 |

# MyBench

        [Benchmark]
        public IList<byte> Where() => data.Where(x => x > 20).ToList();

        [Benchmark]
        public IList<byte> SortedSet() => new SortedSet<byte>(data).ToList();

        [Benchmark]
        public void Sin()
        {
            foreach (var b in data)
            {
                Math.Sin(((double)b) / 42.0);
            }
        }

        [Benchmark]
        public void Dictionary()
        {
            foreach (var b in data)
            {
                dic[b]++;
            }
        }

|     Method |     Job |     Toolchain |        Mean | Ratio |
|----------- |-------- |-------------- |------------:|------:|
|      Where | Default | .NET Core 2.0 |    45.52 us |  0.37 |
|      Where | Default | .NET Core 2.1 |    50.90 us |  0.41 |
|      Where | Default | .NET Core 2.2 |    51.38 us |  0.41 |
|      Where | Default | .NET Core 3.0 |    49.06 us |  0.40 |
|      Where |     Clr |       Default |   123.55 us |  1.00 |
|            |         |               |             |       |
|  SortedSet | Default | .NET Core 2.0 |   617.23 us |  0.35 |
|  SortedSet | Default | .NET Core 2.1 |   591.40 us |  0.34 |
|  SortedSet | Default | .NET Core 2.2 |   593.96 us |  0.34 |
|  SortedSet | Default | .NET Core 3.0 |   587.20 us |  0.33 |
|  SortedSet |     Clr |       Default | 1,754.27 us |  1.00 |
|            |         |               |             |       |
|        Sin | Default | .NET Core 2.0 |   200.04 us |  0.41 |
|        Sin | Default | .NET Core 2.1 |   203.59 us |  0.41 |
|        Sin | Default | .NET Core 2.2 |   200.15 us |  0.41 |
|        Sin | Default | .NET Core 3.0 |   204.11 us |  0.41 |
|        Sin |     Clr |       Default |   493.33 us |  1.00 |
|            |         |               |             |       |
| Dictionary | Default | .NET Core 2.0 |   310.44 us |  1.07 |
| Dictionary | Default | .NET Core 2.1 |   222.51 us |  0.77 |
| Dictionary | Default | .NET Core 2.2 |   218.45 us |  0.77 |
| Dictionary | Default | .NET Core 3.0 |   255.24 us |  0.88 |
| Dictionary |     Clr |       Default |   289.78 us |  1.00 |

# JIT

        [Benchmark(OperationsPerInvoke = 100_000_000)]
        public int EqualityComparerInt32()
        {
            int[] items = s_intArray;

            for (int i = 0; i < items.Length; i++)
                if (EqualityComparer<int>.Default.Equals(items[i], -1))
                    return i;

            return -1;
        }

        [Benchmark(OperationsPerInvoke = Items)]
        public int DictionaryContainsValue()
        {
            Dictionary<int, int> d = s_dict;
            int count = 0;

            for (int i = 0; i < Items; i++)
                if (d.ContainsValue(i))
                    count++;

            return count;
        }

        [Benchmark(OperationsPerInvoke = 10_000)]
        public bool EnumHasFlag()
        {
            FileAccess value = FileAccess.ReadWrite;
            bool result = true;

            for (int i = 0; i < 10_000; i++)
            {
                result &= value.HasFlag(FileAccess.Read);
            }

            return result;
        }

        [Benchmark]
        public bool LoopBodyLayout() => StringsAreEqual(
            "this is a test to see if these strings are equal",
            "this is a test to see if these strings are equal"
            );

        [Benchmark(OperationsPerInvoke = 10_000_000)]
        public int LoweringTESTtoBT()
        {
            int y = 0, x = 0;

            while (x++ < 10_000_000)
                if ((x & (1 << y)) == 0)
                    y++;

            return y;
        }

        [Benchmark]
        public void BoxingAllocations() => BoxingAllocations(default(Dog));

        private void BoxingAllocations<T>(T thing)
        {
            if (thing is IAnimal)
                ((IAnimal)thing).MakeSound();
        }

|                  Method |     Job |     Toolchain |          Mean | Ratio |
|------------------------ |-------- |-------------- |--------------:|------:|
|   EqualityComparerInt32 | Default | .NET Core 2.0 |     2.8902 ns |  1.19 |
|   EqualityComparerInt32 | Default | .NET Core 2.1 |     1.0809 ns |  0.43 |
|   EqualityComparerInt32 | Default | .NET Core 2.2 |     1.0875 ns |  0.46 |
|   EqualityComparerInt32 | Default | .NET Core 3.0 |     1.1691 ns |  0.48 |
|   EqualityComparerInt32 |     Clr |       Default |     2.4445 ns |  1.00 |
|                         |         |               |               |       |
| DictionaryContainsValue | Default | .NET Core 2.0 | 5,301.4990 ns |  1.20 |
| DictionaryContainsValue | Default | .NET Core 2.1 | 2,156.4981 ns |  0.49 |
| DictionaryContainsValue | Default | .NET Core 2.2 | 2,251.0265 ns |  0.51 |
| DictionaryContainsValue | Default | .NET Core 3.0 | 2,292.9484 ns |  0.52 |
| DictionaryContainsValue |     Clr |       Default | 4,477.1106 ns |  1.00 |
|                         |         |               |               |       |
|             EnumHasFlag | Default | .NET Core 2.0 |    20.2835 ns |  0.86 |
|             EnumHasFlag | Default | .NET Core 2.1 |     0.8173 ns |  0.03 |
|             EnumHasFlag | Default | .NET Core 2.2 |     0.8065 ns |  0.03 |
|             EnumHasFlag | Default | .NET Core 3.0 |     0.4163 ns |  0.02 |
|             EnumHasFlag |     Clr |       Default |    23.7034 ns |  1.00 |
|                         |         |               |               |       |
|          LoopBodyLayout | Default | .NET Core 2.0 |    71.5786 ns |  1.94 |
|          LoopBodyLayout | Default | .NET Core 2.1 |    37.6488 ns |  1.02 |
|          LoopBodyLayout | Default | .NET Core 2.2 |    37.1289 ns |  1.01 |
|          LoopBodyLayout | Default | .NET Core 3.0 |    38.6321 ns |  1.05 |
|          LoopBodyLayout |     Clr |       Default |    36.8877 ns |  1.00 |
|                         |         |               |               |       |
|        LoweringTESTtoBT | Default | .NET Core 2.0 |     1.8056 ns |  1.37 |
|        LoweringTESTtoBT | Default | .NET Core 2.1 |     1.2312 ns |  0.93 |
|        LoweringTESTtoBT | Default | .NET Core 2.2 |     1.2022 ns |  0.90 |
|        LoweringTESTtoBT | Default | .NET Core 3.0 |     1.3039 ns |  0.99 |
|        LoweringTESTtoBT |     Clr |       Default |     1.3225 ns |  1.00 |
|                         |         |               |               |       |
|       BoxingAllocations | Default | .NET Core 2.0 |    18.1086 ns |  1.08 |
|       BoxingAllocations | Default | .NET Core 2.1 |     1.8790 ns |  0.11 |
|       BoxingAllocations | Default | .NET Core 2.2 |     2.0924 ns |  0.12 |
|       BoxingAllocations | Default | .NET Core 3.0 |     2.1033 ns |  0.12 |
|       BoxingAllocations |     Clr |       Default |    16.8597 ns |  1.00 |

# Threading

        [Benchmark(OperationsPerInvoke = 1_000_000)]
        public void ThreadStatics()
        {
            for (int i = 0; i < 1_000_000; i++) GetThreadStatic();
        }

        [Benchmark]
        public void TimerContention()
        {
            var tasks = new Task[Environment.ProcessorCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    for (int j = 0; j < 50_000; j++)
                    {
                        using (var t = new Timer(delegate { }, null, 100_000, -1))
                        {
                            await Task.Yield();
                        }
                    }
                });
            }
            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void SerialCancellationTokenRegistration() =>
            s_token.Register(() => { }).Dispose();

        [Benchmark(OperationsPerInvoke = 1_000_000)]
        public void ParallelCancellationTokenRegistration() =>
            Parallel.For(0, 1_000_000, i => s_token.Register(() => { }).Dispose());

        [Benchmark(OperationsPerInvoke = 10_000)]
        public async Task AsyncMethodAwaitInvocation()
        {
            for (int i = 0; i < 10_000; i++) await MethodAsync();
        }


|                                Method |     Job |     Toolchain |               Mean | Ratio |
|-------------------------------------- |-------- |-------------- |-------------------:|------:|
|                         ThreadStatics | Default | .NET Core 2.0 |           9.564 ns |  1.17 |
|                         ThreadStatics | Default | .NET Core 2.1 |           8.093 ns |  0.99 |
|                         ThreadStatics | Default | .NET Core 2.2 |           8.068 ns |  0.99 |
|                         ThreadStatics | Default | .NET Core 3.0 |           7.779 ns |  0.95 |
|                         ThreadStatics |     Clr |       Default |           8.172 ns |  1.00 |
|                                       |         |               |                    |       |
|                       TimerContention | Default | .NET Core 2.0 | 466,720,876.471 ns |  1.62 |
|                       TimerContention | Default | .NET Core 2.1 | 173,399,976.596 ns |  0.61 |
|                       TimerContention | Default | .NET Core 2.2 | 140,698,656.731 ns |  0.49 |
|                       TimerContention | Default | .NET Core 3.0 | 152,115,641.333 ns |  0.64 |
|                       TimerContention |     Clr |       Default | 288,566,760.714 ns |  1.00 |
|                                       |         |               |                    |       |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.0 |         127.337 ns |  0.92 |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.1 |          68.197 ns |  0.49 |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.2 |          74.352 ns |  0.54 |
|   SerialCancellationTokenRegistration | Default | .NET Core 3.0 |          69.990 ns |  0.50 |
|   SerialCancellationTokenRegistration |     Clr |       Default |         138.689 ns |  1.00 |
|                                       |         |               |                    |       |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.0 |          39.402 ns |  0.92 |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.1 |          14.621 ns |  0.34 |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.2 |          16.755 ns |  0.39 |
| ParallelCancellationTokenRegistration | Default | .NET Core 3.0 |          20.286 ns |  0.49 |
| ParallelCancellationTokenRegistration |     Clr |       Default |          42.874 ns |  1.00 |
|                                       |         |               |                    |       |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.0 |          27.305 ns |  0.40 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.1 |          18.015 ns |  0.26 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.2 |          16.603 ns |  0.24 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 3.0 |          17.331 ns |  0.25 |
|            AsyncMethodAwaitInvocation |     Clr |       Default |          69.111 ns |  1.00 |

# String

        [Benchmark]
        public bool StringEquals() =>
            "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages."
            .Equals(
                "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages!"
                );

        [Benchmark]
        public void StringIndexOf() =>
            "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages."
            .IndexOf('.');

        [Benchmark]
        public int IndexOfAny() =>
            "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages."
            .IndexOfAny(s_chars);


        [Benchmark]
        public string StringToLowerChangesNeeded() =>
            "This is a test to see what happens when we call ToLower."
            .ToLower();

        [Benchmark]
        public string StringToLowerAlreadyCased() =>
            "this is a test to see what happens when we call tolower."
            .ToLower();

        [Benchmark]
        public string[] StringSplit() =>
            "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages."
            .Split(s_seps);

        [Benchmark]
        public string StringConcatCharEnumerable() =>
            string.Concat(Enumerable.Range(0, 1000).Select(i => (char)('a' + i % 26)));

|                     Method |     Job |     Toolchain |         Mean | Ratio |
|--------------------------- |-------- |-------------- |-------------:|------:|
|               StringEquals | Default | .NET Core 2.0 |     18.90 ns |  0.89 |
|               StringEquals | Default | .NET Core 2.1 |     12.00 ns |  0.56 |
|               StringEquals | Default | .NET Core 2.2 |     11.51 ns |  0.54 |
|               StringEquals | Default | .NET Core 3.0 |     12.18 ns |  0.57 |
|               StringEquals |     Clr |       Default |     21.24 ns |  1.00 |
|                            |         |               |              |       |
|              StringIndexOf | Default | .NET Core 2.0 |     48.88 ns |  0.48 |
|              StringIndexOf | Default | .NET Core 2.1 |     19.01 ns |  0.19 |
|              StringIndexOf | Default | .NET Core 2.2 |     19.03 ns |  0.19 |
|              StringIndexOf | Default | .NET Core 3.0 |     18.69 ns |  0.18 |
|              StringIndexOf |     Clr |       Default |    101.66 ns |  1.00 |
|                            |         |               |              |       |
|                 IndexOfAny | Default | .NET Core 2.0 |    102.95 ns |  0.74 |
|                 IndexOfAny | Default | .NET Core 2.1 |     38.37 ns |  0.28 |
|                 IndexOfAny | Default | .NET Core 2.2 |     42.32 ns |  0.30 |
|                 IndexOfAny | Default | .NET Core 3.0 |     18.87 ns |  0.14 |
|                 IndexOfAny |     Clr |       Default |    138.91 ns |  1.00 |
|                            |         |               |              |       |
| StringToLowerChangesNeeded | Default | .NET Core 2.0 |    199.77 ns |  0.94 |
| StringToLowerChangesNeeded | Default | .NET Core 2.1 |     95.21 ns |  0.45 |
| StringToLowerChangesNeeded | Default | .NET Core 2.2 |    100.46 ns |  0.47 |
| StringToLowerChangesNeeded | Default | .NET Core 3.0 |     59.11 ns |  0.28 |
| StringToLowerChangesNeeded |     Clr |       Default |    211.58 ns |  1.00 |
|                            |         |               |              |       |
|  StringToLowerAlreadyCased | Default | .NET Core 2.0 |    200.60 ns |  0.93 |
|  StringToLowerAlreadyCased | Default | .NET Core 2.1 |     68.88 ns |  0.32 |
|  StringToLowerAlreadyCased | Default | .NET Core 2.2 |     66.86 ns |  0.31 |
|  StringToLowerAlreadyCased | Default | .NET Core 3.0 |     41.64 ns |  0.19 |
|  StringToLowerAlreadyCased |     Clr |       Default |    214.63 ns |  1.00 |
|                            |         |               |              |       |
|                StringSplit | Default | .NET Core 2.0 |    358.89 ns |  1.00 |
|                StringSplit | Default | .NET Core 2.1 |    256.43 ns |  0.71 |
|                StringSplit | Default | .NET Core 2.2 |    254.56 ns |  0.71 |
|                StringSplit | Default | .NET Core 3.0 |    257.87 ns |  0.72 |
|                StringSplit |     Clr |       Default |    358.82 ns |  1.00 |
|                            |         |               |              |       |
| StringConcatCharEnumerable | Default | .NET Core 2.0 | 22,182.96 ns |  0.77 |
| StringConcatCharEnumerable | Default | .NET Core 2.1 | 17,184.09 ns |  0.59 |
| StringConcatCharEnumerable | Default | .NET Core 2.2 | 16,471.14 ns |  0.57 |
| StringConcatCharEnumerable | Default | .NET Core 3.0 | 14,554.08 ns |  0.50 |
| StringConcatCharEnumerable |     Clr |       Default | 28,958.84 ns |  1.00 |

# FormattingAndParsing

        [Benchmark]
        public void StringFormat() => string.Format("Test {0}", 123456789);

        [Benchmark]
        public void StringBuilderAppend()
        {
            StringBuilder sb = s_builder;
            sb.Length = 0;

            for (int i = 0; i < 100_000; i++)
                sb.Append(i);
        }

        [Benchmark]
        public void Int32Formatting() => 123456789.ToString();

        [Benchmark]
        public void Int32Parsing() => int.Parse("123456789");

        [Benchmark]
        public void DoubleFormatting() => (1234.5678).ToString();

        [Benchmark]
        public void BigIntegerFormatting() => s_bi.ToString("X");

        [Benchmark]
        public void DateTimeOffsetFormatR() => s_dto.ToString("r");

        [Benchmark]
        public void DateTimeOffsetFormatO() => s_dto.ToString("o");

        [Benchmark]
        public byte[] ConvertFromBase64String() => Convert.FromBase64String(s_base64String);

        [Benchmark]
        public byte[] ConvertFromBase64Chars() => Convert.FromBase64CharArray(s_base64Chars, 0, s_base64Chars.Length);

|                  Method |     Job |     Toolchain |            Mean | Ratio |
|------------------------ |-------- |-------------- |----------------:|------:|
|            StringFormat | Default | .NET Core 2.0 |       205.53 ns |  1.03 |
|            StringFormat | Default | .NET Core 2.1 |       154.70 ns |  0.77 |
|            StringFormat | Default | .NET Core 2.2 |       157.73 ns |  0.79 |
|            StringFormat | Default | .NET Core 3.0 |       121.52 ns |  0.61 |
|            StringFormat |     Clr |       Default |       199.90 ns |  1.00 |
|                         |         |               |                 |       |
|     StringBuilderAppend | Default | .NET Core 2.0 | 7,160,585.05 ns |  0.89 |
|     StringBuilderAppend | Default | .NET Core 2.1 | 3,812,760.76 ns |  0.47 |
|     StringBuilderAppend | Default | .NET Core 2.2 | 3,906,503.66 ns |  0.49 |
|     StringBuilderAppend | Default | .NET Core 3.0 | 2,965,858.10 ns |  0.37 |
|     StringBuilderAppend |     Clr |       Default | 8,052,928.96 ns |  1.00 |
|                         |         |               |                 |       |
|         Int32Formatting | Default | .NET Core 2.0 |        74.91 ns |  0.96 |
|         Int32Formatting | Default | .NET Core 2.1 |        38.70 ns |  0.49 |
|         Int32Formatting | Default | .NET Core 2.2 |        37.15 ns |  0.48 |
|         Int32Formatting | Default | .NET Core 3.0 |        35.79 ns |  0.46 |
|         Int32Formatting |     Clr |       Default |        78.21 ns |  1.00 |
|                         |         |               |                 |       |
|            Int32Parsing | Default | .NET Core 2.0 |       124.33 ns |  1.13 |
|            Int32Parsing | Default | .NET Core 2.1 |       104.20 ns |  0.94 |
|            Int32Parsing | Default | .NET Core 2.2 |        96.10 ns |  0.87 |
|            Int32Parsing | Default | .NET Core 3.0 |        22.37 ns |  0.20 |
|            Int32Parsing |     Clr |       Default |       110.41 ns |  1.00 |
|                         |         |               |                 |       |
|        DoubleFormatting | Default | .NET Core 2.0 |       574.91 ns |  0.99 |
|        DoubleFormatting | Default | .NET Core 2.1 |       232.86 ns |  0.40 |
|        DoubleFormatting | Default | .NET Core 2.2 |       236.79 ns |  0.41 |
|        DoubleFormatting | Default | .NET Core 3.0 |       268.78 ns |  0.46 |
|        DoubleFormatting |     Clr |       Default |       580.71 ns |  1.00 |
|                         |         |               |                 |       |
|    BigIntegerFormatting | Default | .NET Core 2.0 |        91.95 ns |  1.06 |
|    BigIntegerFormatting | Default | .NET Core 2.1 |        63.26 ns |  0.73 |
|    BigIntegerFormatting | Default | .NET Core 2.2 |        68.45 ns |  0.79 |
|    BigIntegerFormatting | Default | .NET Core 3.0 |        56.49 ns |  0.65 |
|    BigIntegerFormatting |     Clr |       Default |        87.08 ns |  1.00 |
|                         |         |               |                 |       |
|   DateTimeOffsetFormatR | Default | .NET Core 2.0 |       274.27 ns |  1.08 |
|   DateTimeOffsetFormatR | Default | .NET Core 2.1 |        82.75 ns |  0.33 |
|   DateTimeOffsetFormatR | Default | .NET Core 2.2 |        84.44 ns |  0.33 |
|   DateTimeOffsetFormatR | Default | .NET Core 3.0 |        80.27 ns |  0.32 |
|   DateTimeOffsetFormatR |     Clr |       Default |       253.87 ns |  1.00 |
|                         |         |               |                 |       |
|   DateTimeOffsetFormatO | Default | .NET Core 2.0 |       328.13 ns |  0.43 |
|   DateTimeOffsetFormatO | Default | .NET Core 2.1 |       131.13 ns |  0.17 |
|   DateTimeOffsetFormatO | Default | .NET Core 2.2 |       133.85 ns |  0.18 |
|   DateTimeOffsetFormatO | Default | .NET Core 3.0 |       129.25 ns |  0.17 |
|   DateTimeOffsetFormatO |     Clr |       Default |       754.35 ns |  1.00 |
|                         |         |               |                 |       |
| ConvertFromBase64String | Default | .NET Core 2.0 |     3,264.39 ns |  1.57 |
| ConvertFromBase64String | Default | .NET Core 2.1 |     1,722.97 ns |  0.83 |
| ConvertFromBase64String | Default | .NET Core 2.2 |     2,059.32 ns |  0.99 |
| ConvertFromBase64String | Default | .NET Core 3.0 |     1,648.02 ns |  0.79 |
| ConvertFromBase64String |     Clr |       Default |     2,079.47 ns |  1.00 |
|                         |         |               |                 |       |
|  ConvertFromBase64Chars | Default | .NET Core 2.0 |     3,243.92 ns |  1.56 |
|  ConvertFromBase64Chars | Default | .NET Core 2.1 |     1,738.59 ns |  0.84 |
|  ConvertFromBase64Chars | Default | .NET Core 2.2 |     2,056.39 ns |  0.99 |
|  ConvertFromBase64Chars | Default | .NET Core 3.0 |     1,645.37 ns |  0.79 |
|  ConvertFromBase64Chars |     Clr |       Default |     2,082.23 ns |  1.00 |

# Networking
        [Benchmark]
        [Obsolete]
        public long IPAddressNetworkToHostOrder() => IPAddress.NetworkToHostOrder(s_addr);

        [Benchmark]
        public string UriAllocations() => new Uri("http://127.0.0.1:80").AbsoluteUri;

        [Benchmark]
        public async Task SocketReceiveThenSend()
        {
            byte[] buffer = s_buffer;
            NetworkStream server = s_server, client = s_client;
            for (int i = 0; i < 1000; i++)
            {
                var read = server.ReadAsync(buffer, 0, buffer.Length);
                await client.WriteAsync(buffer, 0, buffer.Length);
                await read;
            }
        }

|                      Method |     Job |     Toolchain |               Mean | Ratio |
|---------------------------- |-------- |-------------- |-------------------:|------:|
| IPAddressNetworkToHostOrder | Default | .NET Core 2.0 |         11.3764 ns | 1.135 |
| IPAddressNetworkToHostOrder | Default | .NET Core 2.1 |          0.0000 ns | 0.000 |
| IPAddressNetworkToHostOrder | Default | .NET Core 2.2 |          0.0101 ns | 0.001 |
| IPAddressNetworkToHostOrder | Default | .NET Core 3.0 |          0.0145 ns | 0.001 |
| IPAddressNetworkToHostOrder |     Clr |       Default |         10.0213 ns | 1.000 |
|                             |         |               |                    |       |
|              UriAllocations | Default | .NET Core 2.0 |      1,123.4314 ns |  0.96 |
|              UriAllocations | Default | .NET Core 2.1 |        750.7269 ns |  0.64 |
|              UriAllocations | Default | .NET Core 2.2 |        745.5232 ns |  0.64 |
|              UriAllocations | Default | .NET Core 3.0 |        716.4724 ns |  0.62 |
|              UriAllocations |     Clr |       Default |      1,164.3810 ns |  1.00 |
|                             |         |               |                    |       |
|       SocketReceiveThenSend | Default | .NET Core 2.0 | 45,866,977.3333 ns |  0.74 |
|       SocketReceiveThenSend | Default | .NET Core 2.1 | 43,933,739.2857 ns |  0.71 |
|       SocketReceiveThenSend | Default | .NET Core 2.2 | 44,321,960.1190 ns |  0.72 |
|       SocketReceiveThenSend | Default | .NET Core 3.0 | 41,577,366.1111 ns |  0.67 |
|       SocketReceiveThenSend |     Clr |       Default | 61,856,634.5588 ns |  1.00 |

# Networking2

        [Benchmark]
        public async Task ConcurrentHttpsGets()
        {
            await Task.WhenAll(
                Enumerable
                .Range(0, Environment.ProcessorCount)
                .Select(async _ =>
                {
                    for (int i = 0; i < 100; i++)
                        await s_client.GetStringAsync(s_uri);
                }));
        }

|              Method |     Job |     Toolchain |      Mean | Ratio |
|-------------------- |-------- |-------------- |----------:|------:|
| ConcurrentHttpsGets | Default | .NET Core 2.0 | 171.82 ms |     ? |
| ConcurrentHttpsGets | Default | .NET Core 2.1 |  17.92 ms |     ? |
| ConcurrentHttpsGets | Default | .NET Core 2.2 |  17.09 ms |     ? |
| ConcurrentHttpsGets | Default | .NET Core 3.0 |  14.66 ms |     ? |
| ConcurrentHttpsGets |     Clr |       Default |        NA |     ? |

Benchmarks with issues:
  Networking2.ConcurrentHttpsGets: Clr(Runtime=Clr)
# AndMore

        [Benchmark]
        public void EnumerateFiles()
        {
			// Enumerate through local clone of https://github.com/dotnet/wpf.git
            foreach (string path in Directory.EnumerateFiles(@"D:\Dev\wpf", "*.wpf", SearchOption.AllDirectories))
            {
            }
        }

        [Benchmark]
        public byte[] DeriveBytes() => s_db.GetBytes(32);

        [Benchmark]
        public void GuidNewGuid() => Guid.NewGuid();

        [Benchmark]
        public void RegexCompiled() => s_phoneNumberRegex.IsMatch("555-867-5309");

|         Method |     Job |     Toolchain |             Mean | Ratio |
|--------------- |-------- |-------------- |-----------------:|------:|
| EnumerateFiles | Default | .NET Core 2.0 | 15,168,046.09 ns |  1.18 |
| EnumerateFiles | Default | .NET Core 2.1 |  5,671,803.02 ns |  0.43 |
| EnumerateFiles | Default | .NET Core 2.2 |  5,687,982.69 ns |  0.43 |
| EnumerateFiles | Default | .NET Core 3.0 |  5,486,062.11 ns |  0.41 |
| EnumerateFiles |     Clr |       Default | 13,272,838.73 ns |  1.00 |
|                |         |               |                  |       |
|    DeriveBytes | Default | .NET Core 2.0 | 10,622,494.27 ns |  0.34 |
|    DeriveBytes | Default | .NET Core 2.1 |  9,767,814.17 ns |  0.31 |
|    DeriveBytes | Default | .NET Core 2.2 |  9,831,424.27 ns |  0.32 |
|    DeriveBytes | Default | .NET Core 3.0 |  8,966,193.42 ns |  0.29 |
|    DeriveBytes |     Clr |       Default | 31,145,062.50 ns |  1.00 |
|                |         |               |                  |       |
|    GuidNewGuid | Default | .NET Core 2.0 |         95.62 ns |  1.01 |
|    GuidNewGuid | Default | .NET Core 2.1 |         92.76 ns |  0.98 |
|    GuidNewGuid | Default | .NET Core 2.2 |         93.39 ns |  0.99 |
|    GuidNewGuid | Default | .NET Core 3.0 |         93.95 ns |  1.00 |
|    GuidNewGuid |     Clr |       Default |         94.32 ns |  1.00 |
|                |         |               |                  |       |
|  RegexCompiled | Default | .NET Core 2.0 |        573.95 ns |  1.67 |
|  RegexCompiled | Default | .NET Core 2.1 |        326.02 ns |  0.95 |
|  RegexCompiled | Default | .NET Core 2.2 |        328.64 ns |  0.96 |
|  RegexCompiled | Default | .NET Core 3.0 |        302.92 ns |  0.88 |
|  RegexCompiled |     Clr |       Default |        343.54 ns |  1.00 |