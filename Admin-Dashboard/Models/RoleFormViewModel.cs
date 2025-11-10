using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin_Dashboard.Models
{
    public class RoleFormViewModel
    {
        //public string Id { get; set; }


        [Required(ErrorMessage = "Name is Required!")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters!")]
        [DisplayName("Role Name")]
        public string Name { get; set; }

        //public bool IsSelected { get; set; }
    }
}
