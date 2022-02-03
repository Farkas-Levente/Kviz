using System;
using System.Collections.Generic;
using System.IO;
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

namespace Tanulo_Kviz
{

    public partial class MainWindow : Window
    {
        List<RadioButton> buttons = new List<RadioButton>();
        public Dictionary<string, Tantargy> tantargyNyilvantarto = new Dictionary<string, Tantargy>();
        public List<Tantargy> targyak = new List<Tantargy>();
        Tantargy selectedTargy = null;
        Tema selectedTema = null;
        public List<Kerdes> betoltottKerdesek = new List<Kerdes>();
        int oldalIndex = 0;
        Kerdes currentKerdes = null;

        List<Button> oldalValtoGombok = new List<Button>();
        bool kiertekelt = false;
        public MainWindow()
        {

            InitializeComponent();

            oldalValtoGombok.Add(oldal_1);
            oldalValtoGombok.Add(oldal_2);
            oldalValtoGombok.Add(oldal_3);
            oldalValtoGombok.Add(oldal_4);
            oldalValtoGombok.Add(oldal_5);
            oldalValtoGombok.Add(oldal_6);
            oldalValtoGombok.Add(oldal_7);
            oldalValtoGombok.Add(oldal_8);
            oldalValtoGombok.Add(oldal_9);
            oldalValtoGombok.Add(oldal_10);

            buttons.Add(valasz1);
            buttons.Add(valasz2);
            buttons.Add(valasz3);
            buttons.Add(valasz4);

            temakorBox.IsEnabled = false;

            
            Tantargy fizika = new Tantargy("fizika.txt");
            tantargyNyilvantarto.Add(fizika.nev, fizika);
            targyBox.Items.Add(fizika.nev);

            Tantargy magyar = new Tantargy("magyar.txt");
            tantargyNyilvantarto.Add(magyar.nev, magyar);
            targyBox.Items.Add(magyar.nev);

            Tantargy informatika = new Tantargy("informatika.txt");
            tantargyNyilvantarto.Add(informatika.nev, informatika);
            targyBox.Items.Add(informatika.nev);

            Tantargy matematika = new Tantargy("matematika.txt");
            tantargyNyilvantarto.Add(matematika.nev, matematika);
            targyBox.Items.Add(matematika.nev);

            Tantargy physics = new Tantargy("physics.txt");
            tantargyNyilvantarto.Add(physics.nev, physics);
            targyBox.Items.Add(physics.nev);

            Tantargy tori = new Tantargy("torike.txt");
            tantargyNyilvantarto.Add(tori.nev, tori);
            targyBox.Items.Add(tori.nev);

        }

        public class Tantargy
        {
            public Dictionary<string, Tema> temaNyilvantarto = new Dictionary<string, Tema>();
            public List<Tema> temak = new List<Tema>();
            public string nev;

            public Tantargy(string eleresiUt)
            {
                string[] allomany = File.ReadAllLines(eleresiUt);

                
                nev = allomany[0].Split(';')[0];


                List<string> temaFajtak = new List<string>();
                foreach (string sor in allomany)
                {
                    string temaNev = sor.Split(';')[1];
                    if (!temaFajtak.Contains(temaNev))
                    {
                        temaFajtak.Add(temaNev);
                        Tema ujTema = new Tema(temaNev,allomany);
                        temaNyilvantarto.Add(ujTema.nev, ujTema);
                        temak.Add(ujTema);
                    }
                }
            }
        }

        public class Tema
        {
            public List<Kerdes> kerdesek = new List<Kerdes>();
            public string nev;
            public Tema(string nev,string[] allomany)
            {
                this.nev = nev;
                foreach(string sor in allomany)
                {
                    if(sor.Split(';')[1] == nev)
                    {
                        Kerdes kerdes = new Kerdes(sor);
                        kerdesek.Add(kerdes);
                    }
                     
                }
            }
        }

        public class Kerdes
        {
            public string kerdes;
            public string helyesValasz;
            public string valasz2;
            public string valasz3;
            public string valasz4;

