using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.CreandoArbol;
using System.Collections;

namespace _OLC1_PROYECT1.CreandoArbol
{

	class Thompson
	{
		public LinkedList<DatoExpresion> lista = new LinkedList<DatoExpresion>();
		LinkedList<Object> Operandos = new LinkedList<Object>();
		LinkedList<Object> Operadores = new LinkedList<Object>();
		//public LinkedList<TablaSiguientes> guardarHojas = new LinkedList<>();
		//public LinkedList<TablaSiguientes> Tabla_de_Simbolos = new LinkedList<>();
		String operadores, idgrafica = "";
		AnalizadorTexto.AnalizadorTexto lex = new AnalizadorTexto.AnalizadorTexto();
		public int contadorhojas = 0;
		public static int conta;
		int arbol = 0;
		String datoanterior = "";
		String Resultado = "";
		public Automata afn;
		//public static Automata t;
		public Thompson()
		{

		}

		public Automata Analizarentrada(LinkedList<DatoExpresion> Pila)
		{
			//// VAMOS A GUARDAR EL AFN EN UNA PILA
			Stack pilaAFN = new Stack();
			for (int i = Pila.Count() - 1; i >= 0; i--)
			{
				//Console.WriteLine(Pila.ElementAt(i).getLexema() + "   " + Pila.ElementAt(i).getTipo() + "   " + Pila.ElementAt(i).getIdgrafica());
				if (Pila.ElementAt(i).getLexema().Equals(".") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

					Automata concat_izq = (Automata)pilaAFN.Pop();
					Automata concat_der = (Automata)pilaAFN.Pop();
					Automata concat_resultado = Concatenacion(concat_izq, concat_der);

					pilaAFN.Push(concat_resultado);
					this.afn = concat_resultado;

				}
				else if (Pila.ElementAt(i).getLexema().Equals("*") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					Automata kleene = CerraduraKleene((Automata)pilaAFN.Pop());
					pilaAFN.Push(kleene);
					this.afn = kleene;
				}
				else if (Pila.ElementAt(i).getLexema().Equals("?") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					Automata uno_epsilon = Uno_epsilon((Automata)pilaAFN.Pop());
					pilaAFN.Push(uno_epsilon);
					this.afn = uno_epsilon;
				}
				else if (Pila.ElementAt(i).getLexema().Equals("|") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					Automata union_izq = (Automata)pilaAFN.Pop();
					Automata union_der = (Automata)pilaAFN.Pop();
					Automata union_resultado = Disyuncion(union_izq, union_der);


					pilaAFN.Push(union_resultado);
					this.afn = union_resultado;
				}
				else if (Pila.ElementAt(i).getLexema().Equals("+") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					

					Automata elementosacado= (Automata)pilaAFN.Pop();
					
					pilaAFN.Push(elementosacado);

					Automata t=new Automata();
					///////////////////////////CLONAMOS NUESTRA VARIABLE PARA HACER EL DE KLEENE Y LA CONCATENACION
					foreach(Estado sta in elementosacado.getEstados())
					{
						t.addEstados((Estado)sta.Clone());
					}
					foreach (Estado sta in elementosacado.getEstadosAceptacion())
					{
						t.addEstadosAceptacion((Estado)sta.Clone());
					}
					int a = 0;
					
					t.setAlfabeto(elementosacado.getAlfabeto());
					t.setLenguajeR((String)elementosacado.getLenguajeR().Clone());
					t.setInicial((Estado)elementosacado.getInicial().Clone());
					t.setEstadoInicial((Estado)elementosacado.getEstadoInicial().Clone());
					
					////////////////SE COMIENZA LA ACCION
					Automata uno_mas = Uno_mas((Automata)pilaAFN.Pop(), t);
					pilaAFN.Push(uno_mas);
					this.afn = uno_mas;
				}
				else
				{
					Automata simple = Simple(Pila.ElementAt(i).getLexema());
					pilaAFN.Push(simple);
					this.afn = simple;
				}
			}

			this.afn.createAlfabeto(Pila);
			this.afn.setTipo("AFN");
			Console.WriteLine(this.afn.DOT_THOMPSON(this.afn));
			return afn;
		}
		/////VOY A CREAR EL METODO PARA HACER UN NODO SIMPLE O UN TOKEN DEL ALFABETO A AGREGAR A THOMPSON
		public Automata Simple(String simbolosimple)
		{
			Automata AFN_SIMPLE = new Automata();
			//definir los nuevos estados
			Estado inicial = new Estado(0);
			Estado aceptacion = new Estado(1);
			//crear una transicion unica con el simbolo
			Trancisiones tran = new Trancisiones();
			tran.Transicion(inicial, aceptacion, simbolosimple);

			inicial.setTransiciones(tran);
			//agrega los estados creados
			AFN_SIMPLE.addEstados(inicial);
			AFN_SIMPLE.addEstados(aceptacion);
			//colocar los estados iniciales y de acpetacion
			AFN_SIMPLE.setEstadoInicial(inicial);
			AFN_SIMPLE.addEstadosAceptacion(aceptacion);
			AFN_SIMPLE.setLenguajeR(simbolosimple + "");
			//Console.WriteLine(tran.DOT_String());
			return AFN_SIMPLE;

		}
		///// METODO QUE ES PARA LA CONCATENACION 
		public Automata Concatenacion(Automata AFN1, Automata AFN2)

