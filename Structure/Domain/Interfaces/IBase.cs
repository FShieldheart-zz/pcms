using System;

namespace Structure.Domain.Interfaces
{
    public interface IBase
    {
        DateTime? CreatedDate { get; set; }
        int Id { get; set; }
        bool? IsDeleted { get; set; }
        DateTime? UpdatedDate { get; set; }

        void Reset();
    }
}