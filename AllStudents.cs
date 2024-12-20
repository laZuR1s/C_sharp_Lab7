using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace C_sharp_Lab7
{
    // Класс, представляющий список всех студентов
    internal class AllStudents
    {
        // Список студентов
        public List<Student> Students = new List<Student>();

        // Конструктор по умолчанию
        public AllStudents()
        {
            Students = new List<Student>();
        }

        // Метод для открытия текстового файла и чтения данных о студентах
        public void OpenTxtFile(string fileName)
        {
            var sr = new StreamReader(fileName);
            try
            {
                while (!sr.EndOfStream)
                {
                    var student = new Student(sr);
                    Students.Add(student);
                }
            }
            catch (FormatException ex)
            {
                Students.Clear();
                MessageBox.Show("Числа в файле должны быть положительными цифрами", "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Students.Clear();
                MessageBox.Show(ex.Message, "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                Students.Clear();
                MessageBox.Show("Номер курса не соответствует количеству сессий", "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Students.Clear();
                MessageBox.Show(ex.Message, "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sr.Close();
            }
        }

        // Метод для сохранения данных о студентах в текстовый файл
        public void SaveTxtFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var student in Students)
                    sw.WriteLine(student);
            }
        }

        // Метод для открытия бинарного файла и чтения данных о студентах
        public void OpenBinFile(string fileName)
        {
            var binFormatter = new BinaryFormatter();
            var file = new FileStream(fileName, FileMode.OpenOrCreate);
            try
            {
                Students = binFormatter.Deserialize(file) as List<Student>;
            }
            catch (Exception ex)
            {
                Students.Clear();
                MessageBox.Show(ex.Message, "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                file.Close();
            }
        }

        // Метод для сохранения данных о студентах в бинарный файл
        public void SaveBinFile(string fileName)
        {
            var binFormatter = new BinaryFormatter();
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, Students);
            }
        }

        // Метод для открытия XML файла и чтения данных о студентах
        public void OpenXmlFile(string fileName)
        {
            var xmlFormatter = new XmlSerializer(typeof(List<Student>));
            var file = new FileStream(fileName, FileMode.OpenOrCreate);
            try
            {
                Students = xmlFormatter.Deserialize(file) as List<Student>;
            }
            catch (Exception ex)
            {
                Students.Clear();
                MessageBox.Show(ex.Message, "Некорректный файл!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                file.Close();
            }
        }

        // Метод для сохранения данных о студентах в XML файл
        public void SaveXmlFile(string fileName)
        {
            var xmlFormatter = new XmlSerializer(typeof(List<Student>));
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(file, Students);
            }
        }

        // Метод для подсчета количества студентов без задолженностей на каждом курсе, отдельно для бюджета и договора
        public (int[] budgetCount, int[] contractCount) CountStudentsWithoutBadMarksOnEachCourse()
        {
            int[] budgetCount = new int[4];
            int[] contractCount = new int[4];
            for (int i = 0; i < 4; i++)
            {
                budgetCount[i] = 0;
                contractCount[i] = 0;
            }

            foreach (var student in Students)
            {
                bool isBad = false;
                foreach (var exam in student.Exams)
                {
                    if (exam.isBad())
                    {
                        isBad = true;
                        break;
                    }
                }

                if (!isBad)
                {
                    if (student.EducationForm == Student.EEducationForm.Budget)
                    {
                        budgetCount[student.Course - 1]++;
                    }
                    else if (student.EducationForm == Student.EEducationForm.Contract)
                    {
                        contractCount[student.Course - 1]++;
                    }
                }
            }

            return (budgetCount, contractCount);
        }
    }
}

