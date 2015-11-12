// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    DataController.cs
// Modified On: 11/12/2015 2:55 PM
// Modified By: Austin, Stephen (saustin)

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Prototype.Scantron;
using RiotSharp.MatchEndpoint;
using Team = RiotSharp.TeamEndpoint.Team;

namespace Prototype.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        // GET: api/data/load
        [HttpGet]
        [Route("teams")]
        public async Task<IEnumerable<Team>> Teams() => await Engine.RetrieveTeams();

        [HttpGet]
        [Route("matches")]
        public async Task<IEnumerable<MatchDetail>> Matches() => await Engine.RetrieveMatchDetails();
    }
}