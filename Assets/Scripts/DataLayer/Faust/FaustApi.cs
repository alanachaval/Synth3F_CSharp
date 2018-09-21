using System;
using System.Runtime.InteropServices;

namespace Assets.Scripts.DataLayer.Faust
{
    public class FaustApi
    {
        public const string DllName = "FaustApi";

        private IntPtr faustApiPointer = IntPtr.Zero;

        public void Init(int channels, int bufferSize, int sampleRate)
        {
            faustApiPointer = InitFaustApi(channels, bufferSize, sampleRate);
        }

        public void Dispose()
        {
            DisposeFaustApi(faustApiPointer);
            faustApiPointer = IntPtr.Zero;
        }

        public void AddPatch(string code, int id)
        {
            AddPatchFaustApi(faustApiPointer, code, id);
        }

        public void RemovePatch(int id)
        {
            RemovePatchFaustApi(faustApiPointer, id);
        }

        public void SetValue(string name, float value)
        {
            SetValueFaustApi(faustApiPointer, name, value);
        }

        public void Connect(int source, int outlet, int target, int inlet)
        {
            ConnectFaustApi(faustApiPointer, source, outlet, target, inlet);
        }

        public void Disconnect(int source, int outlet, int target, int inlet)
        {
            DisconnectFaustApi(faustApiPointer, source, outlet, target, inlet);
        }

        public void Process(float[] data, int channels)
        {
            ProcessFaustApi(faustApiPointer, data, channels);
        }

        public bool IsInitialized()
        {
            return faustApiPointer == IntPtr.Zero;
        }

        [DllImport(DllName)]
        static private extern IntPtr InitFaustApi(int channels, int buffer_size, int sample_rate);

        [DllImport(DllName)]
        static private extern void DisposeFaustApi(IntPtr faust_api_pointer);

        [DllImport(DllName)]
        static private extern void AddPatchFaustApi(IntPtr faust_api_pointer, [In] [MarshalAs(UnmanagedType.LPStr)] string code, int id);

        [DllImport(DllName)]
        static private extern void RemovePatchFaustApi(IntPtr faust_api_pointer, int id);

        [DllImport(DllName)]
        static private extern void SetValueFaustApi(IntPtr faust_api_pointer, [In] [MarshalAs(UnmanagedType.LPStr)]  string name, float value);

        [DllImport(DllName)]
        static private extern void ConnectFaustApi(IntPtr faust_api_pointer, int source, int outlet, int target, int inlet);

        [DllImport(DllName)]
        static private extern void DisconnectFaustApi(IntPtr faust_api_pointer, int source, int outlet, int target, int inlet);

        [DllImport(DllName)]
        static private extern void ProcessFaustApi(IntPtr faust_api_pointer, float[] data, int channels);
    }
}
