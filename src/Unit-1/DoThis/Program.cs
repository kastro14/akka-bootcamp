using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    public static class Program
    {
        private static ActorSystem _myActorSystem;

        public static void Main(string[] args)
        {
            // initialize MyActorSystem
            _myActorSystem = ActorSystem.Create("MyActorSystem");

            // time to make your first actors!
            var consoleWriterProps = Props.Create<ConsoleWriterActor>();
            var consoleWriterActor = _myActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            var tailCoordinatorActor = _myActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            var validationActor = _myActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>();
            var consoleReaderActor = _myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            _myActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