		{Automata AFN_CONCATENACION = new Automata();
			if (AFN2.getTipo()!=null)
			{
				int i = 0;
				
				Trancisiones tran = null;
				for (i = 0; i < AFN1.getEstados().Count(); i++)
				{
					Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
					tmp.setId(i);
					/// ahorita se define el estado inicial
					if (i == 0)
					{
						AFN_CONCATENACION.setEstadoInicial(tmp);
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
						}*/
						foreach (Estado es in AFN1.getEstadosAceptacion())
						{

							
							int idestadfin = AFN1.getEstadosAceptacion().ElementAt(0).getId();
							tmp.setId(idestadfin);
							foreach (Trancisiones a in AFN2.getEstadoInicial().getTransiciones())
							{
								Trancisiones b = new Trancisiones();
								int contadorespacios = 0;
								int inicio = a.getInicio().getId();
								int fin = a.getFin().getId();
								if (inicio > fin)
								{
									contadorespacios = inicio - fin;
								}
								else
								{
									contadorespacios = fin - inicio;
								}
								Estado inicios = new Estado(idestadfin);
								b.setInicio(inicios);
								Estado finales = new Estado(idestadfin + contadorespacios);
								b.setFin(finales);
								b.setSimbolo(a.getSimbolo());
								tmp.setTransiciones(b);
							}

						}

					}
					AFN_CONCATENACION.addEstados(tmp);

				}
				int inicial = i--;
				//// SE PROCEDE A EJECUTAR LO DEL LADO DERECHO
				for (int j = 0; j < AFN2.getEstados().Count(); j++)
				{
					Estado tmp = new Estado(i);
					tmp.setId(i);
					//define el ultimo con estado de aceptacion
					if (AFN2.getEstados().Count() - 1 == j)
					{
						AFN_CONCATENACION.addEstadosAceptacion(tmp);
					}
					else
					{
						foreach (Trancisiones a in AFN2.getEstados().ElementAt(j).getTransiciones())
						{
							Trancisiones b = new Trancisiones();
							int contadorespacios = 0;
							int inicio = a.getInicio().getId();
							int fin = a.getFin().getId();
							if (inicio > fin)
							{
								contadorespacios = inicio - fin;
							}
							else
							{
								contadorespacios = fin - inicio;
							}
							Estado inicios = new Estado(i);
							b.setInicio(inicios);
							Estado finales = new Estado(i + contadorespacios);
							b.setFin(finales);
							b.setSimbolo(a.getSimbolo());
							tmp.setTransiciones(b);
						}
					}

					if (j == 0)
					{

					}
					else
					{
						AFN_CONCATENACION.addEstados(tmp);
					}

					i++;
				}
				Hashtable alfabeto = new Hashtable();
				alfabeto.Add(AFN1.getAlfabeto(), AFN_CONCATENACION.getInicial().getId());
				alfabeto.Add(AFN2.getAlfabeto(), AFN_CONCATENACION.getEstadosAceptacion().ElementAt(0).getId());
				AFN_CONCATENACION.setTipo(null);
				AFN_CONCATENACION.setAlfabeto(alfabeto);
				AFN_CONCATENACION.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
				//Console.WriteLine(tran.toString());
			}
			else {


				int i = 0;
				//se agragaran los estados de cada uno de los automatas del lado derecho e izquierdo para luego unirlos por se de
				// concatencion
				///////////////SE PROCEDE A EJECUTAR LOS DEL LADO IZQUIERDO

				 Trancisiones tran = null;
				for (i = 0; i < AFN1.getEstados().Count(); i++)
				{
					Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
					tmp.setId(i);
					/// ahorita se define el estado inicial
					if (i == 0)
					{
						AFN_CONCATENACION.setEstadoInicial(tmp);
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
						}*/
						foreach (Estado es in AFN1.getEstadosAceptacion())
						{
							tmp = AFN2.getEstadoInicial();
							int idestadfin = AFN1.getEstadosAceptacion().ElementAt(0).getId();
							tmp.setId(idestadfin);
							int a = 0;

						}
					}
					AFN_CONCATENACION.addEstados(tmp);

				}
				i--;
				//// SE PROCEDE A EJECUTAR LO DEL LADO DERECHO
				for (int j = 0; j < AFN2.getEstados().Count(); j++)
				{
					Estado tmp = (Estado)AFN2.getEstados().ElementAt(j);
					tmp.setId(i);

					//define el ultimo con estado de aceptacion
					if (AFN2.getEstados().Count() - 1 == j)
						AFN_CONCATENACION.addEstadosAceptacion(tmp);
					if (j == 0)
					{

					}
					else
					{
						AFN_CONCATENACION.addEstados(tmp);
					}
					i++;
				}

