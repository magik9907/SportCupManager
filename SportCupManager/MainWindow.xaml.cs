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
        List<TournamentTemporary> lists = new List<TournamentTemporary>();
        Tournament CurrentTournament;
        public MainWindow()
        {
            InitializeComponent();
            MenuTournament_Load_Click(new object(), new RoutedEventArgs());
            TournamentLoad_Click(new object(), new RoutedEventArgs());
        }

        private void CollapseAllGrids()
        {
            foreach (Grid grid in MainGrid.Children)
            {
                grid.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuTournament_Create_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TournamentCreateGrid.Visibility = Visibility.Visible;
        }

        private void MenuTournament_Load_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            lists.Clear();
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data";
            String[] list = Directory.GetDirectories(path);

            for (int i = 0; i < list.Length; i++)
            {
                lists.Add(new TournamentTemporary(list[i]));
                Trace.Write("\n" + list[i]);
            }
            Trace.Write("\n\n");

            Resources.ItemsSource = lists;
            Resources.Items.Refresh();
            TournamentLoadGrid.Visibility = Visibility.Visible;
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

            ITournament t = new Tournament(name, (int)dyscypline);
            Save.Tournament(t);
            MenuTournament_Load_Click(sender, e);
        }

        private void TournamentDelete_Click(object sender, RoutedEventArgs e)
        {
            string path = (string)((Button)sender).Tag;
            Directory.Delete(path, true);
            MenuTournament_Load_Click(sender, e);
        }

        private void TournamentLoad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                CurrentlyLoaded.Content = "Wczytany turniej: " + (string)button.Tag;
            else
                CurrentlyLoaded.Content = "Nie wczytano turnieju!";
        }

        private void MenuTeam_Create_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TeamCreateGrid.Visibility = Visibility.Visible;
        }

        private void MenuTeam_Edit_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TeamCreateGrid.Visibility = Visibility.Visible;
        }
    }

    public class TournamentTemporary
    {
        public string Name { get; }
        public string Path { get; }

        public TournamentTemporary(string path)
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
