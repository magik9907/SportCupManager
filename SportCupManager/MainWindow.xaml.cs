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
using TournamentManager.TPerson;
using TournamentManager.TTeam;

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
            foreach (UIElement grid in MainGrid.Children)
            {
                grid.Visibility = Visibility.Collapsed;
            }
        }

        private void SetNotification(string message)
        {
            NotificationLabel.Content = message;
            NotificationLabel.Visibility = Visibility.Visible;
        }

        /* MENU BUTTONS */

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

        private void MenuTeam_Create_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            CollapseAllGrids();
            TeamCreateGrid.Visibility = Visibility.Visible;
        }

        private void MenuTeam_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            CollapseAllGrids();
            TeamListGrid.Visibility = Visibility.Visible;

            if (CurrentTournament.Teams.Count > 0)
                TeamsList.ItemsSource = CurrentTournament.Teams;
            else
                SetNotification("Brak drużyn!");
            TeamsList.Items.Refresh();

            //PlayersListView.ItemsSource = CurrentTournament.Teams;
        }

        /* SUBMIT BUTTONS */

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
            {
                CurrentlyLoaded.Content = "Wczytany turniej: " + (string)button.Tag;
                CurrentTournament = new Tournament((string)button.Tag, 1);
            }
            else
                CurrentlyLoaded.Content = "";
        }

        private void PlayerCreateButton_Click(object sender, RoutedEventArgs e)
        {
            Player player = new Player(PlayerFirstName.Text, PlayerSurName.Text, Convert.ToByte(PlayerAge.Text), Convert.ToByte(PlayerNumber.Text));
        }

        private void TournamentEditButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TeamCreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Create_TeamName.Text;
            ITeam team;
            switch(CurrentTournament.Dyscypline)
            {
                case "volleyball": team = new VolleyballTeam(name, 1); break;
                case "tugofwar": team = new TugOfWarTeam(name, 2); break;
                case "dodgeball": team = new DodgeballTeam(name, 3); break;
                default: team = null; break;
            }

            CurrentTournament.AddTeam(team);
            SetNotification("Pomyślnie dodano drużynę");
            Save.Tournament(CurrentTournament);
            MenuTeam_Edit_Click(sender, e);
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
