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
        private Helpers.Logic oLogic;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            oLogic = new Helpers.Logic(this);
        }

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
