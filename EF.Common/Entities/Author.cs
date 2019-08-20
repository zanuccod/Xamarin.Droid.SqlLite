using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace EF.Common.Entities
{
    public class Author
    {
        [JsonProperty("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("born_date")]
        public string BornDate { get; set; }

        [JsonProperty("country")]
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
