using DataLayer.PureData.Managed.Data;
using DomainLayer.Common;
using Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace DomainLayer.PureData
{
    public class PureDataProcessorTest
    {
        private MessagingTest messaging;
        private IProcessor processor;
        IPatchFactory patchFactory;
        private string testPatch = "testPatch";

        [SetUp]
        public void SetUp()
        {
            messaging = new MessagingTest();
            processor = new PureDataProcessor();
            ((PureDataProcessor)processor).Init(testPatch, 0, messaging);
            patchFactory = new PatchFactory();
            patchFactory.Init();
        }

        [Test]
        public void CreatePatch()
        {
            Patch patch = new Patch()
            {
                Id = 1,
                Code = "vco",
                Values = new SortedList<string, float>()
            };
            processor.CreatePatch(patch);
            Assert.AreEqual(1, messaging.Messages.Count);
            Assert.AreEqual("pd-" + testPatch, messaging.Messages[0].Receiver);
            Assert.AreEqual("x_vco", messaging.Messages[0].Atoms[2].Value);
            Assert.AreEqual("x_vco_1", messaging.Messages[0].Atoms[3].Value);
        }

        [Test]
        public void SendValue()
        {
            Patch patch = new Patch()
            {
                Id = 1,
                Code = "vco",
                Values = new Dictionary<string, float>() { { "freq", 250 } }
            };
            processor.CreatePatch(patch);
            Assert.AreEqual(2, messaging.Messages.Count);
            Assert.IsNull(messaging.Messages[1].Message);
            Assert.AreEqual("x_vco_1_freq", messaging.Messages[1].Receiver);
            Assert.AreEqual(250f, messaging.Messages[1].Atoms[0].Value);
        }

        [Test]
        public void Connect()
        {
            Patch vco = new Patch()
            {
                Id = 1,
                Code = "vco",
                Values = new SortedList<string, float>()
            };
            Patch dac = new Patch()
            {
                Id = 2,
                Code = "dac",
                Values = new SortedList<string, float>()
            };
            Connection connection = new Connection()
            {
                Id = 3,
                SourcePatch = 1,
                SourceOutlet = 0,
                TargetPatch = 2,
                TargetInlet = 1
            };
            processor.CreatePatch(vco);
            processor.CreatePatch(dac);
            processor.Connect(connection);
            TestConnection(messaging.Messages.Count - 5, 3, 2, 0, 0, 1, 1);
        }

        [Test]
        public void Delete()
        {
            Patch vco = new Patch()
            {
                Id = 1,
                Code = "vco",
                Values = new SortedList<string, float>()
            };
            Patch dac = new Patch()
            {
                Id = 2,
                Code = "dac",
                Values = new SortedList<string, float>()
            };
            Connection connection = new Connection()
            {
                Id = 3,
                SourcePatch = 1,
                SourceOutlet = 0,
                TargetPatch = 2,
                TargetInlet = 1
            };
            processor.CreatePatch(vco);
            processor.CreatePatch(dac);
            processor.Connect(connection);
            processor.Delete(vco);
            Assert.AreEqual("pd-" + testPatch, messaging.Messages[6].Receiver);
            Assert.AreEqual("x_vco_1", messaging.Messages[6].Atoms[0].Value);
        }

        [Test]
        public void DeleteComplete()
        {
            Patch vco = patchFactory.CreatePatch("vco");
            vco.Id = 1;
            Patch dac = patchFactory.CreatePatch("dac");
            dac.Id = 2;

            processor.CreatePatch(vco);
            processor.CreatePatch(dac);

            Connection connection = new Connection
            {
                Id = 3,
                SourcePatch = 1,
                SourceOutlet = 0,
                TargetPatch = 2,
                TargetInlet = 1
            };

            processor.Connect(connection);
            vco.AddOutputConnection(connection);
            dac.AddInputConnection(connection);

            processor.Delete(vco);
            processor.Delete(dac);

            processor.CreatePatch(vco);
            processor.CreatePatch(dac);

            processor.Connect(connection);
            vco.AddOutputConnection(connection);
            dac.AddInputConnection(connection);
            TestConnection(messaging.Messages.Count - 5, 3, 2, 0, 0, 1, 1);
        }

        private void TestConnection(int offset, int fadeId, int fadePdId, float sourcePatchId, float sourceOuletId, float targetPatchId, float targetInletId)
        {
            IAtom[] values;
            Assert.AreEqual("pd-" + testPatch, messaging.Messages[offset + 1].Receiver);
            Assert.AreEqual("x_fade", messaging.Messages[offset + 1].Atoms[2].Value);
            Assert.AreEqual("x_fade_" + fadeId, messaging.Messages[offset + 1].Atoms[3].Value);
            Assert.AreEqual("pd-" + testPatch, messaging.Messages[offset + 2].Receiver);
            Assert.AreEqual("connect", messaging.Messages[offset + 2].Message);
            Assert.AreEqual("pd-" + testPatch, messaging.Messages[offset + 3].Receiver);
            Assert.AreEqual("connect", messaging.Messages[offset + 3].Message);
            values = messaging.Messages[offset + 2].Atoms;
            Assert.AreEqual(sourcePatchId, values[0].Value, "sourcePatchId");
            Assert.AreEqual(sourceOuletId, values[1].Value, "sourceOuletId");
            Assert.AreEqual(fadePdId, values[2].Value, "fadePdId");
            Assert.AreEqual(0, values[3].Value);
            values = messaging.Messages[offset + 3].Atoms;
            Assert.AreEqual(fadePdId, values[0].Value, "fadePdId");
            Assert.AreEqual(0, values[1].Value);
            Assert.AreEqual(targetPatchId, values[2].Value, "targetPatchId");
            Assert.AreEqual(targetInletId, values[3].Value, "targetInletId");
            Assert.AreEqual("x_fade_" + fadeId, messaging.Messages[offset + 4].Receiver);
            Assert.IsNull(messaging.Messages[offset + 4].Message);
            Assert.AreEqual(1f, messaging.Messages[offset + 4].Atoms[0].Value);
        }
    }
}