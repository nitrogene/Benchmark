using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;
using BenchmarkDotNet.Environments;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Net.Http;
using System.Reflection;

// dotnet run -c Release -f net472

namespace BenchmarkCore
{
    public class MainConfig : ManualConfig
    {
        public MainConfig()
        {
            Add(Job.Default.With(Platform.X64).With(CsProjCoreToolchain.NetCoreApp20));
            Add(Job.Default.With(Platform.X64).With(CsProjCoreToolchain.NetCoreApp21));
            Add(Job.Default.With(Platform.X64).With(CsProjCoreToolchain.NetCoreApp22));
            Add(Job.Default.With(Platform.X64).With(CsProjCoreToolchain.NetCoreApp30));
        }
    }

    // https://github.com/dotnet/BenchmarkDotNet
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class Md5VsSha256
    {
        private SHA256 sha256 = SHA256.Create();
        private MD5 md5 = MD5.Create();
        private byte[] data;

        [Params(10000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);
    }

    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class MyBench
    {
        private byte[] data;
        private Dictionary<byte, int> dic;

        [Params(10000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);

            dic = new Dictionary<byte, int>();
            for (byte i = 0; i <= byte.MaxValue; ++i)
            {
                dic[i] = 0;
            }
        }

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
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class JIT
    {
        private static int[] s_intArray = Enumerable.Range(0, 100_000_000).ToArray();

        [Benchmark(OperationsPerInvoke = 100_000_000)]
        public int EqualityComparerInt32()
        {
            int[] items = s_intArray;

            for (int i = 0; i < items.Length; i++)
                if (EqualityComparer<int>.Default.Equals(items[i], -1))
                    return i;

            return -1;
        }

        private const int Items = 3_000;
        private static readonly Dictionary<int, int> s_dict = Enumerable.Range(0, Items).ToDictionary(i => i);

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

        private unsafe bool StringsAreEqual(string strA, string strB)
        {
            int length = strA.Length;

            fixed (char* ap = strA, bp = strB)
            {
                char* a = ap;
                char* b = bp;

                while (length != 0)
                {
                    int charA = *a;
                    int charB = *b;

                    if (charA != charB)
                    {
                        return false;
                    }

                    a++;
                    b++;
                    length--;
                }

                return true;
            }
        }

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

        private struct Dog : IAnimal
        {
            public void Bark() { }
            void IAnimal.MakeSound() => Bark();
        }

        private interface IAnimal
        {
            void MakeSound();
        }
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class Threading
    {
        [Benchmark(OperationsPerInvoke = 1_000_000)]
        public void ThreadStatics()
        {
            for (int i = 0; i < 1_000_000; i++) GetThreadStatic();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static object GetThreadStatic() => t_threadStaticValue;

        [ThreadStatic]
        private static object t_threadStaticValue = null;

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

        private static readonly CancellationToken s_token = new CancellationTokenSource().Token;

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

        private async Task MethodAsync() { }
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class String
    {
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

        private static readonly char[] s_chars = { ':', ';' };

        [Benchmark]
        public string StringToLowerChangesNeeded() =>
            "This is a test to see what happens when we call ToLower."
            .ToLower();

        [Benchmark]
        public string StringToLowerAlreadyCased() =>
            "this is a test to see what happens when we call tolower."
            .ToLower();

        private static readonly char[] s_seps = new[] { ',' };

        [Benchmark]
        public string[] StringSplit() =>
            "All the world's a stage, and all the men and women merely players: they have their exits and their entrances; and one man in his time plays many parts, his acts being seven ages."
            .Split(s_seps);

        [Benchmark]
        public string StringConcatCharEnumerable() =>
            string.Concat(Enumerable.Range(0, 1000).Select(i => (char)('a' + i % 26)));
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class FormattingAndParsing
    {
        [Benchmark]
        public void StringFormat() => string.Format("Test {0}", 123456789);

        private static readonly StringBuilder s_builder = new StringBuilder();

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

        private static readonly long s_bi = long.MaxValue;

        [Benchmark]
        public void BigIntegerFormatting() => s_bi.ToString("X");

        private static readonly DateTimeOffset s_dto = DateTimeOffset.UtcNow;

        [Benchmark]
        public void DateTimeOffsetFormatR() => s_dto.ToString("r");

        [Benchmark]
        public void DateTimeOffsetFormatO() => s_dto.ToString("o");

        private static readonly string s_base64String = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce lobortis massa non semper lobortis. Suspendisse potenti. Integer cursus odio vitae metus suscipit, eget elementum lorem facilisis.";
        private static readonly char[] s_base64Chars = s_base64String.ToCharArray();

        [Benchmark]
        public byte[] ConvertFromBase64String() => Convert.FromBase64String(s_base64String);

        [Benchmark]
        public byte[] ConvertFromBase64Chars() => Convert.FromBase64CharArray(s_base64Chars, 0, s_base64Chars.Length);
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class Networking
    {
        [Obsolete]
        private static readonly long s_addr = IPAddress.Loopback.Address;

        [Benchmark]
        [Obsolete]
        public long IPAddressNetworkToHostOrder() => IPAddress.NetworkToHostOrder(s_addr);

        [Benchmark]
        public string UriAllocations() => new Uri("http://127.0.0.1:80").AbsoluteUri;

        private static byte[] s_buffer = new byte[1];
        private static NetworkStream s_client, s_server;

        [GlobalSetup]
        public void CreateConnectedStreams()
        {
            using (var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                listener.Listen(1);

                var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(listener.LocalEndPoint);
                Socket server = listener.Accept();

                s_client = new NetworkStream(client);
                s_server = new NetworkStream(server);
            }
        }

        [GlobalCleanup]
        public void DisposeStreams()
        {
            s_client?.Dispose();
            s_server?.Dispose();
        }

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
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class Networking2
    {
        private static readonly byte[] s_testCertBytes = Convert.FromBase64String(@"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");
        private static Socket s_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static HttpClient s_client = new HttpClient(new HttpClientHandler() { ServerCertificateCustomValidationCallback = delegate { return true; } });
        private static Uri s_uri;

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

        [GlobalSetup]
        public void CreateSocketServer()
        {
            s_listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            s_listener.Listen(int.MaxValue);
            var ep = (IPEndPoint)s_listener.LocalEndPoint;
            s_uri = new Uri($"https://{ep.Address}:{ep.Port}/");

            Task.Run(async () =>
            {
                while (true)
                {
                    Socket s = await s_listener.AcceptAsync();
                    var ignored = Task.Run(async () =>
                    {
                        using (X509Certificate2 serverCert = GetServerCertificate())
                        using (var serverStream = new SslStream(new NetworkStream(s, true), false, delegate { return true; }))
                        {
                            await serverStream.AuthenticateAsServerAsync(serverCert, false, SslProtocols.None, false);
                            using (var reader = new StreamReader(serverStream))
                            using (var writer = new StreamWriter(serverStream) { AutoFlush = true })
                            {
                                while (true)
                                {
                                    while (!string.IsNullOrEmpty(await reader.ReadLineAsync())) ;
                                    await writer.WriteAsync("HTTP/1.1 200 OK\r\nContent-Length: 5\r\n\r\nhello");
                                }
                            }
                        }
                    });
                }
            });
        }

        private static X509Certificate2 GetServerCertificate()
        {
            var certCollection = new X509Certificate2Collection();
            certCollection.Import(s_testCertBytes, "testcertificate", X509KeyStorageFlags.DefaultKeySet);
            return certCollection.Cast<X509Certificate2>().First(c => c.HasPrivateKey);
        }
    }

    // https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/
    [Config(typeof(MainConfig))]
    [ClrJob(baseline: true)]
    [HtmlExporter, RankColumn]
    public class AndMore
    {
        [Benchmark]
        public void EnumerateFiles()
        {
            foreach (string path in Directory.EnumerateFiles(@"d:\", "*.txt", SearchOption.AllDirectories))
            {
            }
        }

        private static readonly Rfc2898DeriveBytes s_db = new Rfc2898DeriveBytes("verysafepassword", 32, 10_000, HashAlgorithmName.SHA256);

        [Benchmark]
        public byte[] DeriveBytes() => s_db.GetBytes(32);

        [Benchmark]
        public void GuidNewGuid() => Guid.NewGuid();

        private static readonly Regex s_phoneNumberRegex = new Regex(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$", RegexOptions.Compiled);

        [Benchmark]
        public void RegexCompiled() => s_phoneNumberRegex.IsMatch("555-867-5309");
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            if(args==null || args.Length!=2)
            {
                Console.WriteLine("Available benchmarks:");
                Assembly.GetExecutingAssembly().GetTypes().Where(u => u.IsPublic && u.Name!="Program" && u.Name != "MainConfig").ToList().ForEach(u => Console.WriteLine(u.Name));
                return;
            }

            switch(args[1])
            {
                case "List":
                    
                    break;

                case "Md5VsSha256":
                    BenchmarkRunner.Run<Md5VsSha256>();
                    break;

                case "MyBench":
                    BenchmarkRunner.Run<MyBench>();
                    break;

                case "JIT":
                    BenchmarkRunner.Run<JIT>();
                    break;

                case "Threading":
                    BenchmarkRunner.Run<Threading>();
                    break;

                case "String":
                    BenchmarkRunner.Run<String>();
                    break;

                case "FormattingAndParsing":
                    BenchmarkRunner.Run<FormattingAndParsing>();
                    break;

                case "Networking":
                    BenchmarkRunner.Run<Networking>();
                    break;

                case "Networking2":
                    BenchmarkRunner.Run<Networking2>();
                    break;

                case "AndMore":
                    BenchmarkRunner.Run<AndMore>();
                    break;

                default:
                    Console.WriteLine("Unknown argument!");
                    break;
            }
        }
    }
}
