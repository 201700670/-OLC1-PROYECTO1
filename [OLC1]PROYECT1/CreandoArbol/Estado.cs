using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _OLC1_PROYECT1.CreandoArbol
{
    class Estado: ICloneable
    {
        //atributos

        //identificador del estado
        private int id;
        //transiciones del estado
        private LinkedList<Trancisiones> transiciones = new LinkedList<Trancisiones>();
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /**
         *  Constructor
         * @param id identificador
         * @param transiciones transiciones
         */
        public Estado(int id, LinkedList<Trancisiones> transiciones)
        {
            this.id = id;
            this.transiciones = transiciones;
        }
        /**
         * Constructor de un estado con solo el identificador
         * @param identificador identificador
         */
        public Estado(int identificador)
        {
            this.id = identificador;

        }

        /**
         * Accesor del atributo identificador
         * @return id
         */
        public int getId()
        {
            return id;
        }
        /**
         * Mutador del atributo identificador
         * @param id identificador
         */
        public void setId(int id)
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
