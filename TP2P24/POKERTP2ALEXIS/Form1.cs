using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static POKERTP2ALEXIS.Main;

namespace POKERTP2ALEXIS
{
    public partial class Form1 : Form
    {
        private Main maMain;
        private Paquet monPaquet;
        private bool cartesChangees = false;
        private decimal credit = 100.0M;
        private int mise = 1;

        public Form1()
        {
            InitializeComponent();

            monPaquet = new Paquet();

            maMain = new Main(monPaquet);

            AfficherImagesMain();

            numericCredit.Enabled = false;

            numericCredit.Value = (decimal)credit;

            numericMise.Maximum = 10;

            numericMise.Value = mise;
        }

        private void AfficherImagesMain()
        {
            List<string> images = maMain.ObtenirImagesMain();

            for (int i = 0; i < images.Count && i < 5; i++)
            {
                string imagePath = images[i];
                PictureBox pictureBox = Controls.Find($"pictureBox{i + 1}", true)[0] as PictureBox;
                pictureBox.ImageLocation = imagePath;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void Garder1_CheckedChanged(object sender, EventArgs e)
        {
            List<int> indexesCartesAChanger = new List<int>();

            maMain.ChangerCartes(indexesCartesAChanger, monPaquet);

            AfficherImagesMain();
        }

        private void numericCredit_ValueChanged(object sender, EventArgs e)
        {
            numericCredit.Value = (decimal)credit;
        }
        private void numericMise_ValueChanged(object sender, EventArgs e)
        {
            mise = (int)numericMise.Value;
        }

        private void btnContinuer_Click(object sender, EventArgs e)
        {
            List<int> indexesCartesAChanger = new List<int>();

            if (!Garder1.Checked)
            {
                indexesCartesAChanger.Add(0);
            }
            if (!Garder2.Checked)
            {
                indexesCartesAChanger.Add(1);
            }
            if (!Garder3.Checked)
            {
                indexesCartesAChanger.Add(2);
            }
            if (!Garder4.Checked)
            {
                indexesCartesAChanger.Add(3);
            }
            if (!Garder5.Checked)
            {
                indexesCartesAChanger.Add(4);
            }

            maMain.ChangerCartes(indexesCartesAChanger, monPaquet);
            AfficherImagesMain();

            Main.TypeMain typeMain = maMain.DeterminerTypeMain();
            decimal gain = 0;

            switch (typeMain)
            {
                case TypeMain.Rien:
                    gain = -mise * 1;
                    break;
                case TypeMain.Paire:
                    gain = mise * 1;
                    break;
                case TypeMain.DeuxPaires:
                    gain = mise * 3;
                    break;
                case TypeMain.Brelan:
                    gain = mise * 5;
                    break;
                case TypeMain.Quinte:
                    gain = mise * 10;
                    break;
                case TypeMain.Couleur:
                    gain = mise * 15;
                    break;
                case TypeMain.Full:
                    gain = mise * 20;
                    break;
                case TypeMain.Carre:
                    gain = mise * 25;
                    break;
                case TypeMain.QuinteFlush:
                    gain = mise * 50;
                    break;
                case TypeMain.QuinteFlushRoyale:
                    gain = mise * 100;
                    break;
            }

            credit += gain;
            NumericGain.Text = gain.ToString("N2");
            numericCredit.Text = credit.ToString("N2");

            string resultat = $"Type de main : {typeMain}\nGains : {gain}";
            MessageBox.Show(resultat);
        }

        private void btnJouer_Click(object sender, EventArgs e)
        {
            decimal miseValue = numericMise.Value;

            if (miseValue > 0 && credit >= miseValue)
            {
                if (credit == 0.0M)
                {
                    credit = 100.0M;
                    numericCredit.Text = credit.ToString("N2");
                }

                credit -= miseValue;
                numericCredit.Text = credit.ToString("N2");
                monPaquet = new Paquet();
                maMain = new Main(monPaquet);
                AfficherImagesMain();
                Garder1.Checked = false;
                Garder2.Checked = false;
                Garder3.Checked = false;
                Garder4.Checked = false;
                Garder5.Checked = false;
                btnContinuer.Enabled = true;
                NumericGain.Text = "0.00";
            }
            else
            {
                MessageBox.Show("Mise invalide ou crédit insuffisant.");
            }
        }



        private void btnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
