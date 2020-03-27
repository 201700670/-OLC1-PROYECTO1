using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.CreandoArbol;
using _OLC1_PROYECT1.Construccion_AFD;
using System.Collections;
using System.Diagnostics;

namespace _OLC1_PROYECT1
{
    public partial class Form1 : Form
    {
        TextBox creado;
        TabPage tabulador;
        String path;
        public String Nombreselectindex = "";
        List<string> parts = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog buscar = new OpenFileDialog();

            if (buscar.ShowDialog() == DialogResult.OK) {

                if (tabControl1.SelectedIndex == 0)
                {
                    path = buscar.FileName;
                    string[] words = path.Split('\\');
                    int a = words.Count();
                    tabPage1.Text = words[a - 1].ToString();
                    parts.Add(path);
                    // tabulador = new TabPage(words[a - 1TabPage].ToString());
                    //tabControl1.TabPages.Add(tabulador);
                    textBox1.Text = System.IO.File.ReadAllText(path);
                }
                else
                {
                    path = buscar.FileName;
                    creado = new TextBox();
                    string[] words = path.Split('\\');
                    int a = words.Count();
                    int indexi = tabControl1.SelectedIndex;
                    tabControl1.SelectTab(indexi);
                    tabulador.Text = words[a - 1].ToString();
                    creado.Multiline = true;
                    Size antento = tabulador.Size;
                    creado.Size = antento;
                    creado.ScrollBars = ScrollBars.Vertical;
                    creado.Text = System.IO.File.ReadAllText(path);
                    parts.Add(path);
                    tabulador.Controls.Add(creado);
                }
        
            }

        }

        private void agregarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabulador = new TabPage("Tabpage");
            tabControl1.TabPages.Add(tabulador);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (creado != null)
            {


                if (tabControl1.SelectedIndex == 0)
                {

                    string[] lineas = { textBox1.Text };
                    using (StreamWriter outputfile = new StreamWriter(parts[0]))
                    {
                        foreach (string linea in lineas)
                        {
                            outputfile.WriteLine(linea);
                        }
                    }
                }
                else
                {
                    string[] lineas = { creado.Text };
                    using (StreamWriter outputfile = new StreamWriter(parts[tabControl1.SelectedIndex]))    
                    {
                        foreach (string linea in lineas)
                        {
                            outputfile.WriteLine(linea);
                        }
                    }
                }
            }
            else if( tabControl1.SelectedIndex== 0)
            {

                string[] lineas = { textBox1.Text };
                using (StreamWriter outputfile = new StreamWriter(path))
                {
                    foreach (string linea in lineas)
                    {
                        outputfile.WriteLine(linea);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("***************INTERPRETA TXT************");
            String texto = textBox1.Text;
            _OLC1_PROYECT1.AnalizadorTexto.AnalizadorTexto lex = new AnalizadorTexto.AnalizadorTexto();
            lex.escanear(texto);
            lex.imprimirLista(lex.salida);
            foreach(PilaExpresion pilas in lex.Pila)
            {
                Console.WriteLine("----------------->        "+pilas.getIdentificador_expresion());
                Thompson a = new Thompson();
                Automata graficar= a.Analizarentrada(pilas.getPilita());
                
                Console.WriteLine(graficar);
                Hashtable alfabeto = graficar.getAlfabeto();
                alfabeto.Add(AnalizadorTexto.AnalizadorTexto.EPSILON, 145);
                graficar.setAlfabeto(alfabeto);
                Transformador AFD = new Transformador();
                AFD_GRAFICA automata = (AFD_GRAFICA)AFD.minimizar(graficar);
                textBox4.Text=automata.CreandoTabla();
                //AFD.minimizar(graficar);
                //Algoritmo simularAFD = new Algoritmo(graficar,"");
                Graphviz.FileDotEngine.Run1(pilas.getIdentificador_expresion(),crandoautomata(graficar));
                listBox1.Items.Add(pilas.getIdentificador_expresion());
            }
            PDF creando = new PDF();
           
            if(lex.errores.Count()> 0)
            {
                creando.crearPDFErrores(lex.errores);
            }
            else
            {
                creando.crearPDFLexico(lex.salida);
            }
        }
        public String crandoautomata(object raiz)
        { 
            String grafo;
            String resultado="";
            grafo = "digraph grafica{ \r\n rankdir=LR; size = \"8,5\" \r\n ";
           
            
            resultado = grafo + ((Automata)raiz).DOT_THOMPSON((Automata)raiz)+"}";
            return resultado;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            Console.WriteLine(listBox1.Items[i].ToString());
            Nombreselectindex = listBox1.Items[i].ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string pdfPath = Path.Combine(Application.StartupPath, Nombreselectindex.Trim()+ ".jpg");
            Process.Start(pdfPath);
        }

        private void reporteTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pdfPath = Path.Combine(Application.StartupPath, "REPORTE_LEXICO.pdf");
            Process.Start(pdfPath);
        }

        private void reporteErroresLéxicosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pdfPath = Path.Combine(Application.StartupPath, "REPORTE_ERRORES.pdf");
            Process.Start(pdfPath);
        }
    }
}
