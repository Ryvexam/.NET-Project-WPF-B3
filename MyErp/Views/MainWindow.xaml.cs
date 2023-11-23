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


        }


    }
}
