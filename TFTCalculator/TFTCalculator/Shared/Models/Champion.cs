using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTCalculator.Shared.Models
{
    public class Champion
    {
        public int level = 0;
        public int tier { get; set; }
        public string name { get; set; }
        public string[] traits { get; set; }
        public Statistics stats { get; set; }
        public Ability ability { get; set; }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Champion)
            {
                return obj.ToString() == this.ToString();
            }
            return base.Equals(obj);
        }
    }

    public class Statistics
    {
        public double[] health { get; set; }
        public double[] attackDamage { get; set; }
        public double[] DPS { get; set; }
        public double[] attackSpeed { get; set; }
        public double armor { get; set; }
        public double magicalResistance { get; set; }
    }

    public class Ability
    {
        public string name { get; set; }
        public string type { get; set; }
        public int startingMana { get; set; }
        public int mana { get; set; }
        public string description { get; set; }
    }
}
