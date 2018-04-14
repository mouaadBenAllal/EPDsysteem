/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/



namespace EpdFirefly.Views.Patientendossier
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Controller.Patientendossier;

    /// <summary>
    ///     Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        private readonly PatientController patientController = new PatientController();

        /// <summary>
        /// adds patient handler
        /// </summary>
        /// <param name="patientenDataGrid"></param>
        public AddPatient(DataGrid patientenDataGrid)
        {
            InitializeComponent();
            opslaan.Click += (sender, e) => patientController.SavePatient(sender, e, Stackpanelpatients,this, patientenDataGrid);
            backbtn.Click += (sender, e) => patientController.Terug(this);
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}