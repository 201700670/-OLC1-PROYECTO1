using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _OLC1_PROYECT1.CreandoArbol;
using _OLC1_PROYECT1.Listas;
namespace _OLC1_PROYECT1
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            ///er1 -> . {letra} *|{letra} |  {digito} "_";
            Thompson a = new Thompson();
            LinkedList<DatoExpresion> n = new LinkedList<DatoExpresion>();
            n.AddLast(new DatoExpresion("+", DatoExpresion.TipoExpresion.OPERADOR, "or" + 2));
            /*n.AddLast(new DatoExpresion(".", DatoExpresion.TipoExpresion.OPERADOR, "punto" + 1));
            n.AddLast(new DatoExpresion("*", DatoExpresion.TipoExpresion.OPERADOR, "asterisco" + 1));*/
            n.AddLast(new DatoExpresion("letra", DatoExpresion.TipoExpresion.CONJUNTOS, "letra" + 1));
            //n.AddLast(new DatoExpresion("digito", DatoExpresion.TipoExpresion.CONJUNTOS, "digito" + 1));
            /*n.AddLast(new DatoExpresion("|", DatoExpresion.TipoExpresion.OPERADOR, "or" + 1));
            n.AddLast(new DatoExpresion("letra", DatoExpresion.TipoExpresion.CONJUNTOS, "letra" + 2));
            n.AddLast(new DatoExpresion("|", DatoExpresion.TipoExpresion.OPERADOR, "or" + 2));
            n.AddLast(new DatoExpresion("digito", DatoExpresion.TipoExpresion.CONJUNTOS, "digito" + 1));
            n.AddLast(new DatoExpresion("_", DatoExpresion.TipoExpresion.CADENA, "cad" + 1));*/
            a.Analizarentrada(n);

        }
    }
}
