using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TFTCalculator.Shared.Models;

namespace TFTCalculator.Server.Handlers
{
    public class ConfigHandler
    {
        private string OddsFileLocation;
        private string ChampionFileLocation;
        private string TraitFileLocation;
        private string ItemFileLocation;

        private string PerfectsFileLocation;


        public ConfigHandler(string setNumber)
        {
            OddsFileLocation = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + $@"\Config\oddsConfiguration.json";

            TraitFileLocation = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + $@"\Config\Set {setNumber}\TraitConfiguration.json";
            ChampionFileLocation = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + $@"\Config\Set {setNumber}\ChampConfiguration.json";
            ItemFileLocation = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + $@"\Config\Set {setNumber}\ItemConfiguration.json";

            PerfectsFileLocation = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath) + $@"\Config\Set {setNumber}\PerfectConfiguration.json";

            this.GetSetContent(setNumber);
        }

        public void GetSetContent(string set)
        {
            if (!File.Exists(OddsFileLocation)) writeProperties(OddsFileLocation, WebCrawlerHandler.GetRollingOdds());

            if (!File.Exists(TraitFileLocation)) writeProperties(TraitFileLocation, WebCrawlerHandler.GetTraits(set));
            if (!File.Exists(ChampionFileLocation)) writeProperties(ChampionFileLocation, WebCrawlerHandler.GetChampions(set));
            if (!File.Exists(ItemFileLocation)) writeProperties(ItemFileLocation, WebCrawlerHandler.GetItems(set));

            GetPerfects(set);
        }

        private void GetPerfects(string set)
        {
            if (!File.Exists(PerfectsFileLocation))
            {
                writeProperties(PerfectsFileLocation, PerfectCalculatorHandler.GetPerfects(PullChampionSet(), PullTraitSet(), PullOdds()));
            }
        }

        public List<RollingOdds> PullOdds()
        {
            string ConfigFileLocation = OddsFileLocation;
            return readProperties<List<RollingOdds>>(ConfigFileLocation);
        }

        public List<Champion> PullChampionSet()
        {
            string ConfigFileLocation = ChampionFileLocation;
            return readProperties<List<Champion>>(ConfigFileLocation);
        }

        public List<Trait> PullTraitSet()
        {
            string ConfigFileLocation = TraitFileLocation;
            return readProperties<List<Trait>>(ConfigFileLocation);
        }

        public List<Item> PullItemSet()
        {
            string ConfigFileLocation = ItemFileLocation;
            return readProperties<List<Item>>(ConfigFileLocation);
        }

        public List<string[]> PullPerfects()
        {
            string ConfigFileLocation = PerfectsFileLocation;
            return readProperties<List<string[]>>(ConfigFileLocation);
        }

        public T readProperties<T>(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                string content = textReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        public void writeProperties(string path, object content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            using (var textReader = new StreamWriter(fileStream))
            {
                string toWrite = JsonConvert.SerializeObject(content, Formatting.Indented);
                textReader.Write(toWrite);
            }
        }
    }
}
