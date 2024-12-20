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
    internal class AllStudents
    {

        public List<Student> Students = new List<Student>();

        public AllStudents()
        {
            Students = new List<Student>();
        }

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

        public void SaveTxtFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var student in Students)
                    sw.WriteLine(student);
            }
        }

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

        public void SaveBinFile(string fileName)
        {
            var binFormatter = new BinaryFormatter();
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, Students);
            }
        }

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

        public void SaveXmlFile(string fileName)
        {
            var xmlFormatter = new XmlSerializer(typeof(List<Student>));
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(file, Students);
            }
        }

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
