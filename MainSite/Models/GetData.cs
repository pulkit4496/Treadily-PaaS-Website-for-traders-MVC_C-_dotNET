using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace MainSite.Models
{
    public class GetData
    {
        //private string _Comments = "*No Description Available*";
        public int TotalViews { get; set; }

        public int Believe { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public string chartImage { get; set; }
        public string post_date { get; set; }
        public string ID { get; set; }
        [Required]
        [Display(Name = "Ticker Name")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Enter valid Ticker Name")]
        public string TickerName { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public string Date { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        
        public string StartTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public string EndTime { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public float Quantity { get; set; }

        [Required]
        [Display(Name = "Initial Price")]
        public float StartPrice { get; set; }

        [Required]
        [Display(Name = "Final Price")]
        public float EndPrice { get; set; }

        [Required]
        public string BuySell { get; set; }
        [Display(Name = "Profit/Loss")]
        public string Profit { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; } 
        //public string Comments { get { return _Comments; } set { value = _Comments; } }
        

    }
}