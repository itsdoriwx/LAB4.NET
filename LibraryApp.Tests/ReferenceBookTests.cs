using LibraryApp.NET;

namespace LibraryApp.Tests
{
    [TestClass]
    public class ReferenceBookTests
    {
        [TestMethod]
        public void ReferenceBook_Creation_ValidParameters_SetsPropertiesCorrectly()
        {
            string title = "Велика енциклопедія";
            string author = "Редколегія";
            int pages = 500;
            string language = "Українська";
            string refType = "Енциклопедія";

            var book = new ReferenceBook(title, author, pages, language, refType);

            Assert.AreEqual(title, book.Title);
            Assert.AreEqual(author, book.Author);
            Assert.AreEqual(pages, book.PageCount);
            Assert.AreEqual(language, book.Language);
            Assert.AreEqual(refType, book.ReferenceType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReferenceBook_Creation_EmptyReferenceType_ThrowsArgumentException()
        {
            new ReferenceBook("Словник", "Автор", 300, "Українська", "");
        }

        [TestMethod]
        public void ReferenceBook_Read_ReturnsCorrectMessage()
        {
            var book = new ReferenceBook("Атлас світу", "Географ", 120, "Українська", "Атлас");

            string result = book.Read();

            StringAssert.Contains(result, "Робота з довідковим виданням");
            StringAssert.Contains(result, "типу Атлас");
        }

        [TestMethod]
        public void ReferenceBook_ImplementsIReadableInterface()
        {
            var book = new ReferenceBook("Орфографічний словник", "Мовознавець", 400, "Українська", "Словник");

            Assert.IsTrue(book is IReadable);
        }

        [TestMethod]
        public void ReferenceBook_AddToLibrary_IncludesReferenceType()
        {
            var book = new ReferenceBook("Довідник школяра", "Педагог", 250, "Українська", "Довідник");

            string result = book.AddToLibrary();

            StringAssert.Contains(result, "Тип видання: Довідник");
        }
    }
}