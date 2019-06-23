using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe.Helpers
{
    public class Logic
    {
        private List<Helpers.MarkType> _Results;
        private List<List<int>> _WinList = new List<List<int>>();
        private bool _PlayerTurn;
        private MainWindow _win;

        public Logic(MainWindow win)
        {
            _win = win;
            generateWinCombinations();
            StartNewGame();
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


        public void StartNewGame()
        {
            _Results = new List<Helpers.MarkType>();
            _PlayerTurn = true;
            _win.mainGrid.Children.Cast<Button>().ToList().ForEach(b => { b.Content = String.Empty; b.Background = Brushes.White; b.Foreground = Brushes.Black; });
        }

        public void ButtonHandler(Button btn, int pos)
        {
            string sMark = _Results.Where(o => o.Index == pos).Select(x => x.Mark).FirstOrDefault();
            if (sMark == String.Empty) return;
            _Results.Add(new Helpers.MarkType { Mark = _PlayerTurn ? "X" : "O", Index = pos });
            btn.Content = _PlayerTurn ? "X" : "O";
            _PlayerTurn ^= true; // bitvise flip
            int result = winnerCheck();
            if (result != 0)
            {
                MessageBox.Show("Winner is: " + (result == 1 ? "Player 1" : "Player 2"));
                StartNewGame();
            }
        }

        private int winnerCheck()
        {

            var xMark = _Results.Where(x => x.Mark == "X").Select(x => x.Index).ToList();
            var oMark = _Results.Where(o => o.Mark == "O").Select(o => o.Index).ToList();
            foreach (List<int> lst in _WinList)
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
            foreach (int i in lst)
            {
                switch (i)
                {
                    case 0:
                        _win.btn00.Foreground = Brushes.Green;
                        break;
                    case 1:
                        _win.btn01.Foreground = Brushes.Green;
                        break;
                    case 2:
                        _win.btn02.Foreground = Brushes.Green;
                        break;
                    case 3:
                        _win.btn10.Foreground = Brushes.Green;
                        break;
                    case 4:
                        _win.btn11.Foreground = Brushes.Green;
                        break;
                    case 5:
                        _win.btn12.Foreground = Brushes.Green;
                        break;
                    case 6:
                        _win.btn20.Foreground = Brushes.Green;
                        break;
                    case 7:
                        _win.btn21.Foreground = Brushes.Green;
                        break;
                    case 8:
                        _win.btn22.Foreground = Brushes.Green;
                        break;
                }

            }
        }

    }

   
}
