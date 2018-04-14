/*
 *Application name  : Elektronische patienten dossier
 *Author            : Team firefly
*/


namespace EpdFirefly.Controller.Patientendossier
{
    using System.Windows;
    using System.Windows.Controls;
    using Users;


    /// <summary>
    ///     Interaction logic for Home.xaml
    /// </summary>
    public partial class PagesNavigation : Window
    {
        private readonly PagesNavigationController pagesNavigationController = new PagesNavigationController();
        /// <summary>
        /// handles pages navigation input and buttons
        /// </summary>
        /// <param name="home"></param>
        /// <param name="patientenDataGrid"></param>
        public PagesNavigation(Home home,DataGrid patientenDataGrid)
        {
            InitializeComponent();
            DossierButton.Click += (sender, e) => pagesNavigationController.PagesHandler(sender, mainFrame, this, home, patientenDataGrid); 
            AnamneseButton.Click += (sender, e) => pagesNavigationController.PagesHandler(sender, mainFrame, this, home, patientenDataGrid);
            MetingenButton.Click += (sender, e) => pagesNavigationController.PagesHandler(sender, mainFrame, this, home, patientenDataGrid);
            RapportageButton.Click += (sender,e) => pagesNavigationController.PagesHandler(sender, mainFrame, this, home, patientenDataGrid);
        }
    }   
}