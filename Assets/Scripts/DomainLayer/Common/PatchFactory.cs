using Entities;
using System.Collections.Generic;

namespace DomainLayer.Common
{
    public class PatchFactory : IPatchFactory
    {
        private IDictionary<string, IDictionary<string, float>> patchTemplates;

        Patch IPatchFactory.CreatePatch(string patchCode)
        {
            return new Patch()
            {
                Code = patchCode,
                Values = new SortedList<string, float>(patchTemplates[patchCode]),
            };
        }

        void IPatchFactory.Init()
        {
            patchTemplates = new SortedList<string, IDictionary<string, float>>
            {
                {
                    "dac",
                    new SortedList<string, float>
                    {
                        { "on_off", 1f },
                    }
                },
                {
                    "eg",
                    new SortedList<string, float>
                    {
                        { "on_off", 1f },
                        { "attack", 1f },
                        { "decay", 1f },
                        { "sustain", 1f },
                        { "release", 0f },
                        { "gate", 1f },
                    }
                },
                {
                    "vca",
                    new SortedList<string, float>
                    {
                        { "on_off", 1f },
                        { "att_control", 1f },
                        { "base", 1f },
                        { "clip", 0f },
                    }
                },
                {
                    "vcf",
                    new SortedList<string, float>
                    {
                        { "on_off", 1f },
                        { "att_signal", 100f },
                        { "att_freq", 100f },
                        { "mode", 0f },
                        { "freq", 261.626f },
                        { "q", 1f }
                    }
                },
                {
                    "vco",
                    new SortedList<string, float>
                    {
                        { "on_off", 1f },
                        { "att_freq0", 1f },
                        { "att_freq1", 1f },
                        { "att_pw", 1f },
                        { "shape", 0f },
                        { "freq", 261.626f },
                        { "offset", 0f },
                        { "pw", 50f }
                    }
                }
            };
        }
    }
}
