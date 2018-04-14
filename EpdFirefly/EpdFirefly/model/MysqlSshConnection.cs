/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/

namespace DatabaseHandler
{
    using System;
    using System.Windows;
    using MySql.Data.MySqlClient;
    using Renci.SshNet;
    /// <summary>
    /// /
    /// </summary>
    public class MysqlSshConnection
    {
        /// <summary>
        /// connection con
        /// </summary>
        public MySqlConnection Con;
        private static readonly PrivateKeyFile sshKey = new PrivateKeyFile(@"open");
        private static readonly SshClient Client = new SshClient("188.166.101.250", "root", sshKey);

        private static QueryBuilder queryBuilder;
        /// <summary>
        /// gets instance of querybuilder if not made makes connection
        /// </summary>
        public static object InstanceCon
        {
            get
            {
                if (!Client.IsConnected || queryBuilder == null)
                {
                    queryBuilder = new QueryBuilder();
                    queryBuilder.Connection();
                }

                return queryBuilder;
            }
        }

        /// <summary>
        /// establishing ssh connection to server where MySql is hosted
        /// </summary>
        public void Connection()
        {
            try
            {
                if (!Client.IsConnected)
                {
                    Client.Connect();
                    ForwardedPortLocal portForwarded = new ForwardedPortLocal("127.0.0.1", 3306, "127.0.0.1", 3306);
                    Client.AddForwardedPort(portForwarded);
                    portForwarded.Start();
                    Con = new MySqlConnection("SERVER=127.0.0.1;PORT=3306;UID=root;PASSWORD=darkmaster;DATABASE=firefly");
                    Con.Open();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("You need a working internet connection, application will be closed!:" +e);
            }
        }

        /// <summary>
        /// disconnects mqsql and ssh client
        /// </summary>
        public void Disconnect()
        {
            Client.Disconnect();
            Con.Close();
        }
    }
}