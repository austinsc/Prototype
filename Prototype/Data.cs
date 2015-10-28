﻿// Copyright (c) 2015 PartsSource, Inc. All Right Reserved.
// 
// This file contains proprietary trade secret information and is the property of PartsSource, Inc.  Reproduction 
// or transmission in any form or by any means, electronic, mechanical or otherwise, is prohibited without 
// express prior written permission.
// 
// Filename:    Data.cs
// Modified On: 04/22/2015 1:56 PM
// Modified By: stephen.austin (stephen.austin)

using System;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.FileSystem;
using Raven.Imports.Newtonsoft.Json;
using Raven.Imports.Newtonsoft.Json.Converters;

namespace Prototype
{
    public static class Data
    {
        private static readonly Lazy<IDocumentStore> _documentStore = new Lazy<IDocumentStore>(() =>
        {
            var store = new DocumentStore
            {
                Url = ConnectionString,
                DefaultDatabase = DatabaseName
            }.Initialize();
            store.Conventions.CustomizeJsonSerializer = serializer =>
            {
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Converters.Add( new StringEnumConverter() );
            };
            return store;
        });

        private static readonly Lazy<IFilesStore> _fileStore = new Lazy<IFilesStore>(() => new FilesStore
        {
            Url = ConnectionString,
            DefaultFileSystem = DatabaseName
        }.Initialize());

        public static string ConnectionString { get; } = "http://localhost:8080/";

        public static string DatabaseName { get; } = "no-defeat";

        /// <summary>
        /// Singleton document store instance used to access the RavenDB server
        /// </summary>
        public static IDocumentStore Store => _documentStore.Value;
        public static IFilesStore Files => _fileStore.Value;
    }
}