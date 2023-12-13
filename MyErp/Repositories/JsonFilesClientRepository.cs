using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyErp.Entities;

namespace MyErp.Repositories
{
    internal class JsonFileClientRepository : IClientRepository
    {
        private const string ClientFileName = "ClientDB.json";

        public async Task Save(IList<ClientEntity> models)
        {
            var serializedContent = JsonSerializer.Serialize(models);

            File.WriteAllText(ClientFileName,serializedContent);
        }

        public async Task<IList<ClientEntity>> Load()
        {
            if (!File.Exists(ClientFileName))
                return new List<ClientEntity>();

            var serializedContent = File.ReadAllText(ClientFileName);

            var clients = JsonSerializer.Deserialize<List<ClientEntity>>(serializedContent);
            return clients ?? new List<ClientEntity>();
        }
    }
}