// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    Engine.cs
// Modified On: 11/12/2015 4:40 PM
// Modified By: Austin, Stephen (saustin)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Newtonsoft.Json;
using RiotSharp;
using RiotSharp.MatchEndpoint;
using Team = RiotSharp.TeamEndpoint.Team;

namespace Prototype.Scantron
{
    public static class Data
    {
        private static readonly Lazy<MongoClient> _client = new Lazy<MongoClient>(() => new MongoClient("mongodb://ndit.pwnt.co"));
        private static readonly Lazy<IMongoDatabase> _database = new Lazy<IMongoDatabase>(() => _client.Value.GetDatabase("no-defeat-in-team"));
        private static readonly Lazy<IMongoCollection<BsonDocument>> _teams = new Lazy<IMongoCollection<BsonDocument>>(() => _database.Value.GetCollection<BsonDocument>("teams"));
        private static readonly Lazy<IMongoCollection<BsonDocument>> _matches = new Lazy<IMongoCollection<BsonDocument>>(() => _database.Value.GetCollection<BsonDocument>("matches"));

        public static IMongoCollection<BsonDocument> Teams => _teams.Value;
        public static IMongoCollection<BsonDocument> Matches => _matches.Value;
    }

    public static class Engine
    {
        private static readonly RiotApi _api = RiotApi.GetInstance("50a768a2-4d69-47fc-806c-8fe097212256");
        
        public static async Task<IEnumerable<Team>> RetrieveTeams()
        {
            var result = new List<Team>();
            var league = await _api.GetChallengerLeagueAsync(Region.na, Queue.RankedTeam5x5);
            foreach(var chunk in league.Entries.Chunk(10))
            {
                var teams = await _api.GetTeamsAsync(Region.na, chunk.Select(x => x.PlayerOrTeamId).ToList());
                result.AddRange(teams.Values);
            }
            await Data.Teams.InsertManyAsync(result.Select(team => BsonDocument.Parse(JsonConvert.SerializeObject(team))));
            return result;
        }

        public static async Task<IEnumerable<MatchDetail>> RetrieveMatchDetails(Region region = Region.na)
        {
            var challenger = await _api.GetChallengerLeagueAsync(region, Queue.RankedTeam5x5);
            var masters = await _api.GetMasterLeagueAsync(region, Queue.RankedTeam5x5);
            var result = new List<MatchDetail>();
            foreach(var chunk in challenger.Entries.Concat(masters.Entries).Chunk(10))
            {
                var teams = await _api.GetTeamsAsync(region, chunk.Select(x => x.PlayerOrTeamId).ToList());
                foreach(var id in teams.Values.SelectMany(x => x.MatchHistory).Select(x => x.GameId).Distinct())
                    if(await Data.Matches.CountAsync(Builders<BsonDocument>.Filter.Eq("matchId", id), new CountOptions { Limit = 1 }) == 0)
                    {
                        var match = await _api.GetMatchAsync(region, id, true);
                        if(match != null)
                        {
                            await Data.Matches.InsertOneAsync(BsonDocument.Parse(JsonConvert.SerializeObject(match)));
                            result.Add(match);
                        }
                    }
            }
            return result;
        }
    }
}