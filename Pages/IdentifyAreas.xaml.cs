using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace DeweyDecimalTrainer.Pages
{
    /// <summary>
    /// Interaction logic for IdentifyAreas.xaml
    /// </summary>
    public partial class IdentifyAreas : Page
    {

        dictionaryClass dClass = new dictionaryClass();
        Dictionary<string, string> categories;
        Dictionary<string, string> subsetCat;
        Dictionary<string, string> remainingCat;

        List<string> modelAnswer;
        List<string> questionList = new List<string>();
        List<string> answerList = new List<string>();
        public ObservableCollection<string> DgQuestions { get; set; }
        public ObservableCollection<string> DgAnswers { get; set; }

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

        public string LeftColumn;
        public string RightColumn;
        public IdentifyAreas()
        {
            InitializeComponent();
        }

        public void RandomizeArea()
        {
            categories = new Dictionary<string, string>();
            subsetCat = new Dictionary<string, string>();
            remainingCat = new Dictionary<string, string>();

            Random rand = new Random();
            int totalCat = dClass.Categories.Count;
            modelAnswer = new List<string>();

            Dictionary<string, string> shuffle = dClass.Categories.OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value);

            for (int i = 0; i < shuffle.Count; i++) categories.Add(shuffle.ElementAt(i).Key, shuffle.ElementAt(i).Value);

            int randomNumber = rand.Next(6, 10);
            
            for (int o = 0; o < categories.Count - randomNumber; o++)
            {
                subsetCat.Add(categories.ElementAt(o).Key, categories.ElementAt(o).Value);
            }

            for (int q = categories.Count - randomNumber; q < categories.Count; q++)
            {
                remainingCat.Add(categories.ElementAt(q).Key, categories.ElementAt(q).Value);
            }

            categories.Clear();

            var key = subsetCat.Keys;
            var val = subsetCat.Values;

            string[] arrayKey = new string[key.Count];
            string[] arrayVal = new string[val.Count];

            int a = 0;

            foreach (string s in key)
            {
                arrayKey[a++] = s;
            }

            int j = 0;

            foreach (string s in val)
            {
                arrayVal[j++] = s;
            }

            subsetCat.Clear();

            for (int k = 0; k < (arrayKey.Length + arrayVal.Length) / 2; k++)
            {
                subsetCat.Add(arrayVal[k], arrayKey[k]);
            }

            foreach (var item in subsetCat)
            {
                categories.Add(item.Key, item.Value);
            }

            foreach (var item in remainingCat)
            {
                categories.Add(item.Key, item.Value);
            }

            // Convert Dictionary to List<T> for comparisons
            foreach (var item in categories)
            {
                modelAnswer.Add(item.Key + "$" + item.Value);
            }

            // Adds the questions and lists to DG
            DataGridPopulation(categories, rand);
        }

        private void DataGridPopulation(Dictionary<string, string> questions, Random rand)
        {
            foreach (var item in questions)
            {
                questionList.Add(item.Key);
                answerList.Add(item.Value);
            }

            questionList = questionList.Take(4).ToList();

            answerList = answerList.Take(7).ToList();

            questionList = questionList.OrderBy(x => rand.Next()).ToList();
            answerList = answerList.OrderBy(x => rand.Next()).ToList();

            DgQuestions = new ObservableCollection<string>(questionList);
            DgAnswers = new ObservableCollection<string>(answerList);

            (this.Content as FrameworkElement).DataContext = this;
        }

        private void ExtractTimer()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Tick += Dt_Tick;
            stopWatch.Start();
            dispatcherTimer.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {

            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                TimerLabel.Content = currentTime;
            }

        }

        private void BtnStart(object sender, RoutedEventArgs e)
        {
            RandomizeArea();
            ExtractTimer();
        }

        private void questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Console.Clear();

                int noOfCorrect = 0;

                List<string> userOrdered = new List<string>();

                for (int i = 0; i < questions.Items.Count; i++) userOrdered.Add(questions.Items[i].ToString() + "$" + answers.Items[i]);

                Console.WriteLine("\nUser Defined Order:");
                foreach (var item in userOrdered)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nCorrect Order:");
                foreach (var item in modelAnswer)
                {
                    Console.WriteLine(item);
                }

                foreach (var uo in userOrdered)
                {
                    foreach (var ma in modelAnswer)
                    {
                        if (uo.Equals(ma)) noOfCorrect++;
                    }
                }

                Console.WriteLine(noOfCorrect);

                if (noOfCorrect == 4)
                {
                    stopWatch.Stop();
                    var rsltMessageBox = MessageBox.Show("The Descriptions Have Been Matched Successfully. " + currentTime, "Complete!");

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.OK:
                            this.NavigationService.Navigate(new IdentifyAreas());
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}

public class DeweyDecimalClass
{
    public int questions { get; set; }
    public string answers { get; set; }
}

public class dictionaryClass
{ 
    public SortedDictionary<string, string> Categories = new SortedDictionary<string, string>(){

         {"000", "General Knowledge"},
         {"100", "Philosphy & Psychology"},
         {"200", "Religion"},
         {"300", "Social Sciences"},
         {"400", "Languages"},
         {"500", "Science" },
         {"600", "Technology" },
         {"700", "Arts & Recreation"},
         {"800", "Literature"},
         {"900", "History & Geography"}
    };
}
