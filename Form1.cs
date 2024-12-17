using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace C_sharp_Lab7
{
    public partial class StudentsListForm : Form
    {
        public StudentsListForm()
        {
            InitializeComponent();
        }

        private int selectedNumber = -1;

        private AllStudents studentsList = new AllStudents();

        private string fileName = string.Empty;

        private enum FileType
        {
            None, Txt, Bin, Xml
        }

        private FileType fileType = FileType.None;
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Найти количество студенто на каждом курсе у которых нет задолженностей.", "Условие задачи", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxAllStudents.Items.Clear();
            studentsList = new AllStudents();
            var dlg = new OpenFileDialog();
            dlg.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                var extension = fileName.Substring(fileName.LastIndexOf('.'));
                switch (extension)
                {
                    case ".txt":
                        fileType = FileType.Txt;
                        studentsList.OpenTxtFile(fileName);
                        ShowList();
                        break;

                    case ".bin":
                        fileType = FileType.Bin;
                        studentsList.OpenBinFile(fileName);
                        ShowList();
                        break;

                    case ".xml":
                        fileType = FileType.Xml;
                        studentsList.OpenXmlFile(fileName);
                        ShowList();
                        break;
                }

            }
        }

        public void ShowList(int index = 0)
        {
            var cnt = listBoxAllStudents.Items.Count;
            for (int i = cnt - 1; i >= index; --i)
            {
                listBoxAllStudents.Items.RemoveAt(i);
            }
            for (var i = index; i < studentsList.Students.Count; ++i)
            {
                listBoxAllStudents.Items.Add((i + 1).ToString() + ". " + studentsList.Students[i].ToListBox());
            }
            listBoxAllStudents.Enabled = true;

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".txt";
            studentsList = new AllStudents();
            fileName = string.Empty;
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                fileName = dlg.FileName;
                StreamWriter f_out = new StreamWriter(fileName, false, Encoding.UTF8);
                f_out.Close();
                listBoxAllStudents.Items.Clear();
                MessageBox.Show("Файл успешно создан");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (fileType)
            {
                case FileType.None:
                    createToolStripMenuItem_Click(sender, e);
                    break;
                case FileType.Txt:
                    studentsList.SaveTxtFile(fileName);
                    MessageBox.Show($"Файл{fileName.Substring(fileName.LastIndexOf("//") + 1)} успешно сохранен");
                    break;
                case FileType.Bin:
                    studentsList.SaveBinFile(fileName);
                    MessageBox.Show($"Файл{fileName.Substring(fileName.LastIndexOf("//") + 1)} успешно сохранен");
                    break;
                case FileType.Xml:
                    studentsList.SaveXmlFile(fileName);
                    MessageBox.Show($"Файл{fileName.Substring(fileName.LastIndexOf("//") + 1)} успешно сохранен");
                    break;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                var extension = fileName.Substring(fileName.LastIndexOf('.'));
                switch (extension)
                {
                    case ".txt":
                        fileType = FileType.Txt;
                        studentsList.SaveTxtFile(fileName);
                        break;

                    case ".bin":
                        fileType = FileType.Bin;
                        studentsList.SaveBinFile(fileName);
                        break;

                    case ".xml":
                        fileType = FileType.Xml;
                        studentsList.SaveXmlFile(fileName);
                        break;

                    default:
                        break;

                }
            }
        }

        private void doTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBoxResult.Items.Clear();
            if (listBoxAllStudents.Items.Count < 1)
                MessageBox.Show("Список пуст!");
            else
            {
                var courseCount = studentsList.CountStudentsWithoutBadMarksOnEachCourse();
                if (courseCount.Count() == 0)
                {
                    MessageBox.Show("Нет неуспеющих студентов");
                }
                else
                {
                    for (var i = 0; i < courseCount.Count(); ++i)
                        listBoxResult.Items.Add((i + 1).ToString() + " course: " + courseCount[i]);
                    listBoxResult.Enabled = true;
                }
            }


        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new StudentForm();
            form.ShowDialog();
            if (form.Changed)
            {
                studentsList.Students.Add(StudentForm.student);
                selectedNumber = studentsList.Students.Count - 1;
                listBoxAllStudents.Items.Add((selectedNumber + 1).ToString() + ". " + studentsList.Students[selectedNumber].ToListBox());
            }
            if (listBoxResult.Items.Count > 0)
                doTaskToolStripMenuItem_Click(sender, e);
        }

        private void deleteStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = showDeleteDialog();
            if (s != String.Empty)
            {
                int index = int.Parse(s) - 1;
                if (index < listBoxAllStudents.Items.Count && index >= 0)
                {
                    studentsList.Students.RemoveAt(index);
                    ShowList(index);
                    if (listBoxResult.Items.Count > 0)
                        doTaskToolStripMenuItem_Click(sender, e);
                }
                else
                    MessageBox.Show("Недопустимый номер студента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string showDeleteDialog()
        {
            var form = new FormForDelete();
            return form.ShowDialog() == DialogResult.OK ? FormForDelete.text : "";
        }

        private void listBoxAllStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedNumber = listBoxAllStudents.SelectedIndex; 
            if(selectedNumber>=0)
            {
                var student= studentsList.Students[selectedNumber];
                var form=new StudentForm(student);  
                form.ShowDialog();  
                if(form.Changed)
                {
                    studentsList.Students[selectedNumber] = StudentForm.student;
                    ShowList(selectedNumber);
                }
                listBoxAllStudents.SelectedIndex = -1;
                if(listBoxResult.Items.Count > 0)
                    doTaskToolStripMenuItem_Click(sender, e); 
            }
        }
    }
}
