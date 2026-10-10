using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.NET
{
    public class ScientificBook : Book, IBorrowable
    {
        private string topic = string.Empty;
        private int citationsCount;

        public string Topic
        {
            get { return topic; }
           
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Тематика наукової книги має бути вказана.", nameof(value));
                }
                topic = value.Trim();
            }
        }

        public int CitationsCount
        {
            get { return citationsCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Кількість цитувань не може бути від'ємною.", nameof(value));
                }
                citationsCount = value;
            }
        }

        public ScientificBook(string title, string author, int pageCount, string language, string topic, int citationsCount) : base(title, author, pageCount, language)
        {
            this.Topic = topic;
            this.CitationsCount = citationsCount;
        }

        public double CalculateCitationIndex()
        {
            double index = ((double)CitationsCount / PageCount) * 100.0;  // к - сть цитувань на 100 сторінок
            return Math.Round(index, 2);
        }

        public override string Read()
        {
            return $"Опрацювання наукової книги «{Title}» галузі {Topic}.";
        }

        public override string AddToLibrary()
        {
            return $"{base.AddToLibrary()}\n" +
                   $"Тип: Наукова література\n" +
                   $"Тематика: {Topic}\n" +
                   $"Кількість цитувань: {CitationsCount}";
        }

        public int GetMaxBorrowDays() // реаліз IBorrowable
        {
            return 14;
        }
    }
}
