using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.Elementos
{
    class Uno_mas:Automata
    {
		Automata AFN1, AFN2;
		int n1, n2;
		public Uno_mas(Automata AFN1, Automata AFN2)
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
			int i = 0;
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i);
				if (i == 0)
				{
					this.setEstadoInicial(tmp);
				}
				if (i == AFN1.getEstados().Count() - 1)// el ultimo estado se concatena con el de kleene
				{

					// creando trancision de estado inicial kleeene y estado aceptacion de  afn1
					for (int k = 0; k < AFN1.getEstadosAceptacion().Count(); k++)
					{
						Trancisiones tran = new Trancisiones();
						tran.Transicion((Estado)AFN1.getEstadosAceptacion().ElementAt(k), AFN2.getEstadoInicial(), AnalizadorTexto.AnalizadorTexto.EPSILON);
						tmp.setTransiciones(tran);
					}
					Estado nuevoInicio = tmp;
					//agregar todos los estados intermedio

					for (int j = 0; j < AFN2.getEstados().Count(); j++)
					{
						Estado tmpi = (Estado)AFN2.getEstados().ElementAt(j);
						tmpi.setId(i + 1);
						this.addEstados(tmpi);
						if (j == 0)
						{
							AFN2.getEstadoInicial().setId(i + 1);
						}
						if (AFN2.getEstados().Count() - 1 == j)
						{
							AFN2.getEstadosAceptacion().Clear();
							Console.WriteLine(AFN2.getEstadosAceptacion().Count());
							tmpi.getTransiciones().Clear();
							AFN2.addEstadosAceptacion(tmpi);

						}
						i++;
					}
					//Se crea un nuevo estado de aceptacion
					Estado nuevoFin = new Estado(i + 1);
					this.addEstados(nuevoFin);
					this.addEstadosAceptacion(nuevoFin);
					//definir estados clave para realizar la cerraduras
					Estado anteriorInicio = AFN2.getEstadoInicial();
					Trancisiones rem = anteriorInicio.getTransiciones().ElementAt(0);
					LinkedList<Estado> anteriorFin = AFN2.getEstadosAceptacion();
					//Se agrega el dato de trancisiones de anteriorinicio y anteriorfin
					String simbolos = rem.getSimbolo();
					Trancisiones unionafinal = new Trancisiones();
					unionafinal.Transicion(AFN2.getInicial(), anteriorFin.ElementAt(AFN2.getEstadosAceptacion().Count() - 1), simbolos);
					anteriorInicio.setTransiciones(unionafinal);


					// agregar transiciones desde el nuevo estado inicial
					Trancisiones tran1 = new Trancisiones();
					tran1.Transicion(nuevoInicio, anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
					nuevoInicio.getTransiciones().AddLast(tran1);
					Trancisiones tran21 = new Trancisiones();
					tran21.Transicion(nuevoInicio, nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
					nuevoInicio.getTransiciones().AddLast(tran21);
					// agregar transiciones desde el anterior estado final
					for (int k = 0; k < anteriorFin.Count(); k++)
					{
						Trancisiones tran3 = new Trancisiones();
						tran3.Transicion(anteriorFin.ElementAt(k), anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
						anteriorFin.ElementAt(k).getTransiciones().AddLast(tran3);
						Trancisiones tran4 = new Trancisiones();
						tran4.Transicion(anteriorFin.ElementAt(k), nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
						anteriorFin.ElementAt(k).getTransiciones().AddLast(tran4);
					}

				}
				this.addEstados(tmp);

			}

			this.setAlfabeto(AFN1.getAlfabeto());
			this.setLenguajeR(AFN1.getLenguajeR());
			return this;
		}
	}
}
