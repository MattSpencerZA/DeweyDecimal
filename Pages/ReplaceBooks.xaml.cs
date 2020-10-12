using DeweyDecimalTrainer.LinkedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DeweyDecimalTrainer.Pages
{
    /// <summary>
    /// Interaction logic for ReplaceBooks.xaml
    /// </summary>
    public partial class ReplaceBooks : Page
    {
        // Correct Order
        DoublyLinkedList correctlyOrderedDeweys = new DoublyLinkedList();
        public static List<string> correctlyOrderedDeweysList = new List<string>();

        // Stopwatches
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

        // Instantiate random number generator.
        private Random random;

        DoublyLinkedList callnumbers = new DoublyLinkedList();

        public ObservableCollection<DecimalClass> Decimal { get; set; }

        public ReplaceBooks()
        {

            InitializeComponent();
            GenerateBooks();

        }

        // Sorts the ordered linked list alphabetically
        private DoublyLinkedList FinalizeOrder(DoublyLinkedList decimals)
        {

            List<string> deweys = new List<string>();
            DoublyLinkedList d = new DoublyLinkedList();

            foreach (var dec in decimals) deweys.Add(dec.Data.Decimal + " " + dec.Data.Author);

            string[] array = deweys.ToArray();

            int i, j, l;

            string[] arrayone = array;
            string temporary;

            l = arrayone.Length;

            for (i = 0; i < l; i++)
            {
                for (j = 0; j < l - 1; j++)
                {
                    if (arrayone[j].CompareTo(arrayone[j + 1]) > 0)
                    {
                        temporary = arrayone[j];
                        arrayone[j] = arrayone[j + 1];
                        arrayone[j + 1] = temporary;
                    }
                }
            }

            for (i = 0; i < l; i++)
            {

                d.Add(new DecimalClass()
                {
                    Decimal = arrayone[i].Split(" ")[0],
                    Author = arrayone[i].Split(" ")[1]
                });

            }

            return d;

        }
        private void Check_Order(object sender, RoutedEventArgs e)
        {
            CheckOrder();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckOrder()
        {

            // Selected order
            List<string> decimals = new List<string>();
            List<string> authors = new List<string>();
            List<string> userSelection = new List<string>();

            try
            {

                var rows = GetDGRows(DataGrid);

                foreach (DataGridRow row in rows)
                {

                    foreach (DataGridColumn column in DataGrid.Columns)
                    {
                        if (column.GetCellContent(row) is TextBlock)
                        {
                            TextBlock cellContent = column.GetCellContent(row) as TextBlock;

                            if (cellContent.Text.Split(" ")[0].Length == 3)
                            {
                                authors.Add(cellContent.Text);
                            }
                            else
                            {
                                decimals.Add(cellContent.Text);
                            }
                        }
                    }
                }
                for (int i = 0; i < decimals.Count; i++) userSelection.Add(decimals[i] + " " + authors[i]);

                bool equal = userSelection.SequenceEqual(correctlyOrderedDeweysList);

                if (equal)
                {
                    stopWatch.Reset();
                    string sMessageBoxText = $"Congratulations! \n Your time was \n{currentTime} ";
                    string sCaption = "Completed! " + currentTime + " ";

                    var btnMessageBox = MessageBoxButton.OK;
                    var icnMessageBox = MessageBoxImage.Information;

                    var rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.OK:
                            this.NavigationService.Navigate(new LeaderBoard());
                            break;
                        case MessageBoxResult.None:
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        case MessageBoxResult.Yes:
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public IEnumerable<DataGridRow> GetDGRows(DataGrid dg)
        {
            var itemsSource = dg.ItemsSource as IEnumerable;

            if (null == itemsSource) yield return null;

            foreach (var item in itemsSource)
            {
                var row = dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (null != row) yield return row;
            }
        }

        // Generates a random number within a range.      
        public string RandomNumberGenerator(int minimum, int maximum, bool isDewey)
        {
            string num = random.Next(minimum, maximum).ToString();

            if (isDewey)
            {
                if (num.ToString().Length == 1)
                {
                    num = "00" + num;
                }
                else if (num.ToString().Length == 2)
                {
                    num = "0" + num;
                }
            }
            return num;
        }

        // Generates a random string with a given size
        public string RandomStringGenerator(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Start.Content.Equals("Restart")) stopWatch.Reset();

            DataGrid.IsEnabled = true;

            GenerateBooks();
            ExtractTimer();

            Start.Content = "Restart";

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
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                    ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                TimerLabel.Content = currentTime;
            }

        }

        private void GenerateBooks()
        {
            // Reset
            foreach (var item in callnumbers)
            {
                callnumbers.Remove(item.Data);
            }

            foreach (var item in correctlyOrderedDeweys)
            {
                correctlyOrderedDeweys.Remove(item.Data);
            }

            correctlyOrderedDeweysList.Clear();

            (this.Content as FrameworkElement).DataContext = null;

            random = new Random();

            int randomNum = Int32.Parse(RandomNumberGenerator(0, 4, false));

            correctlyOrderedDeweys = callnumbers;

            for (int i = 0; i < 10 - randomNum; i++)
            {
                callnumbers.Add(new DecimalClass()
                {
                    Author = RandomStringGenerator(3),
                    Decimal = RandomNumberGenerator(0, 999, true) + "." + RandomNumberGenerator(0, 9999, false)
                });
            }

            for (int i = 0; i < randomNum; i++)
            {
                int index = Int32.Parse(RandomNumberGenerator(0, 10 - randomNum, false));
                callnumbers.Add(new DecimalClass()
                {
                    Author = RandomStringGenerator(3),
                    Decimal = callnumbers.ElementAt(index).Data.Decimal
                });
            }

            // Calculates correct order for deweys
            correctlyOrderedDeweys.CorrectSort();

            foreach (var item in correctlyOrderedDeweys)
            {
                Console.WriteLine(item.Data.Decimal + " " + item.Data.Author);
            }

            // Calculates correct order for authors
            correctlyOrderedDeweys = FinalizeOrder(correctlyOrderedDeweys);

            foreach (var item in correctlyOrderedDeweys)
            {
                correctlyOrderedDeweysList.Add(item.Data.Decimal + " " + item.Data.Author);
            }

            ObservableCollection<DecimalClass> tempData = new ObservableCollection<DecimalClass>();
            foreach (var item in callnumbers)
            {
                tempData.Add(item.Data);
            }
            tempData.Shuffle();

            Decimal = new ObservableCollection<DecimalClass>(tempData);

            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}
