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
        private bool _PlayerTurn;
        private bool _GameEnd;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            startNewGame();
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
            #region Winning Combinations
            var win1 = new[] { 0, 1, 2 };
            var win2 = new[] { 3, 4, 5 };
            var win3 = new[] { 6, 7, 8 };

            var win5 = new[] { 0, 3, 6 };
            var win6 = new[] { 1, 4, 7 };
            var win7 = new[] { 2, 5, 8 };

            var win8 = new[] { 2, 4, 6 };
            var win4 = new[] { 0, 4, 8 };
            #endregion

            var xMark = _Results.Where(x => x.Mark == "X").Select(x => x.Index).ToList();
            var oMark = _Results.Where(o => o.Mark == "O").Select(o => o.Index).ToList();

            if (!win1.Except(xMark).Any() ||
                !win2.Except(xMark).Any() ||
                !win3.Except(xMark).Any() ||
                !win4.Except(xMark).Any() ||
                !win5.Except(xMark).Any() ||
                !win6.Except(xMark).Any() ||
                !win7.Except(xMark).Any() ||
                !win8.Except(xMark).Any()) return 1;

            else if (!win1.Except(oMark).Any() ||
                !win2.Except(oMark).Any() ||
                !win3.Except(oMark).Any() ||
                !win4.Except(oMark).Any() ||
                !win5.Except(oMark).Any() ||
                !win6.Except(oMark).Any() ||
                !win7.Except(oMark).Any() ||
                !win8.Except(oMark).Any()) return 2;
            else
                return 0;
        }
    }
}
