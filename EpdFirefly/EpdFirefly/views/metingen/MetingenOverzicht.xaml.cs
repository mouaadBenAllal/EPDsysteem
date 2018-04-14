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
    ///     Interaction logic for MetingenOverzicht.xaml
    /// </summary>
    public partial class MetingenOverzicht : Page
    {
        private readonly MetingenController metingenController;
        /// <summary>
        /// handles metingen overzicht inputs and buttons
        /// </summary>
        public MetingenOverzicht()
        {
            InitializeComponent();
            metingenController = new MetingenController(this);
            metingenController.OverzichtMeting(metingenDataGrid);

            Ews.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
            Pijnscore.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
            SNAQ.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
            DOS.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
            BradenDecubitus.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
            DecubitusScore.Click += (sender, e) => metingenController.HandleMetingen(sender, e, metingenDataGrid);
        }
        /// <summary>
        /// handles datagrid on double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGridRowHandler(object sender, RoutedEventArgs e)
        {
            metingenController.HandleDetailMetingen(sender, e, metingenDataGrid);
        }
    }
}