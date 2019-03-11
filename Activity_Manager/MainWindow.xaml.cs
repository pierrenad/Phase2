using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void About_Click(object sender, RoutedEventArgs e)   // clic sur about dans file 
        {
            MessageBox.Show("Fait par Pierre Nadin \nCopyright HEPL \nQ2 2018-2019", "About", MessageBoxButton.OK, MessageBoxImage.Information); 
        }

        private void Creer_Click(object sender, RoutedEventArgs e)  // clic sur créer une activité 
        {
            if (TextIntitule.Text != "" && TextDescription.Text != "" && TextLieu.Text != "" && TextDateDebut.Text != "" && TextDateFin.Text != "" && TextOccurences.Text != "" && BoxPeriodicite.Text != "")
            {
                if(Convert.ToDateTime(TextDateDebut.Text) < Convert.ToDateTime(TextDateFin.Text))
                {
                    Activity nouvact = new Activity(TextIntitule.Text, TextDescription.Text, TextLieu.Text, Convert.ToDateTime(TextDateDebut.Text), Convert.ToDateTime(TextDateFin.Text), Convert.ToInt32(TextOccurences.Text), BoxPeriodicite.Text);

                    TabActivites.ItemsSource = nouvact.Lieu;
                    TabActivites.Items.Refresh();

                    ListActivites.Items.Add(nouvact.Intitule);
                    ViderChamps(); 
                }
            }
            else
                MessageBox.Show("Veuillez remplir les champs pour créer une activité","Erreur",MessageBoxButton.OK, MessageBoxImage.Error); 
        }

        private void Annulation_Click(object sender, RoutedEventArgs e) // clic bouton Annuler 
        {
            ViderChamps(); 
        }

        public void ViderChamps()
        {
            TextIntitule.Text = "";
            TextDescription.Text = "";
            TextLieu.Text = "";
            TextDateDebut.Text = "";
            TextDateFin.Text = "";
            TextOccurences.Text = "";
            BoxPeriodicite.Text = "";
        }
    }
}
