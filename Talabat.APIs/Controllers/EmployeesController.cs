using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.Employee_Specifications;

namespace Talabat.APIs.Controllers
{
    public class EmployeesController : ApiBaseController
    {
        private readonly IGenericRepository<Employee> employeeRepository;

        public EmployeesController(IGenericRepository<Employee> _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }


        [HttpGet] // GET : api/employee
        public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecifications();

            var employees = await employeeRepository.GetAllWithSpecAsync(spec);

            if (employees is null)
                return NotFound();

            return Ok(employees);
        }


        [HttpGet("{id:int}")] // GET : api/employee/5
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            EmployeeWithDepartmentSpecifications spec = new EmployeeWithDepartmentSpecifications();

            Employee employee = await employeeRepository.GetEntityWithSpecAsync(spec);

            if (employee is null)
                return NotFound();

            return Ok(employee);
        }
    }
}
