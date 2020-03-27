using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;
using Lucene.Net.Support;

namespace _OLC1_PROYECT1.Construccion_AFD
{
    class AFD
    {
        private Automata afd;
        private Automata afdDirecto;
        private Automata afdMinimo;
        private Algoritmo simulador;
        private  HashMap<String, String > resultadoFollowPos;


        public AFD()
        {
            this.resultadoFollowPos = new HashMap<string, string>();
            simulador = new Algoritmo();
            afd = new Automata();
        }
        /**
   * Conversion de un automata AFN a uno AFD por el
   * metodo de subconjuntos
   * @param afn AFN
   */
        public void conversionAFN(Automata afn)
        {
            //se crea una estructura vacia
            Automata automata = new Automata();
            //se utiliza una cola como la estructura para guardar los subconjuntos a analizar
            Queue<HashSet<Estado>> cola = new Queue<HashSet<Estado>>();
            //se crea un nuevo estado inicial
            Estado inicial = new Estado(0);
            automata.setEstadoInicial(inicial);
            automata.addEstados(inicial);


            //el algoritmo empieza con el e-Closure del estado inicial del AFN
            HashSet<Estado> array_inicial = simulador.eClosure(afn.getEstadoInicial());
            //si el primer e-closure contiene estados de aceptacion hay que agregarlo
            foreach (Estado aceptacion in afn.getEstadosAceptacion())
            {
                if (array_inicial.Contains(aceptacion))
                    automata.addEstadosAceptacion(inicial);
            }

            //lo agregamos a la pila
            cola.Enqueue(array_inicial);
            //variable temporal para guardar el resultado todos los subconjuntos creados
            LinkedList<HashSet<Estado>> temporal = new LinkedList<HashSet<Estado>>();
            //se utilizan esetos indices para saber el estado actuales y anterior
            int indexEstadoInicio = 0;
            while (!cola.Any())
            {
                //actual subconjunto
                HashSet<Estado> actual = cola.Dequeue();
                //se recorre el subconjunto con cada simbolo del alfabeto del AFN
                foreach (String simbolo in afn.getAlfabeto())
                {
                    //se realiza el move con el subconjunto
                    HashSet<Estado> move_result = simulador.move(actual, (String)simbolo);

                    HashSet<Estado> resultado = new HashSet<Estado>();
                    //e-Closure con cada estado del resultado del move y 
                    //se guarda en un solo array (merge)
                    foreach (Estado e in move_result)
                    {
                        //resultado.Add(simulador.eClosure(e));
                    }

                    Estado anterior = (Estado)automata.getEstados().ElementAt(indexEstadoInicio);
                    /*Si el subconjunto ya fue creado una vez, solo se agregan
                    transiciones al automata*/
                    if (temporal.Contains(resultado))
                    {
                        LinkedList<Estado> array_viejo = automata.getEstados();
                        Estado estado_viejo = anterior;
                        //se busca el estado correspondiente y se le suma el offset
                        Estado estado_siguiente = array_viejo.ElementAt(temporal.ElementAt(resultado.Count()).Count() + 1);
                        Trancisiones trancision = new Trancisiones();
                        trancision.Transicion(estado_viejo, estado_siguiente, simbolo);
                        estado_viejo.setTransiciones(trancision);

                    }
                    //si el subconjunto no existe, se crea un nuevo estado
                    else
                    {
                        temporal.AddLast(resultado);
                        cola.Enqueue(resultado);

                        Estado nuevo = new Estado(temporal.ElementAt(resultado.Count()).Count() + 1);
                        Trancisiones transicion = new Trancisiones();
                        transicion.Transicion(anterior, nuevo, simbolo);
                        anterior.setTransiciones(transicion);
                        automata.addEstados(nuevo);
                        //se verifica si el estado tiene que ser de aceptacion
                        foreach (Estado aceptacion in afn.getEstadosAceptacion())
                        {
                            if (resultado.Contains(aceptacion))
                                automata.addEstadosAceptacion(nuevo);
                        }
                    }


                }
                indexEstadoInicio++;

            }

            this.afd = automata;
            //metodo para definir el alfabeto, se copia el del afn
            //definirAlfabeto(afn);
            this.afd.setTipo("AFD");
            Console.WriteLine(afd);
        }


    }
}
