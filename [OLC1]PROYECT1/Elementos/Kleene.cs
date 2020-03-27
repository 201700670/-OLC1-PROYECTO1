using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;
namespace _OLC1_PROYECT1.Elementos
{
    class Kleene: Automata
    {
        Automata AFN;
        public Kleene(Automata AFN1)
        {
            this.AFN = AFN1;
        }
        /*public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Estados(int n)
        {


            Estado nuevoInicio = new Estado(n);
            this.addEstados(nuevoInicio);
            this.setEstadoInicial(nuevoInicio);
            for (int i = 0; i < AFN.getEstados().Count(); i++)
            {
                Estado tmp = (Estado)AFN.getEstados().ElementAt(i);
                //Console.WriteLine("id      " + tmp.getId());
                tmp.setId(i + 1);
                this.addEstados(tmp);
            }
            for (int h = 0; h < AFN.getEstados().Count(); h++)
            {
                Estado tmp = (Estado)AFN.getEstados().ElementAt(h);
                foreach (Trancisiones tra in tmp.getTransiciones())
                {
                    int final = tra.getFin().getId();
                    if (tra.getInicio().getId() == final)
                    {
                        tra.getFin().setId(tra.getInicio().getId() + 1);
                    }
                }
            }

            Estado nuevoFin = new Estado(AFN.getEstados().Count() + 1);
            this.addEstados(nuevoFin);
            this.addEstadosAceptacion(nuevoFin);

            //definir estados clave para realizar la cerraduras
            Estado anteriorInicio = AFN.getEstadoInicial();

            LinkedList<Estado> anteriorFin = AFN.getEstadosAceptacion();

            // agregar transiciones desde el nuevo estado inicial
            Trancisiones tran = new Trancisiones();
            tran.Transicion(nuevoInicio, anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
            nuevoInicio.getTransiciones().AddLast(tran);
            Trancisiones tran2 = new Trancisiones();
            tran2.Transicion(nuevoInicio, nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
            nuevoInicio.getTransiciones().AddLast(tran2);

            // agregar transiciones desde el anterior estado final
            for (int i = 0; i < anteriorFin.Count(); i++)
            {
                Trancisiones tran3 = new Trancisiones();
                tran3.Transicion(anteriorFin.ElementAt(i), anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
                Trancisiones tran4 = new Trancisiones();
                tran4.Transicion(anteriorFin.ElementAt(i), nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
                anteriorFin.ElementAt(i).getTransiciones().AddLast(tran3);
                anteriorFin.ElementAt(i).getTransiciones().AddLast(tran4);
            }
            this.setAlfabeto(AFN.getAlfabeto());
            this.setLenguajeR(AFN.getLenguajeR());


            return this;


        }
            */
        
    }
}
