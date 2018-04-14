/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly
{
    using System.Windows;
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginController loginController;
        /// <summary>
        /// handles mainwindow inputs and buttons
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            loginController = new LoginController(this);
            login.Click += (sender, e) => loginController.HandleLogin(sender, e, loginstackpanel);
            register.Click += (sender, e) => loginController.HandleLogin(sender, e, null);
        }
    }
}