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
using TournamentManager.TMatch;
using TournamentManager.TRound;
using System.Text.RegularExpressions;
using TournamentManager.TException;
using System.Reflection.PortableExecutable;

namespace SportCupManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TournamentTemporary> lists = new List<TournamentTemporary>();
        ITournament CurrentTournament;
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
            SetNotification("");
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
            }

            if (lists.Count <= 0)
                SetNotification("Brak Turniejów!");

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

        private void MenuMatch_Create_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            CollapseAllGrids();
            MatchCreateGrid.Visibility = Visibility.Visible;
        }

        private void MenuMatch_List_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            CollapseAllGrids();
            RoundListGrid.Visibility = Visibility.Visible;
            if (CurrentTournament.League != null && CurrentTournament.League.Rounds.Count > 1)
                RoundList.ItemsSource = CurrentTournament.League.Rounds;
            else
                SetNotification("Brak rund!");
            RoundList.Items.Refresh();
            CurrentMode.Content = "match";
        }

        private void MenuPlayoff_Create_Click(object sender, RoutedEventArgs e)
        {
            
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            if (!(CurrentTournament.League.IsFinished()))
            {
                SetNotification("League nie została dokończona");
                return;
            }
            CollapseAllGrids();
            PlayoffCreateGrid.Visibility = Visibility.Visible;
            CurrentMode.Content = "playoff";
        }

        private void MenuPlayoff_List_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentTournament == null)
            {
                SetNotification("Nie wybrano turnieju!");
                return;
            }
            CollapseAllGrids();
            PlayoffListGrid.Visibility = Visibility.Visible;
            if (CurrentTournament.PlayOff != null && CurrentTournament.PlayOff.Rounds.Count > 1)
                PlayoffList.ItemsSource = CurrentTournament.PlayOff.Rounds;
            else
                SetNotification("Brak rund!");
            PlayoffList.Items.Refresh();
        }

        /* SUBMENU */

        private void TeamEdit_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TeamEditGrid.Visibility = Visibility.Visible;
            Team team = CurrentTournament.FindTeam((string)((Button)sender).Tag);

            Edit_TeamName.Text = team.Name;
            PlayersListView.ItemsSource = team.listPlayers;

            PlayerCreateButton.Tag = team.Name;
            TeamEditButton.Tag = team.Name;
        }

        private void TournamentEdit_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            TournamentEditGrid.Visibility = Visibility.Visible;
            string name = (string)((Button)sender).Tag;

            Edit_TournamentName.Text = name;
            TournamentEditButton.Tag = name;
            RefereeCreateButton.Tag = name;

            CurrentTournament = Read.Tournament(name);
            RefereesListView.ItemsSource = CurrentTournament.Referees;
        }

        private void MatchPreview_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            MatchPreviewGrid.Visibility = Visibility.Visible;
            string name = (string)((Button)sender).Tag;

            MatchList.ItemsSource = CurrentTournament.League.FindRound(name).ListMatches;
            RoundNameHidden.Tag = name;
            CurrentMode.Content = "match";
        }

        private void MatchDetailsPreview_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            MatchDetailsGrid.Visibility = Visibility.Visible;
            int index = (int)((Button)sender).Tag;
            Round currentRound = null;
            if (CurrentMode.Content == "playoff")
                currentRound = CurrentTournament.PlayOff.FindRound((string)PlayoffRoundNameHidden.Tag);
            else if(CurrentMode.Content == "match")
                currentRound = CurrentTournament.League.FindRound((string)RoundNameHidden.Tag);

            TournamentManager.TMatch.Match match = currentRound.ListMatches[index];

            DetailTeamA.Content = match.TeamA.Name;
            DetailTeamB.Content = match.TeamB.Name;
            List<Referee> referees = match.GetReferees();
            DetailRefereeA.Content = referees[0].Fullname + ", " + referees[0].Age + " lat";
            try
            {
                DetailRefereeB.Content = referees[1].Fullname + ", " + referees[1].Age + " lat";
                DetailRefereeC.Content = referees[2].Fullname + ", " + referees[2].Age + " lat";
            }
            catch(ArgumentOutOfRangeException) { }

            if(match.Winner != null)
            { 
                if (match.Winner.Name == match.TeamA.Name)
                {
                    DetailResultA.Content = "ZWYCIĘZCA";
                    DetailResultA.Foreground = Brushes.Green;
                    DetailResultB.Content = "PRZEGRANY";
                    DetailResultB.Foreground = Brushes.Red;
                }
                else if (match.Winner.Name == match.TeamB.Name)
                {
                    DetailResultB.Content = "ZWYCIĘZCA";
                    DetailResultB.Foreground = Brushes.Green;
                    DetailResultA.Content = "PRZEGRANY";
                    DetailResultA.Foreground = Brushes.Red;
                }
                else
                {
                    DetailResultA.Content = "";
                    DetailResultB.Content = "";
                }

                if (match.IsWalkover && match.Winner.Name != match.TeamA.Name)
                {
                    DetailResultA.Content = "PODDAŁ SIĘ";
                    DetailResultA.Foreground = Brushes.Red;
                }
                else if (match.IsWalkover && match.Winner.Name != match.TeamB.Name)
                {
                    DetailResultB.Content = "PODDAŁ SIĘ";
                    DetailResultB.Foreground = Brushes.Red;
                }
            }
            else
            {
                DetailResultA.Content = "";
                DetailResultB.Content = "";
            }

            string[] statsA = match.TeamA.GetStats().Split(", ");
            string[] statsB = match.TeamB.GetStats().Split(", ");

            DetailTeamAStatA.Content = statsA[0];
            DetailTeamAStatB.Content = statsA[1];
            DetailTeamAStatC.Content = statsA[2];

            DetailTeamBStatA.Content = statsB[0];
            DetailTeamBStatB.Content = statsB[1];
            DetailTeamBStatC.Content = statsB[2];

            if (match is VolleyballMatch)
            {
                DetailStatA.Content = "Ilość Punktów";
                DetailStatB.Content = "Wygrane Mecze";
                DetailStatC.Content = "Różnica Małych Punktów";
            }
            else if(match is TugOfWarMatch)
            {
                DetailStatA.Content = "Wygrane Mecze";
                DetailStatB.Content = "Średni Czas Wygrywania";
                DetailStatC.Content = "Średni Czas Przegrywania";
            }
            else if(match is DodgeballMatch)
            {
                DetailStatA.Content = "Wygrane Mecze";
                DetailStatB.Content = "Ilość Wyeliminowanych Graczy";
                DetailStatC.Content = "Ilość Pozostałych Graczy";
            }
        }

        private void MatchEditData_Click(object sender, RoutedEventArgs e)
        {
            int index = (int)((Button)sender).Tag;
            Round currentRound = null;
            if (CurrentMode.Content == "playoff")
                currentRound = CurrentTournament.PlayOff.FindRound((string)PlayoffRoundNameHidden.Tag);
            else if(CurrentMode.Content == "match")
                currentRound = CurrentTournament.League.FindRound((string)RoundNameHidden.Tag);
            TournamentManager.TMatch.Match match = currentRound.ListMatches[index];

            CollapseAllGrids();
            MatchEditDataGrid.Visibility = Visibility.Visible;

            MatchIndexHidden.Content = index;

            ChooseTeamA.Content = match.TeamA.Name;
            ChooseTeamB.Content = match.TeamB.Name;

            Stat2.Visibility = Visibility.Collapsed;
            Stat3.Visibility = Visibility.Collapsed;
            Stat4.Visibility = Visibility.Collapsed;
            Stat5.Visibility = Visibility.Collapsed;
            Stat6.Visibility = Visibility.Collapsed;

            if (match is VolleyballMatch)
            {
                Stat2.Visibility = Visibility.Visible;
                Stat3.Visibility = Visibility.Visible;
                Stat4.Visibility = Visibility.Visible;
                Stat5.Visibility = Visibility.Visible;
                Stat6.Visibility = Visibility.Visible;

                Stat1.Text = match.TeamA.Name+" - Set 1";
                Stat2.Text = match.TeamA.Name+" - Set 2";
                Stat3.Text = match.TeamA.Name+"- Set 3";
                Stat4.Text = match.TeamB.Name+" - Set 1";
                Stat5.Text = match.TeamB.Name + "- Set 2";
                Stat6.Text = match.TeamB.Name + " - Set 3";
            }
            else if (match is TugOfWarMatch)
            {
                Stat1.Text = "Czas Meczu";
            }
            else if (match is DodgeballMatch)
            {
                Stat1.Text = "Ilość pozostałych graczy zwycięskiej drużyny";
            }
        }

        private void PlayoffPreview_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllGrids();
            PlayoffPreviewGrid.Visibility = Visibility.Visible;
            string name = (string)((Button)sender).Tag;
            List <TournamentManager.TMatch.Match> listMatches = CurrentTournament.PlayOff.FindRound(name).ListMatches;
            if(name =="final")
                if (!(CurrentTournament.PlayOff.FindRound("semi-finals").IsFinished()))
                {
                    SetNotification("Semif-finals nie są dokończone");
                    return;
                }    
            if(name == "final" && listMatches.Count == 0)
            {
                try
                {
                    CurrentTournament.PlayOff.GenerateFinals();
                    listMatches = CurrentTournament.PlayOff.FindRound(name).ListMatches;
                }
                catch (RoundNotFinieshedException ev)
                {
                    SetNotification(ev.Message);
                }
            }
            PlayoffMatchList.ItemsSource = listMatches;
            PlayoffRoundNameHidden.Tag = name;
            CurrentMode.Content = "playoff";
        }

        /* SUBMIT BUTTONS */

        private void TournamentCreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Create_TournamentName.Text;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\" + name.Replace(" ", "");
            if (Directory.Exists(path))
            {
                SetNotification("Ta nazwa turnieju jest już zajęta!");
                return;
            }
            TournamentDyscypline dyscypline;
            switch(Create_DyscyplineComboBox.Text)
            {
                case "Volleyball": dyscypline = TournamentDyscypline.volleyball; break;
                case "Tug Of War": dyscypline = TournamentDyscypline.tugofwar; break;
                case "Dodgeball": dyscypline = TournamentDyscypline.dodgeball; break;
                default: dyscypline = TournamentDyscypline.volleyball; break;
            }

            ITournament t = new Tournament(name, dyscypline);
            Save.Tournament(t);
            MenuTournament_Load_Click(sender, e);
        }

        private void TournamentDelete_Click(object sender, RoutedEventArgs e)
        {
            string path = (string)((Button)sender).Tag;
            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException ev)
            {
                SetNotification(ev.Message);
            }
            catch(Exception ev)
            {
                SetNotification(ev.Message);
            }
            TournamentTemporary tour = new TournamentTemporary(path);
            if (CurrentTournament != null && tour.getNameFromPath() == CurrentTournament.Name)
            { 
                CurrentTournament = null;
                TournamentLoad_Click(null, e);
            }
            MenuTournament_Load_Click(sender, e);
        }

        private void TournamentLoad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                CurrentTournament = Read.Tournament((string)button.Tag);
                CurrentlyLoaded.Content = "Wczytany turniej: " + CurrentTournament.Name + "(" + CurrentTournament.Dyscypline + ")";
            }
            else
                CurrentlyLoaded.Content = "";
        }

        private void PlayerCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player player = new Player(PlayerFirstName.Text, PlayerSurName.Text, Convert.ToByte(PlayerAge.Text), Convert.ToByte(PlayerNumber.Text));
                Team team = CurrentTournament.FindTeam((string)((Button)sender).Tag);
                team.AddPlayer(player);
                PlayersListView.Items.Refresh();
                Save.Teams(CurrentTournament.Teams, CurrentTournament.Name);
            }
            catch (FormatException)
            {
                SetNotification("Wiek i numer muszą być liczbą!");
            }
            catch (OverflowException)
            {
                SetNotification("Wiek i numer nie mogą być tak duże!");
            }
        }

        private void TournamentEditButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ((string)((Button)sender).Tag).Replace(" ", "");
            string changedName = Edit_TournamentName.Text.Replace(" ", "");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TournamentManager\\data\\";
            if (path + name != path + changedName)
                try
                {
                    Directory.Move(path + name.Replace(" ", ""), path + changedName.Replace(" ", ""));
                }catch(IOException ev)
                {
                    SetNotification("IOEXCEPTION "+ ev.Message);
                }
                catch(Exception ev)
                {
                    SetNotification(ev.Message);
                }
            if (CurrentTournament != null && CurrentTournament.Name != changedName)
            {
                CurrentTournament.Name = changedName;
                Save.TournamentObject(CurrentTournament, changedName);
            }
            CurrentTournament = null;
            MenuTournament_Load_Click(sender, e);
        }

        private void TeamCreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Create_TeamName.Text;
            int id = CurrentTournament.Teams.Count + 1;
            ITeam team;
            switch(CurrentTournament.Dyscypline)
            {
                case TournamentDyscypline.volleyball: team = new VolleyballTeam(name, id); break;
                case TournamentDyscypline.tugofwar: team = new TugOfWarTeam(name, id); break;
                case TournamentDyscypline.dodgeball: team = new DodgeballTeam(name, id); break;
                default: team = null; break;
            }

            CurrentTournament.AddTeam(team);
            SetNotification("Pomyślnie dodano drużynę");
            Save.Teams(CurrentTournament.Teams, CurrentTournament.Name);
            MenuTeam_Edit_Click(sender, e);
        }

        private void TeamDelete_Click(object sender, RoutedEventArgs e)
        {
            string name = (string)((Button)sender).Tag;
            Team team = CurrentTournament.FindTeam(name);
            CurrentTournament.RemoveTeam(team);
            Save.Teams(CurrentTournament.Teams, CurrentTournament.Name);
            MenuTeam_Edit_Click(sender, e);
        }

        private void TeamEditButton_Click(object sender, RoutedEventArgs e)
        {
            Team team = CurrentTournament.FindTeam((string)((Button)sender).Tag);
            team.Name = Edit_TeamName.Text;
            Save.Teams(CurrentTournament.Teams, CurrentTournament.Name);
            MenuTeam_Edit_Click(sender, e);
        }

        private void RefereeCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = (string)((Button)sender).Tag;
                ITournament tour = Read.Tournament(name);
                int id = tour.Referees.Count + 1;
                Referee referee = new Referee(RefereeFirstName.Text, RefereeSurName.Text, Convert.ToByte(RefereeAge.Text), id);
                tour.AddReferee(referee);
                Save.Referees(tour.Referees, name);
                if (CurrentTournament != null && name == CurrentTournament.Name)
                    CurrentTournament = Read.Tournament(name);
                RefereesListView.ItemsSource = tour.Referees;
                RefereesListView.Items.Refresh();
            }
            catch (FormatException)
            {
                SetNotification("Wiek musi być liczbą!");
            }
            catch (OverflowException)
            {
                SetNotification("Wiek nie może być tak duży!");
            }
        }

        private void MatchCreateButton_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentTournament.Teams.Count < 5)
            {
                SetNotification("Za mało drużyn w turnieju!");
                return;
            }

            try
            {
                DateTime date = Date.SelectedDate.Value;
                int[] formattedDate = { date.Day, date.Month, date.Year };
                int space;
                space = Int32.Parse(SpaceBetweenMatches.Text);

                CurrentTournament.SetAutoLeague(formattedDate, space);
                Save.League(CurrentTournament.League, CurrentTournament.Name);
                MenuMatch_List_Click(sender, e);
            }
            catch (FormatException)
            {
                SetNotification("Dni między meczami musi być liczbą!");
                return;
            }
            catch (NotEnoughRefereesException)
            {
                SetNotification("Za mało sędziów w turnieju!");
            }
        }

        private void MatchDataSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool flag = false;
            try
            {
                Round currentRound = null;
                if(CurrentMode.Content == "playoff")
                    currentRound = CurrentTournament.PlayOff.FindRound((string)PlayoffRoundNameHidden.Tag);
                else if(CurrentMode.Content == "match")
                    currentRound = CurrentTournament.League.FindRound((string)RoundNameHidden.Tag);
                TournamentManager.TMatch.Match match = currentRound.ListMatches[(int)MatchIndexHidden.Content];
                ITeam winner = (WinnerBox.Text == match.TeamA.Name) ? match.TeamA : match.TeamB;
                string stats;

                if (WalkoverCheckbox.IsChecked.Value && winner == match.TeamA)
                {
                    match.Walkover(match.TeamB);
                    flag = true;
                }
                else if (WalkoverCheckbox.IsChecked.Value && winner == match.TeamB)
                {
                    match.Walkover(match.TeamA);
                    flag = true;
                }
                else if (match is VolleyballMatch)
                {
                    stats = match.TeamA.Name + ": " + Stat1.Text + ", " + Stat2.Text + ", " + Stat3.Text + ". " + match.TeamB.Name + ": " + Stat4.Text + ", " + Stat5.Text + ", " + Stat6.Text;
                    match.SetResult(stats, winner);
                    flag = true;
                }
                else if (match is TugOfWarMatch)
                {
                    stats = Stat1.Text;
                    match.SetResult(stats, winner);
                    flag = true;
                }
                else if (match is DodgeballMatch)
                {
                    stats = Stat1.Text;
                    match.SetResult(stats, winner);
                    flag = true;
                }

                if (CurrentMode.Content == "playoff")
                    Save.PlayOff(CurrentTournament.PlayOff, CurrentTournament.Name);
                else if (CurrentMode.Content == "match")
                    Save.League(CurrentTournament.League, CurrentTournament.Name);
            }
            catch (NotNumberMatchLengthException)
            {
                SetNotification("Czas powinien być liczbą!");
            }
            catch (NegativeMatchLengthException)
            {
                SetNotification("Czas powinien być dodatni!");
            }
            catch(TooHighPlayersLeftException)
            {
                SetNotification("Na boisku nie może zostać więcej niż 6 graczy!");
            }
            catch (NegativePlayersNumberException)
            {
                SetNotification("Liczba graczy powinna być dodatnia!");
            }
            catch (NotIntPlayersException)
            {
                SetNotification("Musisz podać poprawną ilość graczy!");
            }
            catch (NonIntScoreException)
            {
                SetNotification("Musisz podać poprawny wynik!");
            }
            catch (TooHighScoreException)
            {
                SetNotification("Wynik seta jest zbyt duży!");
            }
            catch (ThirdSetException)
            {
                SetNotification("Niepoprawnie podane sety!");
            }
            catch (NoSetWinnerException)
            {
                SetNotification("Niepoprawnie podane sety!");
            }
            catch (WrongWinnerException)
            {
                SetNotification("Podany wynik nie zgadza się z podanym zwycięzcą");
            }

            if(flag)
            {
                if (CurrentMode.Content == "playoff")
                    MenuPlayoff_List_Click(sender, e);
                else if (CurrentMode.Content == "match")
                    MenuMatch_List_Click(sender, e);
            }
        }

        private void PlayoffCreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = DatePlayoff.SelectedDate.Value;
                int[] formattedDate = { date.Day, date.Month, date.Year };

                CurrentTournament.SetPlayOff(formattedDate);
                Save.PlayOff(CurrentTournament.PlayOff, CurrentTournament.Name);
                MenuPlayoff_List_Click(sender, e);
            }
            catch (NotEnoughRefereesException)
            {
                SetNotification("Za mało sędziów w turnieju!");
            }
            catch (NotEnoughtTeamsNumber)
            {
                SetNotification("Za mało drużyn w turnieju!");
            }
            catch (LeagueNotFinishedException)
            {
                SetNotification("Liga jeszcze się nie zakończyła!");
            }
            catch (LeagueHasNotBeenCreatedException)
            {
                SetNotification("Najpierw ustal mecze w lidze!");
            }
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
