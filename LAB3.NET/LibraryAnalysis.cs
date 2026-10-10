using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.NET
{
    public static class LibraryAnalysis
    {
        public static int GetTotalPagesByLanguage(List<Book> books, string language)
        {
            if (books == null)
            {
                throw new ArgumentNullException(nameof(books), "Жодної книги не було додано.");
            }

            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentException("Мову пошуку не вказано.", nameof(language));
            }

            string targetLanguage = language.Trim();
            int totalPages = 0;

            foreach (Book book in books)
            {
                if (book != null && string.Equals(book.Language, targetLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    totalPages += book.PageCount;
                }
            }

            return totalPages;
        }
    }
}