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
            //afd = new Automata();
        }
    }
}
