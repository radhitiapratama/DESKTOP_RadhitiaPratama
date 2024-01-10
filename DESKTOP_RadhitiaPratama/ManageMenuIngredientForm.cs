using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DESKTOP_RadhitiaPratama
{
    public partial class ManageMenuIngredientForm : Form
    {
        EsemkaFoodcourtEntities db = new EsemkaFoodcourtEntities();

        List<MenuIngredient> deletedMenuIngredient = new List<MenuIngredient>();
        List<MenuIngredient> addedMenuIngredient = new List<MenuIngredient>();
        int selectedMenuID = 0;

        public ManageMenuIngredientForm()
        {
            InitializeComponent();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadMenu();
        }

        private void ManageMenuIngredientForm_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = decimal.MaxValue;
            loadMenu();
            disableInput();
            loadUnit();
            loadIngredient();
        }

        void loadMenu()
        {
            bindingSource1.Clear();
            var query = db.Menus.AsQueryable();
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                query = query.Where(f => f.Name.Contains(textBox1.Text));
            }

            bindingSource1.DataSource = query.ToList();
        }

        void loadIngredient()
        {
            var result = db.Ingredients.ToList();
            result.Insert(0, new Ingredient
            {
                ID = 0,
                Name = "Select Ingredients",
            });

            comboBox1.DataSource = result;
            comboBox1.SelectedValue = 0;

        }

        void loadUnit()
        {
            var result = db.Units.ToList();
            result.Insert(0, new Unit
            {
                ID = 0,
                Name = "Select Unit",
            });

            comboBox2.DataSource = result;
            comboBox2.SelectedValue = 0;
        }

        void disableInput()
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            numericUpDown1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            comboBox1.SelectedValue = 0;
            comboBox2.SelectedValue = 0;
            numericUpDown1.Value = 1;
        }

        void enableInput()
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            numericUpDown1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            comboBox1.SelectedValue = 0;
            comboBox2.SelectedValue = 0;
            numericUpDown1.Value = 1;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                if (e.ColumnIndex == ActionColumn.Index)
                {
                    e.Value = "Edit Ingredients";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is Menu menu)
            {
                selectedMenuID = menu.ID;

                disableInput();
                var data = db.MenuIngredients.AsNoTracking().Where(f => f.MenuID == menu.ID).ToList();
                bindingSource2.DataSource = data;

                if (e.ColumnIndex == ActionColumn.Index)
                {
                    enableInput();
                }
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < bindingSource2.Count && dataGridView2.Rows[e.RowIndex].DataBoundItem is MenuIngredient menuIng)
            {
                if (e.ColumnIndex == IngredientNameColumn.Index)
                {
                    var check = menuIng.Ingredient;
                    if (check == null)
                    {
                        e.Value = db.Ingredients.Where(f => f.ID == menuIng.IngredientID).First().Name;
                        return;
                    }

                    e.Value = menuIng.Ingredient.Name;
                }

                if (e.ColumnIndex == UnitColumn.Index)
                {
                    if (menuIng.Unit == null)
                    {
                        e.Value = db.Units.Where(f => f.ID == menuIng.UnitID).First().Name;
                        return;
                    }

                    e.Value = menuIng.Unit.Name;
                }

                if (e.ColumnIndex == ActionDetailIColumn.Index)
                {
                    e.Value = "Remove";
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dataGridView2.Rows[e.RowIndex].DataBoundItem is MenuIngredient menuIng)
            {
                if (e.ColumnIndex == ActionDetailIColumn.Index)
                {
                    if (addedMenuIngredient.Contains(menuIng))
                    {
                        addedMenuIngredient.Remove(menuIng);
                        bindingSource2.RemoveCurrent();
                    }
                    else
                    {
                        var dataMenu = db.MenuIngredients.Where(f => f.ID == menuIng.ID).First();
                        deletedMenuIngredient.Add(dataMenu);
                        bindingSource2.RemoveCurrent();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (deletedMenuIngredient.Count > 0)
            {
                db.MenuIngredients.RemoveRange(deletedMenuIngredient);
            }

            if (addedMenuIngredient.Count > 0)
            {
                db.MenuIngredients.AddRange(addedMenuIngredient);
            }

            db.SaveChanges();
            Alerts.Success("Menu ingredient successfully edited!");

            var data = db.MenuIngredients.Where(f => f.MenuID == selectedMenuID).ToList();
            bindingSource2.Clear();
            bindingSource2.DataSource = data;

            disableInput();
            deletedMenuIngredient.Clear();
            addedMenuIngredient.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!validate()) return;

            int ingredientID = (int)comboBox1.SelectedValue;
            int unitID = (int)comboBox2.SelectedValue;

            var check = db.MenuIngredients.Where(f => f.IngredientID == ingredientID && f.MenuID == selectedMenuID).FirstOrDefault();

            if (addedMenuIngredient.Count > 0)
            {
                var check2 = addedMenuIngredient.Where(f => f.IngredientID == ingredientID).FirstOrDefault();
                if (check2 != null)
                {
                    Alerts.Error("Ingredient sudah di gunakan!");
                    return;
                }
            }

            if (check != null)
            {
                Alerts.Error("ingredient sudah di gunakan!");
                return;
            }

            MenuIngredient newIng = new MenuIngredient()
            {
                MenuID = selectedMenuID,
                IngredientID = ingredientID,
                UnitID = unitID,
                Qty = (int)numericUpDown1.Value
            };

            addedMenuIngredient.Add(newIng);
            bindingSource2.Add(newIng);
        }

        bool validate()
        {
            if ((int)comboBox1.SelectedValue == 0 || (int)comboBox2.SelectedValue == 0 || numericUpDown1.Text == "")
            {
                Alerts.Error("All field must be filled!");
                return false;
            }

            if (numericUpDown1.Value <= 0)
            {
                Alerts.Error("Qty minimum is 1");
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            disableInput();
            bindingSource2.Clear();
            deletedMenuIngredient.Clear();
            addedMenuIngredient.Clear();
            selectedMenuID = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new AppConctext(new AdminMainForm());
            this.Close();
        }
    }
}
