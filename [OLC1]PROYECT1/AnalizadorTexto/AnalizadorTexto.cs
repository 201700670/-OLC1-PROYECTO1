using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _OLC1_PROYECT1.AnalizadorTexto.Token;
using _OLC1_PROYECT1.Listas;

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
        int viendosiesconjunto=0, todosentrecorchetes=0;
        public LinkedList<PilaExpresion> Pila;// este me sirve para cuardar el identificador y la pila de las expresiones
        public LinkedList<DatoExpresion> Expresion;//este me sirve para guardar en pila las expresiones regulares con su tipo
        //public static LinkedList<String> CadenasExpresion;// este solo me sirve para capturar las cadenas dentro de una expresion y verificar si existen


        public LinkedList<Token> escanear(String entrada)
        {

            salida = new LinkedList<Token>();
            errores = new LinkedList<Token>();
            //L_Conjuntos = new LinkedList<Conjuntos>();
            Pila = new LinkedList<PilaExpresion>();
            ///CadenasExpresion = new LinkedList<String>();
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
                            estado = 3;
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
                    case 1:
                        //*******************************ESTE ES PARA HACER EL RECONOCIMIENTO DE LOS CONJUNTOS Y SUS IDENTIFICADORES.*************************
                        if (c.Equals(":"))
                        {
                            if (auxLex.Equals("CONJ"))
                            {
                                addToken(Tipo.RESERVADA, auxLex, fila, columna);
                            }
                            auxLex += c;
                            addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                            auxLex = "";
                            estado = 1;
                        }
                        else if (char.IsLetter(d))
                        {
                            viendosiesconjunto = 0;
                            auxLex += c;
                            estado = 1;
                        }
                        else if (char.IsDigit(d))
                        {
                            viendosiesconjunto = 0;
                            auxLex += c;
                            estado = 1;
                        }
                        else if (c.Equals("-"))
                        {
                            if (auxLex.Equals(""))
                            {
                                MacroConjuntos += auxLex;
                                MacroConjuntos += c;
                                estado = 1;
                            }
                            else
                            {
                                identificador = auxLex;
                                addToken(Tipo.IDENTIFICADOR, auxLex, fila, columna);
                                auxLex += c;
                                addToken(Tipo.MENOS, auxLex, fila, columna);
                                auxLex = "";
                                estado = 1;
                            }
                        }
                        else if (c.Equals(">"))
                        {
                            if (viendosiesconjunto == 0)
                            {
                                auxLex += c;
                                addToken(Tipo.MAYOR, auxLex, fila, columna);
                                auxLex = "";
                                estado = 1;
                                viendosiesconjunto = 1;
                            }
                            else
                            {
                                MacroConjuntos += auxLex;
                                auxLex += c;
                                MacroConjuntos += c;
                                estado = 1;
                            }

                        }
                        else if (c.Equals(" "))
                        {
                            columna += 1;
                            estado = 1;
                        }
                        else if (c.Equals("~"))
                        {
                            if (!auxLex.Equals(""))
                            {
                                inicioconj = auxLex;
                                addToken(Tipo.CONJUNTOINICIO, auxLex, fila, columna);
                            }
                            auxLex += c;
                            addToken(Tipo.VIRGULILLA, auxLex, fila, columna);
                            auxLex = "";
                            estado = 1;
                        }
                        else if (c.Equals(","))
                        {
                            contador++;
                            if (contador == 1)
                            {
                                inicioconj = auxLex;
                                contador = 0;
                            }
                            if (!auxLex.Equals(""))
                            {
                                MacroConjuntos += auxLex;
                                addToken(Tipo.CONJUNTO, auxLex, fila, columna);
                            }
                            auxLex += c;
                            MacroConjuntos += c;
                            addToken(Tipo.COMA, auxLex, fila, columna);
                            auxLex = "";
                            bandera = true;
                            estado = 1;
                        }
                        else if (c.Equals(";"))
                        {
                            viendosiesconjunto = 0;
                            finconj = auxLex;
                            if (bandera)
                            {
                                MacroConjuntos += auxLex;

                            }
                            if (!auxLex.Equals(""))
                            {
                                addToken(Tipo.CONJUNTOFINAL, auxLex, fila, columna);
                                auxLex += c;
                                addToken(Tipo.PUNTOYCOMA, auxLex, fila, columna);
                                auxLex = "";
                                estado = 1;
                            }
                            else
                            {
                                auxLex += c;
                                addToken(Tipo.PUNTOYCOMA, auxLex, fila, columna);
                                auxLex = "";
                                estado = 1;
                            }
                            if (bandera)
                            {
                                Console.WriteLine("---------------------------------------------------------->    " + identificador + "   " + MacroConjuntos);
                                //L_Conjuntos.add(new Conjuntos(identificador, MacroConjuntos));
                                MacroConjuntos = "";
                                identificador = "";
                                bandera = false;
                            }
                            else
                            {
                                Console.WriteLine("---------------------------------------------------------->    " + identificador + "   " + inicioconj+"   "+ finconj);
                                
                                if (esMinuscula(inicioconj) == esMinuscula(finconj))
                                {

                                    //L_Conjuntos.add(new Conjuntos(identificador, inicioconj, finconj));
                                }
                                else if (esMayuscula(inicioconj) == esMinuscula(finconj))
                                {
                                    //L_Conjuntos.add(new Conjuntos(identificador, inicioconj, finconj));
                                }
                                else if (esNumero(inicioconj) == esNumero(finconj))
                                {
                                    //L_Conjuntos.add(new Conjuntos(identificador, inicioconj, finconj));
                                }
                                else
                                {
                                    addError(Tipo.DESCONOCIDO, auxLex, "Caracter no definido", fila, columna);
                                }
                            }
                            identificador = "";
                            MacroConjuntos = "";
                            break;
                        }
                        else if (c.Equals("\n"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                MacroConjuntos += auxLex;
                                auxLex += c;
                                addToken(Tipo.CONJUNTO, auxLex, fila, columna);
                                MacroConjuntos += c;
                                auxLex = "";
                                estado = 1;
                            }
                            else
                            {
                                columna = 0;
                                fila += 1;
                                estado = 0;
                                auxLex = "";
                                break;
                            }
                            
                        }
                        else if (c.Equals("\t") || c.Equals("\r"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                MacroConjuntos += auxLex;
                                auxLex +=  c;
                                addToken(Tipo.CONJUNTO, auxLex, fila, columna);
                                MacroConjuntos += c;
                                auxLex="";
                                estado = 1;
                            }
                            else
                            {
                                fila += 1;
                                estado = 1;
                                auxLex = "";
                            }
                        }
                        else if (c.Equals("/"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                MacroConjuntos += auxLex;
                                auxLex += c;
                                MacroConjuntos += c;
                                estado = 1;
                            }
                            else
                            {
                                auxLex += c;
                                addToken(Tipo.DIVISION, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }

                        }
                        else if (c.Equals("<"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                MacroConjuntos += auxLex;
                                auxLex += c;
                                MacroConjuntos += c;
                                estado = 1;
                            }
                            else
                            {
                                auxLex += c;
                                addToken(Tipo.MENOR, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }

                        }
                        else
                        {
                            if (char.Parse(c) <= 125 && char.Parse(c) >= 32 && char.Parse(c) != 34)
                            {
                                auxLex += c;
                                estado = 1;
                            }
                            else
                            {
                                auxLex += c;
                                addError(Tipo.DESCONOCIDO, auxLex, "Caracter no definido", fila, columna);
                                Console.WriteLine("ERROR LEXICO1 CON: " + auxLex + " " + fila + "," + columna);
                                auxLex = "";
                                estado = 1;
                            }
                        }
                        break;

                    case 2:
                        //9****************************************ESTEE CONTENDRA LOS IDENTIFICADORES Y LAS EXPRESIONES REGULARES****************************
                        if (c.Equals(">"))
                        {
                            viendosiesconjunto = 0;
                            if (flagcad)
                            {
                                auxLex += "\\" + c;
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;
                            }
                            else
                            {
                                Expresion = new LinkedList<DatoExpresion>();
                                auxLex += c;
                                addToken(Tipo.MAYOR, auxLex, fila, columna);
                                auxLex = "";
                                estado = 2;
                                viendosiesconjunto = 1;
                            }
                        }
                        else if (c.Equals(" "))
                        {
                            if (todosentrecorchetes == 2)
                            {
                                auxLex += c;
                                estado = 2;
                                columna += 1;
                            }
                            else
                            {
                                columna += 1;
                                estado = 2;
                            }
                        }
                        else if (c.Equals("."))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;

                            }
                            else
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.OPERADOR, "punto" + punto));

                                punto++;
                                addToken(Tipo.CONCATENACION, auxLex, fila, columna);
                                auxLex = "";
                            }
                            estado = 2;
                        }
                        else if (c.Equals("|"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;

                            }
                            else
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.OPERADOR, "or" + disyuncion));

                                disyuncion++;
                                addToken(Tipo.OR, auxLex, fila, columna);
                                auxLex = "";
                            }
                            estado = 2;
                        }
                        else if (c.Equals("+"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.SUMA, auxLex, fila, columna);
                                flagcad = false;

                            }
                            else
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.OPERADOR, "mas" + suma));

                                suma++;
                                addToken(Tipo.SUMA, auxLex, fila, columna);
                                auxLex = "";
                            }
                            estado = 2;
                        }
                        else if (c.Equals("*"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;

                            }
                            else
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.OPERADOR, "multi" + multiplicacion));

                                multiplicacion++;
                                addToken(Tipo.MULTIPLICACION, auxLex, fila, columna);
                                auxLex = "";
                            }
                            estado = 2;
                        }
                        else if (c.Equals("?"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;

                            }
                            else
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.OPERADOR, "interrrogacion" + interrogacion));

                                interrogacion++;
                                addToken(Tipo.INTERROGACION, auxLex, fila, columna);
                                auxLex = "";
                            }
                            estado = 2;
                        }
                        else if (char.IsLetter(d))
                        {
                            auxLex += c;
                            estado = 2;
                        }
                        else if (char.IsDigit(d))
                        {
                            auxLex += c;
                            estado = 2;
                        }
                        else if (c.Equals("\""))
                        {

                            contador++;
                            flagcad = true;
                            if (contador == 2 && !auxLex.Equals(""))
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                contador = 0;
                            }
                            if (contador == 2)
                            {
                                flagcad = false;
                                contador = 0;
                            }
                            auxLex += c;
                            addToken(Tipo.COMILLA, auxLex, fila, columna);
                            auxLex = "";
                            estado = 2;
                        }
                        else if (c.Equals("{"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;

                            }
                            else
                            {
                                addToken(Tipo.LLAVEIZQ, auxLex, fila, columna);
                                auxLex = "";
                                estado = 2;
                            }
                        }
                        else if (c.Equals("}"))
                        {
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;

                            }
                            else
                            {
                                finconj = auxLex;
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CONJUNTOS, auxLex + i));
                                addToken(Tipo.IDENTIFICADOR, auxLex, fila, columna);
                                auxLex += c;
                                addToken(Tipo.LLAVEDER, auxLex, fila, columna);
                                auxLex = "";
                                estado = 2;
                                break;
                            }
                        }
                        else if (c.Equals(";"))
                        {
                            viendosiesconjunto = 0;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);9
                                if (auxLex.Equals("\\n")|| auxLex.Equals("\\t")|| auxLex.Equals("\\r") || auxLex.Equals("\\'") || auxLex.Equals("\\\""))
                                {
                                    addToken(Tipo.MAYOR, auxLex, fila, columna);
                                    auxLex += c;
                                    addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                                    estado = 2;
                                    auxLex = "";
                                }
                                else if (auxLex.Equals(" \\n") || auxLex.Equals(" \\t") || auxLex.Equals(" \\r") || auxLex.Equals(" \\'") || auxLex.Equals(" \\\""))
                                {
                                    addToken(Tipo.SALTO_DE_LINEA, auxLex, fila, columna);
                                    auxLex += c;
                                    addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                                    estado = 2;
                                    auxLex = "";
                                }
                                else
                                {

                                    auxLex = c;
                                    addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                                    flagcad = false;
                                    auxLex = "";
                                    estado = 2;
                                }

                            }
                            else
                            {
                                punto = 1;
                                suma = 0;
                                multiplicacion = 0;
                                interrogacion = 0;
                                disyuncion = 0;
                                Pila.AddLast(new PilaExpresion(Expresion, identificador));
                                auxLex += c;
                                addToken(Tipo.PUNTOYCOMA, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                                break;
                            }
                        }
                        else if (c.Equals("\\"))
                        {
                            auxLex += c;
                            flagcad = true;
                            estado = 2;
                        }
                        else if (c.Equals("\n"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                auxLex += c;
                                addToken(Tipo.SALTO_DE_LINEA, auxLex, fila, columna);
                                estado = 2;
                                auxLex = "";
                            }
                            else
                            {
                                columna = 0;
                                fila += 1;
                                estado = 0;
                                auxLex = "";
                                break;
                            }
                        }
                        else if (c.Equals("\t") || c.Equals("\r"))
                        {
                            if (viendosiesconjunto == 1)
                            {
                                auxLex += c;
                                addToken(Tipo.SALTO_DE_LINEA, auxLex, fila, columna);
                                estado = 2;
                                auxLex = "";
                            }
                            else
                            {
                                fila += 1;
                                estado = 2;
                                auxLex = "";
                            }
                        }
                        else if (c.Equals("/"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;

                            }
                            else
                            {
                                addToken(Tipo.DIVISION, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        else if (c.Equals("<"))
                        {
                            if (flagcad)
                            {
                                auxLex += "\\" + c;
                                Expresion.AddLast(new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;

                            }
                            else
                            {
                                auxLex += c;
                                addToken(Tipo.MENOR, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        else if (c.Equals("["))
                        {
                            todosentrecorchetes++;
                            auxLex += c;
                            addToken(Tipo.CORCHETEIZQ, auxLex, fila, columna);
                            auxLex = "";
                            estado = 2;
                        }
                        else if (c.Equals(":"))
                        {
                            if (todosentrecorchetes == 1)
                            {
                                auxLex += c;
                                addToken(Tipo.CORCHETEIZQ, auxLex, fila, columna);
                                auxLex = "";
                                todosentrecorchetes++;
                                estado = 2;
                            }
                            else if (todosentrecorchetes == 2)
                            {
                                todosentrecorchetes = 0;
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                auxLex += c;
                                addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                                auxLex = "";
                                estado = 2;
                            }
                            else
                            {
                                auxLex += c;
                                estado = 2;
                            }
                        }
                        else if (c.Equals("]"))
                        {
                            todosentrecorchetes = 0;
                            auxLex += c;
                            addToken(Tipo.CORCHETEDER, auxLex, fila, columna);
                            auxLex = "";
                            estado = 2;
                        }
                        else
                        {
                            if (char.Parse(c) <= 125 && char.Parse(c) >= 32 && char.Parse(c) != 34)
                            {
                                auxLex += c;
                                Expresion.AddLast
                                    (new DatoExpresion(auxLex, DatoExpresion.TipoExpresion.CADENA, "cadena" + i));
                                //CadenasExpresion.add(auxLex);
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 2;
                            }
                            else
                            {
                                auxLex += c;
                                addError(Tipo.DESCONOCIDO, auxLex, "Caracter no definido", fila, columna);
                                Console.WriteLine("ERROR LEXICO2 CON: " + auxLex + " " + fila + "," + columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        break;
                    case 3:
                        //********************************************************ESTE ESTADO SIRVE PARA LOS LEXEMAS******************************************
                        if (c.Equals(":"))
                        {
                            if (flagcad)
                            {
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 3;

                            }
                            else
                            {
                                addToken(Tipo.IDENTIFICADOR, auxLex, fila, columna);
                                auxLex += c;
                                addToken(Tipo.DOSPUNTOS, auxLex, fila, columna);
                                auxLex = "";
                                estado = 3;
                            }
                        }
                        else if (char.IsLetter(d))
                        {
                            auxLex += c;
                            estado = 3;
                        }
                        else if (char.IsDigit(d))
                        {
                            auxLex += c;
                            estado = 3;
                        }
                        else if (c.Equals("\""))
                        {
                            contador++;
                            flagcad = true;
                            if (contador == 2 && !auxLex.Equals(""))
                            {
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                contador = 0;
                            }
                            if (contador == 2)
                            {
                                flagcad = false;
                                contador = 0;
                            }
                            auxLex += c;
                            addToken(Tipo.COMILLA, auxLex, fila, columna);
                            auxLex = "";
                            estado = 3;
                        }
                        else if (c.Equals(" "))
                        {
                            columna += 1;
                            estado = 3;
                        }
                        else if (c.Equals("\n"))
                        {
                            if (flagcad)
                            {
                                auxLex += c;
                                estado = 3;
                            }
                            else
                            {
                                columna = 0;
                                fila += 1;
                                estado = 3;
                                auxLex = "";
                                break;
                            }
                        }
                        else if (c.Equals("\t") || c.Equals("\r"))
                        {
                            if (flagcad)
                            {
                                auxLex += c;
                                estado = 3;
                            }
                            else
                            {
                                fila += 1;
                                estado = 3;
                                auxLex = "";
                            }
                        }
                        else if (c.Equals("/"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 3;

                            }
                            else
                            {
                                addToken(Tipo.DIVISION, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        else if (c.Equals("<"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 3;

                            }
                            else
                            {
                                addToken(Tipo.MENOR, auxLex, fila, columna);
                                auxLex = "";
                                estado = 0;
                            }
                        }
                        else if (c.Equals(";"))
                        {
                            auxLex += c;
                            if (flagcad)
                            {
                                addToken(Tipo.CADENA, auxLex, fila, columna);
                                flagcad = false;
                                auxLex = "";
                                estado = 3;

                            }
                            else
                            {
                                addToken(Tipo.PUNTOYCOMA, auxLex, fila, columna);
                                auxLex = "";
                                estado = 3;
                                break;
                            }
                        }
                        else
                        {
                            //System.out.println("NUMERO DE CARACTER "+entrada.codePointAt(i) +"  NOMBRE DE CHAR  "+d+"  ");
                            if (char.Parse(c) <= 125 && char.Parse(c) >= 32 && char.Parse(c) != 34)
                            {
                                auxLex += c;
                                estado = 3;
                            }
                            else
                            {
                                auxLex += c;
                                addError(Tipo.DESCONOCIDO, auxLex, "Caracter no definido", fila, columna);
                                Console.WriteLine("ERROR LEXICO4 CON: " + auxLex + " " + fila + "," + columna);
                                auxLex = "";
                                estado =3 ;
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
        public static bool esMinuscula(String s)
        {
            // Regresa el resultado de comparar la original con sun versión minúscula
            return s.Equals(s.ToLower());
        }

        public static bool esMayuscula(String s)
        {
            // Regresa el resultado de comparar la original con sun versión mayúscula
            return s.Equals(s.ToUpper());
        }

        public static bool esNumero(String cadena)
        {

            bool resultado;

            try
            {
                int.Parse(cadena);
                resultado = true;
            }
            catch (NotFiniteNumberException excepcion)
            {
                resultado = false;
            }

            return resultado;
        }
    }

}
