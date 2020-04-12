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

        [Required(ErrorMessage = "Quantity is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be at least 1")]
        [Index("UserId_Quality_price_UN", 1, IsUnique = true)]
        public float Price { get; set; }

        //[Required]
        //public long ApplicationUser_Id { get; set; }
        [Index("UserId_Quality_price_UN", 2, IsUnique = true)]
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        [Required(ErrorMessage = "Quality required")]
        [Index("UserId_Quality_price_UN", 3, IsUnique = true)]
        [Display(Name = "Quality")]
        public long QualityId { get; set; }
        public Quality Quality { get; set; }

        [Required]
        [Display(Name = "Item Type")]
        public long ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }        
    }
}