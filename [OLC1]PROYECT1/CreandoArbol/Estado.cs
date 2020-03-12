using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _OLC1_PROYECT1.CreandoArbol
{
    class Estado
    {
        //atributos

        //identificador del estado
        private String id;
        //transiciones del estado
        private LinkedList<Trancisiones> transiciones = new LinkedList<Trancisiones>();

        /**
         *  Constructor
         * @param id identificador
         * @param transiciones transiciones
         */
        public Estado(String id, LinkedList<Trancisiones> transiciones)
        {
            this.id = id;
            this.transiciones = transiciones;
        }
        /**
         * Constructor de un estado con solo el identificador
         * @param identificador identificador
         */
        public Estado(String identificador)
        {
            this.id = identificador;

        }

        /**
         * Accesor del atributo identificador
         * @return id
         */
        public String getId()
        {
            return id;
        }
        /**
         * Mutador del atributo identificador
         * @param id identificador
         */
        public void setId(String id)
        {
            this.id = id;
        }
        /**
         * Accesor del atributo transiciones
         * @return array de transiciones
         */
        public LinkedList<Trancisiones> getTransiciones()
        {

            return transiciones;
        }
        /**
         * Agregar transiciones al estado
         * @param tran transicion
         */
        public void setTransiciones(Trancisiones tran)
        {
            this.transiciones.AddLast(tran);
        }
        /**
         * Mostrar el estado
         * @return String
         */
        public String toString()
        {
            return this.id.ToString();
        }
    }
}
