using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTCalculator.Shared.Models
{
    public class RollingOdds
    {
        public RollingOdds(int tier, int[] odds)
        {
            this.tier = tier;
            this.odds = odds;

            for (int i = 0; i < odds.Length; i++)
            {
                if (odds[i] == 0)
                {
                    this.canPurchase = i;
                    break;
                }
                if (odds.Length - 1 == i)
                {
                    this.canPurchase = i + 1;
                }
            }
        }

        public int tier { get; set; }
        public int expLevel { get; set; }
        public int canPurchase { get; set; }
        public int[] odds { get; set; }
    }
}
