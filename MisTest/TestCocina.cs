using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            string data = "Datos de prueba";
            string nombreArchivo = ""; 

            //act
            FileManager.Guardar(data, nombreArchivo, false);

            //assert
            //se espera que se lance una excepción FileManagerException
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            string nombreCocinero = "John";
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>(nombreCocinero);

            //act
            int pedidosCero = cocinero.CantPedidosFinalizados;

            //assert
            Assert.AreEqual(0, pedidosCero);
        }
    }
}