// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    Engine.cs
// Modified On: 10/28/2015 9:56 AM
// Modified By: Austin, Stephen (saustin)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiotSharp;
using RiotSharp.TeamEndpoint;

namespace Prototype.Scantron
{
    public static class Engine
    {
        private static readonly RiotApi _api = RiotApi.GetInstance("50a768a2-4d69-47fc-806c-8fe097212256");

        public static async Task<IEnumerable<Team>> RetrieveTeams()
        {
            var allTeams = new List<Team>();
            var league = await _api.GetChallengerLeagueAsync(Region.na, Queue.RankedSolo5x5);
            foreach(var chunk in league.Entries.Chunk(10))
            {
                var teams = await _api.GetTeamsAsync(Region.na, chunk.Select(x => int.Parse(x.PlayerOrTeamId)).ToList());
                allTeams.AddRange(teams.SelectMany(x => x.Value));
            }
            return allTeams;
        }
    }
}