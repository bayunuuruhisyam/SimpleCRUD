using System.ComponentModel.DataAnnotations;

namespace SimpleCRUD.ViewModels
{
    public class EmployeeViewModel
    {
            public int EmployeeId { get; set; }


            public string EmployeeIdView { get
                {
                return "EMP" + EmployeeId.ToString("D4");
            }
        }

        [Required] public string FullName { get; set; }
            [Required] public string Department { get; set; }
            [Required] public DateTime DateOfBirth { get; set; }
            [Required] public int Age { get; set; }
            [Required] public string PhoneNumber { get; set; }
       public bool IsActive { get; set; } = true;


    }
}
