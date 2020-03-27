using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _OLC1_PROYECT1.CreandoArbol;
using _OLC1_PROYECT1.Listas;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.Construccion_AFD;
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
            Application.Run(new Form1());
            */


            Automata nuevo = new Automata();
            Estado S0 = new Estado(0);
            Estado S1 = new Estado(1);
            Estado S2 = new Estado(2);
            ////
            Trancisiones S0a = new Trancisiones();
            S0a.setInicio(S0);
            S0a.setFin(S1);
            S0a.setSimbolo("a");
            S0.setTransiciones(S0a);
            Trancisiones S0b = new Trancisiones();
            S0b.setInicio(S0);
            S0b.setFin(S0);
            S0b.setSimbolo("a");
            S0.setTransiciones(S0b);
            Trancisiones S0c = new Trancisiones();
            S0c.setInicio(S0);
            S0c.setFin(S1);
            S0c.setSimbolo("b");
            S0.setTransiciones(S0c);
            nuevo.addEstados(S0);
            /////
            Trancisiones S1a = new Trancisiones();
            S1a.setInicio(S1);
            S1a.setFin(S2);
            S1a.setSimbolo("a");
            S1.setTransiciones(S1a);
            Trancisiones S1b = new Trancisiones();
            S1b.setInicio(S1);
            S1b.setFin(S1);
            S1b.setSimbolo("b");
            S1.setTransiciones(S1b);
            Trancisiones S1c = new Trancisiones();
            S1c.setInicio(S1);
            S1c.setFin(S2);
            S1c.setSimbolo("b");
            S1.setTransiciones(S1c);
            nuevo.addEstados(S1);
            ///
            Trancisiones S2a = new Trancisiones();
            S2a.setInicio(S2);
            S2a.setFin(S1);
            S2a.setSimbolo("a");
            S2.setTransiciones(S2a);
            Trancisiones S2b = new Trancisiones();
            S2b.setInicio(S2);
            S2b.setFin(S0);
            S2b.setSimbolo("a");
            S2.setTransiciones(S2b);
            Trancisiones S2c = new Trancisiones();
            S2c.setInicio(S2);
            S2c.setFin(S2);
            S2c.setSimbolo("b");
            S2.setTransiciones(S2c);
            Trancisiones S2d = new Trancisiones();
            S2d.setInicio(S2);
            S2d.setFin(S1);
            S2d.setSimbolo(AnalizadorTexto.AnalizadorTexto.EPSILON);
            S2.setTransiciones(S2d);
            nuevo.addEstados(S2);
            ///
            nuevo.setEstadoInicial(S0);
            nuevo.addEstadosAceptacion(S2);
            Hashtable alfabeto = new Hashtable();
            alfabeto.Add("a", 13);
            alfabeto.Add("b", 22);
            alfabeto.Add(AnalizadorTexto.AnalizadorTexto.EPSILON, 18);
            nuevo.setAlfabeto(alfabeto);
            Transformador AFD = new Transformador();
            AFD_GRAFICA automata = (AFD_GRAFICA)AFD.minimizar(nuevo);
            automata.CreandoTabla();
            String tabla2 = "";
           

            ///er1 -> . {letra} *|{letra} |  {digito} "_";
            /*Thompson a = new Thompson();
            LinkedList<DatoExpresion> n = new LinkedList<DatoExpresion>();
            n.AddLast(new DatoExpresion("+", DatoExpresion.TipoExpresion.OPERADOR, "punto" + 1));
            n.AddLast(new DatoExpresion("letra", DatoExpresion.TipoExpresion.CONJUNTOS, "letra" + 1));*/
            //n.AddLast(new DatoExpresion("digito", DatoExpresion.TipoExpresion.CONJUNTOS, "digito" + 1));
            /*n.AddLast(new DatoExpresion("|", DatoExpresion.TipoExpresion.OPERADOR, "or" + 1));
            n.AddLast(new DatoExpresion("letra", DatoExpresion.TipoExpresion.CONJUNTOS, "letra" + 2));
            n.AddLast(new DatoExpresion("|", DatoExpresion.TipoExpresion.OPERADOR, "or" + 2));
            n.AddLast(new DatoExpresion("digito", DatoExpresion.TipoExpresion.CONJUNTOS, "digito" + 1));
            n.AddLast(new DatoExpresion("_", DatoExpresion.TipoExpresion.CADENA, "cad" + 1));*/
            //a.Analizarentrada(n);

        }
    }
}
