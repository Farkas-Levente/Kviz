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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public Dictionary<string, Tantargy> tantargyNyilvantarto = new Dictionary<string, Tantargy>();
        public List<Tantargy> targyak = new List<Tantargy>();
        Tantargy selectedTargy = null;
        Tema selectedTema = null;
        public MainWindow()
        {

            InitializeComponent();

            

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
            List<Kerdes> kerdesek = new List<Kerdes>();
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

            public Kerdes(string sor)
            {
                string[] splitek = sor.Split(';');
                kerdes = splitek[2];
                helyesValasz = splitek[3];
                valasz2 = splitek[4];
                valasz3 = splitek[5];
                //valasz4 = splitek[6];   
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
            testLabel.Content = selectedTargyString;
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
            
            testlabel2.Content = selectedTemaString;
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
            testLabel3.Content = selectedTargy.nev;
            testlabel4.Content = selectedTema.nev;
        }
    }
}