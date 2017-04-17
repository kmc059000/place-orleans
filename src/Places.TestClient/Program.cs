using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Place.Interfaces;
using Place.Interfaces.Commands;
using Place.Interfaces.Grains;
using Place.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Places.TestClient
{
    class Program
    {
        static int Main(string[] args)
        {
            var config = ClientConfiguration.LocalhostSilo();
            try
            {
                InitializeWithRetries(config, initializeAttemptsBeforeFailing: 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Orleans client initialization failed failed due to {ex}");

                Console.ReadLine();
                return 1;
            }

            DoClientWork().Wait();
            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();
            return 0;
        }

        private static void InitializeWithRetries(ClientConfiguration config, int initializeAttemptsBeforeFailing)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    GrainClient.Initialize(config);
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        private static async Task DoClientWork()
        {
            var random = new Random();
            var stopwatch = new Stopwatch();
            long avg = 0;
            long runs = 0;

            while (true)
            {
                stopwatch.Start();

                await RunBatchOf300(random);

                stopwatch.Stop();

                var elapsed = stopwatch.ElapsedMilliseconds;

                avg = (avg * runs + elapsed) / (++runs);

                Console.WriteLine($"Finished batch in {elapsed}ms with an average of {avg}ms for {runs} runs.");

                stopwatch.Reset();
            }
        }

        private static Task RunBatchOf300(Random random)
        {
            List<Task> tasks = new List<Task>();

            var i = 300;
            while (i-- > 0)
            {
                var userid = random.Next() % 10;
                var x = random.Next() % Constants.Width;
                var y = random.Next() % Constants.Height;

                var color = (Colors)(random.Next() % 256);

                string username = $"TestClient_{userid}";
                var author = GrainClient.GrainFactory.GetGrain<IUserGrain>(username);

                var command = new WritePixelCommand()
                {
                    X = (short)x,
                    Y = (short)y,
                    Author = username,
                    Color = color,
                    Timestamp = DateTime.UtcNow
                };

                //await author.WritePixel(command);
                tasks.Add(author.WritePixel(command));

                //Console.WriteLine($"{username} wrote pixel {x}x{y} with {color.ToString()}");
            }

            Task.WaitAll(tasks.ToArray());

            return TaskDone.Done;
        }
    }
}
