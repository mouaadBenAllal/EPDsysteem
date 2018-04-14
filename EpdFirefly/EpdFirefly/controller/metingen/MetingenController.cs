/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Metingen
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows.Controls;
    using ArrayContainer;
    using DatabaseHandler;
    using Views.Metingen;
    internal class MetingenController
    {
        private MetingenOverzicht metingenOverzicht;
        /// <summary>
        /// constructor for metingencontroller
        /// </summary>
        /// <param name="metingenOverzicht"></param>
        public MetingenController(MetingenOverzicht metingenOverzicht)
        {
            this.metingenOverzicht = metingenOverzicht;
        }
        /// <summary>
        /// handles inputs on metingen page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="overzichtMetingenGrid"></param>
        public void HandleMetingen(object sender, EventArgs e, DataGrid overzichtMetingenGrid)
        {
            Button btn = (Button) sender;
            switch (btn.Name)
            {
                case "Ews":
                    CreateEwsMeting createEwsMeting = new CreateEwsMeting(overzichtMetingenGrid);
                    createEwsMeting.ShowDialog();
                    break;
                case "Pijnscore":
                    CreatePijnMeting createPijnMeting = new CreatePijnMeting(overzichtMetingenGrid);
                    createPijnMeting.ShowDialog();
                    break;
                case "SNAQ":
                    break;
                case "DOS":
                    break;
                case "BradenDecubitus":
                    break;
                case "DecubitusScore":
                    break;
            }
        }
        /// <summary>
        /// fills datagrid with all `metingen` information
        /// </summary>
        /// <param name="overzichtMetingenGrid"></param>
        public void OverzichtMeting(DataGrid overzichtMetingenGrid)
        {
            DataSet allMetingenData = ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery
            ("SELECT 'Pijnmeting' as  `soortmeting`, `id`,`pijnscore` as `score`,`datetime`" +
             " FROM pijnscore inner JOIN patient_has_pijnscore ON patient_has_pijnscore.pijnscore_id = pijnscore.id where patient_id =" +
             " '" + Container.GetInstance.Get("patient")["id"] +
             "' UNION SELECT 'Ewsmeting' as  `soortmeting`, `id`,`ews_score` as `score`,`datetime`" +
             " FROM earlywarningscore INNER JOIN patient_has_earlywarningscore ON patient_has_earlywarningscore.earlywarningscore_id = earlywarningscore.id" +
             " where patient_id = '" + Container.GetInstance.Get("patient")["id"] + "' order by datetime DESC");

            overzichtMetingenGrid.ItemsSource = allMetingenData.Tables[0].DefaultView;
        }
        /// <summary>
        /// handles the details of a `meting`
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="metingenDataGrid"></param>
        public void HandleDetailMetingen(object sender, EventArgs e, DataGrid metingenDataGrid)
        {
            DataRowView selectedRow = metingenDataGrid.SelectedItem as DataRowView;
            Dictionary<string, string> metingenDataRowArray = new Dictionary<string, string>();
            if (selectedRow.Row.ItemArray[0].ToString() == "Ewsmeting")
            {
                EwsMetingController ewsMetingController = new EwsMetingController(null, null);
                metingenDataRowArray["soortmeting"] = "earlywarningscore";
                metingenDataRowArray["id"] = selectedRow.Row.ItemArray[1].ToString();
                ewsMetingController.GetDetailEwsMeting(metingenDataRowArray, metingenDataGrid);
            }
        }
    }
}