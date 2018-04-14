/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/
namespace EpdFirefly.Controller.Patientendossier
{

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using ArrayContainer;
    using DatabaseHandler;
    using EpdFirefly.Controller.Users;
    using Views.Patientendossier;

    internal class PatientController
    {
        /// <summary>
        /// closes window
        /// </summary>
        /// <param name="window"></param>
        public void Terug(AddPatient window)
        {
            Home navigation = new Home();
            window.Close();
            navigation.ShowDialog();
        }

        /// <summary>
        /// opens the create patient window
        /// </summary>
        /// <param name="patientenDataGrid"></param>
        public void AddPatientView(DataGrid patientenDataGrid)
        {
            AddPatient addPatient = new AddPatient(patientenDataGrid);
            addPatient.ShowDialog();
        }

        /// <summary>
        /// adds new patient to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="stackpanel"></param>
        /// <param name="addPatient"></param>
        /// <param name="patientenDataGrid"></param>
        public void SavePatient(object sender, EventArgs e, StackPanel stackpanel, AddPatient addPatient, DataGrid patientenDataGrid)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            array["userid"] = Container.GetInstance.Get("userId");
            foreach (object child in stackpanel.Children)
                if (child.GetType() == typeof(TextBox))
                    array[((TextBox) child).Name] =
                        !string.IsNullOrEmpty(((TextBox) child).Text) ? ((TextBox) child).Text : null;

            if (Checkifempty(addPatient))
            {
                MessageBox.Show("Niet alle velden zijn ingevuld!");
            }
            else
            {
                ((QueryBuilder)MysqlSshConnection.InstanceCon).InsertHandler("patient", array);
                this.PatientOverzicht(patientenDataGrid);
                addPatient.Close();
            }
        }

        /// <summary>
        /// loads a overview of al patients
        /// </summary>
        /// <param name="view"></param>
        public void Overzicht(PatientDossier view)
        {
            DataSet data = ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery(
                "SELECT * FROM patient WHERE id = " + Container.GetInstance.Get("patient")["id"].ToString() + "");
            foreach (DataRow user in data.Tables[0].Rows)

            foreach (DataColumn usercol in data.Tables[0].Columns)
            {
                TextBox textBox = new TextBox();
                textBox.Text = user[usercol.ColumnName].ToString();
                textBox.IsReadOnly = true;
                Label label = new Label();
                label.Content = usercol.ColumnName;
                if ((usercol.ColumnName == "id") | (usercol.ColumnName == "naam"))
                {
                    if (usercol.ColumnName == "naam")
                    {
                        view.Patientstack.Children.Add(label);
                        view.Patientstack.Children.Add(textBox);

                        view.naamblock.Text = "Patient naam: " + user[usercol.ColumnName];


                        }
                }
                else
                {
                    view.Patientstack.Children.Add(label);
                    view.Patientstack.Children.Add(textBox);
                }

                Thickness margin = textBox.Margin;
                margin.Bottom = 20;
                textBox.Margin = margin;
            }
        }

        /// <summary>
        /// deletes selected patient
        /// </summary>
        /// <param name="pagesNavigation"></param>
        /// <param name="patientenDataGrid"></param>
        public void DeletePatient(PagesNavigation pagesNavigation, DataGrid patientenDataGrid)
        {
            MessageBoxResult messageBoxResult =
                MessageBox.Show("Weet u zeker dat u deze patient wilt verwijderen?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery(
                    "DELETE FROM patient WHERE id = " + Container.GetInstance.Get("patient")["id"]);
                Container.GetInstance.Unset("patient");
                this.PatientOverzicht(patientenDataGrid);
                pagesNavigation.Close();
            }
        }

        /// <summary>
        /// loads overzicht.
        /// </summary>
        /// <param name="patientenDataGrid"></param>
        public void PatientOverzicht(DataGrid patientenDataGrid)
        {
            DataSet data = ((QueryBuilder)MysqlSshConnection.InstanceCon).CustomeQuery("SELECT patient.id as `Patienten ID`, patient.naam, patient.achternaam, patient.woonplaats,patient.postcode,patient.straatnaam, CONCAT_WS(' ', users.naam, users.achternaam) as `Behandelaar` FROM patient left join users on patient.userid = users.id where patient.userid = " + Container.GetInstance.Get("userId"));
            patientenDataGrid.ItemsSource = data.Tables[0].DefaultView;
        }

        public bool Checkifempty(AddPatient patientView)
        {
            
            foreach (object fields in patientView.Stackpanelpatients.Children)
            {
                if (fields.GetType() == typeof(TextBox))
                {  
                    if (String.IsNullOrEmpty(((TextBox)fields).Text))
                        return true;
                }
            }

            return false;
        }
    }
}