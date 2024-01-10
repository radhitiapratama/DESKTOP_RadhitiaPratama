using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    class Alerts
    {
        public static DialogResult Success(string msg)
        {
            return MessageBox.Show(msg, "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Error(string msg)
        {
            return MessageBox.Show(msg, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Confirm(string msg)
        {
            return MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
