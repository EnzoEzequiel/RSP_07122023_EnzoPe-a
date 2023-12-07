using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public class Mozo<T> where T : IComestible, new()
    {
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;
        public event DelegadoNuevoPedido<T> OnPedido;

        public bool EmpezarAtrabajar
        {
            get
            {
                return this.tarea != null &&
                       (this.tarea.Status == TaskStatus.Running ||
                        this.tarea.Status == TaskStatus.WaitingToRun ||
                        this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && (this.tarea == null ||
                    this.tarea.Status != TaskStatus.Running &&
                    this.tarea.Status != TaskStatus.WaitingToRun &&
                    this.tarea.Status != TaskStatus.WaitingForActivation))
                {
                    this.cancellation = new CancellationTokenSource();
                    this.tarea = Task.Run(() => TomarPedidos(), this.cancellation.Token);
                }
                else
                {
                    this.cancellation?.Cancel();
                }
            }
        }

        private async void TomarPedidos()
        {
            while (!cancellation.Token.IsCancellationRequested)
            {
                await Task.Delay(5000); 
                NotificarNuevoPedido();
            }
        }

        private void NotificarNuevoPedido()
        {
            DelegadoNuevoPedido<T> handler = OnPedido;
            if (handler != null)
            {
                T menu = new T();
                menu.IniciarPreparacion();
                this.OnPedido.Invoke(menu);
                handler(menu);
            }
        }
    }
}
