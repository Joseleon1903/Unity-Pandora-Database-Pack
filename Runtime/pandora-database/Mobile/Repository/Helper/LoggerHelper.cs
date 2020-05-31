using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Pandora.Database.Mobile.Repository.Enum;
using UnityEngine;

namespace Unity.Pandora.Database.Mobile.Repository.Helper
{
    public static class LoggerHelper
    {
        static string DirectoryPacth = Directory.GetCurrentDirectory() + "\\Logs";
        static string FilePath = Directory.GetCurrentDirectory() + "\\Logs\\AppDatabaseLoggerFile.log";

        readonly private static HashSet<string> testLine = new HashSet<string>();


        public static void LogConsole(string line)
        {
            SettingEnviroment();
            LoggerType typeLog = AppDatabase.Instance.loggerType;

            switch (typeLog) {

                case LoggerType.ONLY_CONSOLE: 

                    Debug.Log(line);

                    break;
                case LoggerType.ONLY_FILE:

                     WriteLine(line);

                    break;

                case LoggerType.DEBUGGER:

                    Debug.Log(line);
                    WriteLine(line);

                    break;
                case LoggerType.NONE:

                    // non logger anithings

                    break;
            }
           
        }

        private static void SettingEnviroment()
        {
            //validar si el file existe 
            // si no existe crear la ruta y el file 
            if (!File.Exists(DirectoryPacth))
            {
                System.IO.Directory.CreateDirectory(DirectoryPacth);
            }
            //----------------------------------------------------------------------
            // creation file if not exist 
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }
            //---------------------------------------------------------------------
        }


        private static void WriteLine(string line)
        {
            testLine.Clear();
            // read de current context 
            string dateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            using (StreamReader sr = File.OpenText(FilePath))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    testLine.Add(s);
                }
                sr.Close();
            }

            testLine.Add("Time: " + dateTime + " class :  AppDatabase.class : " + line);

            // write in file 
            using (StreamWriter fs = File.CreateText(FilePath))
            {
                // Add some text to file   
                foreach (string item in testLine)
                {
                    fs.WriteLine(item);
                }
                fs.Close();
            }
        }

        

    }
}