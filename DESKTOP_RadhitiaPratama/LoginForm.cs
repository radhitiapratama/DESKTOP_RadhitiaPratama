using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    public partial class LoginForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                Alerts.Error("Email must be filled!");
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                Alerts.Error("Password must be filled!");
                return;
            }

            var user = db.Users.Where(f => f.Email == textBox1.Text && f.Password == textBox2.Text).FirstOrDefault();
            if (user == null)
            {
                Alerts.Error("Users not found!");
                return;
            }

            if (user.RoleID != 1)
            {
                Alerts.Error("Sorry you're not admin!");
                return;
            }

            Utils.userID = user.ID;
            new AppConctext(new AdminMainForm());
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = "dgannyt@squidoo.com";
            textBox2.Text = "dN1|qg!,xuZ";
        }
    }
}
