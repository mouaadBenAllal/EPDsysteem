/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Views.Users
{
    using System.Windows;
    using Controller.Users;
    /// <summary>
    ///     Interaction logic for Register.xaml
    /// </summary>

    public partial class Register : Window
    {
        private readonly RegisterController registerController;
        /// <summary>
        /// handles register page inputs and buttons
        /// </summary>

        public Register()
        {
            InitializeComponent();
            registerController = new RegisterController(this);
            login.Click += (sender, e) => registerController.Onbuttonclick(sender, e, null);
            register.Click += (sender, e) => registerController.Onbuttonclick(sender, e, registerstackpanel);
        }
    }
}