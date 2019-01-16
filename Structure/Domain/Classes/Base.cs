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
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Column("created_date")]
        [DataMember(Name = "created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        [DataMember(Name = "updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("is_deleted")]
        [DataMember(Name = "is_deleted")]
        public bool IsDeleted { get; set; }

        public Base()
        {
            Reset();
        }

        public void Reset()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}