            public string valasztott = null;
            public bool isRandomized = false;

            public List<string> sorrend = new List<string>();

            public List<string> valaszok = new List<string>();
            public Kerdes(string sor)
            {
                string[] splitek = sor.Split(';');
                kerdes = splitek[2];
                helyesValasz = splitek[3];
                valasz2 = splitek[4];
                valasz3 = splitek[5];

                valasz4 = splitek[6];
                FeltöltValaszok();

            }

            public void FeltöltValaszok()
            {
                valaszok.Add(helyesValasz);
                valaszok.Add(valasz2);
                valaszok.Add(valasz3);
                valaszok.Add(valasz4);
            }
        }

        private void TargyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temakorBox.Items.Clear();
            temakorBox.Items.Clear();
            selectedTargy = null;
            selectedTema = null;

            Tantargy targy = null;
            string selectedTargyString = targyBox.SelectedItem.ToString();
            tantargyNyilvantarto.TryGetValue(selectedTargyString, out targy);

            temakorBox.IsEnabled = false;

            if (targy == null) return;

            temakorBox.IsEnabled = true;

            selectedTargy = targy;
            foreach(Tema tema in targy.temak)
            {
                temakorBox.Items.Add(tema.nev);
            }
        }

        private void TemaBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            if(temakorBox.SelectedItem == null || temakorBox.Items.Count <= 0) { return; }
            Tema tema = null;
            string selectedTemaString = temakorBox.SelectedItem.ToString();
            
            selectedTargy.temaNyilvantarto.TryGetValue(selectedTemaString, out tema);

            if (tema == null) return;

