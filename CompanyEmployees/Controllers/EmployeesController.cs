using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CompanyEmployees.ActionFilters;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesFromCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employees = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters,false);
            
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(employees.Metadata));
            
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            
            return Ok(employeesDto);
        }
        
        [HttpGet("{id}", Name = "GetEmployeeForCompany")]

        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompanyAsync(companyId,false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, false);
            if(employee == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }
        
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            
            _repository.Employee.CreateEmployeeForCompany(companyId,employeeEntity);
            await _repository.SaveAsync();

            var employeeDto = _mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("GetEmployeeForCompany", new {companyId, id = employeeDto.Id}, employeeDto);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyAttribute))]
        public async Task<IActionResult> DeleteEmployeeFromCompany(Guid companyId,Guid id)
        {
            var employee = HttpContext.Items["employee"] as Employee;
            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
            
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            var employeeEntity = HttpContext.Items["employee"];
            
            _mapper.Map(employee, employeeEntity);
            await _repository.SaveAsync();
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                _logger.LogError("Path Document sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            
            var employeeEntity = HttpContext.Items["employee"];

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            
            patchDocument.ApplyTo(employeeToPatch, ModelState);
            
            TryValidateModel(employeeToPatch);
            
            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            
            _mapper.Map(employeeToPatch, employeeEntity);
             await _repository.SaveAsync();

            return NoContent();
        }
        
    }
}