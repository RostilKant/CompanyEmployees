using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is required field")]
        [MaxLength(30,ErrorMessage = "Maximum Length for the Name is 30 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Address is required field")]
        [MaxLength(30,ErrorMessage = "Maximum Length for the Address is 30 characters")]
        public string Address { get; set; }
        
        public string Country { get; set; }
        
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }

    }
}