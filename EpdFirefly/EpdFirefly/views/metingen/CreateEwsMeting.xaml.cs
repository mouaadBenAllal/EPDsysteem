/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Views.Metingen
{
    using System.Windows;
    using System.Windows.Controls;
    using Controller.Metingen;
    /// <summary>
    ///     Interaction logic for addEwsMeting.xaml
    /// </summary>
    public partial class CreateEwsMeting : Window
    {
        private readonly EwsMetingController ewsMetingController;
        /// <summary>
        /// handles create ewsmetingen inputs and buttons
        /// </summary>
        /// <param name="overzichtMetingenGrid"></param>
        public CreateEwsMeting(DataGrid overzichtMetingenGrid)
        {
            InitializeComponent();
            ewsMetingController = new EwsMetingController(this, null);
            SaveEwsMeting.Click += (sender, e) =>
                ewsMetingController.CreateEwsMetingFunction(sender, e, createEwsGrid, overzichtMetingenGrid);
        }
    }
}