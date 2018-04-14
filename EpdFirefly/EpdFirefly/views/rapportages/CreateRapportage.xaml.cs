/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Views.Rapportages
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Controller.Rapportages;
    /// <summary>
    ///     Interaction logic for CreateRapportage.xaml
    /// </summary>
    public partial class CreateRapportage : Window
    {
        private readonly RapportagesController rapportagesController;


        /// <summary>
        /// handles crearerapportage inputs and buttons
        /// </summary>
        /// <param name="rapportagesDataGrid"></param>
        public CreateRapportage(DataGrid rapportagesDataGrid)
        {
            InitializeComponent();
            rapportagesController = new RapportagesController(this, null);
            createRapportage.Click += (sender, e) =>
                rapportagesController.CreateRapportage(sender, e, createRapportageGrid, rapportagesDataGrid);
        }


        /// <summary>
        /// handles richtextbox events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichboxTurnOffEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                e.Handled = true; 
        }
    }
}