using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    public partial class TableUserControl : UserControl
    {
        bool _ordered;
        ViewReservationForm _form;
        int _reservationID;

        public TableUserControl(ViewReservationForm form, bool ordered, int reservationID, int tableID)
        {
            InitializeComponent();
            _ordered = ordered;
            _form = form;
            tableID = tableID;
            _reservationID = reservationID;

            label1.Text = tableID.ToString();
        }

        private void TableUserControl_Load(object sender, EventArgs e)
        {
            DirectoryInfo dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            var imageDir = Path.Combine(dir.FullName, "Resources");
            if (_ordered)
            {
                var destImage = Path.Combine(imageDir, "table_reserved.png");
                pictureBox1.Image = Image.FromFile(destImage);
            }
            else
            {
                var destImage = Path.Combine(imageDir, "table_free.png");
                pictureBox1.Image = Image.FromFile(destImage);

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            _form.reservationID = _reservationID;
            _form.getCustomer();
            _form.getMenuOrdered();
        }
    }
}
