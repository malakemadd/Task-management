using System.ComponentModel.DataAnnotations;

namespace TaskManagment.Models
{
    public class Tasks
    {
        [Key]
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime TaskDate { get; set; }
        public bool Completed { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
