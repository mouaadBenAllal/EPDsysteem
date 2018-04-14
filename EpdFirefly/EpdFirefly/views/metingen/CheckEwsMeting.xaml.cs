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
    ///     Interaction logic for CheckEwsMeting.xaml
    /// </summary>
    public partial class CheckEwsMeting : Window
    {
        private readonly EwsMetingController ewsMetingController;
        /// <summary>
        /// handles details ewsmetingen inputs and buttons
        /// </summary>
        /// <param name="overzichtMetingenGrid"></param>
        public CheckEwsMeting(DataGrid overzichtMetingenGrid)
        {
            InitializeComponent();
            ewsMetingController = new EwsMetingController(null, this);
            deleteEwsMeting.Click += (sender, e) =>
                ewsMetingController.DeleteEwsMeting(sender, e, ewsMetingId, overzichtMetingenGrid);
        }
    }
}