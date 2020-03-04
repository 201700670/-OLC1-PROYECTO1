using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _OLC1_PROYECT1.AnalizadorTexto.Token;

namespace _OLC1_PROYECT1.AnalizadorTexto
{
    class AnalizadorTexto
    {
        public LinkedList<Token> salida;
        public LinkedList<Token> errores;
        //public static LinkedList<Conjuntos> L_Conjuntos;
        public String MacroConjuntos = "";
        private int estado;
        private String auxLex;
        private int fila, columna;
        private String imprime = "";
        string nombreconjunto = "";
        string inicioconj, finconj, identificador;
        int contador = 0;
        bool bandera = false, flagcad = false;
        //public LinkedList<PilaExpresion> Pila;// este me sirve para cuardar el identificador y la pila de las expresiones
        //public LinkedList<DatoExpresion> Expresion;//este me sirve para guardar en pila las expresiones regulares con su tipo
        public static LinkedList<String> CadenasExpresion;// este solo me sirve para capturar las cadenas dentro de una expresion y verificar si existen


        public LinkedList<Token> escanear(String entrada)
        {

            salida = new LinkedList<Token>();
            errores = new LinkedList<Token>();
            //L_Conjuntos = new LinkedList<Conjuntos>();
            //Pila = new LinkedList<>();
            CadenasExpresion = new LinkedList<String>();
            estado = 0;
            columna = 0;
            fila = 1;
            auxLex = "";
            int punto = 1, suma = 0, multiplicacion = 0, interrogacion = 0, disyuncion = 0;
            int muletilla = 0, muleta = 0, aux = 0;// SON MULETAS PARA LOS COMENTARIOS
            String c;
            char d;

            for (int i = 0; i < entrada.Length; i++)
            {
                //Console.WriteLine("ESTO ES LO DE LA ENTRADA   "+ entrada.Substring(i,1));
                c = (string)(entrada.Substring(i, 1));
                d = char.Parse( entrada.Substring(i, 1));
                columna++;
                switch (estado)
                {
                    case 0:
                        ////*********ESTE SERVIRA PARA LOS COMENTARIOS Y EL INICIO DE LOS CONJUNTOS PARA LAS EXPRESIONES O SI VIENE UN "-" PARA***************
                        //UN IDENTIFICADOR DE UNA GRAMATICA O "ESTADO"

                        if (c.Equals("/"))
                        {
                            auxLex += c;
                            addToken(Tipo.DIVISION, auxLex, fila, columna);
                            auxLex = "";
                            estado = 0;

                        }
                        else if (c.Equals("<"))
                        {
                            if (muletilla == 1 && !auxLex.Equals(""))
                            {
                                addToken(Tipo.COMENTARIO, auxLex, fila, columna);
                                muletilla = 0;
                                estado = 0;
                            }
                            auxLex += c;
                            addToken(Tipo.MENOR, auxLex, fila, columna);
                            auxLex = "";
                            estado = 0;

                        }
                        else if (c.Equals(">"))
                        {
                            auxLex += c;
                            addToken(Tipo.MAYOR, auxLex, fila, columna);
                            auxLex = "";
                            estado = 0;

                        }
                        else if (c.Equals("!"))
                        {
                            aux++;
                            if (muleta == 1 && !auxLex.Equals(""))
                            {
                                addToken(Tipo.COMENTARIO, auxLex, fila, columna);
                                muleta = 0;
                                estado = 0;
                                auxLex = "";
                            }
                            auxLex += c;
                            addToken(Tipo.ADMIRACION, auxLex, fila, columna);
                            auxLex = "";
                            estado = 0;
                            if (aux == 2)
                            {
                                muleta = 0;
                                aux = 0;
                            }
                            else
                            {
                                muleta = 1;
                            }

                        }
                        else if (c.Equals(" "))
                        {
                            auxLex += c;
                            estado = 0;
                        }
                        else if (char.IsLetter(d))
                        {
                            auxLex += c;
                            if (auxLex.Equals("CONJ"))
                            {
                                addToken(Tipo.RESERVADA, auxLex, fila, columna);
                                auxLex = "";
                                estado = 1;
                                break;
                            }
                            estado = 0;
                            
                        }
                        else if (char.IsDigit(d))
                        {
                            auxLex += c;
                            estado = 0;
                        }
                        else if (c.Equals("\n"))
                        {
                            if (muletilla == 1 && muleta != 1)
                            {
                                addToken(Tipo.COMENTARIO, auxLex, fila, columna);
                                muletilla = 0;
                                estado = 0;
                            }
                            columna = 0;
                            fila += 1;
                            estado = 0;
                            muletilla = 1;
                        }
                        else if (c.Equals("\t") || c.Equals("\r"))
                        {
                            columna += 1;
                            estado = 0;
                        }
                        else if (c.Equals("-"))
                        {
                            identificador = auxLex;
                            addToken(Tipo.IDENTIFICADOR, auxLex, fila, columna);
                            auxLex += c;
                            addToken(Tipo.MENOS, auxLex, fila, columna);
                            auxLex = "";
                            estado = 2;/////////ESTE ME LLEVARA PARA LAS EXPRESIONE REGULARES CON SUS IDENTIFICADORES
                            break;
                        }
                        else if (c.Equals(":"))
                        {
                            addToken(Tipo.IDENTIFICADOR, auxLex, fila, columna);
                            auxLex += c;
                            addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                            auxLex = "";
                            estado = 0;
                        }
                        else
                        {
                            if (char.Parse(c) <= 125 && char.Parse(c) >= 32 && char.Parse(c) != 34)
                            {
                                auxLex += c;
                                estado = 0;
                            }
                            else
                            {
                                auxLex += c;
                                addError(Tipo.DESCONOCIDO, auxLex, "Caracter no definido", fila, columna);
                                Console.WriteLine("ERROR LEXICO0 CON: " + c + " " + fila + "," + columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        break;
                }
            }

                if (errores.Count() > 0)
            {
                return errores;
            }
            else
            {
                //HTML_CONJUNTOS conjuntos = new HTML_CONJUNTOS();
                //conjuntos.HTML_CONJUNTOS(L_Conjuntos);
                return salida;
            }
        }

        public void addToken(Tipo tipo, String lexema, int fila, int columna)
        {
            salida.AddLast(new Token(tipo, auxLex, fila, columna));
            auxLex = "";
            estado = 0;
        }

        public void addError(Tipo tipo, String lexema, String descripcion, int filas, int columnas)
        {
            errores.AddLast(new Token(Tipo.DESCONOCIDO, auxLex, descripcion, fila, columna));
            auxLex = "";
            estado = 0;
        }

        public void imprimirLista(LinkedList<Token> l)
        {
            foreach(Token t in l)
            {
                Console.WriteLine(t.getTipoEnString() + "<-->" + t.getValor() + "<-->" + t.getFila() + "<-->" + t.getColumna());
                imprime += (string)(t.getTipoEnString() + " <--> " + t.getValor() + " <--> " + t.getFila() + " <--> " + t.getColumna())+"\n";
                
            }

        }
    }

}
