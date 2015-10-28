// Copyright (c) 2015 The RoviSys Company. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of The RoviSys Company.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    Startup.cs
// Modified On: 10/28/2015 9:22 AM
// Modified By: Austin, Stephen (saustin)

using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Prototype;

[assembly: OwinStartup(typeof(Startup))]

namespace Prototype
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var http = new HttpConfiguration();
            http.Formatters.Clear();
            http.Formatters.Add(new JsonMediaTypeFormatter());
            http.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            http.MapHttpAttributeRoutes();
            app.UseWebApi(http);

            //WebRequest.DefaultWebProxy = new WebProxy("http://localhost:8888/");
        }
    }
}