/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/

namespace EpdFirefly.Controller.Users
{
    using System.Collections.Generic;
    using System.Data;
    using System.Windows.Controls;
    using ArrayContainer;
    using DatabaseHandler;
    using Patientendossier;
    using Views.Anamnese;
    using Views.Metingen;
    using Views.Patientendossier;
    using Views.Rapportages;
    internal class PagesNavigationController
    {
        /// <summary>
        /// Handles the mainframe in the patientendossier page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mainframe"></param>
        /// <param name="pagesNavigation"></param>
        /// <param name="home"></param>
        /// <param name="patientenDataGrid"></param>
        public void PagesHandler(object sender, Frame mainframe, PagesNavigation pagesNavigation, Home home, DataGrid patientenDataGrid)
        {
            Button btn = (Button) sender;
            switch (btn.Name)
            {
                case "DossierButton":
                    PatientDossier patient = new PatientDossier(pagesNavigation, patientenDataGrid);
                    mainframe.NavigationService.Navigate(patient);
                    break;
                case "AnamneseButton":
                    AnamneseOverzicht anamneseButton = new AnamneseOverzicht(pagesNavigation, patientenDataGrid);
                    mainframe.NavigationService.Navigate(anamneseButton);
                    break;
                case "MetingenButton":
                    MetingenOverzicht metingenOverzicht = new MetingenOverzicht();
                    mainframe.NavigationService.Navigate(metingenOverzicht);
                    break;
                case "RapportageButton":
                    RapportagesOverzicht rapportagesOverzicht = new RapportagesOverzicht();
                    mainframe.NavigationService.Navigate(rapportagesOverzicht);
                    break;
                case "PatientDossier":
                    break;
            }
        }

        /// <summary>
        /// opens patient page
        /// </summary>
        /// <param name="patientenDataGrid"></param>
        /// <param name="home"></param>
        public void OpenPatient(DataGrid patientenDataGrid,Home home)
        {
            DataRowView selectedRow = patientenDataGrid.SelectedItem as DataRowView;
            Dictionary<string, string> patientenDataRowArray = new Dictionary<string, string>();
            patientenDataRowArray["id"] = selectedRow.Row.ItemArray[0].ToString();

            DataSet patientDataRow = ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery("SELECT * FROM patient WHERE userid = " + Container.GetInstance.Get("userId") + " AND id = " + patientenDataRowArray["id"]);
            Container.GetInstance.Set("patient", patientDataRow.Tables[0].Rows[0]);
            PagesNavigation navigation = new PagesNavigation(home, patientenDataGrid);

            if (!CheckIfAnamneseForPatient(navigation, patientenDataGrid))
            {
                PatientDossier patient = new PatientDossier(navigation, patientenDataGrid);
                navigation.mainFrame.NavigationService.Navigate(patient);
                navigation.ShowDialog();
            }
        }

        /// <summary>
        /// this functions checks if an anamnese for a patient exists in the database
        /// </summary>
        /// <param name="pagesnavigation"></param>
        /// <param name="patientenDataGrid"></param>
        /// <returns></returns>
        public bool CheckIfAnamneseForPatient(PagesNavigation pagesnavigation, DataGrid patientenDataGrid)
        {
            DataSet data = ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery(
                "SELECT * FROM anamneslijst WHERE patient_id =" + Container.GetInstance.Get("patient")["id"]);
            //if patienten anamnese exist : enable buttons
            if (data.Tables[0].Rows.Count == 0)
            {
                pagesnavigation.DossierButton.IsEnabled = false;
                pagesnavigation.MetingenButton.IsEnabled = false;
                pagesnavigation.RapportageButton.IsEnabled = false;
                AnamneseOverzicht anamneseButton = new AnamneseOverzicht(pagesnavigation,patientenDataGrid);
                pagesnavigation.mainFrame.NavigationService.Navigate(anamneseButton);
                pagesnavigation.ShowDialog();

                return true;
            }
            // else if patienten anamnese does not exist : enable buttons

            pagesnavigation.DossierButton.IsEnabled = true;
            pagesnavigation.MetingenButton.IsEnabled = true;
            pagesnavigation.RapportageButton.IsEnabled = true;

            return false;
        }
    }
}