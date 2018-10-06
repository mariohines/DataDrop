﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataDrop
{

    public class DataController
    {
        private static Dictionary<string, string> stringHolders = new Dictionary<String, String>();
        private bool encrypt, persist, init;
        private static string name, fileName;
        public void Init(string nameInput, bool encryptInput, bool persistInput)
        {
            name = nameInput;
            encrypt = encryptInput;
            persist = persistInput;
            fileName = nameInput + ".dddb";
            if (File.Exists(fileName))
            {
                var lines = File.ReadLines(fileName);
                foreach (var line in lines)
                {
                    var temp = line.Split(',');
                    stringHolders.Add(temp[0], temp[1]);
                }
            }
            init = true;

        }
        public void Insert(string key, string value)
        {
            if (!init) { throw new Exception("Datacontroller not initialised, please use DataController.Init()"); return; }
            stringHolders[key] = "value";
            if (persist) { Save(); }
        }
        private static void Save()
        {
            foreach (KeyValuePair<string, string> kvp in stringHolders)
            {
                File.WriteAllText(fileName, string.Format("{0}, {1} {2}", kvp.Key, kvp.Value, Environment.NewLine));
            }
        }
        public void Delete(string key)
        {
            if (!init) { throw new Exception("Datacontroller not initialised, please use DataController.Init()"); }
            stringHolders.Remove(key);
            if (persist) { Save(); }
        }
        public string Lookup(string key) => stringHolders[key];
        public bool ValueCheck(string key, string expectedValue)
        {
            var actualValue = "";
            return stringHolders.TryGetValue(key, out actualValue) &&
                                actualValue.Equals(expectedValue);
        }
        public bool PresenceCheck(string key){
            var value = "";
                return stringHolders.TryGetValue(key, out value);
        }
        public void RebuildDatabase(){
            File.Delete(fileName);
            Save();
        }
        public void Drop(bool confirm){
            if (confirm){foreach (KeyValuePair<string, string> kvp in stringHolders)
                {
                    stringHolders.Remove(kvp.Key);
                    File.Delete(fileName);
                }}
        }
        }

    }

