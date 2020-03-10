using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_PROYECT1.AnalizadorTexto
{
    class Token
    {
        Tipo tipo;
        private string valor, descripcion;
        private int fila, columna;

        public enum Tipo
        {
            LETRA,
            NUMERO,
            RESERVADA,
            PORCENTAJE,
            DIVISION,
            MULTILINEA,
            COMILLA,
            DOSPUNTOS,
            PUNTO,
            PUNTOYCOMA,
            COMA,
            GUIONBAJO,
            OR,
            MULTIPLICACION,
            SUMA,
            MENOS,
            MAYOR,
            MENOR,
            ADMIRACION,
            LLAVEDER,
            LLAVEIZQ,
            CORCHETEDER,
            CORCHETEIZQ,
            INTERROGACION,
            VIRGULILLA,
            CADENA,
            LINEA_EN_BLANCO,
            IDENTIFICADOR,
            CONCATENACION,
            ASCII,
            SALTO_DE_LINEA,
            COMENTARIO,
            CONJUNTOINICIO,
            CONJUNTO,
            CONJUNTOFINAL,
            DESCONOCIDO
        }

        public Token(Tipo tipotoken, String valor, int fila, int columna)
        {
            this.tipo = tipotoken;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
        }

        public Token(Tipo tipotoken, String valor, String descripcion, int fila, int columna)
        {
            this.tipo = tipotoken;
            this.valor = valor;
            this.descripcion = descripcion;
            this.fila = fila;
            this.columna = columna;
        }

        public Tipo getTipotoken()
        {
            return tipo;
        }

        public void setTipotoken(Tipo tipotoken)
        {
            this.tipo = tipotoken;
        }

        public String getValor()
        {
            return valor;
        }

        public void setValor(String valor)
        {
            this.valor = valor;
        }

        public String getDescripcion()
        {
            return descripcion;
        }

        public void setDescripcion(String descripcion)
        {
            this.descripcion = descripcion;
        }

        public int getFila()
        {
            return fila;
        }

        public void setFila(int fila)
        {
            this.fila = fila;
        }

        public int getColumna()
        {
            return columna;
        }

        public void setColumna(int columna)
        {
            this.columna = columna;
        }

        public String getTipoEnString()
        {
            switch (tipo)
            {
                case Tipo.LETRA:
                    return "Letra";
                case Tipo.NUMERO:
                    return "Numero";
                case Tipo.RESERVADA:
                    return "Palabra reservada";
                case Tipo.PORCENTAJE:
                    return "Porcentaje";
                case Tipo.DIVISION:
                    return "Division";
                case Tipo.MULTILINEA:
                    return "Multilinea";
                case Tipo.COMILLA:
                    return "Comilla";
                case Tipo.DOSPUNTOS:
                    return "Dos puntos";
                case Tipo.PUNTO:
                    return "Punto";
                case Tipo.PUNTOYCOMA:
                    return "Punto y coma";
                case Tipo.COMA:
                    return "Coma";
                case Tipo.GUIONBAJO:
                    return "Guion bajo";
                case Tipo.OR:
                    return "Disyuncion";
                case Tipo.MULTIPLICACION:
                    return "0 o mas veces";
                case Tipo.SUMA:
                    return "una o mas veces";
                case Tipo.MENOS:
                    return "menos";
                case Tipo.MAYOR:
                    return "Mayor";
                case Tipo.MENOR:
                    return "Menor";
                case Tipo.ADMIRACION:
                    return "Signo Admiracion";
                case Tipo.LLAVEDER:
                    return "LLave derecha";
                case Tipo.LLAVEIZQ:
                    return "LLave izquierda";
                case Tipo.CORCHETEDER:
                    return "Corchete derecha";
                case Tipo.CORCHETEIZQ:
                    return "Corchete izquierda";
                case Tipo.INTERROGACION:
                    return "0 o una vez";
                case Tipo.CONCATENACION:
                    return "Concatenacion";
                case Tipo.ASCII:
                    return "Codigo Ascii";
                case Tipo.VIRGULILLA:
                    return "Hasta";
                case Tipo.CADENA:
                    return "Cadena";
                case Tipo.LINEA_EN_BLANCO:
                    return "Linea en blanco";
                case Tipo.IDENTIFICADOR:
                    return "Identificador";
                case Tipo.SALTO_DE_LINEA:
                    return "Salto de linea";
                case Tipo.COMENTARIO:
                    return "Comentario";
                case Tipo.CONJUNTO:
                    return "Conjunto";
                case Tipo.CONJUNTOINICIO:
                    return "Conjunto";
                case Tipo.CONJUNTOFINAL:
                    return "Conjunto";
                case Tipo.DESCONOCIDO:
                    return "Valor Desconocido";
                default:
                    return "desconocido";
            }

        }

    }
}
