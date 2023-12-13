using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MyErp.BusinessLogic;
using MyErp.Repositories;
using MyErp.Views;

namespace MyErp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceProvider = ConfigureServices();
            this.InitializeComponent();
        }

        public static IServiceProvider Services => ((App)Current).ServiceProvider;

        public IServiceProvider ServiceProvider { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<MainViewEntities>();
            services.AddTransient<ClientService>();
            services.AddTransient<IClientRepository, JsonFileClientRepository>();
            return services.BuildServiceProvider();
        }

    }
}
