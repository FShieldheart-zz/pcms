using System.Runtime.Serialization;

namespace API.Model.Classes.View
{
    [DataContract]
    public class BaseViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
