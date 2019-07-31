using BenchmarkDotNet.Running;
using System;

namespace QMapper.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MapContext>();
            Console.ReadLine();
        }
    }
}
