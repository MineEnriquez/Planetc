using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planetc.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [MinLength(4, ErrorMessage="Task must be 8 characters or longer!")]
        public string TaskContent { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId { get; set; }
        
        //navigation properties:
        public User TaskCreator { get; set; }
        public List<Like> ChildLikes { get; set; }
    }
}

