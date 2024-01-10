using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    public partial class ManageMemberForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();
        int op = -1;
        int selectedID;

        public ManageMemberForm()
        {
            InitializeComponent();
        }

        private void ManageMemberForm_Load(object sender, EventArgs e)
        {
            bindingSource2.AddNew();
            loadMember();
            disableInput();
        }

        void resetState()
        {
            bindingSource2.Clear();
            bindingSource2.AddNew();
            selectedID = 0;
            op = -1;
        }

        void disableInput()
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            maskedTextBox1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
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

        void enableInput()
        {
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            maskedTextBox1.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }

        void loadMember()
        {
            bindingSource1.Clear();
            var query = db.Users.Where(f => f.RoleID == 2).AsQueryable();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query = query.Where(f => f.FirstName.Contains(textBox1.Text) || f.LastName.Contains(textBox1.Text) || f.Email.Contains(textBox1.Text));
            }
            bindingSource1.DataSource = query.ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadMember();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is User user)
            {
                if (e.ColumnIndex == MemberSinceColumn.Index)
                {
                    var memberDate = user.DateJoined.Year;
                    var today = DateTime.Now.Year;

                    var diff = today - memberDate;

                    e.Value = $"{user.DateJoined.ToString("dd/MM/yyyy")} ({diff}year(s))";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            disableActButton();
            enableInput();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!validateInput() || !validateEmailUnique() || !validatePhoneNumber() || !validatePassword()) return;

            if (bindingSource2.Current is User user)
            {
                user.PhoneNumber = maskedTextBox1.Text;
                if (op == -1)
                {
                    user.DateJoined = DateTime.Now;
                }
                user.RoleID = 2;

                db.Users.AddOrUpdate(user);
                db.SaveChanges();

                if (op == -1)
                {
                    Alerts.Success("Member successfully inserted");
                }
                else
                {
                    Alerts.Success("Member successfully updated");
                }

                loadMember();
                enableActButton();
                disableInput();
                resetState();
            }
        }

        bool validateEmailUnique()
        {
            var check = db.Users.Where(f => f.Email == textBox4.Text).AsQueryable();

            if (op == 1)
            {
                check = check.Where(f => f.ID != selectedID);
            }

            if (check.FirstOrDefault() != null)
            {
                Alerts.Error("Email already in use!");
                return false;
            }

            return true;
        }

        bool validatePhoneNumber()
        {
            if (maskedTextBox1.Text.Length < 10)
            {
                Alerts.Error("Phone number minimum is 10 digit");
                return false;
            }

            return true;
        }

        bool validatePassword()
        {
            if (textBox5.TextLength < 8)
            {
                Alerts.Error("Password minimum 8 characters");
                return false;
            }

            return true;
        }

        bool validateInput()
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                Alerts.Error("All field must be filled");
                return false;
            }

            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < bindingSource1.Count && dataGridView1.Rows[e.RowIndex].DataBoundItem is User user)
            {
                var data = db.Users.Where(f => f.ID == user.ID).AsNoTracking().FirstOrDefault();
                bindingSource2.DataSource = data;
                maskedTextBox1.Text = data.PhoneNumber;
                selectedID = data.ID;
                disableInput();
                enableActButton();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bindingSource2.Current is User user)
            {
                if (user.ID == 0)
                {
                    Alerts.Error("Select member first!");
                    return;
                }

                op = 1;
                enableInput();
                disableActButton();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            disableInput();
            enableActButton();
            resetState();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bindingSource2.Current is User user)
            {
                if (user.ID == 0)
                {
                    Alerts.Error("Select member first");
                    return;
                }

                var confirm = Alerts.Confirm("Are you sure delete data?");

                if (confirm == DialogResult.Yes)
                {
                    var data = db.Users.Where(f => f.ID == selectedID).First();

                    try
                    {
                        db.Users.Remove(data);
                        db.SaveChanges();
                        Alerts.Success("Memnber sucessfully deleted");
                    }
                    catch (Exception ex)
                    {
                        db.Entry(data).Reload();
                        Alerts.Error("Failed to delete member");
                    }
                    finally
                    {
                        loadMember();
                        disableInput();
                        enableActButton();
                        resetState();
                    }
                }

                if (confirm == DialogResult.No)
                {
                    disableInput();
                    enableActButton();
                    resetState();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new AppConctext(new AdminMainForm());
            this.Close();
        }
    }
}