				Hashtable alfabeto = new Hashtable();
				alfabeto.Add(AFN1.getAlfabeto(), AFN_CONCATENACION.getInicial().getId());
				alfabeto.Add(AFN2.getAlfabeto(), AFN_CONCATENACION.getEstadosAceptacion().ElementAt(0).getId());
				AFN_CONCATENACION.setAlfabeto(alfabeto);
				AFN_CONCATENACION.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
				//Console.WriteLine(tran.toString());
			}
			return AFN_CONCATENACION;
			
		}
		////////////////////////////////////////////////CERRADURA DE KLEENE////////////////////////////////////////////////////
		public Automata CerraduraKleene(Automata automataFN)
		{
			Automata AFN_KLEENE = new Automata();

			//se crea un nuevo estado inicial
			Estado nuevoInicio = new Estado(0);
			AFN_KLEENE.addEstados(nuevoInicio);
			AFN_KLEENE.setEstadoInicial(nuevoInicio);

			//Console.WriteLine("CANTIDAD DE ESTADOS EN  UNO    "+automataFN.getEstados().Count());
			//agregar todos los estados intermedio
			for (int i = 0; i < automataFN.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)automataFN.getEstados().ElementAt(i);
				//Console.WriteLine("id      " + tmp.getId());
				tmp.setId(i + 1);
				AFN_KLEENE.addEstados(tmp);
			}
			for (int h = 0; h < automataFN.getEstados().Count(); h++)
			{
				Estado tmp = (Estado)automataFN.getEstados().ElementAt(h);
				foreach (Trancisiones tra in tmp.getTransiciones())
				{
					int final = tra.getFin().getId();
					if (tra.getInicio().getId() == final)
					{
						tra.getFin().setId(tra.getInicio().getId() + 1);
					}
				}

			}
			//Se crea un nuevo estado de aceptacion
			Estado nuevoFin = new Estado(automataFN.getEstados().Count() + 1);
			AFN_KLEENE.addEstados(nuevoFin);
			AFN_KLEENE.addEstadosAceptacion(nuevoFin);

			//definir estados clave para realizar la cerraduras
			Estado anteriorInicio = automataFN.getEstadoInicial();

			LinkedList<Estado> anteriorFin = automataFN.getEstadosAceptacion();

			// agregar transiciones desde el nuevo estado inicial
			Trancisiones tran = new Trancisiones();
			tran.Transicion(nuevoInicio, anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
			nuevoInicio.getTransiciones().AddLast(tran);
			Trancisiones tran2 = new Trancisiones();
			tran2.Transicion(nuevoInicio, nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
			nuevoInicio.getTransiciones().AddLast(tran2);

			// agregar transiciones desde el anterior estado final
			for (int i = 0; i < anteriorFin.Count(); i++)
			{
				Trancisiones tran3 = new Trancisiones();
				tran3.Transicion(anteriorFin.ElementAt(i), anteriorInicio, AnalizadorTexto.AnalizadorTexto.EPSILON);
				Trancisiones tran4 = new Trancisiones();
				tran4.Transicion(anteriorFin.ElementAt(i), nuevoFin, AnalizadorTexto.AnalizadorTexto.EPSILON);
				anteriorFin.ElementAt(i).getTransiciones().AddLast(tran3);
				anteriorFin.ElementAt(i).getTransiciones().AddLast(tran4);
			}
			AFN_KLEENE.setAlfabeto(automataFN.getAlfabeto());
			AFN_KLEENE.setLenguajeR(automataFN.getLenguajeR());
			return AFN_KLEENE;
		}
		/////////////////////////////////////////////////////// OR///////////////////////////////////////////////////////////////
		public Automata Disyuncion(Automata AFN1, Automata AFN2)
		{
			Automata AFN_DISYUNCION = new Automata();
			//se crea un nuevo estado inicial
			Estado nuevoInicio = new Estado(0);
			//se crea una transicion del nuevo estado inicial al primer automata
			Trancisiones tran = new Trancisiones();
			tran.Transicion(nuevoInicio, AFN2.getEstadoInicial(), AnalizadorTexto.AnalizadorTexto.EPSILON);
			nuevoInicio.setTransiciones(tran);

			AFN_DISYUNCION.addEstados(nuevoInicio);
			AFN_DISYUNCION.setEstadoInicial(nuevoInicio);
			int i = 0;//llevar el contador del identificador de estados

			//se agrega los estados del primer automata
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i + 1);
				AFN_DISYUNCION.addEstados(tmp);
				
			}
			if (AFN1.getEstados().Count() == 3)
			{
				for (int h = 0; h < AFN1.getEstados().Count(); h++)
				{
					Estado tmp = (Estado)AFN1.getEstados().ElementAt(h);
					foreach(Trancisiones tra in tmp.getTransiciones())
					{
						int final = tra.getFin().getId();
						if (tra.getInicio().getId() + 2 != final + 1)
						{
							tra.getFin().setId(tra.getInicio().getId()+1);
						}
					}

				}
			}
			//se agrega los estados del segundo automata
			for (int j = 0; j < AFN2.getEstados().Count(); j++)
			{
				Estado tmp = (Estado)AFN2.getEstados().ElementAt(j);
				tmp.setId(i + 1);
				AFN_DISYUNCION.addEstados(tmp);
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
			AFN_DISYUNCION.addEstados(nuevoFin);
			AFN_DISYUNCION.addEstadosAceptacion(nuevoFin);


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
			AFN_DISYUNCION.setAlfabeto(alfabeto);
			AFN_DISYUNCION.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
			return AFN_DISYUNCION;
		}
		/// <summary>
		/// ////////////////////////////////////////UNO O MAS  .a*a   //////////////////////////////////////////////////
		/// </summary>
		/// <param name="AFN1"></param>
		/// <returns></returns>
		public Automata Uno_mas(Automata AFN1, Automata AFN2)
		{
			Automata AFN_UNOMAS = new Automata();
			//// .a*a
			///se crea un nuevo estado inicial
			/// se crea un automata para la cerradura de kleene
			int i = 0;
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i);
				if (i == 0)
				{
					AFN_UNOMAS.setEstadoInicial(tmp);
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
						AFN_UNOMAS.addEstados(tmpi);
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
					AFN_UNOMAS.addEstados(nuevoFin);
					AFN_UNOMAS.addEstadosAceptacion(nuevoFin);
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
				AFN_UNOMAS.addEstados(tmp);

			}

			AFN_UNOMAS.setAlfabeto(AFN1.getAlfabeto());
			AFN_UNOMAS.setLenguajeR(AFN1.getLenguajeR());

			Hashtable temporal = new Hashtable();
			LinkedList<Trancisiones> te = new LinkedList<Trancisiones>();
			Automata NUEVOCAMBIO = new Automata();
			int auxiliares = 0;
			foreach (Estado estados in AFN_UNOMAS.getEstados())
			{
				Estado nuevos = new Estado(auxiliares);
				Console.WriteLine("idEstado  " + estados.getId() + "  cantidad de trancisiones    " + estados.getTransiciones().Count());
				NUEVOCAMBIO.addEstados(nuevos);
				auxiliares++;
			}
			foreach (Estado estados in AFN_UNOMAS.getEstados())
			{
				foreach (Trancisiones trancision in estados.getTransiciones())
				{
					try
					{
						temporal.Add(trancision.getFin().getId() + "" + trancision.getInicio().getId(), estados.getId());

						te.AddLast(trancision);
						Console.WriteLine(trancision.getInicio().getId() + " ============ " + trancision.getFin().getId());
						foreach(Estado state in NUEVOCAMBIO.getEstados())
						{
							if (trancision.getInicio().getId() == state.getId())
							{
								Trancisiones transi = new Trancisiones();
								Estado iniciotran = new Estado(trancision.getInicio().getId());
								Estado fintran = new Estado(trancision.getFin().getId());
								String simbolotran = trancision.getSimbolo();
								transi.Transicion(iniciotran, fintran, simbolotran);
								state.getTransiciones().AddLast(transi);
							}
						}
					}
					catch (Exception e)
					{
						Console.WriteLine("SE REPITE    " + trancision.getInicio().getId() + "  " + trancision.getFin().getId());
					}
				}
			}


			Automata AFN_UNOOMAS = new Automata();
			/*foreach(Trancisiones auxiliar in te)
			{
				Estado aux = new Estado(auxiliar.getInicio().getId());
				aux.setTransiciones(auxiliar);
				AFN_UNOOMAS.addEstados(aux);
			}
			////borramos todas las trancisiones
			AFN_UNOOMAS.addEstadosAceptacion(AFN_UNOMAS.getEstadosAceptacion().ElementAt(0));
			AFN_UNOOMAS.setEstadoInicial(AFN_UNOMAS.getEstadoInicial());
			AFN_UNOOMAS.setInicial(AFN_UNOMAS.getInicial());
			AFN_UNOOMAS.setLenguajeR(AFN_UNOMAS.getLenguajeR());
			AFN_UNOOMAS.setAlfabeto(AFN_UNOMAS.getAlfabeto());

			Automata TEMPORALES = new Automata();
			int repetidos = 0;
			foreach (Estado auxiliar in AFN_UNOMAS.getEstados())
			{
				Estado anteriores = auxiliar;
				Estado nuevos = new Estado(anteriores.getId());
				TEMPORALES.addEstados(nuevos);
				
			}
			foreach (Estado auxiliar in AFN_UNOOMAS.getEstados())
			{
				foreach (Trancisiones transi in auxiliar.getTransiciones())
				{
					foreach (Estado entry in TEMPORALES.getEstados())
					{
						if (auxiliar.getId() == entry.getId())
						{
							entry.setTransiciones(transi);
							foreach(Trancisiones agregando in entry.getTransiciones())
							{
								agregando.setInicio(transi.getInicio());
								agregando.setFin(transi.getFin());
								agregando.setSimbolo(transi.getSimbolo());
							}
							
						}
						repetidos++;
					}
				}
			}
				
				
			*/
			NUEVOCAMBIO.setTipo("AFN_UNOMAS");
			NUEVOCAMBIO.addEstadosAceptacion(AFN_UNOMAS.getEstadosAceptacion().ElementAt(0));
			NUEVOCAMBIO.setEstadoInicial(NUEVOCAMBIO.getEstados().ElementAt(0));
			NUEVOCAMBIO.setInicial(NUEVOCAMBIO.getEstados().ElementAt(0));
			NUEVOCAMBIO.setLenguajeR(AFN_UNOMAS.getLenguajeR());
			NUEVOCAMBIO.setAlfabeto(AFN_UNOMAS.getAlfabeto());


			return NUEVOCAMBIO;
		}
		/// <summary>
		/// ///////////////////////////////////////////////////////UNO O EPSILON |A ε /////////////////////////
		/// </summary>
		/// <param name="AFN1"></param>
		/// <returns></returns>
		public Automata Uno_epsilon(Automata AFN1)
		{
			Automata AFN_UNOMAS = new Automata();
			//// se realiza el simple con el epsilon
			Automata simple = Simple(AnalizadorTexto.AnalizadorTexto.EPSILON);
			///// se hace una disyuncion con el dato y epsilon
			Automata disyuncion = Disyuncion(AFN1, simple);

			//////devuelve la disyuncion
			return disyuncion;
		}

	}
	
	/// <summary>
	/// ///EN ESTE SE CREA UNA CLASE AUTOMATA QUE SE UTILIZARA PARA HACER EL DE THOMPSON
	/// </summary>
	class Automata: ICloneable
	{

		//es un estado inicial
		private Estado Inicial;
		//como se mantiene el estado final y el estado de aceptacion se utilizara esta lista
		private  LinkedList<Estado> aceptacion;
		//lista de estados
		private  LinkedList<Estado> estados;
		// alfabeto que contiene el, hash para no tener elementos repetidos (lo usare para hacer la tabla de trancisiones)
		private Hashtable alfabeto;
		//atributo para saber si es Determinista o No determinista
		private String tipo;
		//atributo para saber el lenguaje r, la cadena w y el resultado de la simulacion
		private String[] resultadoRegex;
		private String lenguajeR;
		public Automata()
		{
			this.estados = new LinkedList<Estado>();
			this.aceptacion = new LinkedList<Estado>();
			this.alfabeto = new Hashtable();
			this.resultadoRegex = new String[3];

		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
		/**
		 * Accesor del estado inicial del autómata
		 * @return Estado
		 */
		public Estado getEstadoInicial()
		{
			return Inicial;
		}
		/**
		 * Mutador del estado inicial del autómata
		 * @param inicial Estado inicial
		 */
		public void setEstadoInicial(Estado inicial)
		{
			this.Inicial = inicial;
		}
		/**
		 * Accesor del estado de aceptacion o final del autómata
		 * @return Estado
		 */
		public LinkedList<Estado> getEstadosAceptacion()
		{
			return aceptacion;
		}
		/**
		 * Mutador del estado final o aceptacion del autómata
		 * @param fin Estado final
		 */
		public void addEstadosAceptacion(Estado fin)
		{
			this.aceptacion.AddLast(fin);
		}

		/**
		 * Obtener los estados del autómata
		 * @return Array de Estados
		 */
		public LinkedList<Estado> getEstados()
		{
			return estados;
		}

		public Estado getEstados(int index)
		{
			return estados.ElementAt(index);
		}

		/**
		 * Agregar un estado al autómata
		 * @param estado estructura de estado
		 */
		public void addEstados(Estado estado)
		{
			this.estados.AddLast(estado);
		}


		/**
		 * Mostrar los atributos del autómata
		 * @return String
		 */
		public Hashtable getAlfabeto()
		{
			return alfabeto;
		}

		/**
		 * Metodo para definir el alfabeto del automata a partir 
		 * de la expresion regular
		 * @param regex 
		 */
		public void createAlfabeto(LinkedList<DatoExpresion> cadalfabeto)
		{
			int i = 0;
			foreach (DatoExpresion ch in cadalfabeto)
			{

				if (!ch.getLexema().Equals("|") && !ch.getLexema().Equals("*") && !ch.getLexema().Equals(".") && !ch.getLexema().Equals( AnalizadorTexto.AnalizadorTexto.EPSILON))
				{
					Console.WriteLine("CHAR QUE SE AGREGA AL ALFABETO ->"+ch.getLexema());
					try
					{
						this.alfabeto.Add(ch.getLexema(), i);// lo agrega a la tabla hash
					}catch(Exception e){
						Console.WriteLine("SE REPITIO VALOR     " + ch.getLexema());
					}
				}
					i++;
			}
		}

		public void setAlfabeto(Hashtable alfabeto)
		{
			this.alfabeto = alfabeto;
		}

		public void setTipo(String tipo)
		{
			this.tipo = tipo;
		}

		public String getTipo()
		{
			return this.tipo;
		}

		public Estado getInicial()
		{
			return Inicial;
		}

		public void setInicial(Estado inicial)
		{
			this.Inicial = inicial;
		}

		public String[] getResultadoRegex()
		{
			return resultadoRegex;
		}

		public void addResultadoRegex(int key, String value)
		{
			this.resultadoRegex[key] = value;
		}


		public String toString()
		{
			String res="";
			res += "-------" + this.tipo + "---------\r\n";
			res += "Alfabeto " + this.alfabeto.Keys + "\r\n";
			res += "Estado inicial " + this.Inicial + "\r\n";
			res += "Conjutos de estados de aceptacion " + this.aceptacion + "\r\n";
			res += "Conjunto de Estados " + this.estados.ToString() +"	";
			foreach(Estado ins in this.estados)
			{
				res += " , "+ins.getId()+ "\r\n";
			}
			
			res += "Conjunto de transiciones ";
			for (int i = 0; i < this.estados.Count(); i++)
			{
				Estado est = estados.ElementAt(i);
				foreach(Trancisiones t in est.getTransiciones())
				{
					res += est.getId() + "  ->  " + t.getSimbolo()+"\r\n";
				}
				
			}
			res += "\r\n";
			res += "Lenguaje r: " + this.lenguajeR + "\r\n";
			res += "Cadena w ingresada: " + this.resultadoRegex[1] + "\r\n";
			res += "Resultado: " + this.resultadoRegex[2] + "\r\n";


			return res;
		}

		public String DOT_THOMPSON(Automata afn)
		{
			String res = "";
			for (int i = 0; i < this.estados.Count(); i++)
			{
				Estado est = estados.ElementAt(i);
				for (int ii=0; ii<afn.getEstadosAceptacion().Count();ii++)
				{
					Console.WriteLine("ES EL ESTADO FINAL      " + est.getId());
					res += "node[shape = doublecircle, color = purple]; S" + afn.getEstadosAceptacion().ElementAt(ii).getId() + ";\r\n";
				}
			}
			res += "node [shape = circle];\r\n";
			Hashtable temporal = new Hashtable();
			LinkedList<String> te = new LinkedList<string>();
			for (int i = 0; i < this.estados.Count(); i++)
			{
				Estado est = estados.ElementAt(i);

				foreach (Trancisiones tran in est.getTransiciones())
				{
					Console.WriteLine("****************************     "+tran.DOT_String());
					try
					{
						
						temporal.Add(tran.DOT_String(), 12);
						te.AddLast(tran.DOT_String());
					}catch(Exception e)
					{

					}
					
					//res += tran.DOT_String() + "\r\n";

				}

			}
			foreach(String a in te)
			{
				res += a+"\r\n";
			}




			res += "\r\n";
			


			return res;
		}

		public String getLenguajeR()
		{
			return lenguajeR;
		}

		public void setLenguajeR(String lenguajeR)
		{
			this.lenguajeR = lenguajeR;
		}



	}


	/*	class Ors : Automata
		{
			Automata est1;
			Automata est2;
			int n1, n2, n3, n4, n5, n6; //Control de cada nodo de la estructura

			public Ors(Automata est1, Automata est2)
			{
				this.est1 = est1;
				this.est2 = est2;
			}

			public override Object Ejecutar(int n)
			{
				return null;
			}

			public override int Numerar(int n)
			{
				n1 = n;
				n2 = n++;
				if (est1 is And)
				{
					And miand = (And)est1;
					miand.Numerar(n);
					n3 = (int)n;
					n2 = (int)n++;
				}
				else if (est1 is Term)
				{
					n3 = (int)n;
					n2 = (int)n++;
				}
				return 0;
			}
		}


		class And : Automata
		{
			Automata est1;
			Automata est2;
			int n1, n2, n3; //Control de cada nodo de la estructura

			public And(Automata est1, Automata est2)
			{
				this.est1 = est1;
				this.est2 = est2;
			}

			public override Object Ejecutar(int n)
			{
				return null;
			}

			public override int Numerar(int n)
			{

				if (est1 is Ors)
				{
					Ors mior = (Ors)est1;
					n1=mior.Numerar(n);
					n3 =n2++;
					n2 = n1++;
					return n3;
				}
				else if (est1 is Term)
				{
					return n3;

				}
				else
				{
					n1 = n;
					n2 = n1 ++;
					n3 = n2++;
					return n3;
				}
				return -1;
			}
		}


		class Term : Automata
		{
			Automata est1;
			Automata est2;
			int n1, n2, n3, n4, n5, n6; //Control de cada nodo de la estructura

			public Term(Automata est1, Automata est2)
			{
				this.est1 = est1;
				this.est2 = est2;
			}

			public override Object Ejecutar(int n)
			{
				return null;
			}

			public override int Numerar(int n)
			{
				return 0;
			}
		}

		class kleene : Automata
		{
			Automata est1;
			Automata est2;
			int n1, n2, n3, n4; //Control de cada nodo de la estructura

			public kleene(Automata est1, Automata est2)
			{
				this.est1 = est1;
				this.est2 = est2;
			}

			public override Object Ejecutar(int n)
			{
				return null;
			}

			public override int Numerar(int n)
			{
				n1 = n;
				n2 = n++;
				if (est1 is And){
					And miand = (And)est1;
					miand.Numerar(n);
					n3 = (int)n;
					n2 = (int)n++;
				}else if (est1 is Term){
					n3 = (int)n;
					n2 = (int)n++;
				}
				return 0;
			}
		}
		*/

}
