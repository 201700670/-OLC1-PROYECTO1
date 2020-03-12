using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.CreandoArbol;

namespace _OLC1_PROYECT1.CreandoArbol
{
	
	class Thompson
	{
		public LinkedList<DatoExpresion> lista= new LinkedList<DatoExpresion>();
		LinkedList<Object> Operandos = new LinkedList<Object>();
		LinkedList<Object> Operadores = new LinkedList<Object>();
		//public LinkedList<TablaSiguientes> guardarHojas = new LinkedList<>();
		//public LinkedList<TablaSiguientes> Tabla_de_Simbolos = new LinkedList<>();
		String operadores, idgrafica = "";
		AnalizadorTexto. AnalizadorTexto lex = new AnalizadorTexto.AnalizadorTexto();
		public int contadorhojas = 0;
		public static int conta;
		int arbol = 0;
		String datoanterior = "";
		String Resultado = "";
		Automata afn;
		public Thompson()
		{

		}

		public void Analizarentrada(LinkedList<DatoExpresion> Pila)
		{
			foreach(DatoExpresion pila in Pila){
				Console.WriteLine(pila.getLexema()+ "   "+ pila.getTipo()+"   "+ pila.getIdgrafica());
				if (pila.getLexema().Equals(".") && pila.getTipo()==DatoExpresion.TipoExpresion.OPERADOR)
				{
					Automata operadorizq = (Automata)Operadores.ElementAt(Operadores.Count() - 1);
					Operadores.RemoveLast();
					Automata operadorder = (Automata)Operadores.ElementAt(Operadores.Count() - 1);
					Operadores.RemoveLast();
					String Operador = (String)Operandos.ElementAt(Operandos.Count() - 1);
				}
				else if (pila.getLexema().Equals("*") && pila.getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else if (pila.getLexema().Equals("?") && pila.getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else if (pila.getLexema().Equals("|") && pila.getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else if (pila.getLexema().Equals("+") && pila.getTipo() == DatoExpresion.TipoExpresion.OPERADOR)
				{

				}
				else
				{
					idgrafica = pila.getIdgrafica();
					Operadores.AddLast(pila.getLexema());
				}
			}

		}

		

	}


	

	class Tipo
	{
		public enum Tipoarbol
		{
			OR,
			AND,
			KLEEN,
			TERM
		};
	}


	class Automata
	{

		//compuesto por un estado inicial
		private Estado Inicial;
		//en general deberia ser un arreglo de conjuntos finales
		//pero de acuerdo al algoritmo de thomson, siempre 
		//se mantiene un unico estado de aceptacion/final
		private final ArrayList<Estado> aceptacion;
		//array de estados
		private final ArrayList<Estado> estados;
		// alfabeto del autómata, hash para no tener elementos repetidos
		private HashSet alfabeto;
		//atributo para saber si es Determinista o No determinista
		private String tipo;
		//atributo para saber el lenguaje r, la cadena w y el resultado de la simulacion
		private String[] resultadoRegex;
		private String lenguajeR;

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
