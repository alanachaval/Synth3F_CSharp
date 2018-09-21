using System;
using DomainLayer.Common;

namespace DomainLayer.Test
{
    public class TestConfig : IProcessorConfig
    {
        private IProcessor processor;

        void IDisposable.Dispose()
        {
        }

        public IProcessor GetProcessor()
        {
            return processor;
        }

        public void Init()
        {
            processor = new TestProcessor();
        }
    }
}