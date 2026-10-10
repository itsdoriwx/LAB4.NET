using LibraryApp.NET;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LAB3.NET
{
    public partial class MainWindow : Window
    {

        private List<Book> _books = new List<Book>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BookTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FictionFieldsPanel == null || ScientificFieldsPanel == null || ReferenceFieldsPanel == null)
                return;

            if (BookTypeComboBox.SelectedIndex == 0)
            {
                FictionFieldsPanel.Visibility = Visibility.Visible;
                ScientificFieldsPanel.Visibility = Visibility.Collapsed;
                ReferenceFieldsPanel.Visibility = Visibility.Collapsed;
            }
            else if (BookTypeComboBox.SelectedIndex == 1)
            {
                FictionFieldsPanel.Visibility = Visibility.Collapsed;
                ScientificFieldsPanel.Visibility = Visibility.Visible;
                ReferenceFieldsPanel.Visibility = Visibility.Collapsed;
            }
            else if (BookTypeComboBox.SelectedIndex == 2)   // додала для довідкової літератури
            {
                FictionFieldsPanel.Visibility = Visibility.Collapsed;
                ScientificFieldsPanel.Visibility = Visibility.Collapsed;
                ReferenceFieldsPanel.Visibility = Visibility.Visible;
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TitleTextBox.Text;
                string author = AuthorTextBox.Text;
                string language = LanguageTextBox.Text;

                if (string.IsNullOrWhiteSpace(title))
                {
                    MessageBox.Show("Будь ласка, введіть назву книги.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TitleTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(author))
                {
                    MessageBox.Show("Будь ласка, введіть автора книги.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AuthorTextBox.Focus();
                    return;
                }

                // перевірка сторінок на коректність введення
                if (!int.TryParse(PagesTextBox.Text, out int pages))
                {
                    MessageBox.Show("Будь ласка, введіть коректну кількість сторінок.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(language))
                {
                    MessageBox.Show("Будь ласка, введіть мову видання.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    LanguageTextBox.Focus();
                    return;
                }

                Book book;

                // об'єкт відповідно до вибору в ComboBox
                if (BookTypeComboBox.SelectedIndex == 0)
                {
                    string genre = GenreTextBox.Text.Trim();

                    if (string.IsNullOrWhiteSpace(genre))
                    {
                        MessageBox.Show("Будь ласка, вкажіть жанр художнього твору.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                        GenreTextBox.Focus();
                        return;
                    }

                    if (!int.TryParse(EditionCountTextBox.Text, out int editionsCount))
                    {
                        MessageBox.Show("Введіть коректну кількість перевидань.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    book = new FictionBook(title, author, pages, language, genre, editionsCount);
                }
                else if (BookTypeComboBox.SelectedIndex == 1)
                {

                    string topic = FieldOfStudyTextBox.Text.Trim();

                    if (string.IsNullOrWhiteSpace(topic))
                    {
                        MessageBox.Show("Будь ласка, вкажіть наукову галузь.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                        FieldOfStudyTextBox.Focus();
                        return;
                    }

                    if (!int.TryParse(CitationsTextBox.Text, out int citationsCount))
                    {
                        MessageBox.Show("Введіть коректну кількість цитувань.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    book = new ScientificBook(title, author, pages, language, topic, citationsCount);
                }
                else
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)ReferenceTypeComboBox.SelectedItem;
                    string referenceType = selectedItem.Content.ToString();

                    book = new ReferenceBook(title, author, pages, language, referenceType);
                }

                _books.Add(book);

                string bookInfo = book.AddToLibrary();
                BooksListBox.Items.Add(bookInfo);

                // фіксуємо в журналі
                LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Додано наукову книгу: «{book.Title}»\n");
                LogTextBox.ScrollToEnd();

                UpdateBookCounts();

                TitleTextBox.Clear();
                AuthorTextBox.Clear();
                PagesTextBox.Clear();
                LanguageTextBox.Clear();
                GenreTextBox.Clear();
                EditionCountTextBox.Clear();
                FieldOfStudyTextBox.Clear();
                CitationsTextBox.Clear();

                ReferenceTypeComboBox.SelectedIndex = 0;
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReadBookButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = BooksListBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < _books.Count)
            {
                Book selectedBook = _books[selectedIndex];

                IReadable readable = selectedBook;  // виклик методу через IReadable
                string readMessage = readable.Read();

                string ageInfo = "";
                if (selectedBook is IAgeRestricted restricted)  // реалізація IAgeRestricted
                {
                    ageInfo = $" Вікове обмеження: {restricted.GetMinimumAge()}+.";
                }

                string borrowInfo;
                if (selectedBook is IBorrowable borrowable) // IBorrowable, поліморфізм
                {
                    borrowInfo = $"Можна взяти додому на {borrowable.GetMaxBorrowDays()} днів.";
                }
                else
                {
                    borrowInfo = "Тільки для читального залу.";
                }

                LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {readMessage} [{borrowInfo}]{ageInfo}\n");
                LogTextBox.ScrollToEnd();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть книгу зі списку.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = BooksListBox.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < _books.Count)
            {
                Book bookToDelete = _books[selectedIndex];

                // зі списку
                _books.RemoveAt(selectedIndex);

                // зі списку на екрані
                BooksListBox.Items.RemoveAt(selectedIndex);

                LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Видалено книгу: «{bookToDelete.Title}»\n");
                LogTextBox.ScrollToEnd();

                UpdateBookCounts();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть книгу для видалення зі списку.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateBookCounts()
        {
            int fictionCount = 0;
            int scientificCount = 0;
            int referenceCount = 0;

            foreach (Book b in _books)
            {
                if (b is FictionBook)
                {
                    fictionCount++;
                }
                else if (b is ScientificBook)
                {
                    scientificCount++;
                }
                else if (b is ReferenceBook)
                {
                    referenceCount++;
                }
            }

            TotalBooksCountText.Text = _books.Count.ToString();
            FictionCountText.Text = fictionCount.ToString();
            ScientificCountText.Text = scientificCount.ToString();
            ReferenceCountText.Text = referenceCount.ToString();
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Clear();
        }

        private void CalculatePagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (_books.Count == 0)
            {
                MessageBox.Show("У бібліотеці наразі немає жодної книги для аналізу.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                TotalPagesResultText.Text = "книги відсутні";
                return;
            }

            try
            {
                string targetLanguage = FilterLanguageTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(targetLanguage))
                {
                    MessageBox.Show("Будь ласка, введіть мову для пошуку.", "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FilterLanguageTextBox.Focus();
                    return;
                }

                int totalPages = LibraryAnalysis.GetTotalPagesByLanguage(_books, targetLanguage);
                TotalPagesResultText.Text = totalPages.ToString();

                LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Підраховано сторінки для мови «{targetLanguage}»: {totalPages} стор.\n");
                LogTextBox.ScrollToEnd();
            }

            catch (ArgumentException ex) // не ввели мову
            {
                MessageBox.Show(ex.Message, "Помилка аналізу", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час підрахунку: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateCitationIndexButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedIndex = BooksListBox.SelectedIndex;

                if (selectedIndex < 0 || selectedIndex >= _books.Count)
                {
                    MessageBox.Show("Будь ласка, оберіть наукову книгу в каталозі.", "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Book selectedBook = _books[selectedIndex];

                if (selectedBook is not ScientificBook scientificBook)
                {
                    throw new InvalidOperationException("Розрахунок індексу цитування можливий лише для наукової літератури.");
                }

                double index = scientificBook.CalculateCitationIndex();
                CitationIndexResultText.Text = $"{index} цит./100 стор.";

                LogTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] Розраховано індекс цитування для «{scientificBook.Title}»: {index} цит./100 стор.\n");
                LogTextBox.ScrollToEnd();
            }
            catch (InvalidOperationException ex)
            {
                CitationIndexResultText.Text = "недоступно";
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
