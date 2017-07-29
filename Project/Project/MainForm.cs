using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project.TblFaculty;

namespace Project {
    public partial class MainForm : Form {
        private List<Faculty> list; //the list of faculties shown on Datagridview
        private List<string> queries = new List<string>();  //list of queries to execute when click on Save button
        private bool save = true;   //false if any change needs saving, true if there is no change or user clicks on Save button

        public MainForm() {
            InitializeComponent();
        }

        private int AddIntoDataGridView() {
            var id = comboBox.Text;
            var name = txtName.Text;
            if (id.Equals("") || name.Equals("")) return -1;   //terminate method if id, name are empty (-1 means unaccepted value)
            if (list.Any(item => item.Id.Equals(id))) {                     //if the id is existed in the list
                MessageBox.Show("This id is already existed!", "Message",   //inform this id is already existed
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);          //
                return -1;                                                  //terminate method (-1 means unaccepted value)
            }                                                               //
            list.Add(new Faculty(id, name));    //add new faculty to list
            queries.Add(string.Format("INSERT INTO KHOA(MAKHOA,TENKHOA) VALUES ('{0}','{1}')",
                id, name));  //add a query to queries list
            save = false;   //show that there are changes that need saving
            return list.Count - 1;  //position of the added faculty in the list
        }

        private int DeleteFromDataGridView() {
            var id = comboBox.Text;
            if (id.Equals("")) return -1;  //terminate method if id is empty
            for (var i = 0 ; i < list.Count ; i++) {                                    //
                if (!list[i].Id.Equals(id)) continue;                                   //skip if not equal
                list.RemoveAt(i);                                                       //delete value from list if equal
                queries.Add(string.Format("DELETE FROM KHOA WHERE MAKHOA ='{0}'", id)); //add a query to queries list
                save = false;                                                           //show that there are changes that need saving
                return i;                                                               //position of the deleted faculty in the list
            }                                                                           //
            MessageBox.Show("This id is not existed!", "Message",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);    //show message if no id was found
            return -1;  //(-1 means unaccepted value)
        }

        private int UpdateFromDataGridView() {
            var id = comboBox.Text;
            var name = txtName.Text;
            if (id.Equals("") || name.Equals("")) return -1;   //terminate method if id, name are empty (-1 means unaccepted value)
            for (var i = 0 ; i < list.Count ; i++) {                                            //
                if (!list[i].Id.Equals(id)) continue;                                           //skip if not equal
                if (list[i].Name.Equals(name)) return -1;                                       //
                list[i].Name = name;                                                            //update value from list if equal
                queries.Add(string.Format("UPDATE KHOA SET TENKHOA='{0}' WHERE MAKHOA ='{1}'",  //
                    name, id));                                                                 //add command to queries
                save = false;                                                                   //
                return i;                                                                       //terminate method if id is existed
            }
            MessageBox.Show("This id is not existed!", "Message",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);    //show message if no id was found
            return -1; //(-1 means unaccepted value)
        }

