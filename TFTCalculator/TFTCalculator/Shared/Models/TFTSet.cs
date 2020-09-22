using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTCalculator.Shared.Models
{
    public class TFTSet
    {
        public string patch { get; set; }
        public string setNumber { get; set; }

        public List<RollingOdds> rollingOdds { get; set; }

        public List<Champion> champions { get; set; }
        public List<Item> items { get; set; }
        public List<Trait> traits { get; set; }

        public List<string[]> perfects { get; set; }
    }
}
