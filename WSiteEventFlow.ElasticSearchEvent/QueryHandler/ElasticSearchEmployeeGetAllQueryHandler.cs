using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Elasticsearch.ReadStores;
using EventFlow.Elasticsearch.ValueObjects;
using EventFlow.Queries;
using WSiteEventFlow.ElasticSearch.ReadModels;
using Nest;
using WSiteEventFlow.Core.Aggregates.Queries;
using System.Collections.Generic;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.ElasticSearch.QueryHandler
{
    public class ElasticSearchEmployeeGetAllQueryHandler : IQueryHandler<EmployeeGetAllQuery, IReadOnlyCollection<Employee>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IReadModelDescriptionProvider _readModelDescriptionProvider;

        public ElasticSearchEmployeeGetAllQueryHandler(IElasticClient elasticClient, IReadModelDescriptionProvider readModelDescriptionProvider)
        {
            _elasticClient = elasticClient;
            _readModelDescriptionProvider = readModelDescriptionProvider;
        }

        public async Task<IReadOnlyCollection<Employee>> ExecuteQueryAsync(EmployeeGetAllQuery query, CancellationToken cancellationToken)
        {
            ReadModelDescription readModelDescription = _readModelDescriptionProvider.GetReadModelDescription<EmployeeReadModel>();
            string indexName = "eventflow-" + readModelDescription.IndexName.Value;

            await _elasticClient.FlushAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);

            await _elasticClient.RefreshAsync(indexName,
                    d => d.RequestConfiguration(c => c.AllowedStatusCodes((int)HttpStatusCode.NotFound)), cancellationToken)
                    .ConfigureAwait(false);
            var searchResponse = await _elasticClient.SearchAsync<EmployeeReadModel>(d => d
                .RequestConfiguration(c => c
                    .AllowedStatusCodes((int)HttpStatusCode.NotFound))
                .Index(readModelDescription.IndexName.Value)
                .Query(q => q),
                    cancellationToken)
                .ConfigureAwait(false);
            var retList = new List<Employee>();
            foreach (var doc in searchResponse?.Documents)
            {
                var dataEmployee = new Employee(new EmployeeId(doc.Id), doc.FullName ?? string.Empty, doc.Department ?? string.Empty, doc.TenantId);
                retList.Add(dataEmployee);
            }
            return retList;
        }

    }
}
