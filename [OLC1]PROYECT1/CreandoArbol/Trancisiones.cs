using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.CreandoArbol
{
    class Trancisiones// la <T> significa que puede ser de cualquier tipo la coleccion o se un int, float, double, etc.
    {
        //estado inicial de la transicion
        private Estado inicio;
        //estado final de la transiciones
        private Estado fin;
        //simbolo por el cual se realiza la transicion
        private String simbolo;

        /**
         * Constructor de una transicion
         * @param inicio Estado inicial
         * @param fin Estado final
         * @param simbolo simbolo string o character
         */
        public void Transicion(Estado inicio, Estado fin, String simbolo)
        {
            this.inicio = inicio;
            this.fin = fin;
            this.simbolo = simbolo;
        }
        /**
         * Accesor del estado inicial de la transicion
         * @return Estado
         */
        public Estado getInicio()
        {
            return inicio;
        }
        /**
         * Mutador del estado inicial de la transicion
         * @param inicio 
         */
        public void setInicio(Estado inicio)
        {
            this.inicio = inicio;
        }

        /**
         * Accesor del estado final de la transiciones
         * @return Estado
         */
        public Estado getFin()
        {
            return fin;
        }

        /**
         * Mutadro del estado final de la transicion
         * @param fin estado de final o aceptaion
         */
        public void setFin(Estado fin)
        {
            this.fin = fin;
        }
        /**
         * Obtener el simbolo de la transicion
         * @return String
         */
        public String getSimbolo()
        {
            return simbolo;
        }

        /**
         * Mutador del simbolo
         * @param simbolo simbolor string o character
         */
        public void setSimbolo(String simbolo)
        {
            this.simbolo = simbolo;
        }
        /**
         * Mostrar la transicion
         * @return String toString
         */
      
         public String toString()
        {
            return "(" + inicio.getId() + "-" + simbolo + "-" + fin.getId() + ")";
        }
        public String DOT_String()
        {
            return ("S"+this.inicio.getId() + " -> S" + this.fin.getId() + " [label=\"" + this.simbolo + "\"];");
        }

    }
}
