using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.CreandoArbol;
using System.Collections;

namespace _OLC1_PROYECT1.Elementos
{
    class Disyuncion: Automata
    {
        Automata AFN1, AFN2;
        int n1, n2;
        public Disyuncion(Automata AFN1, Automata AFN2)
        {
            this.AFN1 = AFN1;
            this.AFN2 = AFN2;
        }
        public override object Ejecutar(int n)
        {
            return null;
        }

        public override object Estados(int n)
        {

			//se crea un nuevo estado inicial
			Estado nuevoInicio = new Estado(0);
			//se crea una transicion del nuevo estado inicial al primer automata
			Trancisiones tran = new Trancisiones();
			tran.Transicion(nuevoInicio, AFN2.getEstadoInicial(), AnalizadorTexto.AnalizadorTexto.EPSILON);
			nuevoInicio.setTransiciones(tran);

			this.addEstados(nuevoInicio);
			this.setEstadoInicial(nuevoInicio);
			int i = 0;//llevar el contador del identificador de estados

			//se agrega los estados del primer automata
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i + 1);
				this.addEstados(tmp);

			}
			if (AFN1.getEstados().Count() == 3)
			{
				for (int h = 0; h < AFN1.getEstados().Count(); h++)
				{
					Estado tmp = (Estado)AFN1.getEstados().ElementAt(h);
					foreach (Trancisiones tra in tmp.getTransiciones())
					{
						int final = tra.getFin().getId();
						if (tra.getInicio().getId() + 2 != final + 1)
						{
							tra.getFin().setId(tra.getInicio().getId() + 1);
						}
					}

				}
			}
			//se agrega los estados del segundo automata
			for (int j = 0; j < AFN2.getEstados().Count(); j++)
			{
				Estado tmp = (Estado)AFN2.getEstados().ElementAt(j);
				tmp.setId(i + 1);
				this.addEstados(tmp);
				i++;
			}
			if (AFN2.getEstados().Count() == 3)
			{
				for (int h = 0; h < AFN2.getEstados().Count(); h++)
				{
					Estado tmp = (Estado)AFN2.getEstados().ElementAt(h);
					foreach (Trancisiones tra in tmp.getTransiciones())
					{
						int final = tra.getFin().getId();
						if (tra.getInicio().getId() + 2 != final + 1)
						{
							tra.getFin().setId(tra.getInicio().getId() + 1);
						}
					}

				}
			}
			//se crea un nuevo estado final
			Estado nuevoFin = new Estado(AFN1.getEstados().Count() + AFN2.getEstados().Count() + 1);
			this.addEstados(nuevoFin);
			this.addEstadosAceptacion(nuevoFin);


			Estado anteriorInicio = AFN1.getEstadoInicial();
			LinkedList<Estado> anteriorFin = AFN1.getEstadosAceptacion();
			LinkedList<Estado> anteriorFin2 = AFN2.getEstadosAceptacion();

			// se agrega transicion del nuevo estado inicial
			Trancisiones tran1 = new Trancisiones();
			tran1.Transicion(nuevoInicio, anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
			nuevoInicio.getTransiciones().AddLast(tran1);

			// se agregan transicion del anterior AFN  al estado final
			for (int k = 0; k < anteriorFin.Count(); k++)
			{
				Trancisiones tran2 = new Trancisiones();
				tran2.Transicion(anteriorFin.ElementAt(k), nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
				anteriorFin.ElementAt(k).getTransiciones().AddLast(tran2);
			}
			// se agrega transicion del anterior AFN de abajo al estado final
			for (int k = 0; k < anteriorFin.Count(); k++)
			{
				Trancisiones tran2 = new Trancisiones();
				tran2.Transicion(anteriorFin2.ElementAt(k), nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
				anteriorFin2.ElementAt(k).getTransiciones().AddLast(tran2);
			}


			Hashtable alfabeto = new Hashtable();
			alfabeto.Add(AFN1.getAlfabeto(), 13);
			alfabeto.Add(AFN2.getAlfabeto(), 23);
			this.setAlfabeto(alfabeto);
			this.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
			return this;
        }
    }
}
