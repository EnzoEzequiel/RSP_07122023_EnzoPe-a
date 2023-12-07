using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoPedido<T>(T menu);
    public class Cocinero<T> where T : IComestible, new()
    {
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoNuevoPedido<T> OnIngreso;
        private int cantPedidosFinalizados;
        private string nombre;
        private T pedidoEnPreparacion;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private Task tarea;
        private T menu;

        private Mozo<T> mozo;
        private Queue<T> pedidos;

        public Cocinero(string nombre) 
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();
            //this.mozo.OnPedido += TomarNuevoPedido;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.mozo.EmpezarAtrabajar;
                //    return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                //        this.tarea.Status == TaskStatus.WaitingToRun ||
                //        this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                //if (value && !this.HabilitarCocina)
                //{
                //    this.cancellation = new CancellationTokenSource();
                //    this.EmpezarACocinar();
                //}
                //else
                //{
                //    this.cancellation.Cancel();
                //    this.mozo.EmpezarAtrabajar = false;
                //}
                if (value)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.mozo.EmpezarAtrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation?.Cancel();
                    this.mozo.EmpezarAtrabajar = false;
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public Queue<T> Pedidos { get => Pedidos; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }
  
        private void EmpezarACocinar()
        {
            this.mozo.EmpezarAtrabajar = true;

            CancellationToken token = this.cancellation.Token;

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    this.mozo.EmpezarAtrabajar = true;
                    TomarNuevoPedido(this.menu);
                    this.EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;

                    try
                    {
                        DataBaseManager.GuardarTicket(this.Nombre, this.pedidoEnPreparacion);
                    }
                    catch (DataBaseManagerException ex)
                    {
                        throw new DataBaseManagerException("Error al guardar el ticket", ex.InnerException);
                    }
                }
            }, token);
        }

        private void TomarNuevoPedido(T menu)
        {
            if (this.OnIngreso is not null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnIngreso.Invoke(this.menu);
            }
        }
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;
            
            while (this.OnDemora is not null && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                this.OnDemora.Invoke(tiempoEspera);
                Thread.Sleep(1000);
            }

            this.demoraPreparacionTotal += tiempoEspera;

        }
    }
}
