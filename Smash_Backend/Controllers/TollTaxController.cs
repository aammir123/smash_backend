using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smash_Backend.Entities;
using Smash_Backend.Filters;
using Smash_Backend.RequestModels;
using Smash_Backend.Responses;
using Smash_Backend.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smash_Backend.Controllers
{

    [ApiAuth]
    public class TollTaxController : Controller
    {
        private readonly ITollTaxService _tollTaxService;
        public TollTaxController(ITollTaxService tollTaxService)
        {
            _tollTaxService = tollTaxService;
        }
        [Route("TollTax/Entry")]
        [HttpPost]
        public async Task<IActionResult> TollTaxEntry([FromBody] EntryExitRequest request)
        {

            if (request != null)
            {
                List<string> errors = new List<string>();
                var results = new List<ValidationResult>();
                var validationResult = Validator.TryValidateObject(request, new ValidationContext(request, null, null), results, true);
               
                if (validationResult == false)
                {
                    var errors1 = ModelState.SelectMany(e => e.Value.Errors.ToList()).Select(et => et.ErrorMessage).Distinct().ToList();
                    errors = results.Select(e => e.ErrorMessage).Distinct().ToList();                   
                    return BadRequest(errors);
                }
                var entry = await _tollTaxService.TollTaxEntry(request);
                return Created($"TollTax/Entry/{entry.Id}", entry);
            }
            else
            {              
                return BadRequest("Invalid Request");
            }
        }

        [Route("TollTax/Exit")]
        [HttpPut]
        public async Task<IActionResult> TollTaxExit([FromBody] EntryExitRequest request)
        {
            if (request != null)
            {
                List<string> errors = new List<string>();
                var results = new List<ValidationResult>();
                var validationResult = Validator.TryValidateObject(request, new ValidationContext(request, null, null), results, true);

                if (validationResult == false)
                {
                    var errors1 = ModelState.SelectMany(e => e.Value.Errors.ToList()).Select(et => et.ErrorMessage).Distinct().ToList();
                    errors = results.Select(e => e.ErrorMessage).Distinct().ToList();
                    return BadRequest(errors);
                }

                var tollTax = await _tollTaxService.TollTaxExit(request);
                return Ok(tollTax);                                           
            }
            else
            {
                return BadRequest();
            }
        }
        
        [Route("TollTax/Interchanges")]
        [HttpGet]
        public async Task<IActionResult> GetInterchanges()
        {

            var Interchanges = await _tollTaxService.GetInterchanges();
            return Ok(Interchanges);
        }
    }
}