        private void SaveChange() {
            if (save) {
                MessageBox.Show("Nothing was changed. Refreshing data.", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            var details = new FacultyDao().InsertDeleteUpdate(queries); //return errors string
            queries = new List<string>();   //reset queries list
            save = true;    //show that the queries have been executed 
            if (details.Equals("")) {
                MessageBox.Show("Saved successfully. Refreshing data.", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk); //show success message if no error occurs
            } else {
                MessageBox.Show("Something went wrong. Your data might have not been saved. Refreshing data.", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);  //show failed message if any errors occurs
            }
        }

        private void UpdateGridViewComboBoxListTxtSum() {
            var items = new object[list.Count];
            for (var i = 0 ; i < list.Count ; i++) {//
                items[i] = list[i].Id;              //set every Id value of the list into items
            }                                       //
            comboBox.Items.Clear(); //clear comboBox items
            comboBox.Items.AddRange(items); //add new array to comboBox
            txtSum.Text = list.Count.ToString();//set the number of faculty to Sum of faculty text box
            dataGridView.DataSource = new BindingSource(new BindingList<Faculty>(list), null);  //rebind list into Datagridview
        }

        private void InitDataGridView() {
            dataGridView.Columns[0].HeaderText = "Faculty Id";  //set header name for the 1st column
            dataGridView.Columns[1].HeaderText = "Faculty Name"; //set header name for the 2nd column
            dataGridView.Columns[0].Width = 50; //set the size of Name column
            dataGridView.Columns[2].Visible = false; //disable the 3rd column
        }

        private void btExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void comboBox_SelectionChangeCommitted(object sender, EventArgs e) {
            if (list.Count <= 0) return;//if the list is empty, terminate the method
            var pos = comboBox.SelectedIndex;//get selected index of Datagridview
            txtName.Text = list[pos].Name;  //set value to Faculty Name textbox  
            dataGridView.CurrentCell = dataGridView.Rows[pos].Cells[0]; //set selected row in Datagridview
        }

        private void btAdd_Click(object sender, EventArgs e) {
            var pos = AddIntoDataGridView();
            if (pos < 0) return;
            UpdateGridViewComboBoxListTxtSum();
            dataGridView.CurrentCell = dataGridView.Rows[pos].Cells[0]; //set selected row in Datagridview
        }

        private void btDelete_Click(object sender, EventArgs e) {
            var pos = DeleteFromDataGridView();
            if (pos >= 0) {
                UpdateGridViewComboBoxListTxtSum();
            }
            if (pos > 0) {
                dataGridView.CurrentCell = dataGridView.Rows[pos - 1].Cells[0]; //set selected row in Datagridview
            }
        }

        private void btUpdate_Click(object sender, EventArgs e) {
            var pos = UpdateFromDataGridView();
            if (pos < 0) return;
            UpdateGridViewComboBoxListTxtSum();
            dataGridView.CurrentCell = dataGridView.Rows[pos].Cells[0]; //set selected row in Datagridview
        }

        private void btSave_Click(object sender, EventArgs e) {
            SaveChange();
            list = new FacultyDao().GetAllData() ?? new List<Faculty>();//get new list from database, if null create new List
            var pos = -1;
            try {
                pos = dataGridView.CurrentCell.RowIndex;
            } catch (NullReferenceException) { } catch (ArgumentOutOfRangeException) { }
            UpdateGridViewComboBoxListTxtSum();
            try {
                dataGridView.CurrentCell = dataGridView.Rows[pos].Cells[0]; //set selected row in Datagridview
            } catch (NullReferenceException) { } catch (ArgumentOutOfRangeException) { }
        }

        private void btClear_Click(object sender, EventArgs e) {
            comboBox.Text = "";
            txtName.Text = "";
        }

        private void dataGridView_Click(object sender, EventArgs e) {
            if (dataGridView.CurrentCell == null) return;//terminate method if there is no value in datagridview
            var pos = dataGridView.CurrentCell.RowIndex;
            comboBox.Text = dataGridView.Rows[pos].Cells[0].Value.ToString();   //set value to Faculty Id text box  
            txtName.Text = dataGridView.Rows[pos].Cells[1].Value.ToString();//set value to Faculty Name text box
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (save) return;
            var res = MessageBox.Show("You haven't saved your changes yet! Save changes before closing?", "Confirm",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes) {              //confirm save change
                SaveChange();                           //before closing
            }
            if (res == DialogResult.Cancel) {
                e.Cancel = true;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            list = new FacultyDao().GetAllData();
            if (list == null) {
                list = new List<Faculty>();
                MessageBox.Show("Cannot connect to database.", "Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);  //null when cannot connect to the server, cannot find table, ...
            }
            UpdateGridViewComboBoxListTxtSum();
            InitDataGridView();
            if (list.Count <= 0) return;
            comboBox.Text = dataGridView.Rows[0].Cells[0].Value.ToString();
            txtName.Text = dataGridView.Rows[0].Cells[1].Value.ToString();
            tabManagement.Focus();
        }

        private void dataGridView_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Tab) return;
            if (dataGridView.CurrentCell == null) return;//terminate method if there is no value in datagrid view
            var pos = dataGridView.CurrentCell.RowIndex + 1;//position of the next element in datagridview
            try {
                dataGridView.CurrentCell = dataGridView.Rows[pos].Cells[0]; //set selected row in datagridview
            } catch (ArgumentOutOfRangeException) { } catch (NullReferenceException) { }
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Focus);  //full row tab
            e.Handled = true;                                                                   //focus border
        }

    }
}
