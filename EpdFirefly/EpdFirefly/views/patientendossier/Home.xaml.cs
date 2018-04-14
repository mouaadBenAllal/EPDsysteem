/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Patientendossier
{
    using System.Windows;
    using Users;
    
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private readonly PagesNavigationController pagesNavigationController = new PagesNavigationController();
        private readonly PatientController patientController = new PatientController();
        private readonly Logout logoutclass = new Logout();

        /// <summary>
        /// shows all patients
        /// </summary>
        public Home()
        {
            InitializeComponent();
            addpatient.Click += (sender, e) => patientController.AddPatientView(patientenDataGrid);
            logoutbtn.Click += (sender, e) => logoutclass.LogoutFunction(this);
            patientController.PatientOverzicht(patientenDataGrid);
        }
        /// <summary>
        /// handles datagrid row doubleclick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PatientDataGridRowHandler(object sender, RoutedEventArgs e)
        {
           pagesNavigationController.OpenPatient(patientenDataGrid, this);
        }
    }
}
