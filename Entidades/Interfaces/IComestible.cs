namespace Entidades.Interfaces
{
    public interface IComestible
    {
        // Propiedades
        bool Estado { get; }
        string Imagen { get; }
        string Ticket { get; }

        // Métodos
        void FinalizarPreparacion(string cocinero);
        void IniciarPreparacion();
        
    }
}
