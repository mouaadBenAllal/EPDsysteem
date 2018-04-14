/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Security.Cryptography;
    using System.Windows;
    using System.Windows.Controls;
    using DatabaseHandler;
    using MySql.Data.MySqlClient;
    using Views.Users;
    internal class RegisterController
    {
        private readonly Register register;

        /// <summary>
        /// registercontroller constructor
        /// </summary>
        /// <param name="register"></param>
        public RegisterController(Register register)
        {
            this.register = register;
        }

        /// <summary>
        /// Handles login page button inputs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="stackpanel"></param>
        public void Onbuttonclick(object sender, EventArgs e, StackPanel stackpanel)
        {
            MainWindow login = new MainWindow();
            Button buttontest = (Button) sender;

            switch (buttontest.Name)
            {
                case "login":
                    register.Close();
                    login.Show();
                    break;
                case "register":
                    //register code
                    bool registerCompleted = Register(stackpanel);

                    if (registerCompleted)
                    {
                        MessageBox.Show("Bedankt voor het registreren, u kunt nu inloggen!");
                        register.Close();
                        login.Show();
                    }

                    break;
            }
        }

        /// <summary>
        /// this function registers the user
        /// </summary>
        /// <param name="stackpanel"></param>
        /// <returns></returns>
        public bool Register(StackPanel stackpanel)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            string password = null;
            bool registerStageSucces = false;
            // Loops through stackpanel with children
            //runs some function on the basis of a type
            foreach (object child in stackpanel.Children)
            {
                // handles the textbox data
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox) child;

                    if (textBox.Name == "studentnummer" && !int.TryParse(textBox.Text, out int unUsedVariable))
                    {
                        MessageBox.Show("Uw studentennummer mag alleen uit cijfers bestaan!");
                        break;
                    }
                    else if (string.IsNullOrEmpty(textBox.Text))
                    {
                        MessageBox.Show("Vul alle velden in!");
                        break;
                    }

                    if (textBox.Name == "username")
                    {
                        bool exists = CheckUsernameExists(textBox.Text);
                        if (exists)
                        {
                            MessageBox.Show("Gebruikersnaam is in gebruik!");
                            break;
                        }
                        array[textBox.Name] = textBox.Text;
                    }
                    else
                    {
                        array[textBox.Name] = textBox.Text;
                    }
                }

                // handles the passwordbox data
                if (child.GetType() == typeof(PasswordBox))
                {
                    PasswordBox passwordBox = (PasswordBox) child;

                    if (string.IsNullOrEmpty(passwordBox.Password))
                    {
                        MessageBox.Show("Please fill in the password fields");
                        break;
                    }

                    if (!string.IsNullOrEmpty(password) && password == passwordBox.Password)
                    {
                        array["password"] = HashPassword(passwordBox.Password);
                        ((QueryBuilder) MysqlSshConnection.InstanceCon).InsertHandler("users", array);
                        registerStageSucces = true;
                        break;
                    }

                    if (!string.IsNullOrEmpty(password)) MessageBox.Show("wachtwoorden komen niet overeen");
                    password = passwordBox.Password;
                }
            }

            return registerStageSucces;
        }

        /// <summary>
        /// Hashes the input password from the register page sended to the database.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            // Creating the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Creating the Rfc2898DeriveBytes and get the hash value
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            //Combining the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            //Turning the combined salt+hash into a string for storage
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            //returning hashedpassword
            return savedPasswordHash;
        }

        /// <summary>
        /// checks if the username already exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckUsernameExists(string username)
        {
            object data = ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery(
                "SELECT username FROM users where username = '" + MySqlHelper.EscapeString(username) + "'");
            DataSet result = (DataSet) data;
            if (result.Tables[0].Rows.Count == 0) return false;
            return true;
        }
    }
}