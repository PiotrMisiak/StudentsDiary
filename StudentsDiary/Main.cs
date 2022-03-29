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
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {        
        private FileHelper<List<Student>> _fileHelper = 
            new StudentsDiary.FileHelper<List<Student>>(Program.FilePath);

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();
            cbGroupSet.Text = "Wszystkie klasy";
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile().OrderBy(x => x.Id).ToList();
            dgvDiary.DataSource = students;
        }

        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[9].ReadOnly = true;
            dgvDiary.Columns[10].Visible = false;
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditstudent = new AddEditStudent();
            addEditstudent.FormClosing += AddEditstudent_FormClosing;
            addEditstudent.ShowDialog();
        }

        private void AddEditstudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();          
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prosze zaznacz ucznia, którego dane chcesz edytować.");
                return;
            }

            var addEditstudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditstudent.FormClosing += AddEditstudent_FormClosing;
            addEditstudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prosze zaznacz ucznia, którego chcesz usunąć.");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = 
                MessageBox.Show($"Czy na pewno chcesz usunąc ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}", 
                "Usuwanie ucznia", 
                MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));               
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile().OrderBy(x => x.Id).ToList();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();
            var selectedGroup = cbGroupSet.SelectedItem.ToString();
            
            if (selectedGroup == "Klasa 1A")
            
                dgvDiary.DataSource = students.FindAll(x => x.GroupId == "1A");

            else if (selectedGroup == "Klasa 1B")

                dgvDiary.DataSource = students.FindAll(x => x.GroupId == "1B");
           
            else if (selectedGroup == "Klasa 2A")

                dgvDiary.DataSource = students.FindAll(x => x.GroupId == "2A");
           
            else if (selectedGroup == "Klasa 2B")

                dgvDiary.DataSource = students.FindAll(x => x.GroupId == "2B");
            
            else


            RefreshDiary();
        }
    }
}
