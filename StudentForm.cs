using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace C_sharp_Lab7
{
    // Форма для добавления и редактирования информации о студенте
    public partial class StudentForm : Form
    {
        // Статическое свойство для хранения текущего студента
        public static Student student { get; set; }

        // Поле для отслеживания изменений
        private bool changed = false;

        // Свойство для получения значения поля changed
        public bool Changed { get { return changed; } }

        // Поле для определения режима редактирования
        public bool isEdit;

        // Конструктор по умолчанию
        public StudentForm()
        {
            InitializeComponent();
            this.Select();
            StudentForm.student = new Student();
            this.isEdit = false;
        }

        // Конструктор для режима редактирования
        public StudentForm(Student student)
        {
            InitializeComponent();
            this.Select();
            StudentForm.student = student;
            this.isEdit = true;
        }

        // Обработчик события KeyPress для textBoxGroup, чтобы разрешить ввод только цифр
        private void textBoxGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (number != 8 && !char.IsDigit(number))
                e.Handled = true;
        }

        // Обработчик события загрузки формы
        private void StudentForm_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (isEdit)
            {
                OpenForEditing();
            }
        }

        // Метод для открытия формы в режиме редактирования
        private void OpenForEditing()
        {
            textBoxFIO.Text = student.FIO;
            textBoxGroup.Text = student.Group.ToString();
            switch (student.Course)
            {
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 3:
                    radioButton3.Checked = true;
                    break;
                case 4:
                    radioButton4.Checked = true;
                    break;
            }
            if (student.EducationForm == Student.EEducationForm.Budget)
                radioButtonBudget.Checked = true;
            else
                radioButtonContract.Checked = true;
            int examNum = student.Course * 2 * Student.CountExams;
            for (int i = 0; i < examNum; ++i)
            {
                dataGridView1.Rows[i].Cells[2].Value = student.Exams[i].Subject;
                dataGridView1.Rows[i].Cells[3].Value = student.Exams[i].Mark.ToString();
            }
        }

        // Метод для сохранения информации о студенте
        private bool SaveStudent()
        {
            student.FIO = textBoxFIO.Text;
            var group = textBoxGroup.Text;
            if (group[0] == '0')
            {
                MessageBox.Show("Номер группы является положительным числом", "Ошибка в номере группы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            student.Group = byte.Parse(group);

            if (radioButton1.Checked)
                student.Course = 1;
            else if (radioButton2.Checked)
                student.Course = 2;
            else if (radioButton3.Checked)
                student.Course = 3;
            else if (radioButton4.Checked)
                student.Course = 4;

            if (radioButtonBudget.Checked)
                student.EducationForm = Student.EEducationForm.Budget;
            else
                student.EducationForm = Student.EEducationForm.Contract;

            int examNum = student.Course * Student.CountExams * 2;
            student.Exams = new Exam[examNum];
            for (int i = 0; i < examNum; i++)
            {
                student.Exams[i] = new Exam();
                string s = dataGridView1.Rows[i].Cells[2].Value.ToString();
                if (s == string.Empty)
                {
                    MessageBox.Show("Введите название экзамена", "Ошибка в DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                student.Exams[i].Subject = s;

                s = dataGridView1.Rows[i].Cells[3].Value.ToString();
                if (s == string.Empty || !byte.TryParse(s, out byte mark) || mark < 2 || mark > 5)
                {
                    MessageBox.Show("Введите оценку в диапазоне от 2 до 5", "Ошибка в DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                student.Exams[i].Mark = mark;
            }
            return true;
        }

        // Обработчик события нажатия кнопки "Отмена"
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Обработчик события нажатия кнопки "Сохранить"
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (SaveStudent())
            {
                changed = true;
                this.Close();
            }
        }

        // Обработчик события изменения состояния RadioButton
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            int course = Convert.ToInt32(((RadioButton)sender).Text);

            int examNum = course * Student.CountExams * 2;
            for (int i = dataGridView1.Rows.Count; i < examNum; i++)
            {
                dataGridView1.Rows.Add();
                if (i % 10 == 0)
                    dataGridView1.Rows[i].Cells[0].Value = (i / 10 + 1).ToString();
                if (i % 5 == 0)
                    dataGridView1.Rows[i].Cells[1].Value = (i / 5 + 1).ToString();
            }
        }
    }
}

