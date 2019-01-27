using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace API.Model.Classes.Persistence
{
    [DataContract]
    public class ProductPersistenceModel
    {
        [Required]
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
