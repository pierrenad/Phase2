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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO; 

namespace Activity_Manager
{
    /// <summary>
    /// Interaction logic for Option1.xaml
    /// </summary>
    public partial class Option1 : Window
    {
        public delegate void Option(string DossierDeTravail, SolidColorBrush CouleurFond, SolidColorBrush CouleurPolice);
        public event Option EventCouleurFond;

        private MainWindow tmp;
        private string nomFichier;
        private SolidColorBrush couleurFond;
        private SolidColorBrush couleurFond2;

        public Option1(MainWindow mainWindow)
        {
            InitializeComponent();
            tmp = mainWindow;
            nomFichier = mainWindow.nomFichier;
            couleurFond = (SolidColorBrush)tmp.ListActivites.Background;
            couleurFond2 = (SolidColorBrush)tmp.ListActivites.Foreground; 
        }

        #region CHOIX_BUTTONS
        private void DossierChoix_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = nomFichier;
            dialog.ShowDialog();
            nomFichier = dialog.SelectedPath;
        }

        private void CouleurFondChoix_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) couleurFond = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B)); 
        }

        private void CouleurPoiceChoix_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) couleurFond2 = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B)); 
        }

        #endregion

        #region TERMINER_CANCEL_BOUTONS 
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EventCouleurFond?.Invoke(nomFichier, couleurFond, couleurFond2);
            Close(); 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void AppliquerButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventCouleurFond != null) EventCouleurFond(nomFichier, couleurFond, couleurFond2); 
        } 
        #endregion 

    }
}
