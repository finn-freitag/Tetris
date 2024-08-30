using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class ModNumericUpDown : NumericUpDown
    {
        public event KeyEventHandler HandleKeyUp;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;

            if (HandleKeyUp != null)
                HandleKeyUp(this, e);
        }
    }
}
