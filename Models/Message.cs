﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planetc.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [MinLength(4, ErrorMessage="Password must be 8 characters or longer!")]
        public string MessageContent { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId { get; set; }
        
        //navigation properties:
        public User MessageCreator { get; set; }
        public List<Like> ChildLikes { get; set; }
    }
}

