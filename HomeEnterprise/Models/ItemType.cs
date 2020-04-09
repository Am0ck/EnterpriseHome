using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HomeEnterprise.Models
{
    public class ItemType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Type name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Type name cannot exceed 50 characters")]
        public string TypeName { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}