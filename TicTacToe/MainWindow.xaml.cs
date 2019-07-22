using System.Windows;
using System.Windows.Controls;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region PrivateMembers
        private Helpers.Logic oLogic;
        #endregion

        /// <summary>
        /// Constructor for MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            oLogic = new Helpers.Logic(this);
        }

        /// <summary>
        /// Main event for button click from player, this method calls AI turn also, calculates chosen mark based on clicked row/colum in main grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var column = Grid.GetColumn(btn);
            var row = Grid.GetRow(btn);
            var pos = row * 3 + column;
            oLogic.ButtonHandler(btn, pos);
        }


    }
}
