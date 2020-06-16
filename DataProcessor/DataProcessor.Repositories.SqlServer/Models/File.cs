using System;

namespace DataProcessor.Repositories.SqlServer.Models
{
    public partial class File
    {
        public long FileId { get; set; }
        public string Path { get; set; }
        public int FileStatusId { get; set; }
        public int ValidationResultId { get; set; }
        public string CustomData { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public int LastModifiedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User LastModifiedByNavigation { get; set; }
    }
}
