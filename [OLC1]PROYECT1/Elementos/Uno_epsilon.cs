using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.Elementos
{
    class Uno_epsilon: Automata
    {
        Automata simple;
        int n1, n2;
        public Uno_epsilon(Automata AFN1)
        {
            this.simple = AFN1;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Estados(int n)
        {
            //// se realiza el simple con el epsilon
            Automata simple = (Automata)new Simple(AnalizadorTexto.AnalizadorTexto.EPSILON).Estados(0);
            ///// se hace una disyuncion con el dato y epsilon
            Automata disyuncion =new Disyuncion(this.simple, simple);

            //////devuelve la disyuncion
            
            return disyuncion.Estados(0);
        }
    }
}
