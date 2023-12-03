using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{
    public static class IngredientesExtension
    {
        public static double CalcularCostoIngrediente(this List<EIngrediente> ingredientes, int costoInicial)
        {
            // Implementar la lógica para calcular el costo con el porcentaje de los ingredientes
            double porcentajeTotal = 0;

            foreach (var ingrediente in ingredientes)
            {
                // Aquí deberías tener la lógica para obtener el porcentaje de cada ingrediente
                // y sumarlo al porcentajeTotal.
                porcentajeTotal += (int)ingrediente;
            }

            double costoIncrementado = costoInicial + (costoInicial * porcentajeTotal / 100);

            return costoIncrementado;
        }

        public static List<EIngrediente> IngredientesAleatorios(this Random random)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.JAMON,
                EIngrediente.ADHERESO,
                EIngrediente.PANCETA,
                EIngrediente.HUEVO
            };
            int cant = random.Next(1, ingredientes.Count+1);
            return ingredientes.Take(cant).ToList();
        }
    }
}
