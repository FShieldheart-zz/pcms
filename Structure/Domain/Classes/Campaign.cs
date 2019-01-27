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
        [NotNull]
        [Indexed]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column("campaign_start_date")]
        [NotNull]
        public DateTime? StartDate { get; set; }

        [Column("campaign_end_date")]
        [NotNull]
        public DateTime? EndDate { get; set; }

        [Column("campaign_is_active")]
        [NotNull]
        public bool? IsActive { get; set; }

        [Column("campaign_product_id")]
        [ForeignKey(typeof(Product))]
        [NotNull]
        public int? ProductId { get; set; }

        [ManyToOne]
        [NotNull]
        public Product Product { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Name))
            {
                stringBuilder.Append(@"N\A");
            }
            else
            {
                stringBuilder.Append(Name);
            }
            if (StartDate.HasValue)
            {
                stringBuilder.Append(", ");
                stringBuilder.Append(StartDate.Value.ToShortDateString());
            }
            if (EndDate.HasValue)
            {
                stringBuilder.Append(", ");
                stringBuilder.Append(EndDate.Value.ToShortDateString());
            }
            return stringBuilder.ToString();
        }
    }
}
