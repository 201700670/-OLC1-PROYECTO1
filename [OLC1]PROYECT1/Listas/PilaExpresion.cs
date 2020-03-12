using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_PROYECT1.Listas
{
    class PilaExpresion
    {
        LinkedList<DatoExpresion> pilita;
        String Identificador_expresion;

        public PilaExpresion(LinkedList<DatoExpresion> pilita, String Identificador_expresion)
        {
            this.pilita = pilita;
            this.Identificador_expresion = Identificador_expresion;
        }

        public LinkedList<DatoExpresion> getPilita()
        {
            return pilita;
        }

        public void setPilita(LinkedList<DatoExpresion> pilita)
        {
            this.pilita = pilita;
        }

        public String getIdentificador_expresion()
        {
            return Identificador_expresion;
        }

        public void setIdentificador_expresion(String Identificador_expresion)
        {
            this.Identificador_expresion = Identificador_expresion;
        }
    }
}
