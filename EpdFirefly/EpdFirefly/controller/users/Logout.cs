namespace EpdFirefly.Controller.Users
{
    using ArrayContainer;
    using Patientendossier;
    internal class Logout
    {
        /// <summary>
        ///     logs the logged in user out and destroys the session saved in container.cs
        /// </summary>
        /// <param name="home"></param>
        public void LogoutFunction(Home home)
        {
            Container.GetInstance.Destroy();
            MainWindow window = new MainWindow();

            window.Show();
            home.Close();
        }
    }
}