using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.CreandoArbol;
using System.Collections;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.Elementos
{
    class Concatenacion_punto: Automata
    {
        Automata AFN1, AFN2;
        int n1, n2;
        public Concatenacion_punto(Automata AFN1, Automata AFN2)
        {
            this.AFN1 = AFN1;
            this.AFN2 = AFN2;
        }
        /*public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Estados(int n)
        {
			int i = n;
			Trancisiones tran = null;
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i);
				/// ahorita se define el estado inicial
				if (i == 0)
				{
					this.setEstadoInicial(tmp);
				}
				///cuando llega al último, concatena el ultimo con el primero del otro automata con un epsilon
				if (i == AFN1.getEstados().Count() - 1)
				{

					//se utiliza un ciclo porque los estados de aceptacion son un array
					//se coloca un estado de epsilon por si acaso es para hacer la separacion de los nodos

					/*for (int k = 0; k < AFN1.getEstadosAceptacion().Count(); k++)
					{
						tran = new Trancisiones();
						tran.Transicion((Estado)AFN1.getEstadosAceptacion().ElementAt(k), AFN2.getEstadoInicial(), AnalizadorTexto.AnalizadorTexto.EPSILON);
						tmp.setTransiciones(tran);
					}
					foreach (Estado es in AFN1.getEstadosAceptacion())
					{
						tmp = AFN2.getEstadoInicial();
						int idestadfin = AFN1.getEstadosAceptacion().ElementAt(0).getId();
						tmp.setId(idestadfin);
						int a = 0;

					}
				}
				this.addEstados(tmp);

			}
			i--;


			//// SE PROCEDE A EJECUTAR LO DEL LADO DERECHO
			for (int j = 0; j < AFN2.getEstados().Count(); j++)
			{
				Estado tmp = (Estado)AFN2.getEstados().ElementAt(j);
				tmp.setId(i);

				//define el ultimo con estado de aceptacion
				if (AFN2.getEstados().Count() - 1 == j)
					this.addEstadosAceptacion(tmp);
				if (j == 0)
				{

				}
				else
				{
					this.addEstados(tmp);
				}
				i++;
			}
			Hashtable alfabeto = new Hashtable();
			alfabeto.Add(AFN1.getAlfabeto(), 12);
			//alfabeto.Add(AFN2.getAlfabeto(), 22);
			this.setAlfabeto(alfabeto);
			this.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
			//Console.WriteLine(tran.toString());
			return this;
        }*/
    }
}
