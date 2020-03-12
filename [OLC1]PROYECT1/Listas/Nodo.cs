using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_PROYECT1.Listas
{
    class Nodo
    {
        String lexema;
        String idgrafica;
        public Nodo(String lexema, String idgrafica)
        {
            this.lexema = lexema;
            this.Idgrafica = idgrafica;
        }

        public string Lexema { get => lexema; set => lexema = value; }
        public string Idgrafica { get => idgrafica; set => idgrafica = value; }
    }
}
