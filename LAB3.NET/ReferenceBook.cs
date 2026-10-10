using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace LibraryApp.NET
{
    public class ReferenceBook : Book
    {
        public string ReferenceType { get; set; } 

        public ReferenceBook(string title, string author, int pageCount, string language, string referenceType) : base(title, author, pageCount, language)
        {
            if (string.IsNullOrWhiteSpace(referenceType)) 
                throw new ArgumentException("Тип довідкового видання не може бути порожнім.");

            ReferenceType = referenceType;
        }

        public override string Read()
        {
            return $"Робота з довідковим виданням «{Title}» типу {ReferenceType}.";
        }

        public override string AddToLibrary()
        {
            return base.AddToLibrary() + $"\nТип видання: {ReferenceType}";
        }
    }
}
