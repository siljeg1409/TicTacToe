using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Helpers
{
    public class MarkType
    {
        // Copy constructor.
        public MarkType(MarkType v)
        {
            Mark = v.Mark;
            Index = v.Index;
        }

        public MarkType(string m, int i)
        {
            Mark = m;
            Index = i;
        }

        public string Mark { get; set; }
        public int Index { get; set; }
    }
}
