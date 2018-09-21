using Entities;

namespace DomainLayer.Common
{
    /// <summary>
    /// Almacena la informacion de los patches y sus conexiones.
    /// </summary>
    public interface IPatchGraphManager
    {
        /// <summary>
        /// Almacena un nuevo patch y le asigna un ID.
        /// </summary>
        /// <param name="patch">Patch a almacenar.</param>
        /// <returns>ID del patch.</returns>
        int AddPatch(Patch patch);

        /// <summary>
        /// Elimina la informacion que contiene
        /// </summary>
        void Clear();

        /// <summary>
        /// Connecta los patches entre el inlet y outlet recibidos.
        /// Incluye la connexion en los conjuntos de entrada y salida de los patches correspondientes.
        /// </summary>
        /// <param name="sourcePatch">ID del patch de salida.</param>
        /// <param name="sourceOutlet">ID del la salida del patch.</param>
        /// <param name="targetPatch">ID del patch de entrada.</param>
        /// <param name="targetInlet">ID del la entrada del patch.</param>
        /// <returns>Conexion creada con los valores recibidos y una ID</returns>
        Connection Connect(int sourcePatch, int sourceOutlet, int targetPatch, int targetInlet);

        /// <summary>
        /// Elimina una conexion.
        /// Remueve la connexion de los conjuntos de entrada y salida de los patches correspondientes.
        /// </summary>
        /// <param name="connectionId">ID de la connexion a eliminar.</param>
        /// <returns>Conexion eliminada.</returns>
        Connection Disconnect(int connectionId);

        /// <summary>
        /// Retorna la conexion con la id solicitada.
        /// </summary>
        /// <param name="connectionId">ID de la connexion a obtener.</param>
        /// <returns>Conexion con la ID solicitada.</returns>
        Connection GetConnection(int connectionId);

        /// <summary>
        /// Retorna todas sus conexiones.
        /// </summary>
        /// <returns>Array con todas las conexiones.</returns>
        Connection[] GetConnections();

        /// <summary>
        /// Retorna el patch con la id solicitada.
        /// </summary>
        /// <param name="patchId">ID del patch a obtener.</param>
        /// <returns>Patch con la ID solicitada.</returns>
        Patch GetPatch(int patchId);

        /// <summary>
        /// Retorna todos sus patches.
        /// </summary>
        /// <returns>Array con todos los patches.</returns>
        Patch[] GetPatches();

        /// <summary>
        /// Elimina un patch.
        /// </summary>
        /// <param name="patchId">ID del patch a eliminar.</param>
        /// <returns>Patch eliminado</returns>
        Patch RemovePatch(int patchId);
    }
}