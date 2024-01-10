using System;
using System.CodeDom;
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
    public partial class AdminMainForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new AppConctext(new LoginForm());
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AppConctext(new ManageMemberForm());
            this.Close();
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            var user = db.Users.Where(f => f.ID == Utils.userID).FirstOrDefault();
            label2.Text = user.FirstName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AppConctext(new ManageMenuForm());
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AppConctext(new ManageMenuIngredientForm());
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new AppConctext(new ViewReservationForm());
            this.Close();
        }
    }
}
