using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace TicTacToe.Helpers
{
    public class Logic
    {
        #region Private members
        private List<MarkType> _Results;
        private List<List<int>> _WinList = new List<List<int>>();
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
            _Results = new List<MarkType>();
            _win.mainGrid.Children.Cast<Button>().ToList().ForEach(b => { b.Content = String.Empty; b.Background = Brushes.White; b.Foreground = Brushes.Black; });
        }

        public void ButtonHandler(Button btn, int pos)
        {
            string sMark = _Results.Where(o => o.Index == pos).Select(x => x.Mark).FirstOrDefault();
            if (sMark != null) return;
            play(pos, true);
            if (messageHandler(winnerCheck())) return;
            _ResultsAI = new List<MarkType>(_Results);
            _PlayerTurnAI = false;
            play(bestMove(),false);
            messageHandler(winnerCheck());
        }

        private bool messageHandler(int v)
        {
            if((v == 0 && _Results.Count() > 8) || v!= 0)
            {
                MessageBox.Show((v == 0 && _Results.Count() > 8) ? "It's a DRAW!" : v == 1 ? "You are a WINNER!" : "You LOST!");
                StartNewGame();
                return true;
            }
            return false;
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
                int wl = checkPotentialWinLoss(xMarkAI, oMarkAI);
                if (wl != -1) return wl;
                List<int> res = blankMarkAI.Except(xMarkAI).Except(oMarkAI).ToList(); // Possible Blanks to make
                if (res.Count() == 0) return nextMove;
                Random ran = new Random();
                nextMove = res[ran.Next(0, res.Count() - 1)]; // this is so wrong, i need i better "random"
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

        private int checkPotentialWinLoss(List<int> x, List<int> o)
        {
            //First check if there is a chance to win the game
            foreach (List<int> lst in _WinList)
            {
                if (lst.Except(o).Count() == 1)
                {
                    var result = lst.Except(o);
                    foreach (var r in result)
                        if (!x.Contains(r))
                            return r;
                }
            }

            //Second check if there is a chance to lose the game
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

        private void play(int v, bool player)
        {
            switch (v)
            {
                case 0:
                    _win.btn00.Content = player ? "X" : "O";
                    break;
                case 1:
                    _win.btn01.Content = player ? "X" : "O";
                    break;
                case 2:
                    _win.btn02.Content = player ? "X" : "O";
                    break;
                case 3:
                    _win.btn10.Content = player ? "X" : "O";
                    break;
                case 4:
                    _win.btn11.Content = player ? "X" : "O";
                    break;
                case 5:
                    _win.btn12.Content = player ? "X" : "O";
                    break;
                case 6:
                    _win.btn20.Content = player ? "X" : "O";
                    break;
                case 7:
                    _win.btn21.Content = player ? "X" : "O";
                    break;
                case 8:
                    _win.btn22.Content = player ? "X" : "O";
                    break;
            }
            _Results.Add(new MarkType(player ? "X" : "O", v));
        }


    }

   
}
