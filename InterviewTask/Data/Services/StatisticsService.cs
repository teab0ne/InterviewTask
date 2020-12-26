using InterviewTask.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InterviewTask.Data.Services
{
    public class StatisticsService : IStatisticsService
    {
        private List<League> _leagues = new List<League>();
        public List<League> Leagues => _leagues;

        private List<string> _urls = new List<string>
        {
            "https://raw.githubusercontent.com/openfootball/football.json/master/2019-20/en.1.json",
            "https://raw.githubusercontent.com/openfootball/football.json/master/2019-20/en.2.json",
            "https://raw.githubusercontent.com/openfootball/football.json/master/2019-20/en.3.json"
        };

        public static async Task<StatisticsService> Initialize()
        {
            StatisticsService service = new StatisticsService();
            service._leagues = new List<League>();

            using (WebClient client = new WebClient())
            {
                foreach (var url in service._urls)
                {
                    string data = await client.DownloadStringTaskAsync(url);
                    League league = JsonConvert.DeserializeObject<League>(data);
                    league.GenerateInfo();
                    service._leagues.Add(league);
                }
            }

            return service;
        }

        public (string day, int goals) GetBestDay()
        {
            var days = new Dictionary<string, int>();

            foreach (var league in Leagues)
            {
                league.Matches.ForEach(m =>
                {
                    var day = m.Date.ToString("dddd");
                    if (!days.ContainsKey(day))
                        days.Add(day, 0);
                    if (m.Score != null)
                        days[day] += m.Score.ft.Sum();
                });
            }

            var (bestDay, goals) = days.ToList()
                .OrderByDescending(i => i.Value)
                .FirstOrDefault();

            return (bestDay, goals);
        }
    }
}
