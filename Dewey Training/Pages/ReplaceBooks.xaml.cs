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

        // Instantiate random number generator.  
        private Random _random;

        public ObservableCollection<DecimalClass> DeweyDecimal { get; set; }

        DoublyLinkedList decimals = new DoublyLinkedList();

        // Correct Order
        DoublyLinkedList correctlyOrderedDeweys = new DoublyLinkedList();
        public static List<string> correctlyOrderedDeweysList = new List<string>();

        // Timers
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        string currentTime = string.Empty;

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

            foreach (var dec in decimals)
            {
                deweys.Add(dec.Data.Decimal + " " + dec.Data.Author);
            }

            string[] array = deweys.ToArray();

            int i, j, l;

            string[] arr1 = array;
            string temp;

            l = arr1.Length;

            for (i = 0; i < l; i++)
            {
                for (j = 0; j < l - 1; j++)
                {
                    if (arr1[j].CompareTo(arr1[j + 1]) > 0)
                    {
                        temp = arr1[j];
                        arr1[j] = arr1[j + 1];
                        arr1[j + 1] = temp;
                    }
                }
            }

            for (i = 0; i < l; i++)
            {

                d.Add(new DecimalClass()
                {
                    Decimal = arr1[i].Split(" ")[0],
                    Author = arr1[i].Split(" ")[1]
                });

            }

            return d;

        }
        private void Check_Order(object sender, RoutedEventArgs e)
        {
            try { 
                CheckOrder(); 
            } 
            catch {

            }
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //do nothing 

        }

        private void CheckOrder()
        {

            // User defined order
            List<string> decimals = new List<string>();
            List<string> authors = new List<string>();
            List<string> userSelection = new List<string>();

            try
            {

                var rows = GetDataGridRows(dataGrid);

                foreach (DataGridRow row in rows)
                {

                    foreach (DataGridColumn column in dataGrid.Columns)
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

                for (int i = 0; i < decimals.Count; i++)
                {

                    userSelection.Add(decimals[i] + " " + authors[i]);

                }


                bool equal = userSelection.SequenceEqual(correctlyOrderedDeweysList);

                if (equal)
                {
                    stopWatch.Reset();
                    string sMessageBoxText = $"Congratulations! \n Your time was \n{currentTime} ";
                    string sCaption = "Completed! " + currentTime +" ";

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

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        // Generates a random number within a range.      
        public string RandomNumber(int min, int max, bool isDecimalValue)
        {
            string number = _random.Next(min, max).ToString();

            if (isDecimalValue)
            {

                if (number.ToString().Length == 2)
                {
                    number = "0" + number;
                }
                if (number.ToString().Length == 1)
                {
                    number = "00" + number;
                }

            }

            return number;
        }

        // Generates a random string with a given size.    
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            if (Start.Content.Equals("Restart"))
            {

                stopWatch.Reset();

            }

            dataGrid.IsEnabled = true;

            GenerateBooks();

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Tick += Dt_Tick;
            stopWatch.Start();
            dispatcherTimer.Start();

            Start.Content = "Restart";

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

            // Clear values
            foreach (var item in decimals)
            {
                decimals.Remove(item.Data);
            }

            foreach (var item in correctlyOrderedDeweys)
            {
                correctlyOrderedDeweys.Remove(item.Data);
            }

            correctlyOrderedDeweysList.Clear();

            (this.Content as FrameworkElement).DataContext = null;

            _random = new Random();

            int randomValue = Int32.Parse(RandomNumber(0, 4, false));

            correctlyOrderedDeweys = decimals;

            for (int i = 0; i < 10 - randomValue; i++)
            {

                decimals.Add(new DecimalClass()
                {
                    Decimal = RandomNumber(0, 999, true) + "." + RandomNumber(0, 9999, false),
                    Author = RandomString(3)
                });

            }

            for (int i = 0; i < randomValue; i++)
            {

                int index = Int32.Parse(RandomNumber(0, 10 - randomValue, false));
                decimals.Add(new DecimalClass()
                {
                    Decimal = decimals.ElementAt(index).Data.Decimal,
                    Author = RandomString(3)
                });

            }

            // Determine correct order for the Dewey Decimals
            correctlyOrderedDeweys.CorrectSort();

            foreach (var item in correctlyOrderedDeweys)
            {
                Console.WriteLine(item.Data.Decimal + " " + item.Data.Author);
            }

            // Determines the order for the authors
            correctlyOrderedDeweys = FinalizeOrder(correctlyOrderedDeweys);

            foreach (var item in correctlyOrderedDeweys)
            {

                correctlyOrderedDeweysList.Add(item.Data.Decimal + " " + item.Data.Author);

            }

            ObservableCollection<DecimalClass> temp = new ObservableCollection<DecimalClass>();
            foreach (var item in decimals)
            {
                temp.Add(item.Data);
            }

            temp.Shuffle();

            DeweyDecimal = new ObservableCollection<DecimalClass>(temp);

            (this.Content as FrameworkElement).DataContext = this;

        }
    }
}
