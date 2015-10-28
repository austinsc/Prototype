using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Raven.Abstractions.Data;
using RiotSharp;
using RiotSharp.TeamEndpoint;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Prototype.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        private static readonly RiotApi _api = RiotApi.GetInstance("50a768a2-4d69-47fc-806c-8fe097212256");
        // GET: api/data/load
        [HttpGet]
        [Route("load")]
        public async Task<IHttpActionResult> Load()
        {
            using (var bulk = Data.Store.BulkInsert(null, new BulkInsertOptions() { OverwriteExisting = true, SkipOverwriteIfUnchanged = true }))
            {
                var league = await _api.GetChallengerLeagueAsync(Region.na, Queue.RankedSolo5x5);
                foreach (var entry in league.Entries)
                    bulk.Store(entry, entry.PlayerOrTeamId);
                var allTeams = new List<Team>();
                //for (var i = 0; i < league.Entries.Count; i += 10)
                //{
                //    var teams = await _api.GetTeamsAsync(Region.na, league.Entries.GetRange(i, 10).Select(x => x.PlayerOrTeamId).ToList());
                //    foreach (var team in teams)
                //        bulk.Store(team.Value, team.Key);
                //    allTeams.AddRange(teams.Values);
                //}
                return this.Json(new
                {
                    league,
                    teams = allTeams
                });
            }
        }
    }
}
