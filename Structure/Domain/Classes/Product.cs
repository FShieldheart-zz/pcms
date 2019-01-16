using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Structure.Domain.Classes
{
    [DataContract]
    [Table("pcms_product")]
    public class Product : Base
    {
        [Column("product_name")]
        [Indexed]
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public IList<Campaign> Campaigns { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Name);
            return stringBuilder.ToString();
        }
    }
}
