using System;
using DataLayer.PureData.Native;

namespace DataLayer.PureData.Managed
{
	/// <summary>
	/// Pd Patch.
	/// </summary>
	public sealed class PatchFile : IDisposable
	{
		readonly IntPtr _handle;

		internal PatchFile (IntPtr handle)
		{
			_handle = handle;
			DollarZero = General.getdollarzero (_handle);
		}

		~PatchFile ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void Dispose (bool disposing)
		{
			General.closefile (_handle);
		}

		/// <summary>
		/// Gets $0 of Pd patch.
		/// </summary>
		/// <value>The dollar zero.</value>
		public int DollarZero {	get; private set; }
	}

}
