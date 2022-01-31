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
        public MainWindow()
        {

            InitializeComponent();

            Tantargy fizika = new Tantargy("fizika.txt");
            tantargyNyilvantarto.Add(fizika.nev, fizika);
            targyBox.Items.Add(fizika.nev.ToUpper());

            Tantargy matek = new Tantargy("matek.txt");
            tantargyNyilvantarto.Add(matek.nev, matek);
            targyBox.Items.Add(matek.nev.ToUpper());



        }








        public class Tantargy
        {
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
                        Kerdes kerdes = new Kerdes();
                        kerdesek.Add(kerdes);
                    }
                     
                }
            }
        }

        public class Kerdes
        {
            
        }

        private void TargyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temakorBox.Items.Clear();
            Tantargy targy = null;
            string selectedTargyString = targyBox.SelectedItem.ToString();
            string kisbetus =  selectedTargyString.ToLower();
            testLabel.Content = kisbetus;
            tantargyNyilvantarto.TryGetValue(kisbetus, out targy);
            if (targy == null) return;
            foreach(Tema tema in targy.temak)
            {
                temakorBox.Items.Add(tema.nev);
            }
        }
    }
}