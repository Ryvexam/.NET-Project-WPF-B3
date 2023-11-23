using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using MyErp.Entities;
using MyErp.Repository;

namespace MyErp.BusinessLogic
{
    
    public class ClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public void Save(IList<ClientEntity> clients)
        {
            if (clients.Any(x => string.IsNullOrEmpty(x.CompanyName) || string.IsNullOrEmpty(x.FullName)))
                throw new Exception("Un Client doit avoir soit un nom d'entreprise soit Nom-Prénom.");

            if (clients.DistinctBy(x => x.CompanyName).Count() != clients.Count)
                 throw new Exception("Une entreprise avec le même nom existe déja.");

            if (clients.DistinctBy(x => x.FullName).Count() != clients.Count)
                throw new Exception("Une personne avec le même Nom-Prénom existe déja.");

            if (clients.Any(x => x.PostalCode.Length > 10))
                throw new Exception("Le code postal doit comporter au maximum 10 caractères.");

            if (clients.Any(x => string.IsNullOrEmpty(x.CompanyName) || string.IsNullOrEmpty(x.SiretNumber)))
                throw new Exception("Un numéro de SIRET est requis lorsque l'entreprise à un nom");

            if (clients.Any(x => string.IsNullOrEmpty(x.SiretNumber) || x.SiretNumber.Length != 14))
                throw new Exception("Le numéro de SIRET doit etre composé de 14 Chiffres");

            if (clients.Any(x => string.IsNullOrEmpty(x.PhoneNumber) || x.PhoneNumber.Length != 10 || !x.PhoneNumber.StartsWith("0")))
                throw new Exception("Un numéro de téléphone commence OBLIGATOIREMENT par 0 et contient 10 chiffres");

            _repository.Save(clients).Wait();
        }

        public IList<ClientEntity> Load()
        {
            return _repository.Load().Result;
        }

        public ClientEntity CreateClient()
        {
            return new ClientEntity
            {
                SiretNumber = "91753318400011",
                City = "Moulins",
                CompanyName = "RyveWEB",
                CreatedDate = new DateTime(2022, 7, 23),
                FirstName = "Maxime",
                LastName = "VERY",
                IsEnabled = true,
                PhoneNumber = "0640380006",
                PostalCode = "03000"
            };
        }
    }
}