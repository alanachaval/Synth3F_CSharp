using System;

namespace DomainLayer.Common
{
    /// <summary>
    /// Inicializa y configura la herramienta de procesamiento del sonido.
    /// Ademas debe implementar Dispose para liberar todos los recursos utilizados.
    /// </summary>
    public interface IProcessorConfig : IDisposable
    {
        /// <summary>
        /// Retorna el IProcessor.
        /// </summary>
        /// <returns>IProccesor inicializado.</returns>
        IProcessor GetProcessor();

        /// <summary>
        /// Inicializa la herramienta de procesamiento.
        /// </summary>
        void Init();
    }
}
