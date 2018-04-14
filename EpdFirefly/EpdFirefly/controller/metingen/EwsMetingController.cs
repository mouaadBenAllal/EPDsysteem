/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Metingen
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using ArrayContainer;
    using DatabaseHandler;
    using MySql.Data.MySqlClient;
    using Views.Metingen;
    internal class EwsMetingController
    {
        private readonly CheckEwsMeting checkEwsMeting;
        private readonly CreateEwsMeting ewsMeting;
        private readonly MetingenController metingenController = new MetingenController(null);
        private bool pijnScoreGroterDan7 = false;
        /// <summary>
        /// constructor for ewsmetingcontroller
        /// </summary>
        /// <param name="ewsMeting"></param>
        /// <param name="checkEwsMeting"></param>
        public EwsMetingController(CreateEwsMeting ewsMeting, CheckEwsMeting checkEwsMeting)
        {
            this.ewsMeting = ewsMeting;
            this.checkEwsMeting = checkEwsMeting;
        }
        /// <summary>
        /// creates ewsmeting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="ewsGrid"></param>
        /// <param name="overzichtMetingenGrid"></param>
        public void CreateEwsMetingFunction(object sender, EventArgs e, Grid ewsGrid, DataGrid overzichtMetingenGrid)
        {
            Dictionary<string, string> ewsmetingTableQueryArray = new Dictionary<string, string>();
            Dictionary<string, string> patienHasEwsmetingTableQueryArray = new Dictionary<string, string>();
            patienHasEwsmetingTableQueryArray["patient_id"] = Container.GetInstance.Get("patient")["id"].ToString();

            int ewsMetingPoints = 0;

            foreach (object child in ewsGrid.Children)
            {
                if (child.GetType() == typeof(ComboBox))
                {
                    ComboBox comboBoxItems = (ComboBox) child;
                    foreach (ComboBoxItem item in comboBoxItems.Items) // loops through the comboboxitems
                        if (item.IsSelected)
                        {
                            switch (item.Tag.ToString())
                            {
                                case "0":
                                    ewsMetingPoints += 3;
                                    break;
                                case "1":
                                    ewsMetingPoints += 2;
                                    break;
                                case "2":
                                    ewsMetingPoints += 1;
                                    break;
                                case "20":
                                    ewsMetingPoints += 1;
                                    break;
                                case "3":
                                    ewsMetingPoints += 0;
                                    break;
                                case "4":
                                    ewsMetingPoints += 1;
                                    break;
                                case "5":
                                    ewsMetingPoints += 2;
                                    break;
                                case "6":
                                    ewsMetingPoints += 3;
                                    break;
                                default:
                                    MessageBox.Show("something went wrong");
                                    break;
                            }

                            ewsmetingTableQueryArray[comboBoxItems.Name] = item.Tag.ToString();
                            break;
                        }
                }

                if (child.GetType() == typeof(CheckBox))
                {
                    CheckBox checkbox = (CheckBox) child;

                    if (checkbox.IsChecked == true)
                    {
                        switch (checkbox.Name)
                        {
                            case "saturatie_lager_90":
                                ewsMetingPoints += 3;
                                break;
                            case "pijnscore_grote_7":
                                pijnScoreGroterDan7 = true;
                                MessageBox.Show("Patient NIET vervoeren ALS voertuig laten komen");
                                break;
                            case "ongerustheid":
                                ewsMetingPoints += 1;
                                break;
                        }

                        ewsmetingTableQueryArray[checkbox.Name] = "1";
                    }
                    else
                    {
                        ewsmetingTableQueryArray[checkbox.Name] = "0";
                    }
                }

                ewsmetingTableQueryArray["ews_score"] = ewsMetingPoints.ToString();
            }
            ((QueryBuilder)MysqlSshConnection.InstanceCon).InsertHandler("earlywarningscore", ewsmetingTableQueryArray);
            ((QueryBuilder)MysqlSshConnection.InstanceCon).InsertHandler("patient_has_earlywarningscore",
                patienHasEwsmetingTableQueryArray);

            if (ewsMetingPoints < 2)
                MessageBox.Show("Controleer uw patient na 15 minuten opnieuw!");
            if (ewsMetingPoints < 2 && pijnScoreGroterDan7 == false)
                MessageBox.Show("Als ter plaatse vragen!");
            metingenController.OverzichtMeting(overzichtMetingenGrid);
            overzichtMetingenGrid.Items.Refresh();
            ewsMeting.Close();
        }
        /// <summary>
        /// gets details for the selected ews meting
        /// </summary>
        /// <param name="metingenDataRowArray"></param>
        /// <param name="metingenDataGrid"></param>
        public void GetDetailEwsMeting(Dictionary<string, string> metingenDataRowArray, DataGrid metingenDataGrid)
        {
            DataSet ewsDataSet = ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery(
                "SELECT * FROM " + MySqlHelper.EscapeString(metingenDataRowArray["soortmeting"]) + " where `id` = " +
                MySqlHelper.EscapeString(metingenDataRowArray["id"]));

            CheckEwsMeting checkEwsMeting = new CheckEwsMeting(metingenDataGrid);
            checkEwsMeting.ewsMetingId.Content = metingenDataRowArray["id"];

            foreach (object child in checkEwsMeting.detailEwsGrid.Children)
            {
                if (child.GetType() == typeof(ComboBox))
                {
                    ComboBox comboBoxItems = (ComboBox) child;
                    foreach (ComboBoxItem item in comboBoxItems.Items) // loops through the comboboxitems
                        switch (comboBoxItems.Name)
                        {
                            case "ademfrequentie":
                                if (item.Tag.ToString() == ewsDataSet.Tables[0].Rows[0]["ademfrequentie"].ToString())
                                    item.IsSelected = true;
                                break;
                            case "hartfrequentie":
                                if (item.Tag.ToString() == ewsDataSet.Tables[0].Rows[0]["hartfrequentie"].ToString())
                                    item.IsSelected = true;
                                break;
                            case "systolische_bloeddruk":
                                if (item.Tag.ToString() ==
                                    ewsDataSet.Tables[0].Rows[0]["systolische_bloeddruk"].ToString())
                                    item.IsSelected = true;
                                break;
                            case "bewustzijn":
                                if (item.Tag.ToString() == ewsDataSet.Tables[0].Rows[0]["bewustzijn"].ToString())
                                    item.IsSelected = true;
                                break;
                            case "temperatuur":
                                if (item.Tag.ToString() == ewsDataSet.Tables[0].Rows[0]["temperatuur"].ToString())
                                    item.IsSelected = true;
                                break;
                            default:
                                MessageBox.Show("something went wrong, please contact an administrator");
                                break;
                        }
                }

                if (child.GetType() == typeof(CheckBox))
                {
                    CheckBox checkbox = (CheckBox) child;
                    switch (checkbox.Name)
                    {
                        case "saturatie_lager_90":
                            if (ewsDataSet.Tables[0].Rows[0]["saturatie_lager_90"].ToString() == "1")
                                checkbox.IsChecked = true;
                            break;
                        case "pijnscore_grote_7":
                            if (ewsDataSet.Tables[0].Rows[0]["pijnscore_grote_7"].ToString() == "1")
                                checkbox.IsChecked = true;
                            break;
                        case "ongerustheid":
                            if (ewsDataSet.Tables[0].Rows[0]["ongerustheid"].ToString() == "1")
                                checkbox.IsChecked = true;
                            break;
                    }
                }
            }
            checkEwsMeting.ShowDialog();
        }
        /// <summary>
        /// deletes selected ewsmeting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="ewsMetingId"></param>
        /// <param name="overzichtMetingenGrid"></param>
        public void DeleteEwsMeting(object sender, EventArgs e, Label ewsMetingId, DataGrid overzichtMetingenGrid)
        {
            MessageBoxResult messageBoxResult =
                MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery(
                    "Delete from patient_has_earlywarningscore where earlywarningscore_id = '" + ewsMetingId.Content +
                    "'; Delete from earlywarningscore where id = '" + ewsMetingId.Content + "'");
                metingenController.OverzichtMeting(overzichtMetingenGrid);
                overzichtMetingenGrid.Items.Refresh();
                checkEwsMeting.Close();
            }
        }
    }
}