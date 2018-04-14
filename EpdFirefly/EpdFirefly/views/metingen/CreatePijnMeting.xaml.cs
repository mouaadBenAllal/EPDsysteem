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
    ///     Interaction logic for CreatePijnMeting.xaml
    /// </summary>
    public partial class CreatePijnMeting : Window
    {
        private readonly PijnMetingController pijnMetingController;
        /// <summary>
        /// handles details ewsmetingen inputs and buttons
        /// </summary>
        /// <param name="overzichtMetingenGrid"></param>
        public CreatePijnMeting(DataGrid overzichtMetingenGrid)
        {
            pijnMetingController = new PijnMetingController(this);
            InitializeComponent();
            SavePijnScore.Click += (sender, e) =>
                pijnMetingController.CreatePijnMetingFunction(sender, e, createPijnMetingGrid, overzichtMetingenGrid);
        }
    }
}