using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_sharp_Lab7
{
    public partial class FormForDelete : Form
    {
        // Текст, введенный пользователем
        public static string text { get; set; }

        // Конструктор формы
        public FormForDelete()
        {
            InitializeComponent();
            AcceptButton = button1;
        }

        // Обработчик события KeyPress для textBox1, чтобы разрешить ввод только цифр
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (number != 8 && !char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        // Обработчик события нажатия на кнопку "OK"
        private void button1_Click(object sender, EventArgs e)
        {
            FormForDelete.text = textBox1.Text; 
            DialogResult=DialogResult.OK;
            this.Close();
        }
    }
}
