using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POKERTP2ALEXIS
{
    public class Paquet
    {
        public List<Carte> Cartes { get; }

        public Paquet()
        {
            Cartes = GenererPaquetDeCartes();
        }

        private List<Carte> GenererPaquetDeCartes()
        {
            List<Carte> paquet = new List<Carte>();
            string[] couleurs = { "Pique", "Trèfle", "Carreau", "Coeur" };
            string[] valeurs = { "Deux", "Trois", "Quatre", "Cinq", "Six", "Sept", "Huit", "Neuf", "Dix", "Valet", "Dame", "Roi", "As" };

            foreach (string couleur in couleurs)
            {
                foreach (string valeur in valeurs)
                {
                    paquet.Add(new Carte(couleur, valeur));
                }
            }

            return paquet;
        }
    }
}
