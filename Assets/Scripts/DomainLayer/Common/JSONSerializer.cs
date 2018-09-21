using Constants;
using System.Runtime.Serialization.Json;
using System.IO;
using UnityEngine;
using System;
using System.Text;

namespace DomainLayer.Common
{
    public class JSONSerializer : ISerializer
    {
        IPatchGraphManager ISerializer.Load(string filename)
        {
            IPatchGraphManager patchGraphManager;
            string json;
            try
            {
                Tracer.Log("Application.persistentDataPath + Others.SavesFolder + filename" + Application.persistentDataPath + Others.SavesFolder + filename);
                json = File.ReadAllText(Application.persistentDataPath + Others.SavesFolder + filename);
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(PatchGraphManager));
                    patchGraphManager = (PatchGraphManager)deserializer.ReadObject(memoryStream);
                }
            }
            catch (Exception e)
            {
                Tracer.Log(e.Message + e.StackTrace);
                patchGraphManager = new PatchGraphManager();
            }
            return patchGraphManager;
        }

        void ISerializer.Save(IPatchGraphManager patchGraphManager, string filename)
        {
            string json;
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(patchGraphManager.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, patchGraphManager);
                memoryStream.Position = 0;
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    json = streamReader.ReadToEnd();
                }
            }
            try
            {
                Tracer.Log("Application.persistentDataPath + Others.SavesFolder + filename" + Application.persistentDataPath + Others.SavesFolder + filename);
                File.WriteAllText(Application.persistentDataPath + Others.SavesFolder + filename, json, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Tracer.Log(e.Message + e.StackTrace);
            }
        }
    }
}