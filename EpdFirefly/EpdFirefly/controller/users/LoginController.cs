/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/


namespace EpdFirefly
{
    using System;
    using System.Data;
    using System.Security.Cryptography;
    using System.Windows;
    using System.Windows.Controls;
    using ArrayContainer;
    using Controller.Patientendossier;
    using DatabaseHandler;
    using MySql.Data.MySqlClient;
    using EpdFirefly.model;
    using Views.Users;

    internal class LoginController
    {
        private readonly MainWindow login;
        private string userId = "";

        /// <summary>
        ///     handles login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="stackpanel"></param>
        public void HandleLogin(object sender, EventArgs e, StackPanel stackpanel)
        {
            Button btn = (Button) sender;
            switch (btn.Name)
            {
                case "login":
                    if (LoginFieldsChecker(stackpanel))
                    {
                        MessageBox.Show("Success");
                        Container.GetInstance.Set("userId", userId);
                        NavigationContainer.GetInstance.Set("home", new Home());
                        ((MainWindow)NavigationContainer.GetInstance.Get("login")).Close();
                        ((Home)NavigationContainer.GetInstance.Get("home")).ShowDialog();

                    }

                    break;
                case "register":
                    NavigationContainer.GetInstance.Set("register", new Register());
                    ((MainWindow)NavigationContainer.GetInstance.Get("login")).Close();
                    ((Register)NavigationContainer.GetInstance.Get("register")).Show();
                    break;
            }
        }

        /// <summary>
        ///     Checks the fields inserted on login
        /// </summary>
        /// <param name="stackpanel"></param>
        /// <returns>returns the login succes</returns>
        public bool LoginFieldsChecker(StackPanel stackpanel)
        {
            string password = null;
            string username = null;
            bool loginStageSucces = false;

            foreach (object child in stackpanel.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox) child;

                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        MessageBox.Show("Please fill in all the fields");
                        return false;
                    }

                    if (textBox.Name == "Username") username = textBox.Text;
                }

                if (child.GetType() == typeof(PasswordBox))
                {
                    PasswordBox passwordBox = (PasswordBox) child;

                    if (string.IsNullOrEmpty(passwordBox.Password))
                    {
                        MessageBox.Show("Vul een wachtwoord in");
                        return false;
                    }

                    password = passwordBox.Password;
                    loginStageSucces = true;
                    break;
                }
            }

            if (loginStageSucces)
                if (CheckUserDetails(username, password))
                    return true;
            return false;
        }

        /// <summary>
        ///     checks if teh combination of password and username match
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true or false</returns>
        public bool CheckUserDetails(string username, string password)
        {
            string savedPasswordHash = "";
            object data =
                ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery("SELECT * FROM users where username = '" +
                                                                             MySqlHelper.EscapeString(username) + "'");
            DataSet result = (DataSet) data;
            if (result.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Gebruikersnaam bestaat niet");
                return false;
            }

            foreach (DataRow drow in result.Tables[0].Rows)
            {
                /* Fetch the stored value */
                savedPasswordHash = drow["password"].ToString();
                userId = drow["id"].ToString();
                break;
            }

            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                {
                    MessageBox.Show("Uw combinatie van gebruikersnaam en wachtwoord klopt niet");
                    return false;
                }

            return true;
        }
    }
}