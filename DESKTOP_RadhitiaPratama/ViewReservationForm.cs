using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    public partial class ViewReservationForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public int reservationID;

        public ViewReservationForm()
        {
            InitializeComponent();
        }

        private void ViewReservationForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dddd,dd MMMM yyyy";
            dateTimePicker1.Value = DateTime.Now;

            loadTable();
        }

        void loadTable()
        {
            flowLayoutPanel1.Controls.Clear();
            var selectedDate = DateTime.Parse(dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            var dataReservation = db.Reservations.Where(f => f.ReservationDate == selectedDate).ToList();
            var tables = db.Tables.ToList();
            foreach (var table in tables)
            {
                var tableID = table.ID;
                bool ordered = false;

                var check = dataReservation.Where(f => f.TableID == tableID).FirstOrDefault();
                var tmpReservationID = 0;

                if (check != null)
                {
                    ordered = true;
                    tmpReservationID = check.ID;
                }


                TableUserControl tableControl = new TableUserControl(this, ordered, tmpReservationID, tableID);
                flowLayoutPanel1.Controls.Add(tableControl);
            }
        }

        public void getCustomer()
        {
            var customer = db.Reservations.Where(f => f.ID == reservationID).FirstOrDefault();
            if (customer != null)
            {
                textBox1.Text = customer.CustomerFirstName;
                textBox2.Text = customer.CustomerLastName;
                textBox3.Text = customer.CustomerEmail;
                textBox4.Text = customer.CustomerPhoneNumber;
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        public void getMenuOrdered()
        {
            bindingSource1.Clear();
            var menuDetail = db.ReservationDetails.Where(f => f.ReservationID == reservationID).ToList();
            bindingSource1.DataSource = menuDetail;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is ReservationDetail detail)
            {
                if (e.ColumnIndex == MenuColumn.Index)
                {
                    e.Value = detail.Menu.Name;
                }

                if (e.ColumnIndex == QtyColumn.Index)
                {
                    e.Value = detail.Qty;
                }

                if (e.ColumnIndex == PriceColumn.Index)
                {
                    e.Value = detail.Menu.Price.ToString("C2", new CultureInfo("id-ID"));
                }

                if (e.ColumnIndex == SubtotalColumn.Index)
                {
                    var subtotal = detail.Qty * detail.Menu.Price;
                    e.Value = subtotal.ToString("C2", new CultureInfo("id-ID"));
                }

            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            loadTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AppConctext(new AdminMainForm());
            this.Close();
        }
    }
}
