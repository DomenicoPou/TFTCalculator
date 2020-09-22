using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFTCalculator.Shared.Models;

namespace TFTCalculator.Server.Handlers
{
    public class PerfectCalculatorHandler
    {
        const int averageTier = 8;

        public static List<List<string>> GetPerfectsBetter(
            Champion startChamp,
            List<Champion> originalChampionsList,
            Dictionary<string, Trait> traits,
            List<Champion> currentList = null,
            List<Champion> currentChampionList = null,
            List<List<string>> result = null)
        {
            if (result == null) result = new List<List<string>>();
            if (currentList == null) currentList = new List<Champion>() { startChamp };
            if (currentChampionList == null)
            {
                currentChampionList = originalChampionsList;
                currentChampionList.Remove(startChamp);
            }

            foreach (Champion champion in currentChampionList)
            {
                if (ChampionListContainsChampTrait(currentList, champion))
                {
                    List<Champion> editedList = currentList;
                    editedList.Add(champion);
                    List<Champion> editedCurrentChampionList = currentChampionList;
                    editedCurrentChampionList.Remove(champion);

                    if (IsPerfect(editedList.ToArray(), traits))
                    {

                    }
                    else
                    {

                    }
                }
            }
            return result;
        }

        private static bool ChampionListContainsChampTrait(List<Champion> currentList, Champion champion)
        {
            foreach (Champion champ in currentList)
            {
                foreach (string trait in champion.traits)
                {
                    if (champ.traits.Contains(trait))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<List<string>> GetPerfects(List<Champion> champions, List<Trait> traits, List<RollingOdds> rollingOdds)
        {
            try
            {
                List<Champion[]> variations = new List<Champion[]>();
                for (int champCount = 1; champCount <= averageTier; champCount++)
                {
                    Dictionary<string, Trait> traitMap = traits.ToDictionary(x => x.name);
                    List<Champion> champList = (new List<Champion>(champions)).FindAll(x => x.tier <= rollingOdds[champCount].canPurchase);
                    IEnumerable<Champion[]> newPerfects = CombinationsRosettaWoRecursion(champList.ToArray(), champCount, traitMap);
                    variations.AddRange(newPerfects);
                }

                List<List<string>> ret = new List<List<string>>();
                foreach (Champion[] cariationChamps in variations)
                {
                    List<string> addition = new List<string>();
                    foreach (Champion champion in cariationChamps)
                    {
                        addition.Add(champion.name);
                    }
                    ret.Add(addition);
                }
                return ret;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private static List<Champion> GetChampions(IEnumerable<int> enumerable, List<Champion> champions)
        {
            List<Champion> ret = new List<Champion>();
            foreach (int num in enumerable)
            {
                ret.Add(champions[num]);
            }
            return ret;
        }

        private static bool IsPerfect(Champion[] champions, Dictionary<string, Trait> traitMap)
        {
            Dictionary<string, int> traitCount = new Dictionary<string, int>();
            foreach (Champion champion in champions)
            {
                foreach (string trait in champion.traits) 
                {
                    if (traitCount.ContainsKey(trait))
                    {
                        traitCount[trait]++;
                    }
                    else
                    {
                        traitCount.Add(trait, 1);
                    }
                }
            }
            foreach (KeyValuePair<string, int> traitPair in traitCount)
            {
                if (!traitMap[traitPair.Key].setNumbers.Contains(traitPair.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<Champion[]> CombinationsRosettaWoRecursion(Champion[] array, int count, Dictionary<string, Trait> traitMap)
        {
            Champion[] result = new Champion[count];
            foreach (IEnumerable<int> j in GetKCombs<int>(GetNumbers(0, array.Count() - 1), count).ToList())
            {
                HashSet<string> names = new HashSet<string>();
                bool dontYield = false;
                for (int i = 0; i < count; i++)
                {
                    result[i] = array[j.ToArray()[i]];
                }
                if (dontYield) continue;
                //if (IsPerfect(result, traitMap))
                //{
                    yield return result.ToArray();
                //}
            }
        }
        
        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T: IComparable
        {
            if (length == 1) return list.Select(T => new T[] { T });
            return GetKCombs(list, length - 1).SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                (t1, t2) => t1.Concat(new T[] { t2 }));
        }


        /// <summary>
        /// Returns the list of number between min - max
        /// </summary>
        /// <param name="min">The minimum number within the list</param>
        /// <param name="max">The Maximum number within the list</param>
        /// <returns>The list of numbers between min - max</returns>
        private static List<int> GetNumbers(int min, int max)
        {
            // Simply create the numbers between the minimum and maximum given
            List<int> results = new List<int>();
            for (int index = min; index <= max; index++)
            {
                results.Add(index);
            }
            return results;
        }

        /*public static IEnumerable<int[]> CombinationsRosettaWoRecursion(int m, int n)
        {
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>(m);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return (int[])result.Clone(); // thanks to @xanatos
                                                        //yield return result;
                    break;
                }
            }
        }*/
    }
}
