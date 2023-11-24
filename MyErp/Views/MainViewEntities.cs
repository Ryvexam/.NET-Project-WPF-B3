using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using MyErp.Base;
using MyErp.BusinessLogic;
using MyErp.Entities;
using MyErp.Translations;


namespace MyErp.Views
{
    internal class MainViewEntities:ViewEntitiesBase
    {

        private ClientService _clientService;
        public List<string> AvailableLanguages { get; }

        public ObservableCollection<ClientEntity> Clients { get; set; }


        public RelayCommand AddCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand EnableCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }


        public RelayCommand SaveCommand { get; private set; }

        private ClientEntity? _selectedClient;
        public ClientEntity? SelectedClient
        {
            get => _selectedClient;
            set
            {
                SetProperty(ref _selectedClient, value);
                DeleteCommand.NotifyCanExecuteChanged();
                EnableCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
                _clientService.Save(Clients);

                }
        }

        public MainViewEntities(ClientService clientService)
        {


            _clientService = clientService;

            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand(OnDelete,CanDelete);
            EnableCommand = new RelayCommand(ChangeStatus,CanDelete);
            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);

            AvailableLanguages = CultureInfo
                .GetCultures(CultureTypes.NeutralCultures)
                .Select(x => x.DisplayName)
                .ToList();

            Clients = new ObservableCollection<ClientEntity>(_clientService.Load());



            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Clients);
            if (collectionView != null)
            {
                collectionView.SortDescriptions.Add(new SortDescription("CompanyName", ListSortDirection.Ascending));
            }
        }

        private void OnAdd()
        {
            try
            {
                _clientService.Save(Clients);
                try
                {
                    ClientEntity newClient = _clientService.CreateClient();
                    if (!ClientExists(newClient))
                    {
                        Clients.Add(newClient);
                    }
                    else
                    {
                        MessageBox.Show("Le client existe déjà.");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Le client existe déjà.");
                }



            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void OnSave()
        {
            try
            {
                _clientService.Save(Clients);
                MessageBox.Show("Sauvegarde éffectué");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void OnCancel()
        {
            try
            {
                _clientService.Save(Clients);
                Clients = new ObservableCollection<ClientEntity>(_clientService.Load());
                _clientService.Save(Clients);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void OnDelete()
        {
            Clients.Remove(_selectedClient);
            try
            {
                _clientService.Save(Clients);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool CanDelete()
        {
            return _selectedClient != null ;
        }
        private void ChangeStatus()
        {
            if (_selectedClient != null)
            {
                _selectedClient.IsEnabled = !_selectedClient.IsEnabled;
                try
                {
                    _clientService.Save(Clients);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private bool ClientExists(ClientEntity clientToCheck)
        {
            return Clients.Any(client => client.SiretNumber == clientToCheck.SiretNumber);
        }
    }

}
