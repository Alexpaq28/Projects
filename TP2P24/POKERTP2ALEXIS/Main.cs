using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POKERTP2ALEXIS
{
    public class Main
    {
        private List<Carte> _cartesRetirees;
        private Random _alea;

        public List<Carte> MainCourante { get; private set; }

        public Main(Paquet paquet)
        {
            _cartesRetirees = new List<Carte>();
            _alea = new Random();
            MainCourante = GenererMainAleatoire(paquet);
        }

        private List<Carte> GenererMainAleatoire(Paquet paquet)
        {
            List<Carte> main = new List<Carte>();

            for (int i = 0; i < 5; i++)
            {
                int index = _alea.Next(paquet.Cartes.Count);
                Carte carte = paquet.Cartes[index];
                main.Add(carte);
                paquet.Cartes.RemoveAt(index);
            }

            return main;
        }
        public enum TypeMain
        {
            Rien,
            Paire,
            DeuxPaires,
            Brelan,
            Quinte,
            Couleur,
            Full,
            Carre,
            QuinteFlush,
            QuinteFlushRoyale
        }

        public TypeMain DeterminerTypeMain()
        {
            List<int> valeurs = MainCourante.Select(carte => carte.GetForceCarte()).ToList();

            var groupeValeurs = valeurs.GroupBy(valeur => valeur).ToList();
            var groupesTri = groupeValeurs.OrderByDescending(grp => grp.Count()).ThenByDescending(grp => grp.Key).ToList();

            int nbCartesIdentiques = groupesTri.First().Count();
            bool estCouleur = MainCourante.All(carte => carte.Couleur == MainCourante[0].Couleur);

            bool estQuinte = false;
            for (int i = 0; i < valeurs.Count - 1; i++)
            {
                if (valeurs[i] == valeurs[i + 1] - 1)
                {
                    estQuinte = true;
                }
                else
                {
                    estQuinte = false;
                    break;
                }
            }

            if (estQuinte && estCouleur)
            {
                if (valeurs.Contains(10) && valeurs.Contains(13))
                {
                    return TypeMain.QuinteFlushRoyale;
                }
                return TypeMain.QuinteFlush;
            }

            if (nbCartesIdentiques == 4)
            {
                return TypeMain.Carre;
            }
            if (nbCartesIdentiques == 3)
            {
                if (groupesTri.Skip(1).First().Count() == 2)
                {
                    return TypeMain.Full;
                }
                return TypeMain.Brelan;
            }
            if (estCouleur)
            {
                return TypeMain.Couleur;
            }
            if (estQuinte)
            {
                return TypeMain.Quinte;
            }
            if (nbCartesIdentiques == 2)
            {
                if (groupesTri.Skip(1).First().Count() == 2)
                {
                    return TypeMain.DeuxPaires;
                }
                return TypeMain.Paire;
            }

            return TypeMain.Rien;
        }


        public List<string> ObtenirImagesMain()
        {
            List<string> images = new List<string>();

            foreach (Carte carte in MainCourante)
            {
                string image = ObtenirImageCarte(carte);
                images.Add(image);
            }

            return images;
        }

        private string ObtenirImageCarte(Carte carte)
        {
            string imageName = $"{carte.Valeur}_{carte.Couleur}.gif";
            string imagePath = @"C:\Users\alexi\source\repos\TP2P24\POKERTP2ALEXIS\images\" + imageName;
            return imagePath;
        }

        public void ChangerCartes(List<int> indexesCartesAChanger, Paquet paquet)
        {
            foreach (int index in indexesCartesAChanger)
            {
                _cartesRetirees.Add(MainCourante[index]); 
                MainCourante[index] = paquet.Cartes[0]; 
                paquet.Cartes.RemoveAt(0); 
            }

        }

    }
}
