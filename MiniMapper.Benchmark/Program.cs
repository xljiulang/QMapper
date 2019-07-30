using BenchmarkDotNet.Running;
using System;

namespace MiniMapper.Benchmark
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
