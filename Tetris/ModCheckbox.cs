using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class ModCheckbox : CheckBox
    {
        public event KeyEventHandler HandleKeyUp;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (HandleKeyUp != null)
                HandleKeyUp(this, e);
        }
    }
}
