using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kons.Utility
{
    class KnDegine
    {
        static public void setFormFixedDlgStyle(Form _des_form, Form _owner = null)
        {
            if (null != _owner)
            {
                _des_form.Owner = _owner;
            }

            _des_form.MaximizeBox = false;
            _des_form.MinimizeBox = false;
            _des_form.AutoScaleMode = AutoScaleMode.None;
            _des_form.FormBorderStyle = FormBorderStyle.FixedSingle;
            _des_form.StartPosition = FormStartPosition.CenterParent;
        }

        static public void setFormFixedPopupStyle(Form _des_form, Form _owner = null)
        {
            if (null != _owner)
            {
                _des_form.Owner = _owner;
            }

            _des_form.MaximizeBox = false;
            _des_form.MinimizeBox = false;
            _des_form.AutoScaleMode = AutoScaleMode.None;
            _des_form.FormBorderStyle = FormBorderStyle.FixedSingle;
            _des_form.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
