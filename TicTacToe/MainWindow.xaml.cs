using System;
using System.Collections.Generic;
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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region PrivateMembers
        private List<Helpers.MarkType> _Results;
        private List<List<int>> _WinList = new List<List<int>>();
        private bool _PlayerTurn;
        private bool _GameEnd;

        #endregion
        public MainWindow()
        {
            InitializeComponent();
            generateWinCombinations();
            startNewGame();
        }

        private void generateWinCombinations()
        {
            #region Winning Combinations
            _WinList.Add(new List<int> { 0, 1, 2 });
            _WinList.Add(new List<int> { 3, 4, 5 });
            _WinList.Add(new List<int> { 6, 7, 8 });
            _WinList.Add(new List<int> { 0, 3, 6 });
            _WinList.Add(new List<int> { 1, 4, 7 });
            _WinList.Add(new List<int> { 2, 5, 8 });
            _WinList.Add(new List<int> { 2, 4, 6 });
            _WinList.Add(new List<int> { 0, 4, 8 });
            #endregion

        }

        private void startNewGame()
        {
            _Results = new List<Helpers.MarkType>();
            _PlayerTurn = true;
            _GameEnd = false;

            mainGrid.Children.Cast<Button>().ToList().ForEach(b => { b.Content = String.Empty; b.Background = Brushes.White; b.Foreground = Brushes.Blue; });
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {

            var btn = (Button)sender;
            var column = Grid.GetColumn(btn);
            var row = Grid.GetRow(btn);
            var pos = row * 3 + column;
            string sMark = _Results.Where(o => o.Index == pos).Select(x => x.Mark).FirstOrDefault();
            if (sMark == String.Empty) return;
            _Results.Add(new Helpers.MarkType { Mark = _PlayerTurn ? "X" : "O", Index = pos });
            btn.Content = _PlayerTurn ? "X" : "O";
            _PlayerTurn ^= true; // bitvise flip
            btn.Foreground = _PlayerTurn ? Brushes.Blue : Brushes.Red;
            int result = winnerCheck();
            if (result != 0)
            {
                MessageBox.Show("Winner is: " + (result == 1 ? "Player 1" : "Player 2"));
                startNewGame();
            }
               
        }

        private int winnerCheck()
        {
           
            var xMark = _Results.Where(x => x.Mark == "X").Select(x => x.Index).ToList();
            var oMark = _Results.Where(o => o.Mark == "O").Select(o => o.Index).ToList();
            foreach( List<int> lst in _WinList)
            {
                if (!lst.Except(xMark).Any())
                {
                    markWinLine(lst);
                    return 1;
                }
                else if (!lst.Except(oMark).Any())
                {
                    markWinLine(lst);
                    return 2;
                }
            }

            return 0;
    
        }

        private void markWinLine(List<int> lst)
        {
            foreach(int i in lst)
            {
                switch (i)
                {
                    case  0:
                        btn00.Background = Brushes.Green;
                        break;
                    case 1:
                        btn01.Background = Brushes.Green;
                        break;
                    case 2:
                        btn02.Background = Brushes.Green;
                        break;
                    case 3:
                        btn10.Background = Brushes.Green;
                        break;
                    case 4:
                        btn11.Background = Brushes.Green;
                        break;
                    case 5:
                        btn12.Background = Brushes.Green;
                        break;
                    case 6:
                        btn20.Background = Brushes.Green;
                        break;
                    case 7:
                        btn21.Background = Brushes.Green;
                        break;
                    case 8:
                        btn22.Background = Brushes.Green;
                        break;

                }

            }
        }
    }
}
