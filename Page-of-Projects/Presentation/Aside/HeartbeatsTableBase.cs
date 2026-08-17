// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Components.Pages;

public abstract class HeartbeatsTableBase : FoldingDataTable
{
    protected readonly Func<string, IEnumerable<HeartbeatData>,
            DateTime, Comparer<HeartbeatData>, (Predicate<HeartbeatData>, SortedSet<HeartbeatData>)> GetLoopDataSet =
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

    protected Comparer<HeartbeatData>
            HeartbeatComparer = Comparer<HeartbeatData>.Create((x, y) => y.Id.CompareTo(x.Id));

    public static List<HeartbeatData> FiveHeartbeatsFailures()
    {
            return new ProjectsHeartbeatFetch()
                  .ProjectsHeartbeatFailures()
                  .OrderByDescending(d => d.RecordTimestamp)
                  .Take(5).ToList().ConvertAll(i => new HeartbeatData
                                                    {
                                                                    Id = i.Id,
                                                                    Timestamp = i.RecordTimestamp,
                                                                    Agent = i.Agent,
                                                                    Project = i.ProjectName,
                                                                    Domain = i.Domain,
                                                                    RelativeUrl = i.UrlRel,
                                                                    ResponseCode = i.ResponseStatus
                                                    });
    }

    public static List<HeartbeatData> FiveHeartbeats()
    {
            return new ProjectsHeartbeatFetch()
                  .ProjectsHeartbeats()
                  .OrderByDescending(d => d.RecordTimestamp)
                  .Take(5).ToList().ConvertAll(i => new HeartbeatData
                                                    {
                                                                    Id = i.Id,
                                                                    Timestamp = i.RecordTimestamp,
                                                                    Agent = i.Agent,
                                                                    Project = i.ProjectName,
                                                                    Domain = i.Domain,
                                                                    RelativeUrl = i.UrlRel,
                                                                    ResponseCode = i.ResponseStatus
                                                    });
    }
};
