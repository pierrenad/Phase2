﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassActivity 
{
    public class Activity : IComparable<Activity>  
    {
        #region VARIABLES 
        public enum periodicite { quotidienne, hebdomadaire, mensuelle, annuelle, nulle }; 
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

        public Activity()
        {
            Intitule = null;
            Description = null;
            Lieu = null;
            DateHeureDebut = new DateTime();
            DateHeureFin = new DateTime();
            Occurences = 0;
            Periodicite = periodicite.nulle; 
        }

        public Activity(string intitule, string description, string lieu, DateTime dateDebut, DateTime dateFin, int occur, string period)
        {
            Intitule = intitule;
            Description = description;
            Lieu = lieu;
            DateHeureDebut = dateDebut;
            DateHeureFin = dateFin;
            Occurences = occur;
            switch (period)
            {
                case "quotidienne": 
                    Periodicite = periodicite.quotidienne;
                    break;
                case "hebdomadaire":
                    Periodicite = periodicite.hebdomadaire;
                    break;
                case "mensuelle":
                    Periodicite = periodicite.mensuelle; 
                    break;
                case "annuelle":
                    Periodicite = periodicite.annuelle; 
                    break;
            }
        }
        #endregion

        #region METHODES 
        public static String PeriodicityToString(periodicite p)
        {
            switch(p)
            {
                case periodicite.annuelle:
                    return "annuelle"; 
                case periodicite.mensuelle:
                    return "mensuelle";
                case periodicite.hebdomadaire:
                    return "hebdomadaire";
                case periodicite.quotidienne:
                    return "quotidienne";
                default:
                    return ""; 
            }
        }

        public static periodicite StringToPeriodicite(String s)
        {
            switch(s)
            {
                case "annuelle":
                    return periodicite.annuelle;
                case "mensuelle":
                    return periodicite.mensuelle;
                case "hebdomadaire":
                    return periodicite.hebdomadaire;
                case "quotidienne":
                    return periodicite.quotidienne; 
                default:
                    return periodicite.nulle; 
            }
        }

        public int CompareTo(Activity other)
        {
            return Intitule.CompareTo(other.Intitule); 
        } 

        #endregion

    }
}
