using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MyErp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<MainViewEntities>();
            UpdateClients((DataContext as MainViewEntities), true);
            UpdateClientsVisibility(DataContext as MainViewEntities);

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewEntities viewModel)
            {
                UpdateClients(viewModel, false);
                UpdateClientsVisibility(viewModel);

            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewEntities viewModel)
            {
                UpdateClients(viewModel, true);
                UpdateClientsVisibility(viewModel);
            }
        }

        private void UpdateClients(MainViewEntities viewModel, bool shouldShow)
        {
            foreach (var client in viewModel.Clients)
            {
                client.ShouldShown = shouldShow;
            }
        }

        private void UpdateClientsVisibility(MainViewEntities viewModel)
        {
            foreach (var client in viewModel.Clients)
            {
                client.MustShow = client is { ShouldShown: true, IsEnabled: false };
            }
        }

    }
}
