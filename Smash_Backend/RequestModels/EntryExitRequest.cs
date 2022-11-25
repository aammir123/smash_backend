using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace Smash_Backend.RequestModels
{
    public class EntryExitRequest : IValidatableObject
    {              
        public int interchangeId { get; set; }             
        public string numberPlate { get; set; }               
        public DateTime? transactionDateTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();            
            if (interchangeId < 1)
            {
                results.Add(new ValidationResult("interchangeId must be greater than 0"));
            }
            if (transactionDateTime == null || transactionDateTime < DateTime.Now)
            {
                results.Add(new ValidationResult("transactionDateTime is not valid"));
            }
            if (string.IsNullOrEmpty(numberPlate) == true)
            {
                results.Add(new ValidationResult("numberPlate is not valid"));
            }
            return results;
        }
    }
}
