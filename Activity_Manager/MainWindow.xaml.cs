using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassActivity; 


namespace Activity_Manager 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Activity> listAct = new ObservableCollection<Activity>(); // création d'une liste d'activités avec laquelle travailler 

        public MainWindow()
        {
            InitializeComponent();
            TabActivites.DataContext = listAct; // on a listAct dans le datagrid et se met toujours a jour (prend le nom des propriétés pour les headers) 
        }

        #region METHODES_CLICK
        private void About_Click(object sender, RoutedEventArgs e)   // clic sur about dans file 
        {
            MessageBox.Show("Fait par Pierre Nadin \nCopyright HEPL \nQ2 2018-2019", "About", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void Creer_Click(object sender, RoutedEventArgs e)  // clic sur créer une activité 
        {
            if (TextIntitule.Text != "" && TextDescription.Text != "" && TextLieu.Text != "" && TextDateDebut.Text != "" && TextDateFin.Text != "" && TextOccurences.Text != "" && BoxPeriodicite.Text != "")
            {
                if (Convert.ToDateTime(TextDateDebut.Text) < Convert.ToDateTime(TextDateFin.Text))
                {
                    Activity nouvact = new Activity(TextIntitule.Text, TextDescription.Text, TextLieu.Text, Convert.ToDateTime(TextDateDebut.Text), Convert.ToDateTime(TextDateFin.Text), Convert.ToInt32(TextOccurences.Text), BoxPeriodicite.Text);

                    // supprime les éléments de la listBox 
                    foreach (Activity a in listAct)
                    {
                        ListActivites.Items.Remove(a.Intitule); 
                    }

                    listAct.Add(nouvact);   // ajout dans la liste 

                    // on ajoute le nom de l'activité dans la liste des activités (listbox) 
                    foreach (Activity a in listAct)
                    {
                        ListActivites.Items.Add(a.Intitule);
                    }
                    
                    // nettoyer les entrées 
                    ViderChamps();
                }
                else
                    MessageBox.Show("Incohérence entre la date de début et la date de fin", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            else
                MessageBox.Show("Veuillez remplir les champs pour créer une activité","Erreur",MessageBoxButton.OK, MessageBoxImage.Error); 
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            RecupInfoListAct(listAct);
            ViderChamps();
            //ListActivites.  // data binding pour plus facile (peut etre classe pour gerer liste) 
        }

        private void Annulation_Click(object sender, RoutedEventArgs e) // clic bouton Annuler 
        {
            ViderChamps(); 
        }

        private void ListActivites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecupInfoListAct(listAct); 
        }

        #endregion

        #region METHODES_AUTRES 
        public void ViderChamps()   // nettoyer les champs des entrées 
        {
            TextIntitule.Text = "";
            TextDescription.Text = "";
            TextLieu.Text = "";
            TextDateDebut.Text = "";
            TextDateFin.Text = "";
            TextOccurences.Text = "";
            BoxPeriodicite.Text = "";
        }

        public void RecupInfoListAct(ObservableCollection<Activity> liste) 
        {
            foreach (Activity a in liste) 
            {
                if (ListActivites.SelectedItem.Equals(a.Intitule))
                {
                    TextIntitule.Text = a.Intitule;
                    TextDescription.Text = a.Description;
                    TextLieu.Text = a.Lieu;
                    TextDateDebut.Text = Convert.ToString(a.DateHeureDebut);
                    TextDateFin.Text = Convert.ToString(a.DateHeureFin);
                    TextOccurences.Text = Convert.ToString(a.Occurences);
                    BoxPeriodicite.Text = a.Periodicite.ToString();
                }
            }
        }
        #endregion 
    }
}
