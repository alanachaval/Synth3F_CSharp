using System.Diagnostics;

namespace Constants
{
    public class Tracer
    {
        [Conditional("USE_LOGS")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}