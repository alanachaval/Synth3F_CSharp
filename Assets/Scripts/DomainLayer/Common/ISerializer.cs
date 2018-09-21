namespace DomainLayer.Common
{
    /// <summary>
    /// Persiste y recupera los patches.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Crea un IPatchGraphManager con los datos del archivo.
        /// La ruta del archivo la obtiene de la configuracion.
        /// </summary>
        /// <param name="filename">Nombre del Archivo (sin el path).</param>
        /// <returns></returns>
        IPatchGraphManager Load(string filename);

        /// <summary>
        /// Crea o sobrescribe el archivo con los datos del IPatchGraphManager recibido.
        /// La ruta del archivo la obtiene de la configuracion.
        /// </summary>
        /// <param name="patchGraphManager">IPatchGraphManager a guardar.</param>
        /// <param name="filename">Nombre del Archivo (sin el path).</param>
        void Save(IPatchGraphManager patchGraphManager, string filename);
    }
}
