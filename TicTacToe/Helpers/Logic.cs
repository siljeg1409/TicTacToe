using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe.Helpers
{
    public class Logic
    {
        #region Private members
        private List<MarkType> _Results;
        private List<List<int>> _WinList = new List<List<int>>();
        //private bool _PlayerTurn;
        private MainWindow _win;
        private List<MarkType> _ResultsAI;
        private bool _PlayerTurnAI;
        #endregion

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
            _win.mainGrid.Children.Cast<Button>().ToList().ForEach(b => { b.Content = String.Empty; b.Background = Brushes.White; b.Foreground = Brushes.Black; });
        }




        public void ButtonHandler(Button btn, int pos)
        {
            string sMark = _Results.Where(o => o.Index == pos).Select(x => x.Mark).FirstOrDefault();
            if (sMark != null) return;
            _Results.Add(new MarkType("X",pos));
            btn.Content = "X";
            if (messageHandler(winnerCheck())) return;
            _ResultsAI = new List<MarkType>(_Results);
            _PlayerTurnAI = false;
            setButton(bestMove());
            messageHandler(winnerCheck());
        }

        private bool messageHandler(int v)
        {
            if (v != 0)
            {
                MessageBox.Show("Winner is: " + (v == 1 ? "Player 1" : "Player 2"));
                StartNewGame();
                return true;
            }
            else if (v == 0 && _Results.Count() > 8)
            {
                MessageBox.Show("DRAW");
                StartNewGame();
                return true;
            }
            return false;
        }

        private void setButton(int v)
        {
            switch(v)
            {
                case 0:
                    _win.btn00.Content = "O";
                    break;
                case 1:
                    _win.btn01.Content = "O";
                    break;
                case 2:
                    _win.btn02.Content = "O";
                    break;
                case 3:
                    _win.btn10.Content = "O";
                    break;
                case 4:
                    _win.btn11.Content = "O";
                    break;
                case 5:
                    _win.btn12.Content = "O";
                    break;
                case 6:
                    _win.btn20.Content = "O";
                    break;
                case 7:
                    _win.btn21.Content = "O";
                    break;
                case 8:
                    _win.btn22.Content = "O";
                    break;
            }
            _Results.Add(new MarkType("O", v));
        }

        private int bestMove()
        {
            int nextMove = 0;
            List<int> blankMarkAI = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }; // Blank
            int i = 1;
            while (i != 2)
            {
                List<int> xMarkAI = _ResultsAI.Where(x => x.Mark == "X").Select(x => x.Index).ToList(); // player markers
                List<int> oMarkAI = _ResultsAI.Where(o => o.Mark == "O").Select(o => o.Index).ToList(); // AI markers
                int posWin = checkPotentialWin(xMarkAI, oMarkAI);
                if (posWin != -1) return posWin;
                List<int> res = blankMarkAI.Except(xMarkAI).Except(oMarkAI).ToList();
                if (res.Count() == 0) return nextMove;

                Random ran = new Random();
                nextMove = res[ran.Next(0, res.Count() - 1)]; // this is so wrong, need to check if 1 X is missing for win
                _ResultsAI.Add(new MarkType(_PlayerTurnAI ? "X" : "O",nextMove));
                _PlayerTurnAI ^= true;
                i = winnerCheck(false);
                if (i == 1) return nextMove;
            }

            return nextMove;
        }

        private int winnerCheck(bool bPlayer = true)
        {
            var tmpRes = _ResultsAI;
            if (bPlayer)
                tmpRes = _Results;
            var xMark = tmpRes.Where(x => x.Mark == "X").Select(x => x.Index).ToList();
            var oMark = tmpRes.Where(o => o.Mark == "O").Select(o => o.Index).ToList();

            foreach (List<int> lst in _WinList)
            {
                
                if (!lst.Except(xMark).Any())
                {
                    if (bPlayer) markWinLine(lst);
                    return 1;
                }
                else if (!lst.Except(oMark).Any())
                {
                    if (bPlayer) markWinLine(lst);
                    return 2;
                }
            }
            return 0;
        }

        private int checkPotentialWin(List<int> x, List<int> o)
        {
            foreach (List<int> lst in _WinList)
            {
                if (lst.Except(x).Count() == 1)
                {
                    var result = lst.Except(x);
                    foreach (var r in result)
                        if (!o.Contains(r))
                            return r;
                }
            }
            return -1;
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
