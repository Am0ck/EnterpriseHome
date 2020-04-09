using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HomeEnterprise.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //[Required]
        //public IdentityModels IdentittyModels { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        [Required]
        public string Quantity { get; set; }

        [Required]
        public string Quality { get; set; }

        [Required]
        public double Price { get; set; }

        //[Required]
        //public long ApplicationUser_Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}