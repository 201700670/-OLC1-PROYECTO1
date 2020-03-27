using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_PROYECT1.CreandoArbol
{
    class AFD_GRAFICA
    {
        private Automata MINIMIZADO;
        private LinkedList<int>[,] tabtrans;

        public AFD_GRAFICA(Automata MINIMIZADO, LinkedList<int>[,] tabtrans)
        {
            this.MINIMIZADO = MINIMIZADO;
            this.tabtrans = tabtrans;
        }

       

        public String CreandoTabla()
        {
            String tabla2 = "";

            foreach (Object key in MINIMIZADO.getAlfabeto().Keys)
            {
                if (key is System.Collections.Hashtable)
                {
                }
                else
                {
                    tabla2 += "\t" + key;
                }
            }
            tabla2 += "\n"+"\n";     
            LinkedList<int> table = new LinkedList<int>();
            table.AddLast(0);
            for (int i = 0; i < tabtrans.GetLength(0); i++)
            {
                for (int j = 0; j < tabtrans.GetLength(1); j++)
                {
                    if (!tabtrans[i,j].Any())
                    {
                        tabtrans[i,j] = table;
                    }
                }
            }
            int identificando = 0;
            foreach (LinkedList<int> estado in tabtrans)
            {
                Console.Write("Estado " + identificando+"  ");
                foreach (int entero in estado)
                {
                    Console.Write("\t" + entero);
                }
                Console.WriteLine("");
                identificando++;
            }
            for (int i = 0; i < tabtrans.GetLength(0); i++)
            {
                for (int j = 0; j < tabtrans.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        
                        tabla2 = tabla2 + i + "\t" ;
                        foreach(int a in tabtrans[i, j])
                        {
                            //Console.WriteLine("888***********************************---------------->        " + a);
                            tabla2 += a+"\t";
                        }
                    }
                    else
                    {

                        
                        foreach (int a in tabtrans[i, j])
                        {
                            //Console.WriteLine("888***********************************---------------->        " + a);
                            tabla2 += a + "\t";
                        }
                    }
                }
                tabla2 = tabla2 + "\n";
            }

                    Console.WriteLine(tabla2);
            return tabla2;
        }
    }
}
