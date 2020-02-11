using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                    opt =>
                        opt.MapFrom(c => 
                            string.Join(", ",c.Country,c.Address)));
            
            CreateMap<Employee, EmployeeDto>();
        }
    }
}