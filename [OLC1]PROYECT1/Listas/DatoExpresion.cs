using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_PROYECT1.Listas
{
    class DatoExpresion
    {
        String lexema;
        TipoExpresion tipo;
        String idgrafica;
        public DatoExpresion(String lexema, TipoExpresion tipo, String idgrafica)
        {
            this.lexema = lexema;
            this.tipo = tipo;
            this.idgrafica = idgrafica;
        }


        public enum TipoExpresion
        {
            CADENA,
            CONJUNTOS,
            OPERADOR
        }

        public String getIdgrafica()
        {
            return idgrafica;
        }

        public void setIdgrafica(String idgrafica)
        {
            this.idgrafica = idgrafica;
        }


        public String getLexema()
        {
            return lexema;
        }

        public void setLexema(String lexema)
        {
            this.lexema = lexema;
        }

        public TipoExpresion getTipo()
        {
            return tipo;
        }

        public void setTipo(TipoExpresion tipo)
        {
            this.tipo = tipo;
        }
    }
}
