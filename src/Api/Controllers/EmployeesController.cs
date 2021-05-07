using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Controllers;
using Core.Dtos;
using Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [Route("api/employees")]
    public class EmployeesController : ApiControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFeatureManager _featureManager;

        public EmployeesController(IEmployeeRepository employeeRepository, IFeatureManager featureManager)
        {
            _employeeRepository = employeeRepository;
            _featureManager = featureManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(EmployeeDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithDetails(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("oldest")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOldest(
            CancellationToken cancellationToken = default)
        {
            

            var result = await _employeeRepository.GetOldestAsync(cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] int id,
            [FromBody] EmployeePutDto employeePutDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.UpdateAsync(id, employeePutDto, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromBody] EmployeePostDto employeePostDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.InsertAsync(employeePostDto, cancellationToken);
            Response.Headers.Add("x-date-created", DateTime.UtcNow.ToString("s"));
            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken = default)
        {
            var result = await _employeeRepository.DeleteByIdAsync(id, cancellationToken);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}