using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.Elementos
{
    class Simple : Automata
    {
        String simple;
        int n1, n2;
        public Simple(String AFN1)
        {
            this.simple = AFN1;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Estados(int n)
        {
            
            this.n1 = n+1;
            Estado inicial = new Estado(n);
            Estado aceptacion = new Estado(n1);
            Trancisiones tran = new Trancisiones();
            tran.setInicio(inicial);
            tran.setFin(aceptacion);
            tran.setSimbolo(simple);
            inicial.setTransiciones(tran);
            //agrega los estados creados
            ElementoAutomata AFN_SIMPLE = new ElementoAutomata();
            this.addEstados(inicial);
            this.addEstados(aceptacion);
            //colocar los estados iniciales y de acpetacion
            this.setEstadoInicial(inicial);
            this.addEstadosAceptacion(aceptacion);
            this.setLenguajeR(simple + "");
            //Console.WriteLine(tran.DOT_String());
            return this;
        }
    }
}
