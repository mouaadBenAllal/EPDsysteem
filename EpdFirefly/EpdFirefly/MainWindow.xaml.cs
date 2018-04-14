/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/



namespace EpdFirefly
{
    using EpdFirefly.Controller.Patientendossier;
    using EpdFirefly.model;
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
            loginController = new LoginController();
            login.Click += (sender, e) => loginController.HandleLogin(sender, e, loginstackpanel);
            register.Click += (sender, e) => loginController.HandleLogin(sender, e, null);
            (NavigationContainer.GetInstance).Set("login", this);
        }
    }
}