using Entities;

namespace DomainLayer.Common
{
    /// <summary>
    /// Crea los patches con sus valores por default
    /// </summary>
    public interface IPatchFactory
    {
        /// <summary>
        /// Crea una instancia del patch
        /// </summary>
        /// <param name="patchCode">Codigo del tipo de patch a crear</param>
        /// <returns>Patch con sus valores inicializados</returns>
        Patch CreatePatch(string patchCode);

        /// <summary>
        /// Inicializa los valores por default segun su configuracion
        /// </summary>
        void Init();
    }
}