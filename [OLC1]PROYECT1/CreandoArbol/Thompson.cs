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
		public Thompson()
		{

		}

		public void Analizarentrada(LinkedList<DatoExpresion> Pila)
		{
			//// VAMOS A GUARDAR EL AFN EN UNA PILA
			Stack pilaAFN = new Stack();
			for(int i=Pila.Count()-1; i>=0; i--)
			{
				Console.WriteLine(Pila.ElementAt(i).getLexema() + "   " + Pila.ElementAt(i).getTipo() + "   " + Pila.ElementAt(i).getIdgrafica());
				if (Pila.ElementAt(i).getLexema().Equals(".") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					
					Automata concat_param1 = (Automata)pilaAFN.Pop();
					Automata concat_param2 = (Automata)pilaAFN.Pop();
					Automata concat_result = concatenacion(concat_param1, concat_param2);

					pilaAFN.Push(concat_result);
					this.afn = concat_result;

				}
				else if (Pila.ElementAt(i).getLexema().Equals("*") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{
					Automata kleene = cerraduraKleene((Automata)pilaAFN.Pop());
					pilaAFN.Push(kleene);
					this.afn = kleene;
				}
				else if (Pila.ElementAt(i).getLexema().Equals("?") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else if (Pila.ElementAt(i).getLexema().Equals("|") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else if (Pila.ElementAt(i).getLexema().Equals("+") && Pila.ElementAt(i).getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else
				{
					idgrafica = Pila.ElementAt(i).getIdgrafica();
					Operadores.AddLast(Pila.ElementAt(i).getLexema());
					Automata simple = afnSimple(Pila.ElementAt(i).getLexema());
					pilaAFN.Push(simple);
					this.afn = simple;
				}
			}
			
			this.afn.createAlfabeto(Pila);
			this.afn.setTipo("AFN");
			Console.WriteLine(this.afn.DOT_THOMPSON());
		}
		/////VOY A CREAR EL METODO PARA HACER UN NODO SIMPLE O UN TOKEN DEL ALFABETO A AGREGAR A THOMPSON
		public Automata afnSimple(String simbolosimple)
		{
			Automata automataFN = new Automata();
			//definir los nuevos estados
			Estado inicial = new Estado(0);
			Estado aceptacion = new Estado(1);
			//crear una transicion unica con el simbolo
			Trancisiones tran = new Trancisiones();
			tran.Transicion(inicial, aceptacion, simbolosimple);

			inicial.setTransiciones(tran);
			//agrega los estados creados
			automataFN.addEstados(inicial);
			automataFN.addEstados(aceptacion);
			//colocar los estados iniciales y de acpetacion
			automataFN.setEstadoInicial(inicial);
			automataFN.addEstadosAceptacion(aceptacion);
			automataFN.setLenguajeR(simbolosimple + "");
			Console.WriteLine(tran.DOT_String());
			return automataFN;

		}
		///// METODO QUE ES PARA LA CONCATENACION 
		public Automata concatenacion(Automata AFN1, Automata AFN2)
		{
			Automata afn_concat = new Automata();
			int i = 0;
			//se agragaran los estados de cada uno de los automatas del lado derecho e izquierdo para luego unirlos por se de
			// concatencion
			///////////////SE PROCEDE A EJECUTAR LOS DEL LADO IZQUIERDO
			Trancisiones tran=null;
			for (i = 0; i < AFN1.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)AFN1.getEstados().ElementAt(i);
				tmp.setId(i);
				/// ahorita se define el estado inicial
				if (i == 0)
				{
					afn_concat.setEstadoInicial(tmp);
				}
				///cuando llega al último, concatena el ultimo con el primero del otro automata con un epsilon
				if (i == AFN2.getEstados().Count() - 1)
				{
					
					//se utiliza un ciclo porque los estados de aceptacion son un array
					for (int k = 0; k < AFN1.getEstadosAceptacion().Count(); k++)
					{
						tran = new Trancisiones();
						tran.Transicion((Estado)AFN1.getEstadosAceptacion().ElementAt(k), AFN2.getEstadoInicial(), AnalizadorTexto.AnalizadorTexto.EPSILON);
						tmp.setTransiciones(tran);
					}
				}
				afn_concat.addEstados(tmp);

			}

			//// SE PROCEDE A EJECUTAR LO DEL LADO DERECHO
			for (int j = 0; j < AFN2.getEstados().Count(); j++)
			{
				Estado tmp = (Estado)AFN2.getEstados().ElementAt(j);
				tmp.setId(i);

				//define el ultimo con estado de aceptacion
				if (AFN1.getEstados().Count() - 1 == j)
					afn_concat.addEstadosAceptacion(tmp);
				afn_concat.addEstados(tmp);
				i++;
			}
			Hashtable alfabeto = new Hashtable();
			alfabeto.Add(AFN1.getAlfabeto(),12);
			alfabeto.Add(AFN2.getAlfabeto(),22);
			afn_concat.setAlfabeto(alfabeto);
			afn_concat.setLenguajeR(AFN1.getLenguajeR() + " " + AFN2.getLenguajeR());
			Console.WriteLine(tran.toString());
			return afn_concat;
		}
		////////////////////////////////////////////////CERRADURA DE KLEENE////////////////////////////////////////////////////
		public Automata cerraduraKleene(Automata automataFN)
		{
			Automata afn_kleene = new Automata();

			//se crea un nuevo estado inicial
			Estado nuevoInicio = new Estado(0);
			afn_kleene.addEstados(nuevoInicio);
			afn_kleene.setEstadoInicial(nuevoInicio);

			//agregar todos los estados intermedio
			for (int i = 0; i < automataFN.getEstados().Count(); i++)
			{
				Estado tmp = (Estado)automataFN.getEstados().ElementAt(i);
				Console.WriteLine("id      " + tmp.getId());
				tmp.setId(i + 1);
				afn_kleene.addEstados(tmp);
			}

			//Se crea un nuevo estado de aceptacion
			Estado nuevoFin = new Estado(automataFN.getEstados().Count() + 1);
			afn_kleene.addEstados(nuevoFin);
			afn_kleene.addEstadosAceptacion(nuevoFin);

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
			afn_kleene.setAlfabeto(automataFN.getAlfabeto());
			afn_kleene.setLenguajeR(automataFN.getLenguajeR());
			return afn_kleene;
		}
	}

	/// <summary>
	/// ///EN ESTE SE CREA UNA CLASE AUTOMATA QUE SE UTILIZARA PARA HACER EL DE THOMPSON
	/// </summary>
	class Automata
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

				if (!ch.getLexema().Equals('|') && !ch.getLexema().Equals('.') && !ch.getLexema().Equals('.') && !ch.getLexema().Equals( AnalizadorTexto.AnalizadorTexto.EPSILON_CHAR))
				{
					Console.WriteLine("CHAR QUE SE AGREGA AL ALFABETO ->"+ch.getLexema());
					this.alfabeto.Add(ch.getLexema(), i);// lo agrega a la tabla hash
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

		public String DOT_THOMPSON()
		{
			String res = "";
			
			res += "Conjunto de transiciones ";
			for (int i = 0; i < this.estados.Count(); i++)
			{
				Estado est = estados.ElementAt(i);
				if (i == 0)
				{
					res += est.getId() + "  ->  "+ est.getTransiciones().First().getSimbolo()+ "\r\n";
				}
				if (i != 0)
				{
					Estado estanterior = estados.ElementAt(i - 1);
					foreach (Trancisiones t in est.getTransiciones())
					{
						res += est.getId()+ "  ->  "+"  " + t.getSimbolo() + "\r\n";
						
					}
				}
				

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
