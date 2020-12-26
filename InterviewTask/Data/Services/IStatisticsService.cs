using InterviewTask.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.Data.Services
{
    public interface IStatisticsService
    {
        List<League> Leagues { get; }
        (string day, int goals) GetBestDay();
    }
}
