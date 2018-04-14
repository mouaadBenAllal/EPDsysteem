/*
 *Application name  : Elektronische patientendossier
 *Author            : Team firefly
*/

namespace EpdFirefly.Controller.Anamnese
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using ArrayContainer;
    using DatabaseHandler;
    using Patientendossier;
    using Views.Anamnese;
    internal class AnamneseController
    {
        /// <summary>
        /// Method to navigate to the create anamnese page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="nav"></param>
        /// <param name="pagesNavigation"></param>
        /// <param name="patientenDataGrid"></param>
        public void CreateAnamnese(object sender, NavigationService nav , PagesNavigation pagesNavigation, DataGrid patientenDataGrid)
        {
            AnamneseCreate anamneseCreate = new AnamneseCreate(pagesNavigation, patientenDataGrid);
            Button btn = (Button) sender;
            if (btn.Name == "CreateAnamneseBtn") nav.Navigate(anamneseCreate);
        }


        /// <summary>
        /// Method that inserts all fields into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="stackpanel"></param>
        /// <param name="nav"></param>
        /// <param name="page"></param>
        /// <param name="patientenDataGrid"></param>
        public void Create(object sender, StackPanel stackpanel, NavigationService nav, PagesNavigation page, DataGrid patientenDataGrid)
        {

            Dictionary<string, string> array = new Dictionary<string, string>();

            foreach (FrameworkElement child in stackpanel.Children)
            {
                Grid grid = child as Grid;

                foreach (object textfield in grid.Children)

                    if (textfield.GetType() == typeof(TextBox))
                    {
                        TextBox textbox = textfield as TextBox;
                        array[textbox.Name] = !string.IsNullOrEmpty(textbox.Text) ? textbox.Text : null;
                    }
            }

            if (!Checkifempty(stackpanel))
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u de anamnese wilt opslaan?", "Anamnse opslaan", MessageBoxButton.YesNoCancel);
                AnamneseOverzicht anamneseOverzicht = new AnamneseOverzicht(page, patientenDataGrid);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageBox.Show("Anamanese aangemaakt! Klik op 'Anamnese' in het menu om deze te bekijken.");
                        ((QueryBuilder)MysqlSshConnection.InstanceCon).InsertHandler("anamneslijst", array);
                        nav.Navigate(anamneseOverzicht);
                        page.Close();
                        Home home = new Home();
                        PagesNavigation a = new PagesNavigation(home, patientenDataGrid);
                        a.ShowDialog();

                        break;

                    case MessageBoxResult.No:
                        break;

                    case MessageBoxResult.Cancel:
                        break;
                }
            }            
        }


        /// <summary>
        /// Shows summary of all anamneses.
        /// </summary>
        /// <param name="overzicht"></param>
        public void Overzicht(AnamneseOverzicht overzicht)
        {
            
            DataSet data = ((QueryBuilder) MysqlSshConnection.InstanceCon).CustomeQuery(
                "SELECT * FROM anamneslijst WHERE patient_id = " + Container.GetInstance.Get("patient")["id"]);

            // patiennaam
            overzicht.patientNaamTxt.Text = "Patient naam: " + Container.GetInstance.Get("patient")["naam"];


            if (data.Tables[0].Rows.Count > 0) overzicht.CreateAnamneseBtn.Visibility = Visibility.Hidden;
            if (data.Tables[0].Rows.Count > 0) overzicht.updateBtn.Visibility = Visibility.Visible;
            if (data.Tables[0].Rows.Count > 0) overzicht.deletebtn.Visibility = Visibility.Hidden;

            foreach (DataRow anamnese in data.Tables[0].Rows)
            foreach (DataColumn dc in data.Tables[0].Columns)
            {
                TextBox textBox = new TextBox();
                textBox.Text = anamnese[dc.ColumnName].ToString();
                textBox.IsReadOnly = true;
                Label label = new Label();
                label.Content = dc.ColumnName;

                overzicht.Patientstack.Children.Add(label);
                overzicht.Patientstack.Children.Add(textBox);

                Thickness margin = textBox.Margin;
                margin.Bottom = 20;
                textBox.Margin = margin;
            }        
        }

        /// <summary>
        /// Method that checks if 
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public bool Checkifempty(StackPanel stack)
        {
            int count = 0;
            foreach (object grid in stack.Children)
            {
                foreach (object textfield in ((Grid)grid).Children)
                    if (textfield.GetType() == typeof(TextBox))
                        if (!String.IsNullOrEmpty(((TextBox) textfield).Text))
                            count++;
            }

            if (count >= 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}