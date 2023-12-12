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
            if (clients.Any(x => string.IsNullOrEmpty(x.CompanyName) && string.IsNullOrEmpty(x.FullName)))
                throw new Exception("Un Client doit avoir soit un nom d'entreprise soit Nom-Prénom.");

            if (clients.Where(x => !string.IsNullOrEmpty(x.CompanyName))
                    .DistinctBy(x => x.CompanyName)
                    .Count() != clients.Count(cl => !string.IsNullOrEmpty(cl.CompanyName)))
            {
                throw new Exception("Une entreprise avec le même nom existe déjà.");
            }

            if (clients.Any(x => string.IsNullOrEmpty(x.FirstName) || string.IsNullOrEmpty(x.LastName)))
            {
                throw new Exception("Il doit y avoir et un Nom et un Prénom");
            }
            
            if (clients.DistinctBy(x => x.FullName).Count() != clients.Count)
                throw new Exception("Une personne avec le même Nom-Prénom existe déja.");

            if (clients.Any(x => x.PostalCode.Length > 10))
                throw new Exception("Le code postal doit comporter au maximum 10 caractères.");

            if (clients.Any(x => string.IsNullOrEmpty(x.CompanyName) && string.IsNullOrEmpty(x.SiretNumber)))
                throw new Exception("Un numéro de SIRET est requis lorsque l'entreprise à un nom");

            if (clients.Any(x => string.IsNullOrEmpty(x.SiretNumber) || x.SiretNumber.Length != 14))
                throw new Exception("Le numéro de SIRET doit etre composé de 14 Chiffres");

            if (clients.Any(x => !string.IsNullOrEmpty(x.PhoneNumber) && (x.PhoneNumber.Length != 10 || !x.PhoneNumber.StartsWith("0"))))
                throw new Exception("Un numéro de téléphone commence OBLIGATOIREMENT par 0 et contient 10 chiffres");

            _repository.Save(clients).Wait();
        }

        public IList<ClientEntity> Load()
        {
            return _repository.Load().Result;
        }

        public ClientEntity CreateClient()
        {
            var random = new Random();

            var siretNumber = random.Next(100000000, 999999999).ToString() + random.Next(10000,99999).ToString();

            var firstNameArray = new string[] { "Maxime", "Sophie", "Lucas", "Julie", "Thomas", "Marie", "Alexandre", "Camille", "Émilie", "Antoine" };
            var lastNameArray = new string[] { "VERY", "DUPONT", "MARTIN", "LEFEBVRE", "MOREAU", "PETIT", "ROUX", "DURAND", "SIMON", "LAURENT" };
            var companyNameArray = new string[] { "RyveWEB", "TechPlus", "InnovSolutions", "WebCreative", "NetEvolve", "CodeStream", "FutureTech", "PixelWizards", "CloudPioneers", "DigitalHorizon" };
            var CityArray = new string[] { "Moulins","Vichy","Clermont-Ferrand","Riom","Lyon","Tours","Paris","Bordeaux","Marseille"};


            var phoneNumber = "06" + random.Next(10000000, 99999999).ToString();

            var year = random.Next(1950, 2023);
            var month = random.Next(1, 13);
            var day = random.Next(1, 29);
            var createdDate = new DateTime(year, month, day);
            var postalCode = random.Next(10000,99999).ToString();
            return new ClientEntity
            {
                SiretNumber = siretNumber,
                City = CityArray[random.Next(CityArray.Length)],
                CompanyName = companyNameArray[random.Next(companyNameArray.Length)] + random.Next(000000,999999).ToString(),
                CreatedDate = createdDate,
                FirstName = firstNameArray[random.Next(firstNameArray.Length)]+ random.Next(000000,999999).ToString(),
                LastName = lastNameArray[random.Next(lastNameArray.Length)],
                IsEnabled = true,
                PhoneNumber = phoneNumber,
                PostalCode = postalCode
            };
        }

    }
}