using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.NET
{
    public class FictionBook : Book, IBorrowable, IAgeRestricted
    {
        private string genre = string.Empty;
        private int editionsCount;

        public string Genre
        {
            get { return genre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Жанр художньої книги має бути вказаний.", nameof(value));
                }
                genre = value.Trim();
            }
        }

        public int EditionsCount
        {
            get { return editionsCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Кількість перевидань не може бути від'ємною.", nameof(value));
                }
                editionsCount = value;
            }
        }

        public FictionBook(string title, string author, int pageCount, string language, string genre, int editionsCount) : base(title, author, pageCount, language)
        {
            this.Genre = genre;
            this.EditionsCount = editionsCount;
        }

        public int GetEditionsCount()
        {
            return EditionsCount;
        }

        public override string Read()
        {
            return $"Читання художнього твору «{Title}» у жанрі {Genre}.";
        }

        public override string AddToLibrary()
        {
            return $"{base.AddToLibrary()}\n" +
                   $"Тип: Художня література\n" +
                   $"Жанр: {Genre}\n" +
                   $"Кількість перевидань: {EditionsCount}";
        }

        public int GetMaxBorrowDays() // реаліз IBorrowable
        {
            return 30;
        }

        public int GetMinimumAge()  // IAgeRestricted
        {
            return 16;
        }
    }
}
