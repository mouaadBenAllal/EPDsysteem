/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Rapportages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using ArrayContainer;
    using DatabaseHandler;
    using Views.Rapportages;
    internal class RapportagesController
    {
        private readonly CreateRapportage createRapportage;
        private readonly RapportagesOverzicht rapportagesOverzicht;

        /// <summary>
        ///     constructor for rapportagescontroller
        /// </summary>
        /// <param name="createRapportage"></param>
        /// <param name="rapportagesOverzicht"></param>
        public RapportagesController(CreateRapportage createRapportage, RapportagesOverzicht rapportagesOverzicht)
        {
            this.createRapportage = createRapportage;
            this.rapportagesOverzicht = rapportagesOverzicht;
        }

        /// <summary>
        ///     opens create `rapportage` window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="rapportagesDataGrid"></param>
        public void NieuwRapportageWindow(object sender, EventArgs e, DataGrid rapportagesDataGrid)
        {
            CreateRapportage createRapportageWindow = new CreateRapportage(rapportagesDataGrid);
            createRapportageWindow.ShowDialog();
        }

        /// <summary>
        ///     creates `rapportage` and saved the created `rapportage` in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="createRapportageGrid"></param>
        /// <param name="rapportagesDataGrid"></param>
        public void CreateRapportage(object sender, EventArgs e, Grid createRapportageGrid,
            DataGrid rapportagesDataGrid)
        {
            Dictionary<string, string> insertRapportageArray = new Dictionary<string, string>();
            foreach (object child in createRapportageGrid.Children)
            {
                if (child.GetType() == typeof(TextBox))
                {
                    TextBox textBox = (TextBox) child;
                    if (textBox.Text == "")
                    {
                        MessageBox.Show("Vul een titel in!");
                        return;
                    }

                    insertRapportageArray["titel"] = textBox.Text;
                }

                if (child.GetType() == typeof(RichTextBox))
                {
                    RichTextBox richTextBox = (RichTextBox) child;
                    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    if (range.Text == "\r\n")
                    {
                        MessageBox.Show("Vul een samenvatting in!");
                        return;
                    }

                    insertRapportageArray["rapportage"] = range.Text;
                }
            }

            insertRapportageArray["user_id"] = Container.GetInstance.Get("userId");
            insertRapportageArray["patient_id"] = Container.GetInstance.Get("patient")["id"].ToString();

            ((QueryBuilder) MysqlSshConnection.InstanceCon).InsertHandler("rapportages", insertRapportageArray);
            OverzichtRapportages(rapportagesDataGrid);
            rapportagesDataGrid.Items.Refresh();
            createRapportage.Close();
        }

        /// <summary>
        ///     loads an overview of all the `rapportages`
        /// </summary>
        /// <param name="overzichtRapportagesDataGrid"></param>
        public void OverzichtRapportages(DataGrid overzichtRapportagesDataGrid)
        {
            DataSet allRapportagesData = ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery
            ("SELECT CONCAT_WS(' ', users.naam, users.achternaam)  as behandelaar, rapportages.id as `rapportages ID`, rapportages.titel, rapportages.datetime,rapportages.rapportage " +
             "FROM rapportages LEFT JOIN users on rapportages.user_id = users.id where rapportages.patient_id = '" +
             Container.GetInstance.Get("patient")["id"] + "' ORDER BY rapportages.datetime desc");
            overzichtRapportagesDataGrid.ItemsSource = allRapportagesData.Tables[0].DefaultView;
        }

        /// <summary>
        ///     handles rapportage detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="metingenDataGrid"></param>
        public void HandleDetailRapportages(object sender, EventArgs e, DataGrid metingenDataGrid)
        {
            //todo
        }
    }
}