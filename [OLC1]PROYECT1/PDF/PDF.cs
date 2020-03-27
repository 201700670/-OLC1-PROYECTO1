using System.Drawing;
using System.IO;
using System.Diagnostics;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using _OLC1_PROYECT1.AnalizadorTexto;
using System;

namespace _OLC1_PROYECT1
{
    class PDF
    {
        public void crearPDF()
        {

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.LETTER);

            PdfWriter.GetInstance(doc, new FileStream("foto.pdf", FileMode.Create));
            doc.Open();

            // Creamos la imagen y le ajustamos el tamaño
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance("er1.jpg");
            imagen.BorderWidth = 0;
            //imagen.Alignment = Element.ALIGN_RIGHT;
            //float percentage = 0.0f;
            //percentage = 150 / imagen.Width;
            //imagen.ScalePercent(percentage * 100);
            imagen.ScaleToFit(300, 300);


            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("Mi primer documento PDF"));
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
            clApellido.BorderWidth = 0;
            clApellido.BorderWidthBottom = 0.75f;

            PdfPCell clPais = new PdfPCell(new Phrase("País", _standardFont));
            clPais.BorderWidth = 0;
            clPais.BorderWidthBottom = 0.75f;
            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            // Llenamos la tabla con información
            clNombre = new PdfPCell(new Phrase("Roberto", _standardFont));
            clNombre.BorderWidth = 0;

            clApellido = new PdfPCell(new Phrase("Torres", _standardFont));
            clApellido.BorderWidth = 0;

            clPais = new PdfPCell(new Phrase("Puerto Rico", _standardFont));
            clPais.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);


            // Insertamos la imagen en el documento
            doc.Add(imagen);
            doc.Add(tblPrueba);
            // Cerramos el documento
            doc.Close();
        }
        public void crearPDFLexico(LinkedList<Token> entrada)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.LETTER);

            PdfWriter.GetInstance(doc, new FileStream("REPORTE_LEXICO.pdf", FileMode.Create));
            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("LEXICO"));
            doc.Add(Chunk.NEWLINE);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(5);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell Numero = new PdfPCell(new Phrase("No", _standardFont));
            Numero.BorderWidth = 0;
            Numero.BorderWidthBottom = 0.75f;

            PdfPCell Identificador = new PdfPCell(new Phrase("Tipo", _standardFont));
            Identificador.BorderWidth = 0;
            Identificador.BorderWidthBottom = 0.75f;

            PdfPCell Lexema = new PdfPCell(new Phrase("Valor", _standardFont));
            Lexema.BorderWidth = 0;
            Lexema.BorderWidthBottom = 0.75f;

            PdfPCell Fila = new PdfPCell(new Phrase("Fila", _standardFont));
            Fila.BorderWidth = 0;
            Fila.BorderWidthBottom = 0.75f;

            PdfPCell Columna = new PdfPCell(new Phrase("Columna", _standardFont));
            Columna.BorderWidth = 0;
            Columna.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(Numero);
            tblPrueba.AddCell(Identificador);
            tblPrueba.AddCell(Lexema);
            tblPrueba.AddCell(Fila);
            tblPrueba.AddCell(Columna);


            int contador = 0;
            
            foreach (Token t in entrada)
            {
                //Console.WriteLine(t.getTipoEnString() + "<-->" + t.getValor() + "<-->" + t.getFila() + "<-->" + t.getColumna());
                Numero = new PdfPCell(new Phrase(contador.ToString(), _standardFont));
                Numero.BorderWidth = 1;
                Identificador = new PdfPCell(new Phrase(t.getTipoEnString().ToString(), _standardFont));
                Identificador.BorderWidth = 1;
                Lexema = new PdfPCell(new Phrase(t.getValor(), _standardFont));
                Lexema.BorderWidth = 1;
                Fila = new PdfPCell(new Phrase(t.getFila().ToString(), _standardFont));
                Fila.BorderWidth = 1;
                Columna = new PdfPCell(new Phrase(t.getColumna().ToString(), _standardFont));
                Columna.BorderWidth = 1;
                tblPrueba.AddCell(Numero);
                tblPrueba.AddCell(Identificador);
                tblPrueba.AddCell(Lexema);
                tblPrueba.AddCell(Fila);
                tblPrueba.AddCell(Columna);
                contador++;
            }

            doc.Add(tblPrueba);
            // Cerramos el documento
            doc.Close();
        }


        public void crearPDFErrores(LinkedList<Token> entrada)
        {
            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.LETTER);

            PdfWriter.GetInstance(doc, new FileStream("REPORTE_ERRORES.pdf", FileMode.Create));
            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("ERRORES LEXICOS"));
            doc.Add(Chunk.NEWLINE);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(5);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell Numero = new PdfPCell(new Phrase("No", _standardFont));
            Numero.BorderWidth = 0;
            Numero.BorderWidthBottom = 0.75f;

            PdfPCell Identificador = new PdfPCell(new Phrase("Lexema", _standardFont));
            Identificador.BorderWidth = 0;
            Identificador.BorderWidthBottom = 0.75f;

            PdfPCell Lexema = new PdfPCell(new Phrase("Descripcion", _standardFont));
            Lexema.BorderWidth = 0;
            Lexema.BorderWidthBottom = 0.75f;

            PdfPCell Fila = new PdfPCell(new Phrase("Fila", _standardFont));
            Fila.BorderWidth = 0;
            Fila.BorderWidthBottom = 0.75f;

            PdfPCell Columna = new PdfPCell(new Phrase("Columna", _standardFont));
            Columna.BorderWidth = 0;
            Columna.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(Numero);
            tblPrueba.AddCell(Identificador);
            tblPrueba.AddCell(Lexema);
            tblPrueba.AddCell(Fila);
            tblPrueba.AddCell(Columna);


            int contador = 0;

            foreach (Token t in entrada)
            {
                //Console.WriteLine(t.getTipoEnString() + "<-->" + t.getValor() + "<-->" + t.getFila() + "<-->" + t.getColumna());
                //errores.AddLast(new Token(Tipo.DESCONOCIDO, auxLex, descripcion, fila, columna));
                Numero = new PdfPCell(new Phrase(contador.ToString(), _standardFont));
                Numero.BorderWidth = 1;
                Identificador = new PdfPCell(new Phrase(t.getValor().ToString(), _standardFont));
                Identificador.BorderWidth = 1;
                Lexema = new PdfPCell(new Phrase(t.getDescripcion(), _standardFont));
                Lexema.BorderWidth = 1;
                Fila = new PdfPCell(new Phrase(t.getFila().ToString(), _standardFont));
                Fila.BorderWidth = 1;
                Columna = new PdfPCell(new Phrase(t.getColumna().ToString(), _standardFont));
                Columna.BorderWidth = 1;
                tblPrueba.AddCell(Numero);
                tblPrueba.AddCell(Identificador);
                tblPrueba.AddCell(Lexema);
                tblPrueba.AddCell(Fila);
                tblPrueba.AddCell(Columna);
                contador++;
            }

            doc.Add(tblPrueba);
            // Cerramos el documento
            doc.Close();
        }
    }
}
