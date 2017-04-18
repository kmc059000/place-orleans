using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Diagnostics;
using System.Threading;

namespace Place.WebApi.App_Start
{
    public static class OrleansConfig
    {
        public static bool Register()
        {
            var config = ClientConfiguration.LocalhostSilo();
            try
            {
                return InitializeWithRetries(config, initializeAttemptsBeforeFailing: 5);
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Orleans client initialization failed failed due to {ex}");

                return false;
            }
        }

        private static bool InitializeWithRetries(ClientConfiguration config, int initializeAttemptsBeforeFailing)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    return true;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Trace.TraceInformation($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }
    }
}