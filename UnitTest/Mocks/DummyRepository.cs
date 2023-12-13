using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyErp.Entities;
using MyErp.Repositories;

namespace UnitTest.Mocks
{
    internal class DummyClientRepository: IClientRepository
    {
        public List<ClientEntity> SavedClients { get; private set; } = new List<ClientEntity>();

        public Task Save(IList<ClientEntity> models)
        {
            SavedClients = models.ToList();
            return Task.CompletedTask;
        }

        public Task<IList<ClientEntity>> Load()
        {
            return Task.FromResult((IList<ClientEntity>)SavedClients);
        }
    }
}