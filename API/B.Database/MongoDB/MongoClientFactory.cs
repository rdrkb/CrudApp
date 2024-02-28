using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Database.MongoDB
{
    public class MongoClientFactory
    {
        private readonly ConcurrentDictionary<string, MongoClient> _clientsMapper;

        public MongoClientFactory() 
        {
            _clientsMapper = new ConcurrentDictionary<string, MongoClient>();
        }

        public MongoClient GetClient(string connectionString)
        {
            if (_clientsMapper.TryGetValue(connectionString, out var client))
            {
                return client;
            }

            var newClient = new MongoClient(connectionString);

            _clientsMapper.TryAdd(connectionString, newClient);
            
            return newClient;
        }
    }
}
