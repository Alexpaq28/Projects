using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POKERTP2ALEXIS
{
    public class Carte
    {
        public string Valeur { get; }
        public string Couleur { get; }

        public Carte(string couleur, string valeur)
        {
            Couleur = couleur;
            Valeur = valeur;
        }

        public int GetForceCarte()
        {
            string[] valeursOrdinales = { "Deux", "Trois", "Quatre", "Cinq", "Six", "Sept", "Huit", "Neuf", "Dix", "Valet", "Dame", "Roi", "As" };
            int force = Array.IndexOf(valeursOrdinales, Valeur);

            if (force == 0 && Valeur == "As")
            {
                force = 13;
            }

            return force + 2;
        }

    }
}
