// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    DataController.cs
// Modified On: 10/28/2015 9:29 AM
// Modified By: Austin, Stephen (saustin)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Prototype.Scantron;
using Raven.Abstractions.Data;
using Raven.Abstractions.Extensions;
using RiotSharp;
using RiotSharp.TeamEndpoint;


namespace Prototype.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        private static readonly BulkInsertOptions _BULK_INSERT_OPTIONS = new BulkInsertOptions
        {
            OverwriteExisting = true,
            SkipOverwriteIfUnchanged = true
        };

        // GET: api/data/load
        [HttpGet]
        [Route("load")]
        public async Task<IEnumerable<Team>> Load()
        {
            var data = await Engine.RetrieveTeams();
            using (var bulk = Data.Store.BulkInsert(null, _BULK_INSERT_OPTIONS))
                data.ForEach(x => bulk.Store(x, $"teams/{x.FullId}"));
            return data;
        }
    }

}