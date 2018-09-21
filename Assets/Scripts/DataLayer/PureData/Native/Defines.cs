using System;
using System.Runtime.InteropServices;

namespace DataLayer.PureData.Native
{
	static class Defines
    {

        public const string DllName = "pd";

        public const CallingConvention CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;
	}
}