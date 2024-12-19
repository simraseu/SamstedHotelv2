using Microsoft.Extensions.Configuration;
using SamstedHotel.View;
using System.IO;
using System.Windows;

namespace SamstedHotel
{
    public partial class App : Application
    {
        public static string ConnectionString { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load connection string from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConnectionString = config.GetConnectionString("DefaultConnection");

            // Ensure the connection string is available
            if (string.IsNullOrEmpty(ConnectionString))
            {
                MessageBox.Show("Connection string is missing or invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            // Start the main window using the default constructor
            var mainWindow = new VelkommenView();
            mainWindow.Show();
        }
    }
}
