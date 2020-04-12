using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HomeEnterprise.Models
{
    public class Quality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Quality name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Quality name cannot exceed 50 characters")]
        public string QualityName { get; set; }
    }
}