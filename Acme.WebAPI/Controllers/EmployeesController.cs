using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Acme.Business.Managers;
using Acme.Core.Repository;
using Acme.WebCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EFCore.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : BaseController
    {
        private readonly EmployeeManager _manager;

        public EmployeesController(IUnitOfWork unitOfWork, ILogger<EmployeesController> logger)
            : base(unitOfWork, logger)
        {
            _manager = new EmployeeManager(UnitOfWork);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeModel>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return ExecuteHandledOperation<IActionResult>(() => {
                var employees = _manager.Get().AsQueryable().ProjectTo<Acme.WebCore.Models.EmployeeModel>();
                return Ok(employees);
            });
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        public IActionResult Get(long id)
        {
            return ExecuteHandledOperation<IActionResult>(() => {
                var employee = _manager.GetSingle(e => e.EmployeeId.Equals(id));
                if (employee == null)
                {
                    return NotFound("The Employee record couldn't be found.");
                }
                return Ok(Mapper.Map<Acme.Core.Domain.Employee, Acme.WebCore.Models.EmployeeModel>(employee));
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] EmployeeModel model)
        {
            return await ExecuteHandledOperationAsync<IActionResult>(async() => {
                if (model == null)
                {
                    return BadRequest("Employee is null.");
                }

                var employee = Mapper.Map<Acme.WebCore.Models.EmployeeModel, Acme.Core.Domain.Employee>(model);
                await _manager.AddAsync(employee);
                await UnitOfWork.SaveAsync();

                return CreatedAtRoute(
                    "Get",
                    new { Id = employee.EmployeeId },
                    employee);
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] EmployeeModel model)
        {
            return await ExecuteHandledOperationAsync<IActionResult>(async() => {
                if (model == null)
                {
                    return BadRequest("Employee is null.");
                }

                var employee = _manager.GetSingle(e => e.EmployeeId.Equals(id));
                if (employee == null)
                {
                    return NotFound("The Employee record couldn't be found.");
                }

                Mapper.Map<Acme.WebCore.Models.EmployeeModel, Acme.Core.Domain.Employee>(model, employee);
                _manager.Update(employee);
                await UnitOfWork.SaveAsync();

                return CreatedAtRoute(
                    "Get",
                    new { Id = employee.EmployeeId },
                    employee);
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return await ExecuteHandledOperationAsync<IActionResult>(async() => {
                var employee = _manager.GetSingle(e => e.EmployeeId.Equals(id));
                if (employee == null)
                {
                    return NotFound("The Employee record couldn't be found.");
                }

                _manager.Delete(employee);
                await UnitOfWork.SaveAsync();

                return Ok();
            });

        }
    }
}