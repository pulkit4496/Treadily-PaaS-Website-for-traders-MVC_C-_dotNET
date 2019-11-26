using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainSite.Models
{
    public class UserData
    {
        public float gross_profit { get; set; }
        public float gross_loss { get; set; }
        public float largest_winning { get; set; }
        public float largest_losing { get; set; }
        public int consecutive_win { get; set; }
        public int consecutive_lose { get; set; }
        public float tot_gain { get; set; }
        public float avg_gain { get; set; }
        //public float tot_gain_pct { get; set; }
        public float avg_gainn { get; set; }
        public string total_trd { get; set; }
        public int win_trd { get; set; }
        public float win_pct { get; set; }
        public int lose_trd { get; set; }



    }
}