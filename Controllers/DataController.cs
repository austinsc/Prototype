using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RiotSharp;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Prototype.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private static readonly RiotApi _api = RiotApi.GetInstance("");
        // GET: api/data/load
        [HttpGet("load")]
        [AllowAnonymous]
        public async Task<IEnumerable<object>> Load()
        {
            using(var session = Data.Store.OpenAsyncSession())
            {
                var league = await _api.GetChallengerLeagueAsync(Region.na, Queue.RankedSolo5x5);
                foreach(var entry in league.Entries)
                    await session.StoreAsync(entry);
                await session.SaveChangesAsync();
                return league.Entries;
            }
        }
    }
}
