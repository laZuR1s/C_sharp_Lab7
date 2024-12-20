using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_sharp_Lab7
{
    // Класс, представляющий экзамен
    [Serializable]
    public class Exam
    {
        // Название предмета
        public string Subject { get; set; }

        // Оценка за экзамен
        private byte _mark;
        public byte Mark
        {
            get
            {
                return _mark;
            }
            set
            {
                // Проверка, что оценка находится в диапазоне от 2 до 5
                if (value < 2 || value > 5)
                    throw new ArgumentOutOfRangeException("Оценка должна находиться в диапазоне от 2 до 5");
                _mark = value;
            }
        }

        // Конструктор по умолчанию
        public Exam()
        {
            Subject = string.Empty;
            _mark = 2;
        }

        // Конструктор с параметрами
        public Exam(string subject, byte mark)
        {
            Subject = subject;
            Mark = mark;
        }

        // Метод для проверки, является ли оценка неудовлетворительной
        public bool isBad()
        {
            return Mark == 2;
        }

        // Переопределение метода ToString для вывода информации об экзамене
        public override string ToString()
        {
            return $"{Subject}: {_mark}";
        }
    }

    // Класс, представляющий студента
    [Serializable]
    public class Student
    {
        // Перечисление форм обучения
        public enum EEducationForm { Budget, Contract }

        // ФИО студента
        public string FIO { get; set; }

        // Курс студента
        private byte _course;
        public byte Course
        {
            get { return _course; }
            set
            {
                // Проверка, что курс находится в диапазоне от 1 до 4
                if (value < 1 || value > 4)
                    throw new ArgumentOutOfRangeException("Несуществующий номер курса");
                _course = value;
            }
        }

        // Группа студента
        private byte _group;
        public byte Group
        {
            get { return _group; }
            set
            {
                // Проверка, что номер группы находится в диапазоне от 0 до 100
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Несуществующий номер группы");
                _group = value;
            }
        }

        // Массив экзаменов студента
        public Exam[] Exams { get; set; }

        // Форма обучения студента
        public EEducationForm EducationForm { get; set; }

        // Константы для количества экзаменов и сессий
        static public readonly byte CountExams = 5;
        static public readonly byte CountSessions = 8;

        // Конструктор по умолчанию
        public Student()
        {
            FIO = string.Empty;
            _group = 1;
            _course = 1;
            Exams = new Exam[CountExams * CountSessions];
            for (var i = 0; i < Exams.Length; ++i)
                Exams[i] = new Exam();
            EducationForm = EEducationForm.Contract;
        }

        // Конструктор с параметрами
        public Student(string fio, byte course, byte group, Exam[] exams, EEducationForm educationForm)
        {
            FIO = fio;
            _course = course;
            _group = group;
            Exams = exams;
            EducationForm = educationForm;
        }

        // Конструктор для чтения данных из файла
        public Student(StreamReader sr)
        {
            FIO = sr.ReadLine();
            Course = byte.Parse(sr.ReadLine());
            Group = byte.Parse(sr.ReadLine());

            int examNum = CountExams * _course * 2;
            Exams = new Exam[examNum];

            for (var i = 0; i < examNum; ++i)
            {
                var lines = sr.ReadLine().Split(':');
                var subject = lines[0];
                var mark = byte.Parse(lines[1].Trim());
                Exams[i] = new Exam(subject, mark);
            }
            if (sr.ReadLine().ToLower() == "бюджет")
            {
                EducationForm = EEducationForm.Budget;
            }
            else
            {
                EducationForm = EEducationForm.Contract;
            }
        }

        // Метод для вывода информации о студенте в ListBox
        public string ToListBox()
        {
            return $"{FIO}, {_course}-й курс, группа {_group}, {(EducationForm == EEducationForm.Budget ? "бюджет" : "договор")}";
        }

        // Переопределение метода ToString для вывода информации о студенте
        public override string ToString()
        {
            string result = $"{FIO}\n{_course}\n{_group}\n";
            int examNum = _course * CountExams * 2;
            for (var i = 0; i < examNum; ++i)
                result += Exams[i].ToString() + "\n";
            result += EducationForm == EEducationForm.Budget ? "Бюджет" : "Договор";
            return result;
        }
    }
}

