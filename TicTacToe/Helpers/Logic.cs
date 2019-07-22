using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe.Helpers
{
    /// <summary>
    /// Main logic class
    /// </summary>
    public class Logic
    {
        #region Private members
        private List<MarkType> _Results;
        private List<List<int>> _WinList = new List<List<int>>();
        private MainWindow _win;
        private List<MarkType> _ResultsAI;
        private bool _PlayerTurnAI;
        #endregion

        /// <summary>
        /// Logic class constructor
        /// </summary>
        /// <param name="win">MainWindow for easy in-class manipulation</param>
        public Logic(MainWindow win)
        {
            _win = win;
            generateWinCombinations();
            StartNewGame();
        }

        /// <summary>
        /// Generates matrix of winning combinations for ai move and winner check
        /// </summary>
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

        /// <summary>
        /// Method for starting the new game, clears marked fields from main window and resets main list
        /// </summary>
        public void StartNewGame()
        {
            _Results = new List<MarkType>();
            _win.mainGrid.Children.Cast<Button>().ToList().ForEach(b => { b.Content = String.Empty; b.Background = Brushes.White; b.Foreground = Brushes.Black; });
        }

        /// <summary>
        /// Method for button handling, marking fields (X O)
        /// Calling ai move
        /// Checking for win/loss
        /// </summary>
        /// <param name="btn">Clicked button</param>
        /// <param name="pos">Marker (X O) position</param>
        public void ButtonHandler(Button btn, int pos)
        {
            string sMark = _Results.Where(o => o.Index == pos).Select(x => x.Mark).FirstOrDefault();
            if (sMark != null) return;
            Play(pos, true);
            if (MessageHandler(winnerCheck())) return;
            _ResultsAI = new List<MarkType>(_Results);
            _PlayerTurnAI = false;
            Play(BestMove(),false);
            MessageHandler(winnerCheck());
        }

        /// <summary>
        /// Message will be shown if there is a win/loss/draw
        /// </summary>
        /// <param name="v">
        /// 0 & all fields used => draw, 1 => player is a winner, 2 => player is a looser
        /// </param>
        /// <returns>Returs true if game is over</returns>
        private bool MessageHandler(int v)
        {
            if((v == 0 && _Results.Count() > 8) || v!= 0)
            {
                MessageBox.Show((v == 0 && _Results.Count() > 8) ? "It's a DRAW!" : v == 1 ? "You are a WINNER!" : "You LOST!");
                StartNewGame();
                return true;
            }
            return false;
        }

       
        /// <summary>
        /// Main function for checking "best" possible move for AI to make
        /// </summary>
        /// <returns>Returns AI move</returns>
        private int BestMove()
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
                if (res.Contains(4)) return 4; // Mark center if free, else go random
                Random ran = new Random();
                nextMove = res[ran.Next(0, res.Count() - 1)]; // this is so wrong, i need i better "random", it will do for now
                _ResultsAI.Add(new MarkType(_PlayerTurnAI ? "X" : "O",nextMove));
                _PlayerTurnAI ^= true;
                i = winnerCheck(false);
                if (i == 1) return nextMove;
            }
            return nextMove;
        }

        /// <summary>
        /// Function for checking if there is a winner
        /// </summary>
        /// <param name="bPlayer">Player move (true) or AI move (false)</param>
        /// <returns>Returns 0 if draw, 1 if player wins, 2 if AI wins</returns>
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
                    if (bPlayer) MarkWinLine(lst);
                    return 1;
                }
                else if (!lst.Except(oMark).Any())
                {
                    if (bPlayer) MarkWinLine(lst);
                    return 2;
                }
            }
            return 0;
        }

        /// <summary>
        /// Function for checking if there is a potential win or loss, so that AI will mark one field for win, or block player win with a mark
        /// </summary>
        /// <param name="x">List of player markers</param>
        /// <param name="o">List of AI markers</param>
        /// <returns>Returns mark for win or block, or -1 for AI to make "random" move</returns>
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

        /// <summary>
        /// Method for marking winning line
        /// </summary>
        /// <param name="lst">List of winning combination</param>
        private void MarkWinLine(List<int> lst)
        {
            foreach (int i in lst)
            {
                var btn = (Button)_win.FindName("btn" + i);
                btn.Foreground = Brushes.Green; ;

            }
        }

        /// <summary>
        /// Method for marking the X, O positions
        /// </summary>
        /// <param name="v">Position to be marked</param>
        /// <param name="player">if True than mark is X, if false mark is O</param>
        private void Play(int v, bool player)
        {
            var btn = (Button)_win.FindName("btn" + v);
            btn.Content = player ? "X" : "O";
            _Results.Add(new MarkType(player ? "X" : "O", v));
        }

    }
   
}