            selectedTema = tema;
        }

        private void General_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTema == null && selectedTargy == null)
            {
                MessageBox.Show("Kérem válasszon tantárgyat és témakört!");
                return;
            }
            else if(selectedTema == null)
            {
                MessageBox.Show("Kérem válasszon témakört!");
                return;
            }
            else if (selectedTargy == null)
            {
                MessageBox.Show("Kérem válasszon tantárgyat!");
                return;
            }

            kvizLap.Visibility = Visibility.Visible;
            foLap.Visibility = Visibility.Hidden;

            tantargy.Content = selectedTargy.nev;
            temakor.Content = selectedTema.nev;
            GeneralKerdesSor();

            foreach (RadioButton button in buttons)
            {
                if (button.Content.ToString() == currentKerdes.helyesValasz)
                {
                    button.Foreground = Brushes.Black;
                }
                else
                {
                    button.Foreground = Brushes.Black;
                }
                button.IsEnabled = true;
            }
            kiertekelt = false;
        }


        private void GeneralKerdesSor()
        {
            Random random = new Random();
            List<Kerdes> osszesKerdes =  selectedTema.kerdesek;
            List<Kerdes> tempKerdesek =  selectedTema.kerdesek;
            for (int i = 0; i < 10; i++)
            {
                int randomIndex = random.Next(0, tempKerdesek.Count);
                betoltottKerdesek.Add(tempKerdesek[randomIndex]);
                tempKerdesek.RemoveAt(randomIndex);
            }
            BetoltKerdes(betoltottKerdesek[0]);
            oldalIndex = 0;
        }

        private void BetoltKerdes(Kerdes ujKerdes)
        {
            Random random = new Random();
            List<string> tempValasz = new List<string>();
            currentKerdes = ujKerdes;
            foreach (var item in ujKerdes.valaszok)
            {
                tempValasz.Add(item);
            }
            List<RadioButton> tempButton = new List<RadioButton>();
            tempButton = buttons;

            if(ujKerdes.isRandomized == false)
            {
                for (int i = 0; i < 4; i++)
                {

                    int randomIndex = random.Next(0, tempValasz.Count);

                    buttons[i].Content = tempValasz[randomIndex];
                    ujKerdes.sorrend.Add(tempValasz[randomIndex]);
                    tempValasz.RemoveAt(randomIndex);
                    


                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    buttons[i].Content = ujKerdes.sorrend[i];
                }
            }
            foreach (RadioButton but in buttons)
            {
                if(but.Content.ToString() == ujKerdes.valasztott && ujKerdes.valasztott != null)
                {
                    but.IsChecked = true;
                }
            }

            ujKerdes.isRandomized = true;
            kerdes.Content = ujKerdes.kerdes;
            oldalIndex = betoltottKerdesek.IndexOf(ujKerdes);
            oladalJelzo.Content = $"{oldalIndex+1}/10";
            foreach(Button button in oldalValtoGombok)
            {
                button.Background = Brushes.White;
            }
            oldalValtoGombok[oldalIndex].Background = Brushes.Aqua;

            if(kiertekelt)
            {
                foreach (RadioButton button in buttons)
                {
                    if (button.Content.ToString() == ujKerdes.helyesValasz)
                    {
                        button.Foreground = Brushes.Green;
                    }
                    else
                    {
                        button.Foreground = Brushes.Red;
                    }
                    button.IsEnabled = false;


                }
            }
           
        }

        private void Kovetkezo_Oldal(object sender, RoutedEventArgs e)
        {
            if (oldalIndex + 1 >= betoltottKerdesek.Count) return;
            Nullazas();
            BetoltKerdes(betoltottKerdesek[oldalIndex +1]);
          
        }

        private void elozoLap_Betolt(object sender, RoutedEventArgs e)
        {
            if (oldalIndex <= 0) return;
            BetoltKerdes(betoltottKerdesek[oldalIndex - 1]);
        }

       

        private void Valasz1_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.valasztott = valasz1.Content.ToString();
        }

        private void Valasz2_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.valasztott = valasz2.Content.ToString();
        }

        private void Valasz3_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.valasztott = valasz3.Content.ToString();
        }

        private void Valasz4_Checked(object sender, RoutedEventArgs e)
        {
            currentKerdes.valasztott = valasz4.Content.ToString();
        }

        private void oldal_1_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[0]);
        }

        private void Nullazas()
        {
            valasz1.IsChecked = false;
            valasz2.IsChecked = false;
            valasz3.IsChecked = false;
            valasz4.IsChecked = false;
        }

        private void oldal_2_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[1]);
        }

        private void oldal_10_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[9]);
        }

        private void oldal_9_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[8]);
        }

        private void oldal_8_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[7]);
        }

        private void oldal_7_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[6]);
        }

        private void oldal_6_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[5]);
        }

        private void oldal_5_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[4]);
        }

        private void oldal_4_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[3]);
        }

        private void oldal_3_Click(object sender, RoutedEventArgs e)
        {
            Nullazas();
            BetoltKerdes(betoltottKerdesek[2]);
        }

        private void kiertekeles(object sender, RoutedEventArgs e)
        {
            kiertekelt = true;

            foreach(RadioButton button in buttons)
            {
                if(button.Content.ToString() == currentKerdes.helyesValasz)
                {
                    button.Foreground = Brushes.Green;
                }
                else
                {
                    button.Foreground = Brushes.Red;
                }
                button.IsEnabled = false;
                
            }
            
            int helyesValaszok = 0;
            foreach (Kerdes kerdes in betoltottKerdesek)
            {
                if(kerdes.valasztott == kerdes.helyesValasz)
                {
                    helyesValaszok++;
                }
            }

            MessageBox.Show($"{helyesValaszok}/10 pontod lett.");
            
            kilepes.Visibility = Visibility.Visible;
        }

        private void kilep(object sender, RoutedEventArgs e)
        {
            
            Nullazas();
            betoltottKerdesek.Clear();
            oldalIndex = 0;

            kvizLap.Visibility = Visibility.Hidden;
            foLap.Visibility = Visibility.Visible;

            kilepes.Visibility = Visibility.Hidden;
            selectedTema = null;
            selectedTargy = null;
           

        }
    }
}