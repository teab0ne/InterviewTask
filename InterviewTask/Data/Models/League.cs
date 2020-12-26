using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.Data.Models
{
    public class League
    {
        public string Name { get; set; }
        public List<Match> Matches { get; set; }

        private List<Team> Teams { get; set; } = new List<Team>();
        private Dictionary<Team, Score> Statistics { get; set; } = new Dictionary<Team, Score>();

        public void GenerateInfo()
        {
            GetUniqueTeams();
            GenerateStatistics();
        }

        public Team GetBestAttackTeam()
        {
            var (bestAttackTeam, _) = Statistics
                .OrderByDescending(s => s.Value.ft[0])
                .FirstOrDefault();
            return bestAttackTeam;
        }

        public Team GetBestDefenseTeam()
        {
            var (bestDefenseTeam, _) = Statistics
                .OrderBy(s => s.Value.ft[1])
                .FirstOrDefault();
            return bestDefenseTeam;
        }

        public Team GetBestTeam()
        {
            var (team, _) = Statistics.ToList()
                .OrderByDescending(item => (item.Value.ft[0] - item.Value.ft[1]))
                .ThenByDescending(item => item.Value.ft[0])
                .FirstOrDefault();
            return team;
        }

        private void GetUniqueTeams()
        {
            foreach (var match in Matches)
            {
                var currTeam1 = new Team { Name = match.Team1 };
                var currTeam2 = new Team { Name = match.Team2 };

                if (!Teams.Contains(currTeam1))
                    Teams.Add(currTeam1);
                if (!Teams.Contains(currTeam2))
                    Teams.Add(currTeam2);
            }
        }

        private void GenerateStatistics()
        {
            Statistics = new Dictionary<Team, Score>();

            foreach (var team in Teams)
            {
                var matches = Matches
                    .Where(m => m.Team1 == team.Name || m.Team2 == team.Name)
                    .ToList();

                int goals = 0;
                int misses = 0;

                foreach (var m in matches)
                {
                    if (m.Score == null)
                        continue;

                    if (m.Team1 == team.Name)
                    {
                        goals += m.Score.ft[0];
                        misses += m.Score.ft[1];
                    }

                    if (m.Team2 == team.Name)
                    {
                        goals += m.Score.ft[1];
                        misses += m.Score.ft[0];
                    }
                }

                Statistics.Add(team, new Score() { ft = new[] { goals, misses } });
            }
        }
    }
}
