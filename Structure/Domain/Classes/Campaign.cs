using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace Structure.Domain.Classes
{
    [DataContract]
    [Table("pcms_campaign")]
    public class Campaign : Base
    {
        [Column("campaign_name")]
        [Indexed]
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Column("campaign_start_date")]
        [DataMember(Name = "start_date")]
        public DateTime StartDate { get; set; }

        [Column("campaign_end_date")]
        [DataMember(Name = "end_date")]
        public DateTime EndDate { get; set; }

        [Column("campaign_is_active")]
        [DataMember(Name = "is_active")]
        public bool IsActive { get; set; }

        [ManyToOne]
        [DataMember(Name = "product")]
        public Product Product { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Name);
            stringBuilder.Append(", ");
            stringBuilder.Append(StartDate.ToShortDateString());
            stringBuilder.Append(", ");
            stringBuilder.Append(EndDate.ToShortDateString());
            return stringBuilder.ToString();
        }
    }
}
