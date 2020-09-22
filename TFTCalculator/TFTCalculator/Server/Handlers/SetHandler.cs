using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFTCalculator.Shared.Models;

namespace TFTCalculator.Server.Handlers
{
    public class SetHandler
    {
        Dictionary<string, TFTSet> sets;

        public SetHandler()
        {
            sets = new Dictionary<string, TFTSet>();
            GetAllCurrentSets();
        }

        private void GetAllCurrentSets()
        {
            decimal currentSet = (decimal)1;
            List<Task> tasks = new List<Task>();
            while (true)
            {
                tasks.Add(Task.Factory.StartNew((object iter) =>
                {
                    try
                    {
                        string currentSetNumber = ((decimal)iter).ToString();
                        if (currentSetNumber.Contains(".0"))
                        {
                            currentSetNumber = Convert.ToInt32(Convert.ToDouble(currentSetNumber)).ToString();
                        }
                        sets.Add(currentSetNumber, ObtainSet(currentSetNumber));
                    }
                    catch (Exception ex)
                    {

                    }
                }, currentSet));
                currentSet += (decimal)0.5;
                if (currentSet == 10) break;
            }
            Task.WaitAll(tasks.ToArray());
        }

        public TFTSet ObtainSet(string set)
        {
            ConfigHandler handler = new ConfigHandler(set);
            //rollingOdds = handler.PullOdds();
            TFTSet resultSet = new TFTSet();

            resultSet.traits = handler.PullTraitSet();
            resultSet.champions = handler.PullChampionSet();
            resultSet.items = handler.PullItemSet();

            resultSet.perfects = handler.PullPerfects();

            resultSet.setNumber = set;
            return resultSet;
        }
    }
}
