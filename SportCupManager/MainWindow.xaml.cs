using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using System.Diagnostics;
using TournamentManager;
using TournamentManager.TEnum;

namespace SportCupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         List<Tournament> lists = new List<Tournament>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CollapseAllGrids()
        {
            foreach (Grid grid in MainGrid.Children)
            {
                grid.Visibility = Visibility.Collapsed;
            }
        }

        private void Tournament_Create_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TournamentCreateGrid.Visibility = Visibility.Visible;
        }

        private void Tournament_Load_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TournamentLoadGrid.Visibility = Visibility.Visible;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data";
            String[] list = Directory.GetDirectories(path);

            for (int i = 0; i < list.Length; i++)
            {
                lists.Add(new Tournament(list[i]));
            }

            Trace.Write("\n\n" + list + "\n\n");

            Resources.ItemsSource = lists;
        }

        private void TournamentCreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Create_TournamentName.Text;
            TournamentDyscypline dyscypline;
            switch(Create_DyscyplineComboBox.Text)
            {
                case "Volleyball": dyscypline = TournamentDyscypline.volleyball; break;
                case "Tug Of War": dyscypline = TournamentDyscypline.tugofwar; break;
                case "Dodgeball": dyscypline = TournamentDyscypline.dodgeball; break;
                default: dyscypline = TournamentDyscypline.volleyball; break;
            }

            ITournament t = new TournamentManager.Tournament(name, (int)dyscypline);
            Save.Tournament(t);
            Tournament_Load_Click(sender, e);
        }
    }

    public class Tournament
    {
        public string Name { get; }
        public string Path { get; }

        public Tournament(string path)
        {
            Path = path;
            Name = getNameFromPath();
        }

        public string getNameFromPath()
        {
            int index = Path.LastIndexOf("\\");
            return Path.Substring(index+1);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
