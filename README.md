# Benchmark
Following [Stephen Toub](https://devblogs.microsoft.com/dotnet/author/toub/) [post from April 2018 on DotNetCore 2.0 and 2.1 benchmarks](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/), I have decided to update it with latest DotNetCore3.0 preview. Most of the following are taken from his post. 

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
| Method |     Job |     Toolchain |     Mean |     Error |    StdDev | Ratio | Rank |
|------- |-------- |-------------- |---------:|----------:|----------:|------:|-----:|
| Sha256 | Default | .NET Core 2.0 | 49.20 us | 0.2710 us | 0.2263 us |  0.52 |    1 |
| Sha256 | Default | .NET Core 2.1 | 49.18 us | 0.4893 us | 0.4577 us |  0.52 |    1 |
| Sha256 | Default | .NET Core 2.2 | 48.57 us | 0.4054 us | 0.3792 us |  0.52 |    1 |
| Sha256 | Default | .NET Core 3.0 | 48.95 us | 0.2999 us | 0.2659 us |  0.52 |    1 |
| Sha256 |     Clr |       Default | 94.14 us | 0.4557 us | 0.4039 us |  1.00 |    2 |
|        |         |               |          |           |           |       |      |
|    Md5 | Default | .NET Core 2.0 | 23.01 us | 0.1937 us | 0.1812 us |  0.95 |    1 |
|    Md5 | Default | .NET Core 2.1 | 23.11 us | 0.0741 us | 0.0693 us |  0.95 |    1 |
|    Md5 | Default | .NET Core 2.2 | 23.18 us | 0.1490 us | 0.1164 us |  0.95 |    1 |
|    Md5 | Default | .NET Core 3.0 | 23.15 us | 0.1127 us | 0.1054 us |  0.95 |    1 |
|    Md5 |     Clr |       Default | 24.35 us | 0.1706 us | 0.1596 us |  1.00 |    2 |

# MyBench
|     Method |     Job |     Toolchain |        Mean |      Error |     StdDev | Ratio | Rank |
|----------- |-------- |-------------- |------------:|-----------:|-----------:|------:|-----:|
|      Where | Default | .NET Core 2.0 |    45.52 us |  0.2824 us |  0.2642 us |  0.37 |    1 |
|      Where | Default | .NET Core 2.1 |    50.90 us |  0.8996 us |  0.8415 us |  0.41 |    3 |
|      Where | Default | .NET Core 2.2 |    51.38 us |  0.9879 us |  0.9240 us |  0.41 |    3 |
|      Where | Default | .NET Core 3.0 |    49.06 us |  1.0560 us |  0.9878 us |  0.40 |    2 |
|      Where |     Clr |       Default |   123.55 us |  2.5350 us |  3.6357 us |  1.00 |    4 |
|            |         |               |             |            |            |       |      |
|  SortedSet | Default | .NET Core 2.0 |   617.23 us |  7.9080 us |  7.3971 us |  0.35 |    2 |
|  SortedSet | Default | .NET Core 2.1 |   591.40 us | 11.3189 us | 12.1111 us |  0.34 |    1 |
|  SortedSet | Default | .NET Core 2.2 |   593.96 us | 11.1707 us | 12.4162 us |  0.34 |    1 |
|  SortedSet | Default | .NET Core 3.0 |   587.20 us |  4.9534 us |  4.6334 us |  0.33 |    1 |
|  SortedSet |     Clr |       Default | 1,754.27 us | 34.0348 us | 31.8362 us |  1.00 |    3 |
|            |         |               |             |            |            |       |      |
|        Sin | Default | .NET Core 2.0 |   200.04 us |  1.7726 us |  1.6581 us |  0.41 |    1 |
|        Sin | Default | .NET Core 2.1 |   203.59 us |  3.1986 us |  2.9920 us |  0.41 |    1 |
|        Sin | Default | .NET Core 2.2 |   200.15 us |  3.7693 us |  3.5258 us |  0.41 |    1 |
|        Sin | Default | .NET Core 3.0 |   204.11 us |  3.9899 us |  3.7322 us |  0.41 |    1 |
|        Sin |     Clr |       Default |   493.33 us |  3.3153 us |  3.1012 us |  1.00 |    2 |
|            |         |               |             |            |            |       |      |
| Dictionary | Default | .NET Core 2.0 |   310.44 us |  5.8807 us |  7.0006 us |  1.07 |    4 |
| Dictionary | Default | .NET Core 2.1 |   222.51 us |  4.3450 us |  6.8917 us |  0.77 |    1 |
| Dictionary | Default | .NET Core 2.2 |   218.45 us |  4.2955 us |  7.9620 us |  0.77 |    1 |
| Dictionary | Default | .NET Core 3.0 |   255.24 us |  4.9871 us |  7.6158 us |  0.88 |    2 |
| Dictionary |     Clr |       Default |   289.78 us |  6.4472 us |  6.0307 us |  1.00 |    3 |

# JIT
|                  Method |     Job |     Toolchain |          Mean |       Error |      StdDev | Ratio | Rank |
|------------------------ |-------- |-------------- |--------------:|------------:|------------:|------:|-----:|
|   EqualityComparerInt32 | Default | .NET Core 2.0 |     2.8902 ns |   0.0569 ns |   0.1097 ns |  1.19 |    4 |
|   EqualityComparerInt32 | Default | .NET Core 2.1 |     1.0809 ns |   0.0295 ns |   0.0857 ns |  0.43 |    1 |
|   EqualityComparerInt32 | Default | .NET Core 2.2 |     1.0875 ns |   0.0317 ns |   0.0920 ns |  0.46 |    1 |
|   EqualityComparerInt32 | Default | .NET Core 3.0 |     1.1691 ns |   0.0317 ns |   0.0910 ns |  0.48 |    2 |
|   EqualityComparerInt32 |     Clr |       Default |     2.4445 ns |   0.0486 ns |   0.0770 ns |  1.00 |    3 |
|                         |         |               |               |             |             |       |      |
| DictionaryContainsValue | Default | .NET Core 2.0 | 5,301.4990 ns | 104.3500 ns | 177.1942 ns |  1.20 |    4 |
| DictionaryContainsValue | Default | .NET Core 2.1 | 2,156.4981 ns |  59.2500 ns | 164.1814 ns |  0.49 |    1 |
| DictionaryContainsValue | Default | .NET Core 2.2 | 2,251.0265 ns |  44.8440 ns |  94.5911 ns |  0.51 |    2 |
| DictionaryContainsValue | Default | .NET Core 3.0 | 2,292.9484 ns |  45.5859 ns |  77.4083 ns |  0.52 |    2 |
| DictionaryContainsValue |     Clr |       Default | 4,477.1106 ns | 206.1363 ns | 588.1184 ns |  1.00 |    3 |
|                         |         |               |               |             |             |       |      |
|             EnumHasFlag | Default | .NET Core 2.0 |    20.2835 ns |   0.3841 ns |   0.4572 ns |  0.86 |    3 |
|             EnumHasFlag | Default | .NET Core 2.1 |     0.8173 ns |   0.0162 ns |   0.0304 ns |  0.03 |    2 |
|             EnumHasFlag | Default | .NET Core 2.2 |     0.8065 ns |   0.0156 ns |   0.0214 ns |  0.03 |    2 |
|             EnumHasFlag | Default | .NET Core 3.0 |     0.4163 ns |   0.0083 ns |   0.0181 ns |  0.02 |    1 |
|             EnumHasFlag |     Clr |       Default |    23.7034 ns |   0.3211 ns |   0.3004 ns |  1.00 |    4 |
|                         |         |               |               |             |             |       |      |
|          LoopBodyLayout | Default | .NET Core 2.0 |    71.5786 ns |   0.9483 ns |   0.8871 ns |  1.94 |    3 |
|          LoopBodyLayout | Default | .NET Core 2.1 |    37.6488 ns |   0.6049 ns |   0.5658 ns |  1.02 |    1 |
|          LoopBodyLayout | Default | .NET Core 2.2 |    37.1289 ns |   0.4704 ns |   0.4400 ns |  1.01 |    1 |
|          LoopBodyLayout | Default | .NET Core 3.0 |    38.6321 ns |   0.7802 ns |   0.7298 ns |  1.05 |    2 |
|          LoopBodyLayout |     Clr |       Default |    36.8877 ns |   0.4995 ns |   0.4672 ns |  1.00 |    1 |
|                         |         |               |               |             |             |       |      |
|        LoweringTESTtoBT | Default | .NET Core 2.0 |     1.8056 ns |   0.0258 ns |   0.0241 ns |  1.37 |    3 |
|        LoweringTESTtoBT | Default | .NET Core 2.1 |     1.2312 ns |   0.0245 ns |   0.0409 ns |  0.93 |    1 |
|        LoweringTESTtoBT | Default | .NET Core 2.2 |     1.2022 ns |   0.0235 ns |   0.0373 ns |  0.90 |    1 |
|        LoweringTESTtoBT | Default | .NET Core 3.0 |     1.3039 ns |   0.0079 ns |   0.0070 ns |  0.99 |    2 |
|        LoweringTESTtoBT |     Clr |       Default |     1.3225 ns |   0.0093 ns |   0.0087 ns |  1.00 |    2 |
|                         |         |               |               |             |             |       |      |
|       BoxingAllocations | Default | .NET Core 2.0 |    18.1086 ns |   0.3985 ns |   0.3914 ns |  1.08 |    4 |
|       BoxingAllocations | Default | .NET Core 2.1 |     1.8790 ns |   0.0679 ns |   0.0635 ns |  0.11 |    1 |
|       BoxingAllocations | Default | .NET Core 2.2 |     2.0924 ns |   0.0381 ns |   0.0357 ns |  0.12 |    2 |
|       BoxingAllocations | Default | .NET Core 3.0 |     2.1033 ns |   0.0560 ns |   0.0524 ns |  0.12 |    2 |
|       BoxingAllocations |     Clr |       Default |    16.8597 ns |   0.3755 ns |   0.4018 ns |  1.00 |    3 |

# Threading
|                                Method |     Job |     Toolchain |               Mean |              Error |             StdDev | Ratio | Rank |
|-------------------------------------- |-------- |-------------- |-------------------:|-------------------:|-------------------:|------:|-----:|
|                         ThreadStatics | Default | .NET Core 2.0 |           9.564 ns |          0.1377 ns |          0.1288 ns |  1.17 |    3 |
|                         ThreadStatics | Default | .NET Core 2.1 |           8.093 ns |          0.1601 ns |          0.1843 ns |  0.99 |    2 |
|                         ThreadStatics | Default | .NET Core 2.2 |           8.068 ns |          0.1503 ns |          0.1406 ns |  0.99 |    2 |
|                         ThreadStatics | Default | .NET Core 3.0 |           7.779 ns |          0.0626 ns |          0.0555 ns |  0.95 |    1 |
|                         ThreadStatics |     Clr |       Default |           8.172 ns |          0.0819 ns |          0.0766 ns |  1.00 |    2 |
|                                       |         |               |                    |                    |                    |       |      |
|                       TimerContention | Default | .NET Core 2.0 | 466,720,876.471 ns | 10,889,183.9536 ns | 11,182,390.2081 ns |  1.62 |    4 |
|                       TimerContention | Default | .NET Core 2.1 | 173,399,976.596 ns |  3,466,881.2497 ns |  6,761,885.6195 ns |  0.61 |    2 |
|                       TimerContention | Default | .NET Core 2.2 | 140,698,656.731 ns |  2,745,179.8590 ns |  3,757,633.1771 ns |  0.49 |    1 |
|                       TimerContention | Default | .NET Core 3.0 | 152,115,641.333 ns |  7,406,157.2037 ns | 21,837,221.6152 ns |  0.64 |    1 |
|                       TimerContention |     Clr |       Default | 288,566,760.714 ns |  3,064,382.4076 ns |  2,716,495.1865 ns |  1.00 |    3 |
|                                       |         |               |                    |                    |                    |       |      |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.0 |         127.337 ns |          2.2559 ns |          2.1102 ns |  0.92 |    4 |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.1 |          68.197 ns |          1.0597 ns |          0.9912 ns |  0.49 |    1 |
|   SerialCancellationTokenRegistration | Default | .NET Core 2.2 |          74.352 ns |          1.5255 ns |          1.8735 ns |  0.54 |    3 |
|   SerialCancellationTokenRegistration | Default | .NET Core 3.0 |          69.990 ns |          0.3559 ns |          0.3329 ns |  0.50 |    2 |
|   SerialCancellationTokenRegistration |     Clr |       Default |         138.689 ns |          0.5225 ns |          0.4887 ns |  1.00 |    5 |
|                                       |         |               |                    |                    |                    |       |      |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.0 |          39.402 ns |          0.5082 ns |          0.4754 ns |  0.92 |    4 |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.1 |          14.621 ns |          0.2341 ns |          0.2190 ns |  0.34 |    1 |
| ParallelCancellationTokenRegistration | Default | .NET Core 2.2 |          16.755 ns |          0.3349 ns |          0.5686 ns |  0.39 |    2 |
| ParallelCancellationTokenRegistration | Default | .NET Core 3.0 |          20.286 ns |          0.5275 ns |          1.5388 ns |  0.49 |    3 |
| ParallelCancellationTokenRegistration |     Clr |       Default |          42.874 ns |          0.8045 ns |          0.7525 ns |  1.00 |    5 |
|                                       |         |               |                    |                    |                    |       |      |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.0 |          27.305 ns |          0.5444 ns |          0.5093 ns |  0.40 |    4 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.1 |          18.015 ns |          0.2368 ns |          0.2215 ns |  0.26 |    3 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 2.2 |          16.603 ns |          0.1224 ns |          0.1145 ns |  0.24 |    1 |
|            AsyncMethodAwaitInvocation | Default | .NET Core 3.0 |          17.331 ns |          0.0749 ns |          0.0701 ns |  0.25 |    2 |
|            AsyncMethodAwaitInvocation |     Clr |       Default |          69.111 ns |          0.3627 ns |          0.3215 ns |  1.00 |    5 |

# String
|                     Method |     Job |     Toolchain |         Mean |       Error |      StdDev | Ratio | Rank |
|--------------------------- |-------- |-------------- |-------------:|------------:|------------:|------:|-----:|
|               StringEquals | Default | .NET Core 2.0 |     18.90 ns |   0.0737 ns |   0.0689 ns |  0.89 |    4 |
|               StringEquals | Default | .NET Core 2.1 |     12.00 ns |   0.0792 ns |   0.0740 ns |  0.56 |    2 |
|               StringEquals | Default | .NET Core 2.2 |     11.51 ns |   0.0844 ns |   0.0789 ns |  0.54 |    1 |
|               StringEquals | Default | .NET Core 3.0 |     12.18 ns |   0.0936 ns |   0.0875 ns |  0.57 |    3 |
|               StringEquals |     Clr |       Default |     21.24 ns |   0.1458 ns |   0.1292 ns |  1.00 |    5 |
|                            |         |               |              |             |             |       |      |
|              StringIndexOf | Default | .NET Core 2.0 |     48.88 ns |   0.3873 ns |   0.3623 ns |  0.48 |    3 |
|              StringIndexOf | Default | .NET Core 2.1 |     19.01 ns |   0.0861 ns |   0.0806 ns |  0.19 |    2 |
|              StringIndexOf | Default | .NET Core 2.2 |     19.03 ns |   0.1120 ns |   0.0993 ns |  0.19 |    2 |
|              StringIndexOf | Default | .NET Core 3.0 |     18.69 ns |   0.0852 ns |   0.0797 ns |  0.18 |    1 |
|              StringIndexOf |     Clr |       Default |    101.66 ns |   0.5142 ns |   0.4810 ns |  1.00 |    4 |
|                            |         |               |              |             |             |       |      |
|                 IndexOfAny | Default | .NET Core 2.0 |    102.95 ns |   0.5614 ns |   0.5252 ns |  0.74 |    4 |
|                 IndexOfAny | Default | .NET Core 2.1 |     38.37 ns |   0.1317 ns |   0.1232 ns |  0.28 |    2 |
|                 IndexOfAny | Default | .NET Core 2.2 |     42.32 ns |   0.2544 ns |   0.2380 ns |  0.30 |    3 |
|                 IndexOfAny | Default | .NET Core 3.0 |     18.87 ns |   0.0969 ns |   0.0906 ns |  0.14 |    1 |
|                 IndexOfAny |     Clr |       Default |    138.91 ns |   0.6674 ns |   0.6243 ns |  1.00 |    5 |
|                            |         |               |              |             |             |       |      |
| StringToLowerChangesNeeded | Default | .NET Core 2.0 |    199.77 ns |   0.9599 ns |   0.8979 ns |  0.94 |    4 |
| StringToLowerChangesNeeded | Default | .NET Core 2.1 |     95.21 ns |   0.4730 ns |   0.4424 ns |  0.45 |    2 |
| StringToLowerChangesNeeded | Default | .NET Core 2.2 |    100.46 ns |   0.4273 ns |   0.3336 ns |  0.47 |    3 |
| StringToLowerChangesNeeded | Default | .NET Core 3.0 |     59.11 ns |   0.3998 ns |   0.3544 ns |  0.28 |    1 |
| StringToLowerChangesNeeded |     Clr |       Default |    211.58 ns |   1.2790 ns |   1.1338 ns |  1.00 |    5 |
|                            |         |               |              |             |             |       |      |
|  StringToLowerAlreadyCased | Default | .NET Core 2.0 |    200.60 ns |   1.2173 ns |   1.1387 ns |  0.93 |    4 |
|  StringToLowerAlreadyCased | Default | .NET Core 2.1 |     68.88 ns |   0.3604 ns |   0.3371 ns |  0.32 |    3 |
|  StringToLowerAlreadyCased | Default | .NET Core 2.2 |     66.86 ns |   0.4231 ns |   0.3303 ns |  0.31 |    2 |
|  StringToLowerAlreadyCased | Default | .NET Core 3.0 |     41.64 ns |   0.2625 ns |   0.2456 ns |  0.19 |    1 |
|  StringToLowerAlreadyCased |     Clr |       Default |    214.63 ns |   1.0258 ns |   0.9595 ns |  1.00 |    5 |
|                            |         |               |              |             |             |       |      |
|                StringSplit | Default | .NET Core 2.0 |    358.89 ns |   5.6969 ns |   4.7572 ns |  1.00 |    2 |
|                StringSplit | Default | .NET Core 2.1 |    256.43 ns |   1.0946 ns |   1.0239 ns |  0.71 |    1 |
|                StringSplit | Default | .NET Core 2.2 |    254.56 ns |   1.9359 ns |   1.8109 ns |  0.71 |    1 |
|                StringSplit | Default | .NET Core 3.0 |    257.87 ns |   1.8935 ns |   1.7712 ns |  0.72 |    1 |
|                StringSplit |     Clr |       Default |    358.82 ns |   1.8090 ns |   1.6921 ns |  1.00 |    2 |
|                            |         |               |              |             |             |       |      |
| StringConcatCharEnumerable | Default | .NET Core 2.0 | 22,182.96 ns | 159.5897 ns | 149.2803 ns |  0.77 |    4 |
| StringConcatCharEnumerable | Default | .NET Core 2.1 | 17,184.09 ns | 104.9031 ns |  98.1264 ns |  0.59 |    3 |
| StringConcatCharEnumerable | Default | .NET Core 2.2 | 16,471.14 ns | 130.1933 ns | 121.7829 ns |  0.57 |    2 |
| StringConcatCharEnumerable | Default | .NET Core 3.0 | 14,554.08 ns |  51.4388 ns |  48.1159 ns |  0.50 |    1 |
| StringConcatCharEnumerable |     Clr |       Default | 28,958.84 ns | 187.2654 ns | 175.1682 ns |  1.00 |    5 |

# FormattingAndParsing
|                  Method |     Job |     Toolchain |            Mean |          Error |         StdDev | Ratio | Rank |
|------------------------ |-------- |-------------- |----------------:|---------------:|---------------:|------:|-----:|
|            StringFormat | Default | .NET Core 2.0 |       205.53 ns |      1.2487 ns |      1.1069 ns |  1.03 |    5 |
|            StringFormat | Default | .NET Core 2.1 |       154.70 ns |      0.7733 ns |      0.6856 ns |  0.77 |    2 |
|            StringFormat | Default | .NET Core 2.2 |       157.73 ns |      1.0409 ns |      0.9737 ns |  0.79 |    3 |
|            StringFormat | Default | .NET Core 3.0 |       121.52 ns |      0.2735 ns |      0.2425 ns |  0.61 |    1 |
|            StringFormat |     Clr |       Default |       199.90 ns |      0.9454 ns |      0.8381 ns |  1.00 |    4 |
|                         |         |               |                 |                |                |       |      |
|     StringBuilderAppend | Default | .NET Core 2.0 | 7,160,585.05 ns | 20,199.7636 ns | 18,894.8714 ns |  0.89 |    4 |
|     StringBuilderAppend | Default | .NET Core 2.1 | 3,812,760.76 ns | 15,628.8993 ns | 14,619.2821 ns |  0.47 |    2 |
|     StringBuilderAppend | Default | .NET Core 2.2 | 3,906,503.66 ns | 22,204.8467 ns | 20,770.4274 ns |  0.49 |    3 |
|     StringBuilderAppend | Default | .NET Core 3.0 | 2,965,858.10 ns |  9,989.3908 ns |  9,344.0824 ns |  0.37 |    1 |
|     StringBuilderAppend |     Clr |       Default | 8,052,928.96 ns | 46,313.6890 ns | 43,321.8534 ns |  1.00 |    5 |
|                         |         |               |                 |                |                |       |      |
|         Int32Formatting | Default | .NET Core 2.0 |        74.91 ns |      0.4458 ns |      0.4170 ns |  0.96 |    4 |
|         Int32Formatting | Default | .NET Core 2.1 |        38.70 ns |      0.2346 ns |      0.2079 ns |  0.49 |    3 |
|         Int32Formatting | Default | .NET Core 2.2 |        37.15 ns |      0.1798 ns |      0.1682 ns |  0.48 |    2 |
|         Int32Formatting | Default | .NET Core 3.0 |        35.79 ns |      0.2815 ns |      0.2633 ns |  0.46 |    1 |
|         Int32Formatting |     Clr |       Default |        78.21 ns |      0.5662 ns |      0.5296 ns |  1.00 |    5 |
|                         |         |               |                 |                |                |       |      |
|            Int32Parsing | Default | .NET Core 2.0 |       124.33 ns |      0.9005 ns |      0.8423 ns |  1.13 |    5 |
|            Int32Parsing | Default | .NET Core 2.1 |       104.20 ns |      0.3925 ns |      0.3277 ns |  0.94 |    3 |
|            Int32Parsing | Default | .NET Core 2.2 |        96.10 ns |      0.2880 ns |      0.2694 ns |  0.87 |    2 |
|            Int32Parsing | Default | .NET Core 3.0 |        22.37 ns |      0.0670 ns |      0.0627 ns |  0.20 |    1 |
|            Int32Parsing |     Clr |       Default |       110.41 ns |      0.6124 ns |      0.5728 ns |  1.00 |    4 |
|                         |         |               |                 |                |                |       |      |
|        DoubleFormatting | Default | .NET Core 2.0 |       574.91 ns |      1.5858 ns |      1.4833 ns |  0.99 |    4 |
|        DoubleFormatting | Default | .NET Core 2.1 |       232.86 ns |      1.3149 ns |      1.2300 ns |  0.40 |    1 |
|        DoubleFormatting | Default | .NET Core 2.2 |       236.79 ns |      1.1060 ns |      1.0345 ns |  0.41 |    2 |
|        DoubleFormatting | Default | .NET Core 3.0 |       268.78 ns |      4.7626 ns |      4.4550 ns |  0.46 |    3 |
|        DoubleFormatting |     Clr |       Default |       580.71 ns |      3.5272 ns |      3.2993 ns |  1.00 |    4 |
|                         |         |               |                 |                |                |       |      |
|    BigIntegerFormatting | Default | .NET Core 2.0 |        91.95 ns |      0.3926 ns |      0.3672 ns |  1.06 |    5 |
|    BigIntegerFormatting | Default | .NET Core 2.1 |        63.26 ns |      0.3258 ns |      0.3047 ns |  0.73 |    2 |
|    BigIntegerFormatting | Default | .NET Core 2.2 |        68.45 ns |      0.2818 ns |      0.2636 ns |  0.79 |    3 |
|    BigIntegerFormatting | Default | .NET Core 3.0 |        56.49 ns |      0.3583 ns |      0.3351 ns |  0.65 |    1 |
|    BigIntegerFormatting |     Clr |       Default |        87.08 ns |      2.4328 ns |      2.3893 ns |  1.00 |    4 |
|                         |         |               |                 |                |                |       |      |
|   DateTimeOffsetFormatR | Default | .NET Core 2.0 |       274.27 ns |      2.5943 ns |      2.4267 ns |  1.08 |    5 |
|   DateTimeOffsetFormatR | Default | .NET Core 2.1 |        82.75 ns |      0.4000 ns |      0.3741 ns |  0.33 |    2 |
|   DateTimeOffsetFormatR | Default | .NET Core 2.2 |        84.44 ns |      0.6511 ns |      0.6091 ns |  0.33 |    3 |
|   DateTimeOffsetFormatR | Default | .NET Core 3.0 |        80.27 ns |      0.4601 ns |      0.3842 ns |  0.32 |    1 |
|   DateTimeOffsetFormatR |     Clr |       Default |       253.87 ns |      1.1434 ns |      1.0695 ns |  1.00 |    4 |
|                         |         |               |                 |                |                |       |      |
|   DateTimeOffsetFormatO | Default | .NET Core 2.0 |       328.13 ns |      3.3498 ns |      3.1334 ns |  0.43 |    4 |
|   DateTimeOffsetFormatO | Default | .NET Core 2.1 |       131.13 ns |      0.5091 ns |      0.4513 ns |  0.17 |    2 |
|   DateTimeOffsetFormatO | Default | .NET Core 2.2 |       133.85 ns |      0.5987 ns |      0.5307 ns |  0.18 |    3 |
|   DateTimeOffsetFormatO | Default | .NET Core 3.0 |       129.25 ns |      0.7155 ns |      0.6693 ns |  0.17 |    1 |
|   DateTimeOffsetFormatO |     Clr |       Default |       754.35 ns |      3.0447 ns |      2.8480 ns |  1.00 |    5 |
|                         |         |               |                 |                |                |       |      |
| ConvertFromBase64String | Default | .NET Core 2.0 |     3,264.39 ns |     63.5665 ns |     62.4308 ns |  1.57 |    4 |
| ConvertFromBase64String | Default | .NET Core 2.1 |     1,722.97 ns |      7.6517 ns |      7.1574 ns |  0.83 |    2 |
| ConvertFromBase64String | Default | .NET Core 2.2 |     2,059.32 ns |     17.0744 ns |     15.9714 ns |  0.99 |    3 |
| ConvertFromBase64String | Default | .NET Core 3.0 |     1,648.02 ns |     10.3609 ns |      9.6915 ns |  0.79 |    1 |
| ConvertFromBase64String |     Clr |       Default |     2,079.47 ns |     14.5141 ns |     13.5765 ns |  1.00 |    3 |
|                         |         |               |                 |                |                |       |      |
|  ConvertFromBase64Chars | Default | .NET Core 2.0 |     3,243.92 ns |     15.9009 ns |     14.8738 ns |  1.56 |    4 |
|  ConvertFromBase64Chars | Default | .NET Core 2.1 |     1,738.59 ns |      9.4698 ns |      8.3947 ns |  0.84 |    2 |
|  ConvertFromBase64Chars | Default | .NET Core 2.2 |     2,056.39 ns |     10.3447 ns |      9.6764 ns |  0.99 |    3 |
|  ConvertFromBase64Chars | Default | .NET Core 3.0 |     1,645.37 ns |      9.2786 ns |      8.6792 ns |  0.79 |    1 |
|  ConvertFromBase64Chars |     Clr |       Default |     2,082.23 ns |     14.1930 ns |     13.2761 ns |  1.00 |    3 |

# Networking
|                      Method |     Job |     Toolchain |               Mean |             Error |            StdDev | Ratio | Rank |
|---------------------------- |-------- |-------------- |-------------------:|------------------:|------------------:|------:|-----:|
| IPAddressNetworkToHostOrder | Default | .NET Core 2.0 |         11.3764 ns |         0.0654 ns |         0.0612 ns | 1.135 |    4 |
| IPAddressNetworkToHostOrder | Default | .NET Core 2.1 |          0.0000 ns |         0.0000 ns |         0.0000 ns | 0.000 |    1 |
| IPAddressNetworkToHostOrder | Default | .NET Core 2.2 |          0.0101 ns |         0.0070 ns |         0.0066 ns | 0.001 |    2 |
| IPAddressNetworkToHostOrder | Default | .NET Core 3.0 |          0.0145 ns |         0.0104 ns |         0.0097 ns | 0.001 |    2 |
| IPAddressNetworkToHostOrder |     Clr |       Default |         10.0213 ns |         0.0806 ns |         0.0753 ns | 1.000 |    3 |
|                             |         |               |                    |                   |                   |       |      |
|              UriAllocations | Default | .NET Core 2.0 |      1,123.4314 ns |         9.2681 ns |         8.6693 ns |  0.96 |    3 |
|              UriAllocations | Default | .NET Core 2.1 |        750.7269 ns |         3.4076 ns |         3.1875 ns |  0.64 |    2 |
|              UriAllocations | Default | .NET Core 2.2 |        745.5232 ns |         4.2014 ns |         3.9299 ns |  0.64 |    2 |
|              UriAllocations | Default | .NET Core 3.0 |        716.4724 ns |         4.2971 ns |         4.0195 ns |  0.62 |    1 |
|              UriAllocations |     Clr |       Default |      1,164.3810 ns |         8.1489 ns |         7.6225 ns |  1.00 |    4 |
|                             |         |               |                    |                   |                   |       |      |
|       SocketReceiveThenSend | Default | .NET Core 2.0 | 45,866,977.3333 ns |   862,066.0543 ns |   806,377.1220 ns |  0.74 |    3 |
|       SocketReceiveThenSend | Default | .NET Core 2.1 | 43,933,739.2857 ns |   699,698.4740 ns |   620,264.4722 ns |  0.71 |    2 |
|       SocketReceiveThenSend | Default | .NET Core 2.2 | 44,321,960.1190 ns |   588,227.5136 ns |   521,448.3693 ns |  0.72 |    2 |
|       SocketReceiveThenSend | Default | .NET Core 3.0 | 41,577,366.1111 ns |   637,375.2303 ns |   596,201.1859 ns |  0.67 |    1 |
|       SocketReceiveThenSend |     Clr |       Default | 61,856,634.5588 ns | 1,227,863.1179 ns | 1,260,925.0211 ns |  1.00 |    4 |

# Networking2
|              Method |     Job |     Toolchain |      Mean |     Error |    StdDev | Ratio | Rank |
|-------------------- |-------- |-------------- |----------:|----------:|----------:|------:|-----:|
| ConcurrentHttpsGets | Default | .NET Core 2.0 | 171.82 ms | 3.2813 ms | 3.9062 ms |     ? |    4 |
| ConcurrentHttpsGets | Default | .NET Core 2.1 |  17.92 ms | 0.2355 ms | 0.2087 ms |     ? |    3 |
| ConcurrentHttpsGets | Default | .NET Core 2.2 |  17.09 ms | 0.1083 ms | 0.1013 ms |     ? |    2 |
| ConcurrentHttpsGets | Default | .NET Core 3.0 |  14.66 ms | 0.0825 ms | 0.0772 ms |     ? |    1 |
| ConcurrentHttpsGets |     Clr |       Default |        NA |        NA |        NA |     ? |    ? |

Benchmarks with issues:
  Networking2.ConcurrentHttpsGets: Clr(Runtime=Clr)
# AndMore
|         Method |     Job |     Toolchain |             Mean |           Error |          StdDev |           Median | Ratio | Rank |
|--------------- |-------- |-------------- |-----------------:|----------------:|----------------:|-----------------:|------:|-----:|
| EnumerateFiles | Default | .NET Core 2.0 | 15,168,046.09 ns | 301,514.4715 ns | 674,380.3219 ns | 14,902,099.22 ns |  1.18 |    4 |
| EnumerateFiles | Default | .NET Core 2.1 |  5,671,803.02 ns |  32,065.1911 ns |  29,993.7996 ns |  5,662,130.47 ns |  0.43 |    2 |
| EnumerateFiles | Default | .NET Core 2.2 |  5,687,982.69 ns |  37,287.1180 ns |  31,136.4323 ns |  5,693,364.06 ns |  0.43 |    2 |
| EnumerateFiles | Default | .NET Core 3.0 |  5,486,062.11 ns |  53,750.2686 ns |  41,964.6742 ns |  5,472,049.22 ns |  0.41 |    1 |
| EnumerateFiles |     Clr |       Default | 13,272,838.73 ns |  55,278.1604 ns |  49,002.6493 ns | 13,264,251.56 ns |  1.00 |    3 |
|                |         |               |                  |                 |                 |                  |       |      |
|    DeriveBytes | Default | .NET Core 2.0 | 10,622,494.27 ns |  48,030.0505 ns |  44,927.3391 ns | 10,625,339.06 ns |  0.34 |    3 |
|    DeriveBytes | Default | .NET Core 2.1 |  9,767,814.17 ns |  90,633.1319 ns |  80,343.9107 ns |  9,747,663.28 ns |  0.31 |    2 |
|    DeriveBytes | Default | .NET Core 2.2 |  9,831,424.27 ns |  47,293.0973 ns |  44,237.9926 ns |  9,827,928.13 ns |  0.32 |    2 |
|    DeriveBytes | Default | .NET Core 3.0 |  8,966,193.42 ns |  42,796.3563 ns |  37,937.8551 ns |  8,969,565.63 ns |  0.29 |    1 |
|    DeriveBytes |     Clr |       Default | 31,145,062.50 ns | 236,013.2714 ns | 220,766.9605 ns | 31,120,050.00 ns |  1.00 |    4 |
|                |         |               |                  |                 |                 |                  |       |      |
|    GuidNewGuid | Default | .NET Core 2.0 |         95.62 ns |       0.5829 ns |       0.5167 ns |         95.48 ns |  1.01 |    1 |
|    GuidNewGuid | Default | .NET Core 2.1 |         92.76 ns |       0.2648 ns |       0.2477 ns |         92.72 ns |  0.98 |    1 |
|    GuidNewGuid | Default | .NET Core 2.2 |         93.39 ns |       0.8988 ns |       0.8407 ns |         93.56 ns |  0.99 |    1 |
|    GuidNewGuid | Default | .NET Core 3.0 |         93.95 ns |       0.5616 ns |       0.5253 ns |         94.08 ns |  1.00 |    1 |
|    GuidNewGuid |     Clr |       Default |         94.32 ns |       0.5870 ns |       0.5491 ns |         94.19 ns |  1.00 |    1 |
|                |         |               |                  |                 |                 |                  |       |      |
|  RegexCompiled | Default | .NET Core 2.0 |        573.95 ns |       2.7249 ns |       2.4155 ns |        573.61 ns |  1.67 |    4 |
|  RegexCompiled | Default | .NET Core 2.1 |        326.02 ns |       2.3026 ns |       2.1538 ns |        326.34 ns |  0.95 |    2 |
|  RegexCompiled | Default | .NET Core 2.2 |        328.64 ns |       2.2583 ns |       2.1124 ns |        328.68 ns |  0.96 |    2 |
|  RegexCompiled | Default | .NET Core 3.0 |        302.92 ns |       6.5920 ns |       6.1662 ns |        300.11 ns |  0.88 |    1 |
|  RegexCompiled |     Clr |       Default |        343.54 ns |       3.9856 ns |       3.5331 ns |        342.10 ns |  1.00 |    3 |