using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryApp.NET;

namespace LibraryApp.Tests
{
    [TestClass]
    public class LibraryTests
    {
        [TestMethod] // перевірка коректності створення художньої книги та отримання кількості видань
        public void FictionBook_CreationAndGetEditionsCount_ReturnsCorrectValues()
        {
            var book = new FictionBook("Тіні забутих предків", "Михайло Коцюбинський", 180, "Українська", "Повість", 3);

            Assert.AreEqual("Тіні забутих предків", book.Title);
            Assert.AreEqual("Михайло Коцюбинський", book.Author);
            Assert.AreEqual(180, book.PageCount);
            Assert.AreEqual("Українська", book.Language);
            Assert.AreEqual("Повість", book.Genre);
            Assert.AreEqual(3, book.GetEditionsCount());
        }

        [TestMethod] // перевірка формування тексту методу читання художнього твору
        public void FictionBook_Read_ReturnsValidString()
        {
            var book = new FictionBook("Місто", "Валер'ян Підмогильний", 333, "Українська", "Урбаністичний роман", 5);

            string result = book.Read();

            Assert.IsTrue(result.Contains("Місто"));
            Assert.IsTrue(result.Contains("Урбаністичний роман"));
        }

        [TestMethod] // перевірка викидання винятку при відсутності або порожній назві книги
        [ExpectedException(typeof(ArgumentException))]
        public void Book_EmptyTitle_ThrowsArgumentException()
        {
            new FictionBook("", "Автор", 200, "Українська", "Жанр", 1);
        }

        [TestMethod] // перевірка викидання винятку при відсутності автора книги
        [ExpectedException(typeof(ArgumentException))]
        public void Book_EmptyAuthor_ThrowsArgumentException()
        {
            new FictionBook("Назва книги", "   ", 200, "Українська", "Жанр", 1);
        }

        [TestMethod] // перевірка викидання винятку при некоректній кількості сторінок 
        [ExpectedException(typeof(ArgumentException))]
        public void Book_InvalidPageCount_ThrowsArgumentException()
        {
            new FictionBook("Назва", "Автор", 0, "Українська", "Жанр", 1);
        }

        [TestMethod] // перевірка точності математичного розрахунку індексу цитування наукової книги
        public void ScientificBook_CalculateCitationIndex_CalculatesCorrectly()
        {
            var book = new ScientificBook("Чисельні методи", "О. В. Сидоренко", 200, "Українська", "Математика", 50);

            double index = book.CalculateCitationIndex();

            Assert.AreEqual(25.0, index, 0.001);
        }

        [TestMethod] // перевірка формування тексту методу читання для наукової книги
        public void ScientificBook_Read_ReturnsValidString()
        {
            var book = new ScientificBook("Алгоритми та структури даних", "Кнут", 650, "Англійська", "CS", 120);

            string result = book.Read();

            Assert.IsTrue(result.Contains("Алгоритми та структури даних"));
            Assert.IsTrue(result.Contains("CS"));
        }

        [TestMethod] // перевірка підрахунку сумарної кількості сторінок книг для вказаної мови
        public void LibraryAnalysis_GetTotalPagesByLanguage_ReturnsCorrectSum()
        {
            var books = new List<Book>
            {
                new FictionBook("Книга 1", "Автор 1", 150, "Українська", "Драма", 1),
                new ScientificBook("Книга 2", "Автор 2", 250, "Українська", "Фізика", 10),
                new FictionBook("Book 3", "Author 3", 100, "English", "Novel", 2)
            };

            int totalPagesUk = LibraryAnalysis.GetTotalPagesByLanguage(books, "Українська");

            Assert.AreEqual(400, totalPagesUk);
        }

        [TestMethod] // перевірка нечутливості фільтрації за мовою до регістру літер
        public void LibraryAnalysis_GetTotalPagesByLanguage_CaseInsensitive()
        {
            var books = new List<Book>
            {
                new FictionBook("Книга 1", "Автор 1", 120, "Українська", "Детектив", 1)
            };

            int totalPages = LibraryAnalysis.GetTotalPagesByLanguage(books, "українська");

            Assert.AreEqual(120, totalPages);
        }

        [TestMethod] // перевірка викидання винятку при передачі null замість списку книг
        [ExpectedException(typeof(ArgumentNullException))]
        public void LibraryAnalysis_NullList_ThrowsArgumentNullException()
        {
            LibraryAnalysis.GetTotalPagesByLanguage(null, "Українська");
        }
    }
}