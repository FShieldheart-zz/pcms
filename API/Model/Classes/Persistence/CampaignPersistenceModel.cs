using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace API.Model.Classes.Persistence
{
    [DataContract]
    public class CampaignPersistenceModel
    {
        [Required]
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [DataMember(Name = "product_id")]
        public int? ProductId { get; set; }

        [Required]
        [DataMember(Name = "is_active")]
        public bool? IsActive { get; set; }
    }
}
