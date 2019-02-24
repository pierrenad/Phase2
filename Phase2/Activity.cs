using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phase2
{
    class Activity
    {
        #region VARIABLES 
        public enum periodicite { quotidienne, hebdomadaire, mensuelle, annuelle }; 
        private string _intitule;
        private string _description;
        private string _lieu;
        private DateTime _dateHeureDebut;
        private DateTime _dateHeureFin;
        private int _occurences;
        private periodicite _periodicite;
        #endregion

        #region PROPRIETES 
        public string Intitule
        {
            get { return _intitule; } 
            set { _intitule = value; } 
        }

        public string Description
        {
            get { return _description; } 
            set { _description = value; } 
        }

        public string Lieu
        {
            get { return _lieu; }
            set { _lieu = value; } 
        }

        public DateTime DateHeureDebut 
        {
            get { return _dateHeureDebut; }
            set { _dateHeureDebut = value;  } 
        }

        public DateTime DateHeureFin
        {
            get { return _dateHeureFin; }
            set { _dateHeureFin = value; } 
        }

        public int Occurences
        {
            get { return _occurences; }
            set { _occurences = value; } 
        }

        public periodicite Periodicite 
        {
            get { return _periodicite; }
            set { _periodicite = value; } 
        } 
        #endregion 


    }
}
