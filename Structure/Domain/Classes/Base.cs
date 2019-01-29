using SQLite;
using Structure.Domain.Interfaces;
using System;
using System.Runtime.Serialization;

namespace Structure.Domain.Classes
{
    [DataContract]
    public abstract class Base : IBase
    {
        [PrimaryKey, AutoIncrement]
        [Column("p_id")]
        public int Id { get; set; }

        [Column("created_date")]
        [NotNull]
        public DateTime? CreatedDate { get; set; }

        [Column("updated_date")]
        [NotNull]
        public DateTime? UpdatedDate { get; set; }

        [Column("is_deleted")]
        [NotNull]
        public bool? IsDeleted { get; set; }

        public Base()
        {
            Reset();
        }

        public void Reset()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            IsDeleted = false;
        }
    }
}
