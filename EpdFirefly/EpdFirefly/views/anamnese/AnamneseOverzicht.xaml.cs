/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/


namespace EpdFirefly.Views.Anamnese
{
    using System.Windows.Controls;
    using Controller.Anamnese;
    using Controller.Patientendossier;
    /// <summary>
    ///     Interaction logic for AnamneseOverzicht.xaml
    /// </summary>
    public partial class AnamneseOverzicht : Page
    {
        private readonly AnamneseController anamneseController = new AnamneseController();
        private readonly PatientController patientController = new PatientController();

        /// <summary>
        /// loads anamnese overview
        /// </summary>
        /// <param name="pagesNavigation"></param>
        /// <param name="patientenDataGrid"></param>
        public AnamneseOverzicht(PagesNavigation pagesNavigation,DataGrid patientenDataGrid)
        {
            InitializeComponent();
            anamneseController.Overzicht(this);
            CreateAnamneseBtn.Click += (sender, e) => anamneseController.CreateAnamnese(sender, NavigationService, pagesNavigation, patientenDataGrid);
            deletebtn.Click += (sender, e) => patientController.DeletePatient(pagesNavigation, patientenDataGrid);
        }
    }
}