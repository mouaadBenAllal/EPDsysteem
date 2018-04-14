/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace DatabaseHandler
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows;
    using MySql.Data.MySqlClient;
    internal class QueryBuilder : MysqlSshConnection
    {
        /// <summary>
        /// accepts a raw query and returns dataset
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet CustomeQuery(string query)
        {
            try
            {
                MySqlCommand com = new MySqlCommand(query, Con);
                com.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(com);
                da.Fill(ds);
                return ds;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// selects all by tablename
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataSet SelectAll(string tablename)
        {
            try
            {
                MySqlCommand com = new MySqlCommand("select * from " + MySqlHelper.EscapeString(tablename), Con);
                com.CommandType = CommandType.Text;
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(com);
                da.Fill(ds);
                return ds;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        /// <summary>
        ///  querybuilder handles insert queries
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        public void Insert(string tablename, string columnName, string values)
        {
            new MySqlCommand("INSERT INTO " + tablename + "(" + columnName + ") VALUES(" + values + ")", Con)
                .ExecuteNonQuery();
        }

        public void InsertHandler(string tableName, Dictionary<string, string> array)
        {
            string key = null;
            string value = null;
            foreach (KeyValuePair<string, string> element in array)
                if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(value))
                {
                    string input = !string.IsNullOrEmpty(element.Value)
                        ? "'" + MySqlHelper.EscapeString(element.Value) + "'"
                        : "NULL";
                    key = element.Key;
                    value = input;
                }
                else
                {
                    string input = !string.IsNullOrEmpty(element.Value)
                        ? "'" + MySqlHelper.EscapeString(element.Value) + "'"
                        : "NULL";
                    key += ", " + element.Key;
                    value += ", " + input;
                }

            try
            {
                Insert(tableName, key, value);
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}