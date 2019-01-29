using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace API.Model.Classes.View
{
    [DataContract]
    public class CampaignViewModel : BaseViewModel
    {
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "start_date")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "end_date")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "product")]
        public ProductViewModel Product { get; set; }

        [DataMember(Name = "is_active")]
        public bool IsActive { get; set; }
    }
}
