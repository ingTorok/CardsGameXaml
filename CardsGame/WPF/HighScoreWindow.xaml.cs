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
        /// 
        /// </summary>
        GameCounters GameCounters;

        /// <summary>
        /// 
        /// </summary>
        Score Score = new Score();

        /// <summary>
        /// 
        /// </summary>
        List<Score> HighScoreList;

        /// <summary>
        /// 
        /// </summary>
        string FileName = "HighScore.txt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameCounters"></param>

        public HighScoreWindow(GameCounters gameCounters)
        {
            HighScoreList = new List<Score>();
            InitializeComponent();
            GameCounters = gameCounters;
            LoadGameScore();
            LoadScore();
            WriteGameScore();
            Show();

        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGameScore()
        {
            Score.TotalPoints = GameCounters.TotalPoints.ToString();
            Score.BestStreak = GameCounters.BestStreak.ToString();
            Score.GameDate = DateTime.Now.ToString("dd/mm/yyyy");
            Score.BestReaction = "100";
        }

        /// <summary>
        /// This function will write the new GameScore on HighScoreWindow
        /// </summary>
        private void WriteGameScore()
        {
            TextBlockScore.Text = Score.TotalPoints;
            TextBlockBestStreak.Text = Score.BestStreak;
            TextBlockDate.Text = Score.GameDate;
            TextBlockBestReaction.Text = Score.BestReaction;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadScore()
        {
            Score oldScore = new Score();
            HighScoreList = new List<Score>();
        
            if (File.Exists(FileName))
            {
                List<string> stringList;
                var fs = new FileStream(FileName, FileMode.Open);
                var xmlSerializer = new XmlSerializer(typeof(List<string>));
                stringList = (List<string>)xmlSerializer.Deserialize(fs);

                foreach (string item in stringList)
                {
                    string[] stringScore = item.Split(';');

                    oldScore.Name = stringScore[0];
                    oldScore.TotalPoints = stringScore[1];
                    oldScore.BestStreak = stringScore[2];
                    oldScore.BestReaction = stringScore[3];
                    oldScore.GameDate = stringScore[4];

                    HighScoreList.Add(oldScore);
                }

                fs.Close();

                ShowScore();
            }

            
        }

        private void ShowScore()
        {4

            foreach (Score item in HighScoreList)
            {
                ListBoxName.Items.Add(Score.Name);
                ListBoxScore.Items.Add(Score.TotalPoints);
                ListBoxBestSreak.Items.Add(Score.BestStreak);
                ListBoxBestReaction.Items.Add(Score.BestReaction);
                ListBoxDate.Items.Add(Score.GameDate);
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
         
            List<string> stringList = new List<string>();
            HighScoreList.Add(Score);

            foreach (Score item in HighScoreList)
            {
                stringList.Add(item.Name + ";" + item.TotalPoints+ ";" + item.BestStreak + ";"+ item.BestReaction + ";"+ item.GameDate);
            }
            var fs = new FileStream(FileName, FileMode.Create);
            var xmlSerializer = new XmlSerializer(typeof(List<string>));
            xmlSerializer.Serialize(fs, stringList);
            Debug.WriteLine(HighScoreList.Count);
            fs.Close();

        }

        private void ButtonAddScore_Click(object sender, RoutedEventArgs e)
        {
            AddNewScore();
            ShowScore();
        }
    }
}
