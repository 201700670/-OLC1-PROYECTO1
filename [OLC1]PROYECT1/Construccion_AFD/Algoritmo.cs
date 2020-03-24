using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.Construccion_AFD
{
    class Algoritmo
    {
        private String resultado;

        public Algoritmo()
        {

        }

        public Algoritmo(Automata afn_simulacion, String regex)
        {
            simular(regex, afn_simulacion);
        }
        public bool simular(String regex, Automata automata)
        {
            Estado inicial = automata.getEstadoInicial();
            LinkedList<Estado> estados = automata.getEstados();
            LinkedList<Estado> aceptacion = new LinkedList<Estado>(automata.getEstadosAceptacion());

            HashSet<Estado> conjunto = eClosure(inicial);
            foreach (char ch in regex.ToCharArray())
            {
                conjunto = move(conjunto, ch.ToString());
                HashSet<Object> temp = new HashSet<Object>();
                IEnumerable<Estado> iter = conjunto.AsEnumerable();
                int a = 0;
                while (iter.Any())
                {
                    Estado siguiente = iter.ElementAt(a);
                    /**
                     * En esta parte es muy importante el metodo addAll
                     * porque se tiene que agregar el eClosure de todo el conjunto
                     * resultante del move y se utiliza un hashSet temporal porque
                     * no se permite la mutacion mientras se itera
                     */
                    temp.Add(eClosure(siguiente));
                    a++;

                }
                //conjunto = temp;


            }


            bool res = false;

            foreach (Estado estado_aceptacion in aceptacion)
            {
                if (conjunto.Contains(estado_aceptacion))
                {
                    res = true;
                }
            }
            if (res)
            {
                //System.out.println("Aceptado");
                //this.resultado = "Aceptado";
                return true;
            }
            else
            {
                //System.out.println("NO Aceptado");
                // this.resultado = "No Aceptado";
                return false;
            }
        }

        public HashSet<Estado> eClosure(Estado eClosureEstado)
        {
            Stack<Estado> pilaClosure = new Stack<Estado>();
            Estado actual = eClosureEstado;
            actual.getTransiciones();
            HashSet<Estado> resultado = new HashSet<Estado>();

            pilaClosure.Push(actual);
            while (pilaClosure.Count()!=0)
            {
                actual = pilaClosure.Pop();

                foreach (Trancisiones t in actual.getTransiciones())
                {

                    if (t.getSimbolo().Equals(AnalizadorTexto.AnalizadorTexto.EPSILON) && !resultado.Contains(t.getFin()))
                    {
                        resultado.Add(t.getFin());
                        pilaClosure.Push(t.getFin());
                    }
                }
            }
            resultado.Add(eClosureEstado); //la operacion e-Closure debe tener el estado aplicado
            return resultado;
        }


        public HashSet<Estado> move(HashSet<Estado> estados, String simbolo)
        {

            HashSet<Estado> alcanzados = new HashSet<Estado>();
            IEnumerable<Estado> iterador = estados.AsEnumerable();
            int a = 0;
            while (iterador.Any())
            {

                foreach (Trancisiones t in (LinkedList<Trancisiones>)iterador.ElementAt(a).getTransiciones())
                {
                    Estado siguiente = t.getFin();
                    String simb = (String)t.getSimbolo();
                    if (simb.Equals(simbolo))
                    {
                        alcanzados.Add(siguiente);
                    }

                }
                a++;
            }
            return alcanzados;

        }

    }
}
