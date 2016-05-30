using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using IronMan.Demo.Resources;

namespace IronMan.Demo.Models
{
    public class AccountModel
    {
        [Display(ResourceType = typeof(Tips), Name = "LoginNameDisplay")]
        [Required(ErrorMessageResourceType = typeof(Tips), ErrorMessageResourceName = "LoginNameRequired")]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(Tips), Name = "PasswordDisplay")]
        [Required(ErrorMessageResourceType = typeof(Tips), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
