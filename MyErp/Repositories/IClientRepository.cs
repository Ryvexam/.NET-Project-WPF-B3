using MyErp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyErp.Repository
{
    public interface IClientRepository
    {
        Task Save(IList<ClientEntity> models);

        Task<IList<ClientEntity>> Load();
    }
}