using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Entify.Application.Extensions;

namespace Entify.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class EntifyScalarFunctionsBenchmark : EntifyBase
{
    [Benchmark]
    public async Task<string> ExecProcScalarAsyncReturnsString()
    {
        var parameters = Connection.ToDbParameters(new
        {
            Op = "STRING"
        }).ToArray();
        
        return await Connection.ExecProcScalarAsync<string>("SpScalarValues", parameters);
    }
    
    [Benchmark]
    public async Task<int> ExecProcScalarAsyncReturnsInt()
    {
        return await Connection.ExecProcScalarAsync<int>("SpScalarValues", Connection.ToDbParameters(new
        {
            Op = "INT"
        }).ToArray());
    }
}