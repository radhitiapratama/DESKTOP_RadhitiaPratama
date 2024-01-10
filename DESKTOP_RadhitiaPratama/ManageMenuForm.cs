using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    public partial class ManageMenuForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();
        int selectedID = 0;

        public ManageMenuForm()
        {
            InitializeComponent();
        }

        private void ManageMenuForm_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = decimal.MaxValue;
            bindingSource2.AddNew();
            loadMenu();
            disableInput();
            loadCategory();
        }

        void loadCategory()
        {
            var query = db.Categories.ToList();
            query.Insert(0, new Category
            {
                ID = 0,
                Name = "Select Category"
            });

            comboBox1.DataSource = query;
            comboBox1.SelectedIndex = 0;
        }

        void disableInput()
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            comboBox1.Enabled = false;
            comboBox1.SelectedValue = 0;
            numericUpDown1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        void enableInput()
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            comboBox1.Enabled = true;
            numericUpDown1.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        void disableActButton()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }

        void enableActButton()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        void resetState()
        {
            bindingSource2.Clear();
            bindingSource2.AddNew();
            selectedID = 0;
            comboBox1.SelectedValue = 0;
            numericUpDown1.Value = 1;
        }

        void loadMenu()
        {
            bindingSource1.Clear();

            var query = db.Menus.AsQueryable();
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query = query.Where(f => f.Name.Contains(textBox1.Text) || f.Category.Name.Contains(textBox1.Text));
            }

            bindingSource1.DataSource = query.ToList();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                if (e.ColumnIndex == CategoryColumn.Index)
                {
                    e.Value = menu.Category.Name;
                }

                if (e.ColumnIndex == PriceColumn.Index)
                {
                    e.Value = menu.Price.ToString("C2", new CultureInfo("id-ID"));
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadMenu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new AppConctext(new AdminMainForm());
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            disableActButton();
            enableInput();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!validate()) return;

            if (bindingSource2.Current is Menu menu)
            {
                menu.CategoryID = (int)comboBox1.SelectedValue;
                menu.Price = (double)numericUpDown1.Value;
                db.Menus.AddOrUpdate(menu);
                db.SaveChanges();

                if (selectedID == 0)
                {
                    Alerts.Success("Menu successfully inserted");
                }
                else
                {
                    Alerts.Success("Failed to add menu");
                }

                resetState();
                loadMenu();
                disableInput();
                enableActButton();
            }
        }

        bool validate()
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || (int)comboBox1.SelectedValue == 0 || numericUpDown1.Text == "")
            {
                Alerts.Error("All field must be filled!");
                return false;
            }

            if (numericUpDown1.Value <= 0)
            {
                Alerts.Error("Minimum price is 1");
                return false;
            }

            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dataGridView1.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                var data = db.Menus.AsNoTracking().Where(f => f.ID == menu.ID).First();
                bindingSource2.DataSource = data;
                selectedID = data.ID;
                comboBox1.SelectedValue = data.CategoryID;
                numericUpDown1.Value = (decimal)data.Price;

                disableInput();
                enableActButton();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bindingSource2.Current is Menu menu)
            {
                if (menu.ID == 0)
                {
                    Alerts.Error("Select menu first!");
                    return;
                }

                enableInput();
                disableActButton();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bindingSource2.Current is Menu menu)
            {
                if (menu.ID == 0)
                {
                    Alerts.Error("Select menu first!");
                    return;
                }

                var confirmn = Alerts.Confirm("Are you sure to delete menu?");

                if (confirmn == DialogResult.Yes)
                {
                    var data = db.Menus.Where(f => f.ID == selectedID).First();

                    try
                    {
                        db.Menus.Remove(data);
                        db.SaveChanges();
                        Alerts.Success("Menu deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        Alerts.Error("Failed to delete menu");
                        db.Entry(data).Reload();
                        //db.Entry(data).State = System.Data.Entity.EntityState.Deleted;
                    }
                    finally
                    {
                        loadMenu();
                        enableActButton();
                        disableInput();
                        resetState();
                    }
                }

                if (confirmn == DialogResult.No)
                {
                    enableActButton();
                    disableInput();
                    resetState();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            disableInput();
            enableActButton();
            resetState();
        }
    }
}
