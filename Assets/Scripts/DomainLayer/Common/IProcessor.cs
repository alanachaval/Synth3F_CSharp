using Entities;

namespace DomainLayer.Common
{
    /// <summary>
    /// Acceso para el uso de la herramienta de procesamiento del sonido.
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Elimina todos los patches recibidos.
        /// </summary>
        /// <param name="patches">patches a eliminar.</param>
        void Clear(Patch[] patches);

        /// <summary>
        /// Connecta los patches entre el inlet y outlet recibidos.
        /// </summary>
        /// <param name="connection">Datos de la conexion.</param>
        void Connect(Connection connection);

        /// <summary>
        /// Desconecta los patches entre el inlet y outlet recibidos.
        /// </summary>
        /// <param name="connection">Datos de la conexion.</param>
        void Disconnect(Connection connection);

        /// <summary>
        /// Crea el patch recibido con los parametros inicializados.
        /// </summary>
        /// <param name="patch">Patch a crear con sus respectivos valores.</param>
        void CreatePatch(Patch patch);

        /// <summary>
        /// Elimina el patch y sus conexiones.
        /// </summary>
        /// <param name="patch">Patch a eliminar.</param>
        void Delete(Patch patch);

        /// <summary>
        /// Crea las conexiones recibidas.
        /// </summary>
        /// <param name="connections">Array de coneciones a crear.</param>
        void LoadConnections(Connection[] connections);

        /// <summary>
        /// Crea los patches recibidos.
        /// </summary>
        /// <param name="patches">Array de patches a crear.</param>
        void LoadPatches(Patch[] patches);

        /// <summary>
        /// Setea el valor del parametro solicitado al patch en la herramienta de procesamiento del sonido.
        /// </summary>
        /// <param name="patch">Patch a modificar con el valor del parametro modificado.</param>
        /// <param name="parameter">Nombre del parametro a modificar.</param>
        void SetValue(Patch patch, string parameter);

        /// <summary>
        /// Procesa la seccion del sonido solicitada.
        /// </summary>
        /// <param name="data">
        /// Array sobre el cual se escribiran los datos del sonido.
        /// Distribucion de los canales:
        /// [(C1,1),(C2,1),...,(Cn,1),(C1,2),(C2,2),...,(Cn,2),...,(C1,m),(C2,m),...,(Cn,m)].
        /// </param>
        /// <param name="channels">Cantidad de canales a procesar</param>
        void Process(float[] data, int channels);
    }
}
