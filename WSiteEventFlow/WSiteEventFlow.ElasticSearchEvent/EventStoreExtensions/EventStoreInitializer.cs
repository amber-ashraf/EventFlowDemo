﻿using System;
using System.Data.Common;
using EventFlow;
using EventFlow.EventStores.EventStore.Extensions;
using EventFlow.Extensions;
using EventFlow.MetadataProviders;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace WSiteEventFlow.ElasticSearchEventStore.EventStoreExtensions
{
    public static class EventStoreInitializer
    {
        public static IEventFlowOptions ConfigureEventStore(this IEventFlowOptions options)
        {

            //string eventStoreUrl = Environment.GetEnvironmentVariable("EVENTSTOREURL");
            string connectionString = $"ConnectTo=tcp://admin:changeit@localhost:1113; HeartBeatTimeout=500";
            Uri eventStoreUri = GetUriFromConnectionString(connectionString);

            ConnectionSettings connectionSettings = ConnectionSettings.Create()
                .EnableVerboseLogging()
                .KeepReconnecting()
                .KeepRetrying()
                .SetDefaultUserCredentials(new UserCredentials("admin", "changeit"))
                .Build();

            IEventFlowOptions eventFlowOptions = options
                .AddMetadataProvider<AddGuidMetadataProvider>()
                .UseEventStoreEventStore(eventStoreUri, connectionSettings);

            return eventFlowOptions;
        }

        private static Uri GetUriFromConnectionString(string connectionString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            string connectTo = (string)builder["ConnectTo"];

            return connectTo == null ? null : new Uri(connectTo);
        }
    }
}
