﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Akka.NET Project">
//     Copyright (C) 2009-2016 Lightbend Inc. <http://www.lightbend.com>
//     Copyright (C) 2013-2016 Akka.NET project <https://github.com/akkadotnet/akka.net>
// </copyright>
//-----------------------------------------------------------------------

using System;
using Akka.Actor;
using Akka.Event;

namespace TimeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("TimeServer"))
            {
                Console.Title = "Server";
                var server = system.ActorOf<TimeServerActor>("time");
                Console.ReadLine();
                Console.WriteLine("Shutting down...");
                Console.WriteLine("Terminated");
            }
        }

        public class TimeServerActor : TypedActor, IHandle<string>
        {
            private readonly ILoggingAdapter _log = Context.GetLogger();

            public void Handle(string message)
            {
                if (message.ToLowerInvariant() == "gettime")
                {
                    Console.WriteLine("get msg gettime");
                    var time =DateTime.UtcNow.ToLongTimeString();
                    Sender.Tell(time, Self);
                }
                else
                {
                    Console.WriteLine("get msg other");
                    _log.Error("Invalid command: {0}", message);
                    var invalid = "Unrecognized command";
                    Sender.Tell(invalid, Self);
                }
            }
        }
    }
}
