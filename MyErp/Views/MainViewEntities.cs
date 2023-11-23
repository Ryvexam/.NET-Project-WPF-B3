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
        public bool ShowDisabledClients { get; set; }

        private ClientService _clientService;
        public List<string> AvailableLanguages { get; }

        public ObservableCollection<ClientEntity> Clients { get; }

        
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand EnableCommand { get; private set; }

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
                }
        }

        public MainViewEntities(ClientService clientService)
        {


            _clientService = clientService;

            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand(OnDelete,CanDelete);
            EnableCommand = new RelayCommand(ChangeStatus,CanDelete);

            SaveCommand = new RelayCommand(OnSave);

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
                ClientEntity newClient = _clientService.CreateClient();

                if (!ClientExists(newClient))
                {
                    Clients.Add(newClient);
                    _clientService.Save(Clients);
                }
                else
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
        private Button GetButtonFromCommand(RelayCommand command)
        {
            var button = command.GetType().GetField("_button", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(command) as Button;
            return button;
        }
        private bool ClientExists(ClientEntity clientToCheck)
        {
            return Clients.Any(client => client.SiretNumber == clientToCheck.SiretNumber);
        }
    }

}
