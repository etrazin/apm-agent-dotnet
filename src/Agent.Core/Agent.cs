﻿using System;
using Elastic.Agent.Core;
using Elastic.Agent.Core.Config;
using Elastic.Agent.Core.Logging;


//the namespace is TBD! It currently does not fit the location of the file, 
//but other agents use something similar
namespace Elastic.Apm
{
    public static class Agent
    {
        private static LogLevel? logLevel;
        public static LogLevel LogLevel
        {
            get => logLevel?? Config.LogLevel;
            set => logLevel = value;

        }

        private static IConfig config = new EnvironmentVariableConfig();
        public static IConfig Config 
        {
            get
            {
                if (config?.Logger == null)
                {
                    config.Logger = CreateLogger("Config");
                }

                return config;
            }
            set
            {
                config = value;
                config.Logger = CreateLogger("Config");
            } 
        }

        /// <summary>
        /// Returns a logger with a specific prefix. The idea behind this class 
        /// is that each component in the agent creates its own agent with a
        /// specific <paramref name="prefix"/> which makes correlating logs easier
        /// </summary>
        /// <returns>The logger.</returns>
        /// <param name="prefix">Prefix.</param>
        public static AbstractLogger CreateLogger(String prefix = "")
        {
            var logger = Activator.CreateInstance(loggerType) as AbstractLogger;
            logger.Prefix = prefix;
            return logger;          
        }

        /// <summary>
        /// Sets the type of the logger.
        /// </summary>
        /// <param name="t">T.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        internal static void SetLoggerType<T>() where T : AbstractLogger, new()
        {
            loggerType = typeof(T);
        }

        static Type loggerType = typeof(ConsoleLogger);
    }
}
