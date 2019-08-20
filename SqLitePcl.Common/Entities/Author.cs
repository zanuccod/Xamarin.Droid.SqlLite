using System;

namespace SqLitePcl.Common.Entities
{
    public class Author
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string BornDate { get; set; }

        public string Country { get; set; }

        public bool Equals(Author item)
        {
            var result = true;
            result &= (Id == item.Id);
            result &= (Name.Equals(item.Name));
            result &= (Surname.Equals(item.Surname));
            result &= (BornDate.Equals(item.BornDate));
            result &= (Country.Equals(item.Country));
            return result;
        }
    }
}
