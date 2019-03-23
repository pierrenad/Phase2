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
using Microsoft.Win32;
using System.IO; 
using ClassActivity;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using CsvHelper;
using static Activity_Manager.Option1; 


namespace Activity_Manager 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string nomFichier = "FichierPhase2";
        private static CsvHelper.Configuration.Configuration csvConf = new CsvHelper.Configuration.Configuration { Delimiter = ";" }; 
        ObservableCollection<Activity> listAct = new ObservableCollection<Activity>(); // création d'une liste d'activités avec laquelle travailler 

        public MainWindow()
        {
            InitializeComponent();
            TabActivites.DataContext = listAct; // on a listAct dans le datagrid et se met toujours a jour (prend le nom des propriétés pour les headers) 
        }

        #region MENU 
        #region TOOLS 
        private void MenuOption_Click(object sender, RoutedEventArgs e)
        {
            Option1 o1 = new Option1(this);
            o1.EventCouleurFond += o1_EventCouleurFond; 
            o1.Show();
        }
        private void o1_EventCouleurFond(string DossierDeTravail, SolidColorBrush CouleurFond, SolidColorBrush CouleurPolice) 
        {
            ListActivites.Background = CouleurFond;
            ListActivites.Foreground = CouleurPolice; 
            nomFichier = DossierDeTravail;
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fait par Pierre Nadin \nCopyright HEPL \nQ2 2018-2019", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region FILE 
        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog

#pragma warning disable CS0164 // Cette étiquette n'est pas référencée
        https://docs.microsoft.com/fr-fr/dotnet/api/microsoft.win32.openfiledialog?redirectedfrom=MSDN&view=netframework-4.7.2
#pragma warning restore CS0164 // Cette étiquette n'est pas référencée

            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Activity";                  // nom par défaut  
            dlg.DefaultExt = ".xml";                    // extension par défaut 
            dlg.Filter = "Text documents (.xml)|*.xml"; // extensions pour filtre (celles à afficher) 
            dlg.InitialDirectory = nomFichier;

            //// supprimer les activités de la listAct 
            //foreach (Activity a in listAct)
            //{
            //    listAct.Remove(a);
            //}
            listAct.Clear(); 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                LoadFromXMLFormat(filename);    // on load le document 
            }
            #endregion 
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            #region Application.Current.Shutdown();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Activity";                  // nom par défaut  
            dlg.DefaultExt = ".xml";                    // extension par défaut 
            dlg.Filter = "Text documents (.xml)|*.xml"; // extensions pour filtre (celles à afficher) 
            dlg.InitialDirectory = nomFichier;

            // Show save file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                List<Activity> tmp = new List<Activity>();  // creation d'une liste car on sait pas travailler avec ObservableCollection 

                // on copie notre liste (ObservableCollection) dans la liste (List) 
                foreach (Activity a in listAct)
                {
                    tmp.Add(a); 
                }
                SaveAsXMLFormat(tmp, filename); // on save en .xml 
            }
            #endregion
        }

        private void MenuImport_Click(object sender, RoutedEventArgs e)
        {
            #region OpenFileDialog

#pragma warning disable CS0164 // Cette étiquette n'est pas référencée
        https://docs.microsoft.com/fr-fr/dotnet/api/microsoft.win32.openfiledialog?redirectedfrom=MSDN&view=netframework-4.7.2
#pragma warning restore CS0164 // Cette étiquette n'est pas référencée

            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document";                  // nom par défaut  
            dlg.DefaultExt = ".csv";                    // extension par défaut 
            dlg.Filter = "Text documents (.csv)|*.csv"; // extensions pour filtre (celles à afficher) 
            dlg.InitialDirectory = nomFichier;

            //// supprimer les activités de la listAct 
            //foreach (Activity a in listAct)
            //{
            //    listAct.Remove(a);
            //} 
            listAct.Clear(); 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                // on cree une list comme pour le save car on peut pas travailler avec ObservableCollection 
                List<Activity> tmp = new List<Activity>();

                string filename = dlg.FileName;
                tmp = ReadFile<Activity>(filename); // on récupère les activités dans le fichier 

                // on copier la liste récupérée dans notre ObservableCollection 
                foreach (Activity a in tmp) 
                {
                    listAct.Add(a); 
                }

                // on refresh la liste d'activités et datagrid (sinon peut garder anciens éléments) 
                ListActivites.Items.Clear();
                TabActivites.ItemsSource = null;
                TabActivites.Items.Refresh();

                foreach (Activity a in listAct) ListActivites.Items.Add(a.Intitule);

                TabActivites.ItemsSource = listAct;
                TabActivites.Items.Refresh();
            }
            #endregion 
        }
        public List<T> ReadFile<T>(string filename) where T : class // T est ici une classe, Activity en l'occurence  
        {
            try
            {
                List<T> tmp = new List<T>();
                using (TextReader tr = new StreamReader(filename, Encoding.GetEncoding(1252)))
                {
                    var csv = new CsvReader(tr, csvConf);
                    tmp = csv.GetRecords<T>().ToList();
                }
                return tmp;
            }
            catch
            {
                List<T> tmp = new List<T>();
                return tmp; 
            } 
        }

        private void MenuExport_Click(object sender, RoutedEventArgs e)
        {
            #region Application.Current.Shutdown();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document";                  // nom par défaut  
            dlg.DefaultExt = ".csv";                    // extension par défaut 
            dlg.Filter = "Text documents (.csv)|*.csv"; // extensions pour filtre (celles à afficher) 
            dlg.InitialDirectory = nomFichier;

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                List<Activity> tmp = new List<Activity>();

                // on copie notre liste (ObservableCollection) dans la liste tmp (List) car on travaille avec List 
                foreach(Activity a in listAct)
                {
                    tmp.Add(a); 
                }
                
                bool Erreur = WriteFile<Activity>(tmp, filename);

            }
            #endregion
        }
        public bool WriteFile<T>(List<T> tmp, string filename) where T : class // T est ici une classe, Activity en l'occurence  
        {
            try
            {
                using (TextWriter tr = new StreamWriter(filename, true, Encoding.GetEncoding(1252)))
                {
                    var csv = new CsvWriter(tr, csvConf);
                    csv.WriteRecords(tmp);
                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
        #endregion 
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
            try
            {
                // supprimer l'acticité de la listAct
                foreach (Activity a in listAct)
                {
                    if (ListActivites.SelectedItem.Equals(a.Intitule))
                    {
                        listAct.Remove(a);
                        break;
                    }
                }
            }
            catch (System.NullReferenceException) { }

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
            TabActivites.ItemsSource = listAct;
            TabActivites.Items.Refresh();  
        }
        #endregion

        #region RECHERCHES 
        private void ValideRecherche_Click(object sender, RoutedEventArgs e)
        {
            if(RechercheLieuText.Text == "" && RechercheDateDebText.Text == "" && RechercheDateFinText.Text == "")
            {
                TabActivites.ItemsSource = listAct;
                TabActivites.Items.Refresh();
            }
            // recherche lieu 
            if (RechercheLieuText.Text != "")
            {
                
                string lieu = RechercheLieuText.Text;

                List<Activity> ListLieu = new List<Activity>();

                foreach (Activity a in listAct)
                {
                    if (a.Lieu.Contains(lieu))
                    {
                        ListLieu.Add(a);
                    }
                }

                ListLieu.Sort();
                TabActivites.ItemsSource = ListLieu;
                TabActivites.Items.Refresh();
            }
            //else
            //{
            //    TabActivites.ItemsSource = listAct;
            //    TabActivites.Items.Refresh();
            //}

            // recherche date 
            if (RechercheDateDebText.Text != "" || RechercheDateFinText.Text != "")
            {                
                List<Activity> ListDate = new List<Activity>();
                DateTime Ddeb, Dfin;

                if (RechercheDateDebText.Text != "" && RechercheDateFinText.Text == "")
                {
                    Ddeb = Convert.ToDateTime(RechercheDateDebText.Text);
                    foreach (Activity a in listAct)
                    {
                        if (Ddeb <= Convert.ToDateTime(a.DateHeureDebut))
                        {
                            ListDate.Add(a);
                        }
                    }
                }
                if (RechercheDateDebText.Text == "" && RechercheDateFinText.Text != "")
                {
                    Dfin = Convert.ToDateTime(RechercheDateFinText.Text);
                    foreach (Activity a in listAct)
                    {
                        if (Dfin >= Convert.ToDateTime(a.DateHeureFin))
                        {
                            ListDate.Add(a);
                        }
                    }
                }
                if (RechercheDateDebText.Text != "" && RechercheDateFinText.Text != "")
                {
                    Ddeb = Convert.ToDateTime(RechercheDateDebText.Text);
                    Dfin = Convert.ToDateTime(RechercheDateFinText.Text);

                    foreach (Activity a in listAct)
                    {
                        if (Ddeb <= Convert.ToDateTime(a.DateHeureDebut) && Dfin >= Convert.ToDateTime(a.DateHeureFin))
                        {
                            ListDate.Add(a);
                        }
                    }
                }

                ListDate.Sort();
                TabActivites.ItemsSource = ListDate;
                TabActivites.Items.Refresh();
            }
            //else
            //{
            //    TabActivites.ItemsSource = listAct;
            //    TabActivites.Items.Refresh();
            //}
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

        private void ViderTout_Click(object sender, RoutedEventArgs e)
        {
            listAct.Clear();
            ViderChamps();
            ListActivites.Items.Clear(); 
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
            try
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
            catch(System.NullReferenceException)
            {

            }
        }
        #endregion

        #region XML CSV
        private static void SaveAsXMLFormat(List<Activity> tmp, string filename)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Activity>));
            using (Stream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) // nom, mode creation, permissions, permissions de partage
            {
                xmlFormat.Serialize(fStream, tmp);
            }
        }
        private void LoadFromXMLFormat(string filename)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Activity>));
            using (Stream fStream = File.OpenRead(filename))
            {
                List<Activity> tmp = (List<Activity>)xmlFormat.Deserialize(fStream);    // récupère info et met dans liste
                
                // on copie ce qu'on recupere dans notre liste (ObservableCollection) 
                foreach(Activity a in tmp)
                {
                    listAct.Add(a); 
                }

                ListActivites.Items.Clear();        // vider la liste d'activités 
                TabActivites.ItemsSource = null;    // vider le datagrid 
                TabActivites.Items.Refresh(); 

                // on ajoute les noms dans la liste d'activités 
                foreach (Activity a in listAct)
                {
                    ListActivites.Items.Add(a.Intitule);
                } 

                TabActivites.ItemsSource = listAct; // ajoute dans datagrid 
                TabActivites.Items.Refresh();
            }
        }
        #endregion
        
    }
}
