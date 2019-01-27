using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace API.Model.Classes.View
{
    [DataContract]
    public class ProductViewModel : BaseViewModel
    {
        [MaxLength(50)]
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
