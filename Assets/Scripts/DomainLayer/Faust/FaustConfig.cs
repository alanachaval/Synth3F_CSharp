using Assets.Scripts.DataLayer.Faust;
using DomainLayer.Common;
using System;
using UnityEngine;

namespace DomainLayer.Faust
{
    public class FaustConfig : IProcessorConfig
    {
        private FaustApi faustApi;
        private FaustProcessor faustProcessor;

        IProcessor IProcessorConfig.GetProcessor()
        {
            return faustProcessor;
        }

        void IProcessorConfig.Init()
        {
            faustApi = new FaustApi();
            int bufferSize;
            int noOfBuffers;
            AudioSettings.GetDSPBufferSize(out bufferSize, out noOfBuffers);
            faustApi.Init(2, bufferSize, AudioSettings.outputSampleRate);
            faustProcessor = new FaustProcessor();
            faustProcessor.SetFaustApi(faustApi);
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (faustApi != null && faustApi.IsInitialized())
            {
                faustApi.Dispose();
            }
        }

        ~FaustConfig()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void System.IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
