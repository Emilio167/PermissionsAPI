using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Nest;

namespace Infrastructure.Search
{
    public class ElasticsearchService
    {
        private readonly ElasticClient _client;

        public ElasticsearchService()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                            .DefaultIndex("permissions");
            _client = new ElasticClient(settings);
        }

        public async Task IndexPermissionAsync(Permission permission)
        {
            await _client.IndexDocumentAsync(permission);
        }
    }
}
