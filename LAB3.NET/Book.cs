using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.NET
{
    public abstract class Book : IReadable
    {
        private string title = string.Empty;
        private string author = string.Empty;
        private int pageCount;
        private string language = string.Empty;

        public string Title
        {
            get { return title; }
            
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Назва книги не може бути відсутньою.", nameof(value));
                }
                title = value.Trim();
            }
        }

        public string Author
        {
            get { return author; }
           
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Автор не може бути відсутнім.", nameof(value));
                }
                author = value.Trim();
            }
        }

        public int PageCount
        {
            get { return pageCount; }
           
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Кількість сторінок має бути більшою за нуль.", nameof(value));
                }
                pageCount = value;
            }
        }

        public string Language
        {
            get { return language; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Мова книги має бути вказаною.", nameof(value));
                }
                language = value.Trim();
            }
        }

        public Book(string title, string author, int pageCount, string language)
        {
            this.Title = title;
            this.Author = author;
            this.PageCount = pageCount;
            this.Language = language;
        }

        public abstract string Read();

        public virtual string AddToLibrary()
        {
            return $"Назва: {Title}\n" + 
                   $"Автор: {Author}\n" + 
                   $"Кількість сторінок: {PageCount}\n" + 
                   $"Мова: {Language}";
        }
    }
}
