using System.Reflection;

namespace DomainLayer.Common
{
    public class MainFactory
    {
        public T Create<T>(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            T processorConfig = (T)assembly.CreateInstance(className);
            return processorConfig;
        }
    }
}
