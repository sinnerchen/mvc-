using System.ComponentModel.DataAnnotations;
using IronMan.Demo.Resources;

namespace IronMan.Demo.Models
{
    public class StudentNewModel
    {
        [Display(ResourceType = typeof(Student), Name = "NameDisplay")]
        [Required(ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "NameRequired")]
        [RegularExpression(@"^[\u3000-\u9FA5\x20]{2,10}$", ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "NameRegex")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Student), Name = "SexDisplay")]
        [Required(ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "SexRequired")]
        [RegularExpression(@"^[FMO]$", ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "SexRegex")]
        public string Sex { get; set; }

        [Display(ResourceType = typeof(Student), Name = "BornDisplay")]
        [RegularExpression(@"^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$", ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "BornRegex")]
        public string Born { get; set; }

        [Display(ResourceType = typeof(Student), Name = "AddressDisplay")]
        [StringLength(200, ErrorMessageResourceType = typeof(Student), ErrorMessageResourceName = "AddressLength")]
        public string Address { get; set; }
    }
}
