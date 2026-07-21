// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components.Pages;

public partial class HeartbeatFailures : ComponentBase
{
    private readonly ProjectsHeartbeatFetch _projectsHeartbeatFetch = new();

    private List<Heartbeat> Heartbeats => _projectsHeartbeatFetch.ProjectsHeartbeatFailures();

    private Func<string, IEnumerable<HeartbeatData>,
            DateTime, Comparer<HeartbeatData>, (Predicate<HeartbeatData>, SortedSet<HeartbeatData>)> _getLoopDataSet =
            (dateMeasure, heartbeats, date, recordComparison) =>
            {
                var timeValue = dateMeasure switch
                                {
                                        "YEAR" => date.Year,
                                        "MONTH" => date.Month,
                                        "DAY" => date.Day,
                                        _ => throw new ArgumentException("Invalid date measure")
                                };

                Predicate<HeartbeatData> selection = dateMeasure switch
                                                     {
                                                             "YEAR" => i => i.Timestamp.Year == timeValue,
                                                             "MONTH" => i => i.Timestamp.Month == timeValue,
                                                             "DAY" => i => i.Timestamp.Day == timeValue,
                                                             _ => throw new ArgumentException("Invalid date measure")
                                                     };

                var heartbeatData = heartbeats.ToList();
                var heartbeatsSelection = heartbeatData.Where(selection.Invoke);
                var sortedHeartbeats = new SortedSet<HeartbeatData>(heartbeatsSelection, recordComparison);

                return (selection, sortedHeartbeats);
            };
};
