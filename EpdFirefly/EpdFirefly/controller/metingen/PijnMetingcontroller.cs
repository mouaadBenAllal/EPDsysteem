/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Metingen
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using ArrayContainer;
    using DatabaseHandler;
    using Views.Metingen;
    internal class PijnMetingController
    {
        private readonly CreatePijnMeting pijnMeting;


        /// <summary>
        ///     constructor for pijnmetingcontroller
        /// </summary>
        /// <param name="pijnMeting"></param>
        public PijnMetingController(CreatePijnMeting pijnMeting)
        {
            this.pijnMeting = pijnMeting;
        }


        /// <summary>
        /// creates new pijnmeting
        /// </summary>
        /// <param name="sends"></param>
        /// <param name="e"></param>
        /// <param name="pijnscoreGrid"></param>
        /// <param name="overzichtMetingenGrid"></param>
        public void CreatePijnMetingFunction(object sends, EventArgs e, Grid pijnscoreGrid,
            DataGrid overzichtMetingenGrid)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            Dictionary<string, string> patienHasPijnmetingTableQueryArray = new Dictionary<string, string>();
            patienHasPijnmetingTableQueryArray["patient_id"] = Container.GetInstance.Get("patient")["id"].ToString();
            //loops through grid.childeren
            foreach (object child in pijnscoreGrid.Children)
                if (child.GetType() == typeof(ComboBox))
                {
                    ComboBox comboBoxItems = (ComboBox) child;
                    foreach (ComboBoxItem item in comboBoxItems.Items) // loops through the comboboxitems
                        if (item.IsSelected)
                        {
                            array[comboBoxItems.Name] = item.Tag.ToString();

                            switch (item.Tag.ToString())
                            {
                                case "0":
                                    MessageBox.Show("De patient heeft geen pijn!");
                                    break;
                                case "1":
                                    MessageBox.Show("De patient heeft milde pijn!");
                                    break;
                                case "2":
                                    MessageBox.Show("De patient heeft matige pijn!");
                                    break;
                                case "3":
                                    MessageBox.Show("De patient heeft ernstige pijn!");
                                    break;
                                case "4":
                                    MessageBox.Show("De patient heeft ergst denkbare pijn!");
                                    break;
                                default:
                                    MessageBox.Show("Er is iets fout gegaan!");
                                    break;
                            }

                            break;
                        }

                    ((QueryBuilder) MysqlSshConnection.InstanceCon).InsertHandler("pijnscore", array);
                    ((QueryBuilder) MysqlSshConnection.InstanceCon).InsertHandler("patient_has_pijnscore",
                        patienHasPijnmetingTableQueryArray);
                    MetingenController metingenController = new MetingenController(null);
                    metingenController.OverzichtMeting(overzichtMetingenGrid);
                    overzichtMetingenGrid.Items.Refresh();
                    pijnMeting.Close();
                }
        }
    }
}