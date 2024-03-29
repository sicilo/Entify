﻿using Entify.Application.Contracts.Services;
using Entify.Infrastructure.Abstractions;
using Entify.Infrastructure.Extensions;
using System.Data.Common;

namespace Entify.Application.Services
{
    public sealed class QueryService<T> : BaseConnection<T>, IQueryService<T> where T : DbConnection
    {
        public QueryService(T connection) : base(connection)
        {
        }


        public async Task ExecQueryAsync(string query)
        {
            await using var connection = CreateConnectionInstance();

            await connection.ExecNonQueryScriptAsync(query);
        }

        public async Task<TR> ExecScalarQueryAsync<TR>(string query)
        {
            await using var connection = CreateConnectionInstance();

            return await connection.ExecScalarScriptAsync<TR>(query);
        }

        public async Task<TR> ExecEntityQueryAsync<TR>(string query) where TR : class
        {
            await using var connection = CreateConnectionInstance();

            return await connection.ExecEntityScriptAsync<TR>(query);
        }

        public async Task<IEnumerable<TR>> ExecReaderQueryAsync<TR>(string query)
        {
            await using var connection = CreateConnectionInstance();
            
            return await connection.ExecReaderScriptAsync<TR>(query);
        }

        public async Task<TR> ExecMultiReaderQueryAsync<TR>(string query) where TR : class
        {
            await using var connection = CreateConnectionInstance();
            
            return await connection.ExecMultiReaderScriptAsync<TR>(query);
        }
    }
}
