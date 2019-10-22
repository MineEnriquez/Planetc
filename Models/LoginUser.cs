using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Planetc.Models
{
  public class LoginUser
  {
    [EmailAddress]
    [Required]
    [MinLength(5)]
    [Display(Name = "Email Address:")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [Display(Name = "Password:")]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string Password { get; set; }
  }
}
