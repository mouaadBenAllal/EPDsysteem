/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Views.Rapportages
{
    using System.Windows;
    using System.Windows.Controls;
    using Controller.Rapportages;
    /// <summary>
    ///     Interaction logic for RapportagesOverzicht.xaml
    /// </summary>
    public partial class RapportagesOverzicht : Page
    {
        private readonly RapportagesController rapportagesController;
        /// <summary>
        /// handles rapportagesoverzicht input and buttons
        /// </summary>
        public RapportagesOverzicht()
        {
            InitializeComponent();
            rapportagesController = new RapportagesController(null, this);
            rapportagesController.OverzichtRapportages(rapportagesDataGrid);
            createRapportage.Click += (sender, e) =>
                rapportagesController.NieuwRapportageWindow(sender, e, rapportagesDataGrid);
        }
        /// <summary>
        /// handles datagrid rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGridRowHandler(object sender, RoutedEventArgs e)
        {
            rapportagesController.HandleDetailRapportages(sender, e, rapportagesDataGrid);
        }
    }
}