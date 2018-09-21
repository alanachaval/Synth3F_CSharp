using UnityEngine;
using System;
using System.IO;
using DataLayer.PureData;
using DomainLayer.Common;
using DataLayer.PureData.Managed;
using Constants;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace DomainLayer.PureData
{
    public class PureDataConfig : IProcessorConfig
    {
        private int patchSystemId;
        private int numberOfTicks;
        private PureDataProcessor pureDataProcessor;

        IProcessor IProcessorConfig.GetProcessor()
        {
            return pureDataProcessor;
        }

        void IProcessorConfig.Init()
        {
            string mainPatchName = "empty.pd";
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidForceLoad();
#endif
            ResolvePath();
            OpenPd();
            patchSystemId = LoadPatch(mainPatchName);
            LibPD.ComputeAudio(true);
            pureDataProcessor = new PureDataProcessor();
            pureDataProcessor.Init(mainPatchName, numberOfTicks, new Messaging());
        }

        private void ResolvePath()
        {
            string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
            string dllPath = Application.dataPath + "/Plugins";
            dllPath.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (!currentPath.Contains(dllPath))
            {
                Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
            }
        }

        private int OpenPd()
        {
            int bufferSize;
            int noOfBuffers;
            AudioSettings.GetDSPBufferSize(out bufferSize, out noOfBuffers);
            numberOfTicks = bufferSize / LibPD.BlockSize;
            int unitySR = AudioSettings.outputSampleRate;
            return LibPD.OpenAudio(2, 2, unitySR);
        }

        private int LoadPatch(string patchName)
        {
            string assetsPath = Application.streamingAssetsPath + "/PdAssets/";
            string path = assetsPath + patchName;
#if UNITY_ANDROID && !UNITY_EDITOR
		    path = JavaLoadPatch(path, patchName);
#endif
            return LibPD.OpenPatch(path);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                LibPD.ClosePatch(patchSystemId);
                LibPD.Release();

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PureDataConfig() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

#if UNITY_ANDROID && !UNITY_EDITOR
        private string JavaLoadPatch(string path, string patchName)
        {
            string patchJar = Application.persistentDataPath + "/" + patchName;
            Tracer.Log("patchJar" + patchJar);
            if (File.Exists(patchJar))
            {
                Tracer.Log("Patch already unpacked");
                File.Delete(patchJar);

                if (File.Exists(patchJar))
                {
                    Tracer.Log("Couldn't delete");
                }
            }

            WWW dataStream = new WWW(path);


            // Hack to wait till download is done
            while (!dataStream.isDone)
            {
            }

            if (!String.IsNullOrEmpty(dataStream.error))
            {
                Tracer.Log("### WWW ERROR IN DATA STREAM:" + dataStream.error);
            }

            File.WriteAllBytes(patchJar, dataStream.bytes);

            return patchJar;

        }

        private void AndroidForceLoad()
        {
            try { ForceLoadpd(); } catch (Exception) { }
            try { ForceLoadbob_tilde(); } catch (Exception) { }
            try { ForceLoadbonk_tilde(); } catch (Exception) { }
            try { ForceLoadchoice(); } catch (Exception) { }
            try { ForceLoadfiddle_tilde(); } catch (Exception) { }
            try { ForceLoadloop_tilde(); } catch (Exception) { }
            try { ForceLoadlrshift_tilde(); } catch (Exception) { }
            try { ForceLoadpdnative(); } catch (Exception) { }
            try { ForceLoadpdnativeopensl(); } catch (Exception) { }
            try { ForceLoadpique(); } catch (Exception) { }
            try { ForceLoadsigmund_tilde(); } catch (Exception) { }
            try { ForceLoadstdout(); } catch (Exception) { }

            string assetsPath = Application.streamingAssetsPath + "/PdAssets/";

            string path;

            string[] patches =
            {
                "attenuator.pd",
                "param.pd",
                "x_dac.pd",
                "x_eg.pd",
                "x_fade.pd",
                "x_vca.pd",
                "x_vcf.pd",
                "x_vco.pd"
            };
            foreach (string patchName in patches)
            {
                path = assetsPath + patchName;
                JavaLoadPatch(path, patchName);
            }
        }

        [DllImport("pd")]
        static extern private void ForceLoadpd();
        [DllImport("bob_tilde")]
        static extern private void ForceLoadbob_tilde();
        [DllImport("bonk_tilde")]
        static extern private void ForceLoadbonk_tilde();
        [DllImport("choice")]
        static extern private void ForceLoadchoice();
        [DllImport("fiddle_tilde")]
        static extern private void ForceLoadfiddle_tilde();
        [DllImport("loop_tilde")]
        static extern private void ForceLoadloop_tilde();
        [DllImport("lrshift_tilde")]
        static extern private void ForceLoadlrshift_tilde();
        [DllImport("pdnative")]
        static extern private void ForceLoadpdnative();
        [DllImport("pdnativeopensl")]
        static extern private void ForceLoadpdnativeopensl();
        [DllImport("pique")]
        static extern private void ForceLoadpique();
        [DllImport("sigmund_tilde")]
        static extern private void ForceLoadsigmund_tilde();
        [DllImport("stdout")]
        static extern private void ForceLoadstdout();
#endif
    }
}