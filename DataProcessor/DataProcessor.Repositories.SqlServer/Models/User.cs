using System.Collections.Generic;

namespace DataProcessor.Repositories.SqlServer.Models
{
    public partial class User
    {
        public User()
        {
            FileCreatedByNavigation = new HashSet<File>();
            FileLastModifiedByNavigation = new HashSet<File>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<File> FileCreatedByNavigation { get; set; }
        public virtual ICollection<File> FileLastModifiedByNavigation { get; set; }
    }
}
