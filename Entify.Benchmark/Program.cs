// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using Entify.Benchmark;

BenchmarkRunner.Run<EntifyScalarFunctionsBenchmark>();