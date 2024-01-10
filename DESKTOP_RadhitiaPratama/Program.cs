using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DESKTOP_RadhitiaPratama
{
    class AppConctext : ApplicationContext
    {
        public AppConctext(Form form)
        {
            form.FormClosed += Form_FormClosed;
            form.Show();
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var count = Application.OpenForms.Cast<Form>().Where(f => f.TopLevel).Count();
            if (count == 0)
            {
                Application.Exit();
                ExitThread();
            }
        }
    }

    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppConctext(new LoginForm()));
        }
    }
}
