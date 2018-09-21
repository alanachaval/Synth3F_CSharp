using UnityEngine;
using Entities;
using Constants;
using PresentationLayer.Presenters;
using System;
using System.Collections.Generic;

namespace DomainLayer.Common
{
    public class MainManager : MonoBehaviour
    {
        public PatchCreator patchCreator;
        public WaveDrawer waveDrawer;
        private IProcessorConfig processorConfig;
        private IProcessor processor;
        private IPatchGraphManager patchGraphManager;
        private IPatchFactory patchFactory;
        private ISerializer serializer;
        private float[] soundData;
        private int soundDataOffset = 0;

        private static MainManager instance;

        private MainManager() { }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                string[] config =
                {
                    //"Synth3F_CSharp", "DomainLayer.Test.TestConfig",
                    "Synth3F_CSharp", "DomainLayer.PureData.PureDataConfig",
                    //"Synth3F_CSharp", "DomainLayer.Faust.FaustConfig",
                    "Synth3F_CSharp", "DomainLayer.Common.PatchGraphManager",
                    "Synth3F_CSharp", "DomainLayer.Common.PatchFactory",
                    "Synth3F_CSharp", "DomainLayer.Common.XMLSerializer"
                };
                Init(config);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public Connection Connect(int sourcePatch, int sourceOutlet, int targetPatch, int targetInlet)
        {
            Connection connection = patchGraphManager.Connect(sourcePatch, sourceOutlet, targetPatch, targetInlet);
            processor.Connect(connection);
            return connection;
        }

        public Patch CreatePatch(string patchCode)
        {
            Patch patch = patchFactory.CreatePatch(patchCode);
            patchGraphManager.AddPatch(patch);
            processor.CreatePatch(patch);
            return patch;
        }

        public void Delete(Patch patch)
        {
            patchGraphManager.RemovePatch(patch.Id);
            processor.Delete(patch);
        }

        public static MainManager GetInstance()
        {
            return instance;
        }

        public void Init(string[] config)
        {
            MainFactory mainFactory = new MainFactory();
            processorConfig = mainFactory.Create<IProcessorConfig>(config[0], config[1]);
            processorConfig.Init();
            processor = processorConfig.GetProcessor();
            patchGraphManager = mainFactory.Create<IPatchGraphManager>(config[2], config[3]);
            patchFactory = mainFactory.Create<IPatchFactory>(config[4], config[5]);
            patchFactory.Init();
            serializer = mainFactory.Create<ISerializer>(config[6], config[7]);
            ((IWaveDrawer)waveDrawer).Init(Others.WavePoints);
            soundData = new float[Others.WavePoints * 2];
        }

        public void Load(string filename)
        {
            processor.Clear(patchGraphManager.GetPatches());
            patchGraphManager.Clear();
            patchCreator.Clear();
            WireDrawer.GetInstance().Clear();
            patchGraphManager = serializer.Load(filename);
            Patch[] patches = patchGraphManager.GetPatches();
            processor.LoadPatches(patches);
            patchCreator.Load(patches);
            Connection[] connections = patchGraphManager.GetConnections();
            processor.LoadConnections(connections);
            WireDrawer.GetInstance().Load(connections);
        }

        public void Save(string filename)
        {
            serializer.Save(patchGraphManager, filename);
        }

        public void SetValue(Patch patch, string parameter, float value)
        {
            patch.Values[parameter] = value;
            processor.SetValue(patch, parameter);
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            processor.Process(data, channels);
            data.CopyTo(soundData, soundDataOffset);
            soundDataOffset += data.Length;
            if (soundDataOffset == soundData.Length)
            {
                soundDataOffset = 0;
                ((IWaveDrawer)waveDrawer).LoadWave(soundData);
            }
        }

        void OnApplicationQuit()
        {
            processorConfig.Dispose();
        }
    }
}