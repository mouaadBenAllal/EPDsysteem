/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/


namespace EpdFirefly.Views.Anamnese
{
    using System.Windows.Controls;
    using ArrayContainer;
    using Controller.Anamnese;
    using Controller.Patientendossier;
    /// <summary>
    ///     Interaction logic for AnamneseCreate.xaml
    /// </summary>

    public partial class AnamneseCreate : Page
    {

        private AnamneseController anamnesecontroller = new AnamneseController();

        /// <summary>
        /// creates new anamnse
        /// </summary>
        /// <param name="pagesNavigation"></param>
        /// <param name="patienDataGrid"></param>
        public AnamneseCreate(PagesNavigation pagesNavigation, DataGrid patienDataGrid)
        {
            InitializeComponent();
            gordon_saveBtn.Click += (sender, e) =>
                anamnesecontroller.Create(sender, stackpanel, NavigationService, pagesNavigation, patienDataGrid);
            patient_id.Text = Container.GetInstance.Get("patient")["id"].ToString();
            patient_id.IsReadOnly = true;
        }
    }
}