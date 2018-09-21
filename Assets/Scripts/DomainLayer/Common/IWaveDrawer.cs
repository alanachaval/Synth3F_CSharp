namespace DomainLayer.Common
{
    /// <summary>
    /// Muestra al usuario las ondas generadas.
    /// </summary>
    public interface IWaveDrawer
    {
        /// <summary>
        /// Inicializa con la cantidad de ondas solicitadas.
        /// </summary>
        /// <param name="channels">Cantidad de canales.</param>
        void Init(int channels);

        /// <summary>
        /// Carga una nueva seccion para mostrar.
        /// </summary>
        /// <param name="data">
        /// Array con los datos del sonido.
        /// Distribucion de los canales:
        /// [(C1,1),(C2,1),...,(Cn,1),(C1,2),(C2,2),...,(Cn,2),...,(C1,m),(C2,m),...,(Cn,m)].
        /// </param>
        void LoadWave(float[] data);
    }
}