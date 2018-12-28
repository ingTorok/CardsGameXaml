using CardsGame.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace CardsGame.WPF
{
    /// <summary>
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        /// <summary>
        /// local Variable we will store GameCounters from GameSpace (MainWindow)
        /// </summary>
        GameCounters GameCounters;

        /// <summary>
        /// local Variable to get and store Score from GameCounters
        /// </summary>
        Score Score = new Score();

        /// <summary>
        /// in this List we read from file and store the HighScores
        /// </summary>
        List<Score> HighScoreList;

        /// <summary>
        /// The name of the txt. File where the HighScore are stored
        /// </summary>
        string FileName = "HighScore.txt";

        /// <summary>
        /// The constructer for HighScoreWindow
        /// </summary>
        /// <param name="gameCounters">In this variable we will get GameCounters from GameSpace (MainWindow)</param>
        public HighScoreWindow(GameCounters gameCounters)
        {
            HighScoreList = new List<Score>();
            InitializeComponent();
            GameCounters = gameCounters;
            LoadGameScore();
            LoadScore();
            WriteGameScore();

            ButtonAddScore.IsEnabled = true;
            ButtonAddScore.Visibility = Visibility.Visible;
            ButtonExit.IsEnabled = false;
            ButtonExit.Visibility = Visibility.Hidden;

            Show();

        }

        /// <summary>
        /// Here we will get the Scoredetails from GameCounters to hold in Score
        /// </summary>
        private void LoadGameScore()
        {
            Score.TotalPoints = GameCounters.TotalPoints;
            Score.BestStreak = GameCounters.BestStreak.ToString();
            Score.GameDate = DateTime.Now.ToString("dd/MM/yyyy");
            Score.BestReaction = "100";
        }

        /// <summary>
        /// This function will write the new GameScore on HighScoreWindow
        /// </summary>
        private void WriteGameScore()
        {
            TextBlockScore.Text = Score.TotalPoints.ToString();
            TextBlockBestStreak.Text = Score.BestStreak;
            TextBlockDate.Text = Score.GameDate;
            TextBlockBestReaction.Text = Score.BestReaction;
        }
        /// <summary>
        /// This function will open the file to load existing/saved score
        /// </summary>
        private void LoadScore()
        {
            //The HighScore List will be reacreated
            HighScoreList = new List<Score>();
        
            if (File.Exists(FileName))
            {//we will chcek if the filename exists (firstplay?)
                List<string> stringList;
                var fs = new FileStream(FileName, FileMode.Open);
                var xmlSerializer = new XmlSerializer(typeof(List<string>));
                stringList = (List<string>)xmlSerializer.Deserialize(fs);

                foreach (string item in stringList)
                {//in every Line of the txt. File are saved the details for the Score, separated with ";"
                 //we will split the strings
                 //the spilted Strings are stored in oldScore
                 
                    //in this variable we will hold temporary the Scores readed from the txt. File
                    Score oldScore = new Score();

                    //todo: handel parse!
                    string[] stringScore;
                    stringScore = item.Split(';');
                    oldScore.Name = stringScore[0];
                    oldScore.TotalPoints = long.Parse(stringScore[1]);
                    oldScore.BestStreak = stringScore[2];
                    oldScore.BestReaction = stringScore[3];
                    oldScore.GameDate = stringScore[4];

                    //finally we will the readed Scores to the HighScoreList
                    HighScoreList.Add(oldScore);
                }

                fs.Close();

                ShowScore();
            }

            
        }

        /// <summary>
        /// This function will actualise the Listboxes in HighScoreWindow
        /// </summary>
        private void ShowScore()
        {
            if (ListBoxScore.Items.Count <1 //if the HighScoreWindow ist first Opened 
                && HighScoreList.Count > 0) // and the HigSchoreList contains items
            {                               // then we will add to the corresponding Listbox the details of the Score readed from the txt. File
                WriteScoresToListboxes();
            }
            else
            {//othwerwise we will remove all files from the Listboxes and then we fill fill up again with the new Score
                ListBoxName.Items.Clear();
                ListBoxScore.Items.Clear();
                ListBoxBestSreak.Items.Clear();
                ListBoxBestReaction.Items.Clear();
                ListBoxDate.Items.Clear();

                WriteScoresToListboxes();
            }
            
        }

        /// <summary>
        /// This function will add the Scoredetails to the corresponding Listbox on HighScoreWindow
        /// </summary>
        private void WriteScoresToListboxes()
        {
            foreach (Score item in HighScoreList)
            {
                ListBoxName.Items.Add(item.Name);
                ListBoxScore.Items.Add(item.TotalPoints);
                ListBoxBestSreak.Items.Add(item.BestStreak);
                ListBoxBestReaction.Items.Add(item.BestReaction);
                ListBoxDate.Items.Add(item.GameDate);
            }
        }

        /// <summary>
        /// In this function we add the new score and write to file
        /// </summary>
        private void AddNewScore()
        {
            //todo: irja ki hogy nem lehet a nevben ";"

            if (TextBoxName.Text.Contains(";"))
            {//the textbox can't contain ";", because the string splitcharachter is ";"
                return;
            }
            else
            {
                Score.Name = TextBoxName.Text;
            }

            //the New Score is added
            HighScoreList.Add(Score);

            //the new ScoreList is sorted
            HighScoreList = HighScoreList.OrderByDescending(x => x.TotalPoints).ToList();

            //if the List contains more then 15 elements we will delete (we store only the top15)
            while (HighScoreList.Count > 15)
            {
                HighScoreList.RemoveAt(HighScoreList.Count - 1);
            }
 
            //the Scoredetails in the neHighScoreList we will convert to strings, separated with ";" and added to List stringList
            List<string> stringList = new List<string>();

            foreach (Score item in HighScoreList)
            {
                stringList.Add(item.Name + ";" + item.TotalPoints.ToString() + ";" + item.BestStreak + ";"+ item.BestReaction + ";"+ item.GameDate);
            }

            //here we write to file the newScores
            var fs = new FileStream(FileName, FileMode.Create);
            var xmlSerializer = new XmlSerializer(typeof(List<string>));
            xmlSerializer.Serialize(fs, stringList);
            //Debug.WriteLine(HighScoreList.Count);
            fs.Close();

            //we will block and hide the "Add" Button
            ButtonAddScore.IsEnabled = false;
            ButtonAddScore.Visibility = Visibility.Hidden;

            //we will show button "Exit"
            ButtonExit.IsEnabled = true;
            ButtonExit.Visibility = Visibility.Visible;

            ShowScore();

        }

        /// <summary>
        /// This function will be executed at clicking on "Add" Button on HighScoreWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddScore_Click(object sender, RoutedEventArgs e)
        {
            AddNewScore();
            ShowScore();
        }

        /// <summary>
        /// close HighScoreWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// cathing the "Up" key to add new score/exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (ButtonAddScore.IsEnabled && e.Key==Key.Up)
            {
                AddNewScore();
            }
            else if (!ButtonAddScore.IsEnabled && e.Key == Key.Up)
            {
                Close();
            }
        }
    }
}
