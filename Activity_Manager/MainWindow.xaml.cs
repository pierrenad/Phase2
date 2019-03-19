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

        #region MENU 
        private void MenuOption_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fait par Pierre Nadin \nCopyright HEPL \nQ2 2018-2019", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
        #endregion

        #region TOOLBAR 
        private void Creer_Click(object sender, RoutedEventArgs e)  // clic sur créer une activité 
        {
            if (TextIntitule.Text != "" && TextDescription.Text != "" && TextLieu.Text != "" && TextDateDebut.Text != "" && TextDateFin.Text != "" && TextOccurences.Text != "" && BoxPeriodicite.Text != "")
            {
                if (Convert.ToDateTime(TextDateDebut.Text) <= Convert.ToDateTime(TextDateFin.Text))
                {
                    // supprime les éléments de la listBox 
                    foreach (Activity a in listAct)
                    {
                        ListActivites.Items.Remove(a.Intitule); 
                    }

                    // ajout dans la liste d'une nouvelle activité 
                    try
                    {
                        listAct.Add(new Activity
                        {
                            Intitule = TextIntitule.Text,
                            Description = TextDescription.Text,
                            Lieu = TextLieu.Text,
                            DateHeureDebut = Convert.ToDateTime(TextDateDebut.Text),
                            DateHeureFin = Convert.ToDateTime(TextDateFin.Text),
                            Occurences = Convert.ToInt32(TextOccurences.Text),
                            Periodicite = Activity.StringToPeriodicite(BoxPeriodicite.Text)
                        });
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Pas bonne occurence !", "erreur", MessageBoxButton.OK, MessageBoxImage.Error); 
                    }

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

        private void Supprimer_Click(object sender, RoutedEventArgs e) // clic sur supprimer une activité 
        {
            RecupInfoListAct(listAct);  // récupérer les infos de l'activité choisie 

            // supprimer l'acticité de la listAct
            foreach (Activity a in listAct)
            {
                if (ListActivites.SelectedItem.Equals(a.Intitule))
                {
                    listAct.Remove(a);
                    break; 
                }
            }

            // supprime les éléments de la listBox 
            ListActivites.Items.Clear(); 

            // on remet la listActivites a jour 
            foreach (Activity a in listAct)
            {
                ListActivites.Items.Add(a.Intitule); 
            }

            ViderChamps();  // on vide les champs de l'activité 
        }

        private void Modifier_Click(object sender, RoutedEventArgs e) // clic sur modifier une activité 
        {
            if (TextIntitule.Text != "" && TextDescription.Text != "" && TextLieu.Text != "" && TextDateDebut.Text != "" && TextDateFin.Text != "" && TextOccurences.Text != "" && BoxPeriodicite.Text != "")
            {
                if (Convert.ToDateTime(TextDateDebut.Text) < Convert.ToDateTime(TextDateFin.Text))
                {
                    // cherche l'activité dans la liste pour la modifier mais modifie pas 
                    foreach (Activity a in listAct)
                    {
                        if (ListActivites.SelectedItem.Equals(a.Intitule))
                        {
                            a.Intitule = TextIntitule.Text;
                            a.Description = TextDescription.Text;
                            a.Lieu = TextLieu.Text;
                            a.DateHeureDebut = Convert.ToDateTime(TextDateDebut.Text);
                            a.DateHeureFin = Convert.ToDateTime(TextDateFin.Text);
                            a.Occurences = Convert.ToInt32(TextOccurences.Text);
                            a.Periodicite = Activity.StringToPeriodicite(BoxPeriodicite.Text);
                            break;
                        }
                    }
                }
                else
                    MessageBox.Show("Incohérence entre la date de début et la date de fin", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Veuillez remplir les champs pour créer une activité", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);


            // supprime les éléments de la listBox 
            ListActivites.Items.Clear();

            // on remet la listActivites a jour 
            foreach (Activity a in listAct)
            {
                ListActivites.Items.Add(a.Intitule);
            }

            // remet le tableau a jour 
            TabActivites.DataContext = null;
            TabActivites.DataContext = listAct; 
        }
        #endregion

        #region AUTRES_METHODES
        private void ListActivites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecupInfoListAct(listAct); 
        }

        private void Annulation_Click(object sender, RoutedEventArgs e) // clic bouton Annuler 
        {
            ViderChamps();
        }

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

        public void RecupInfoListAct(ObservableCollection<Activity> liste) // récupère infos d'une activité et les affiches dans les champs 
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
