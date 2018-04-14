
namespace EpdFirefly.Views.Patientendossier
{
    using System.Windows.Controls;
    using Controller.Patientendossier;
    /// <summary>
    ///     Interaction logic for PatientDossier.xaml
    /// </summary>
    public partial class PatientDossier : Page
    {
        private readonly PatientController patientController = new PatientController();

        /// <summary>
        /// handles patient page functions
        /// </summary>
        /// <param name="pagesNavigation"></param>
        /// <param name="patientenDataGrid"></param>
        public PatientDossier(PagesNavigation pagesNavigation, DataGrid patientenDataGrid)
        {
            InitializeComponent();
            patientController.Overzicht(this);
            deletebtn.Click += (sender, e) => patientController.DeletePatient(pagesNavigation, patientenDataGrid);
        }
    }
}