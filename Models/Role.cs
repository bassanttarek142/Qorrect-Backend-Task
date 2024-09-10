using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qorrect_Backend_Task.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } 
    }
}
