using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Smash_Backend.Entities
{
    public partial class EntryExitTransactions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string NumberPlate { get; set; }
        public int? EntryInterchangeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EntryDateTime { get; set; }
        public int? ExitInterchangeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExitDateTime { get; set; }
    }
}
