﻿using System;
using System.Diagnostics;

namespace ATM.Simulates.Webview.Helpers
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            //Log = log4net.LogManager.GetLogger(typeof(Logger));
        }

        public static void Error(object msg)
        {
            Log = log4net.LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().ReflectedType);
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log = log4net.LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().ReflectedType);
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log = log4net.LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().ReflectedType);
            Log.Error(ex.Message, ex);
        }

        public static void Info(object msg)
        {
            Log = log4net.LogManager.GetLogger(new StackTrace().GetFrame(1).GetMethod().ReflectedType);
            Log.Info(msg);
        }
    }
}
