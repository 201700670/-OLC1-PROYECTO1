using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC1_PROYECT1.Listas;
using _OLC1_PROYECT1.AnalizadorTexto;
using _OLC1_PROYECT1.CreandoArbol;
using System.Collections;
using System.Numerics;

namespace _OLC1_PROYECT1.CreandoArbol
{
    class Transformador
    {
        private String nombre;
        private int numestados;
        private int estadoInicial;
        private Hashtable alfabeto;
        private LinkedList<Estado> estadoFinal;
        private LinkedList<int>[,] tabtrans;
        public Object minimizar(Automata automata)
        {

            numestados = automata.getEstados().Count();
            alfabeto = automata.getAlfabeto();
            estadoInicial = automata.getEstadoInicial().getId();
            estadoFinal = automata.getEstadosAceptacion();
            int fila = 0, columna = 0;
            int contadorparatablanoString = 0;
            foreach (Object key in alfabeto.Keys)
            {
                if (key is System.Collections.Hashtable)
                {
                    contadorparatablanoString++;
                }
                else { Console.WriteLine(key); }
            }
            tabtrans = new LinkedList<int>[numestados, alfabeto.Keys.Count - contadorparatablanoString];
            foreach (Estado state in automata.getEstados())
            {
                foreach (Object key in alfabeto.Keys)
                {
                    if (key is System.Collections.Hashtable)
                    {
                        contadorparatablanoString++;
                    }
                    else {
                        LinkedList<int> auxiliar = new LinkedList<int>();
                        foreach (Trancisiones tran in state.getTransiciones())
                        {
                            if (tran.getSimbolo().Equals(key))
                            {
                                auxiliar.AddLast(tran.getFin().getId());
                              
                            }
                        }
                        tabtrans[columna, fila] = auxiliar;
                        fila++;
                    }
                }


                fila = 0;
                columna++;
            }
            
            quitarTansicionesVacias(automata);
            //quitarIndeterminismo();
            /*while (!verificarMinimo())
            {
                minimizar();
            }*/
            //tabtrans = automata.get();
            Automata MINIMIZADO = new Automata();
            int contador = 0;
            /*foreach (Estado state in automata.getEstados())
            {
                Estado nuevoestado = state;

                foreach (Trancisiones tran in state.getTransiciones())
                {
                    nuevoestado.setTransiciones(tran);
                }
                MINIMIZADO.addEstados(nuevoestado);
            }*/
            MINIMIZADO.addEstadosAceptacion(automata.getEstadosAceptacion().ElementAt(0));
            MINIMIZADO.setEstadoInicial(automata.getEstados().ElementAt(0));
            MINIMIZADO.setInicial(automata.getEstados().ElementAt(0));
            MINIMIZADO.setLenguajeR(automata.getLenguajeR());
            MINIMIZADO.setAlfabeto(alfabeto);

            return new AFD_GRAFICA(MINIMIZADO, tabtrans);
        }
        private void quitarIndeterminismo()
        {

            List<LinkedList<int>> nuevosEstados = new List<LinkedList<int>>();
            Hashtable TEMPORAL = new Hashtable();
            LinkedList<int> ts;
            List<int> ts2;

            LinkedList<int> c = new LinkedList<int>();
            c.AddLast(0);
            nuevosEstados.Add(c);
                TEMPORAL.Add(c, 12);
            
            foreach (Object s in alfabeto.Keys)
            {
                if (s is System.Collections.Hashtable)
                {
                }
                else
                {
                    for (int cont = 0; cont < numestados; cont++)
                    {
                        ts = obtenerTransicion(cont, (String)s);
                        bool verificando = false;
                        bool[] comprobando = new bool[ts.Count];
                        int comprueba = 0;
                        foreach (LinkedList<int> ver in nuevosEstados)
                        {

                            foreach (int a in ver)
                            {
                                for (int i = 0; i < ts.Count(); i++)
                                {
                                    if (a == ts.ElementAt(i))
                                    {
                                        comprobando[comprueba] = true;
                                        comprueba++;
                                    }
                                }

                            }
                            comprueba = 0;
                        }
                        bool vers=true;
                        for(int al=0;al<comprobando.Count();al++)
                        {
                            
                                if (al == 0)
                                {
                                    vers = comprobando[0];
                                }
                            if ((al+1)!=comprobando.Count())
                            {
                                
                                    vers = vers && comprobando[al + 1];

                            }
                            else
                            {
                                vers = vers && comprobando[al];
                            }
                        }
                        Console.WriteLine(vers);

                        foreach(LinkedList<int> ver in nuevosEstados)
                        {
                            
                            foreach(int a in ver)
                            {
                               
                                if (!nuevosEstados.Contains(ts) && ts.Contains(a)&& !vers)
                                {
                                    try
                                    {
                                        TEMPORAL.Add(ts, 12);
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                            }
                        }
                        if (ts.Count() != 0 && !nuevosEstados.Contains(ts)&&!vers)
                        {

                            
                            
                            nuevosEstados.Add(ts);
                        }
                    }
                }
            }
            List<LinkedList<int>> temporal = new List<LinkedList<int>>(nuevosEstados);

            /*foreach (LinkedList<int> t in temporal)
            {
                ts2 = new List<int>();
                
                foreach (Object s in alfabeto.Keys)
                {
                    if (s is System.Collections.Hashtable)
                    {
                    }
                    else
                    {
                        foreach (int i in t)
                        {
                            ts2.AddRange(obtenerTransicion(i, (String)s));
                        }
                        if (ts2.Count() != 0 && !nuevosEstados.Contains(ts2))
                        {
                            nuevosEstados.Add(ts2);
                        }
                    }
                }
            }

            LinkedList<int>[,] tablaaux = new LinkedList<int>[nuevosEstados.Count,alfabeto.Keys.Count];


            List<int> tranO;
            LinkedList<int>tran;
            foreach (Object s in alfabeto.Keys)
            {
                if (s is System.Collections.Hashtable)
                {
                }
                else
                {
                    foreach (List<int> t in nuevosEstados)
                    {
                        tranO = new List<int>();
                        tran = new LinkedList<int>();
                        foreach (int i in t)
                        {
                            tranO.AddRange(obtenerTransicion(i, (String)s));
                        }
                        
                            ///-nuevo
                        if (nuevosEstados.Contains(tranO))
                        {
                            foreach (List<int> te in nuevosEstados)
                            {
                                if (tranO == te)
                                {
                                    foreach(int agregar in tranO)
                                    {
                                        tran.AddLast(agregar);
                                    }
                                }
                            }
                        }

                        

                        HashSet<Hashtable> a = new HashSet<Hashtable>();
                        a.Add(alfabeto);
                        int contador = 0;
                        foreach (Object key in alfabeto.Keys)
                        {
                            if (key is System.Collections.Hashtable)
                            {

                            }
                            else
                            {
                                if (key is String)
                                {
                                    if (key.Equals(s))
                                    {
                                        break;
                                    }
                                }
                            }
                            contador++;
                        }
                        int count = 0;
                        if (nuevosEstados.Contains(tranO))
                        {
                            foreach (List<int> te in nuevosEstados)
                            {
                                if (t == te)
                                {
                                    break;
                                }
                                count++;
                            }
                        }

                        tablaaux[count,contador] = tran;
                    }
                }
            }

            LinkedList<Estado> finales = new LinkedList<Estado>();

            foreach (LinkedList<int> t in nuevosEstados)
            {
                foreach (Estado i in estadoFinal)
                {
                    if (t.Contains(i.getId()))
                    {
                        

                            finales.AddLast(i);
                        
                            //finales.add(nuevosEstados.indexOf(t));
                    }
                }
            }

            numestados = nuevosEstados.Count;

            estadoFinal = finales;
            tabtrans = tablaaux;*/
        }
        private void quitarTansicionesVacias(Automata automata)
        {
            //crearemos a partir de nuestro alfabeto la tabla de trancisiones 
            int contadorparatablanoString = 0;
            foreach (Object key in alfabeto.Keys)
            {
                if (key is System.Collections.Hashtable)
                {
                    contadorparatablanoString++;
                }
                else { Console.WriteLine(key); }
            }

            List<int> tran;
            SortedSet<int> clau;
            LinkedList<SortedSet<int>> clau2;
            Hashtable alfabetoTemp = (Hashtable)alfabeto.Clone();
            alfabetoTemp.Remove(AnalizadorTexto.AnalizadorTexto.EPSILON);
            LinkedList<int> [,] tablatransicionesTemp = new LinkedList<int>[numestados, alfabetoTemp.Keys.Count-contadorparatablanoString];

            for (int a = 0; a < alfabetoTemp.Keys.Count-contadorparatablanoString; a++)
            {
                for (int q = 0; q < numestados; q++)
                {
                    tablatransicionesTemp[q,a] = new LinkedList<int>();
                }

            }
            foreach (Object key in alfabeto.Keys)
            {
                if (key is System.Collections.Hashtable)
                {
                    contadorparatablanoString++;
                }
                else {
                    if (!((String)key).Equals(AnalizadorTexto.AnalizadorTexto.EPSILON))
                    {
                        for (int cont = 0; cont < numestados; cont++)
                        {
                            //System.out.print(cont + " "+s+" -");
                            ///aqui va a ir por numero de estados del automata que es el count
                            tran = new List<int>();
                            clau = cerrarVacias(cont);
                            clau2 = new LinkedList<SortedSet<int>>();
                            foreach (int i in clau)
                            {
                                LinkedList<int> temp = obtenerTransicion(i, (String)key);
                                
                                    tran.AddRange(obtenerTransicion(i, (String)key));
                               
                                

                            }
                            
                                int contaa = 0;
                                foreach (int i in tran)
                                {
                                    clau2.AddLast(cerrarVacias(i));

                                    LinkedList<Object> a = new LinkedList<Object>();
                                    a.AddLast(alfabetoTemp);
                                    int contador = 0;
                                    foreach (Object llave in alfabeto.Keys)
                                    {
                                        if (llave is System.Collections.Hashtable)
                                        {

                                        }
                                        else
                                        {
                                            if (llave is String)
                                            {
                                                if (llave.Equals(key))
                                                {
                                                    break;
                                                }
                                            }contador++;
                                        }
                                        
                                    }
                                if (clau2.ElementAt(contaa).Count() > 1)
                                {
                                    int aux1 = 0;
                                    foreach (int auxil in clau2.ElementAt(contaa))
                                    {
                                        if (tablatransicionesTemp[cont, contador].Contains(auxil))
                                        {

                                        }
                                        else
                                        {
                                            tablatransicionesTemp[cont, contador].AddLast(auxil);
                                        }
                                    }
                                }
                                else
                                {
                                    if (tablatransicionesTemp[cont, contador].Contains(clau2.ElementAt(contaa).ElementAt(0)))
                                    {

                                    }
                                    else
                                    {
                                        tablatransicionesTemp[cont, contador].AddLast(clau2.ElementAt(contaa).ElementAt(0));
                                    }

                                }
                                    contaa++;
                                
                            }
                        }
                    }
                }
            }
            SortedSet<int> f = cerrarVacias(estadoInicial);
            bool cq0F = false;
            foreach (Estado i in estadoFinal)
            {
                //int
                if (f.Contains(i.getId()))
                {
                    cq0F = true;
                }
            }
            if (cq0F)
            {
                estadoFinal.AddLast(automata.getEstadoInicial());
            }
            alfabeto = alfabetoTemp;
            tabtrans = tablatransicionesTemp;

        }

        private SortedSet<int> cerrarVacias(int q)
        {
            SortedSet<int> cierre = new SortedSet<int>();
            _ = new LinkedList<int>();
            Stack<LinkedList<int>> pila = new Stack<LinkedList<int>>();
            pila.Push(obtenerTransicion(q, AnalizadorTexto.AnalizadorTexto.EPSILON));
            cierre.Add(q);
            int contatemporal = 0;
            while (pila.Any())
            {
                LinkedList<int> ts = pila.Pop();

                foreach (int i in ts)
                {
                    if (!cierre.Contains(i))
                    {
                        pila.Push(obtenerTransicion(i, AnalizadorTexto.AnalizadorTexto.EPSILON));
                    }
                    contatemporal++;
                }
                if (ts.Count > 0)
                {
                    Console.WriteLine(ts);
                    if (cierre.Contains(ts.ElementAtOrDefault(0)))
                    {

                    }
                    else
                    {
                        cierre.Add(ts.ElementAtOrDefault(0));
                    }
                }
            }
            return cierre;
        }

        private LinkedList<int> obtenerTransicion(int q0, String e)
        {
            HashSet<Hashtable> a = new HashSet<Hashtable>();
            a.Add(alfabeto);
            int contador = 0;
            foreach (Object key in alfabeto.Keys)
            {
                if (key is System.Collections.Hashtable)
                {

                }
                else
                {
                    if (key is String )
                    {
                        if (key.Equals(e))
                        {
                            break;
                        }
                    }contador++;
                }
                
            }
            Console.WriteLine(tabtrans[q0,contador]);
           return tabtrans[q0,contador];
        }

        private bool verificarMinimo()
        {
            bool f = true;

            int[,] estados = new int[numestados,numestados];
            LinkedList<int> r;
            LinkedList<int> t;
            int y;
            int x;
            int tamaño = 0;
            for (int cont = 1; cont < numestados; cont++)
            {
                for (int cont2 = 0; cont2 < cont; cont2++)
                {
                    foreach (Estado states in estadoFinal)
                    {
                        if ((states.getId().Equals(cont) && !states.getId().Equals(cont2)) || (states.getId().Equals(cont2) && !states.getId().Equals(cont)))
                        {
                            estados[cont, cont2] = 1;
                        }
                    }
                    tamaño = 0;
                    int contadorparatablanoString=0;
                    foreach (Object s in alfabeto.Keys)
                    {
                        if (s is System.Collections.Hashtable)
                        {
                            contadorparatablanoString++;
                        }
                        else
                        {
                            r = obtenerTransicion(cont, (String)s);
                            t = obtenerTransicion(cont2,(String) s);
                            if (r.Count() > 0 && t.Count() > 0)
                            {
                                tamaño++;
                                y = r.First();
                                x = t.First();

                                if (x < y)
                                {
                                    if (estados[y,x] == 1)
                                    {
                                        estados[cont,cont2] = 1;
                                    }
                                }
                                else
                                {
                                    if (estados[x,y] == 1)
                                    {
                                        estados[cont,cont2] = 1;
                                    }
                                }
                                if (y != x)
                                {
                                    estados[cont,cont2] = 1;
                                }
                            }
                        }
                        if (tamaño != alfabeto.Keys.Count)
                        {
                            estados[cont,cont2] = 1;
                        }
                    }




                }
            }

            for (int cont = 1; cont < numestados; cont++)
            {
                for (int cont2 = 0; cont2 < cont; cont2++)
                {
                    if (estados[cont,cont2] == 0)
                    {
                        f = false;
                    }
                }
            }

            return f;

        }
        private void minimizar()
        {

            int[,] estados = new int[numestados,numestados];
            LinkedList<int> r;
            LinkedList<int> t;
            int x;
            int y;
            int tamaño = 0;
            for (int cont = 1; cont < numestados; cont++)
            {
                for (int cont2 = 0; cont2 < cont; cont2++)
                {
                    foreach (Estado states in estadoFinal)
                    {

                        if ((states.getId().Equals(cont) && !states.getId().Equals(cont2)) || (states.getId().Equals(cont2) && !states.getId().Equals(cont)))
                        {
                            estados[cont, cont2] = 1;
                        }
                    }
                    tamaño = 0;
                    foreach (Object s in alfabeto.Keys)
                    {
                        if (s is System.Collections.Hashtable)
                        {
                            
                        }
                        else
                        {
                            r = obtenerTransicion(cont, (String)s);
                            t = obtenerTransicion(cont2, (String)s);
                            if (r.Count() > 0 && t.Count() > 0)
                            {
                                tamaño++;
                                x = r.First();
                                y = t.First();

                                if (y < x)
                                {
                                    if (estados[x,y] == 1)
                                    {
                                        estados[cont,cont2] = 1;
                                    }
                                }
                                else
                                {
                                    if (estados[y,x] == 1)
                                    {
                                        estados[cont,cont2] = 1;
                                    }
                                }
                                if (x != y)
                                {
                                    estados[cont,cont2] = 1;
                                }
                            }
                        }
                    }
                    if (tamaño != alfabeto.Keys.Count)
                    {
                        estados[cont,cont2] = 1;
                    }
                }
            }
            LinkedList<List<int>> vector = new LinkedList<List<int>>();
            List<int> ts;
            bool f;

            for (int cont = 1; cont < numestados; cont++)
            {
                for (int cont2 = 0; cont2 < cont; cont2++)
                {
                    if (estados[cont,cont2] == 0)
                    {
                        ts = new List<int>();
                        f = true;

                        ts.Add(cont);
                        ts.Add(cont2);

                        foreach (List<int> tsmod in vector)
                        {
                            if (tsmod.Contains(cont) || tsmod.Contains(cont))
                            {
                                tsmod.AddRange(ts);
                                f = false;
                            }
                        }
                        if (f)
                        {
                            vector.AddLast(ts);
                        }
                    }
                }
            }

            f = true;

            for (int cont = 0; cont < numestados; cont++)
            {
                f = true;
                foreach (List<int> tsmod in vector)
                {
                    if (tsmod.Contains(cont))
                    {
                        f = false;
                    }
                }
                if (f)
                {
                    ts = new List<int>();
                    ts.Add(cont);
                    vector.AddLast(ts);
                }
            }

            LinkedList<int>[,] tablaTemp = new LinkedList<int>[vector.Count(),alfabeto.Keys.Count];

            List<int> tran;
            int t0;
            LinkedList<int> t1;
            foreach (Object s in alfabeto.Keys)
            {
                if (s is System.Collections.Hashtable)
                {

                }
                else
                {
                    foreach (List<int> tsi in vector)
                    {
                        tran = new List<int>();
                        foreach (int i in tsi)
                        {
                            tran.AddRange(obtenerTransicion(i, (String)s));
                        }

                        int counter2 = 0;
                        if (vector.Contains(tsi))
                        {
                            foreach (List<int> elemento in vector)
                            {
                                if (elemento == tsi)
                                {

                                }
                                counter2++;
                            }
                        }


                        t0 = vector.ElementAt(counter2).ElementAt(0);
                        t1 = new LinkedList<int>();
                        foreach (List<int> tsi2 in vector)
                        {
                            int contador = 0;
                            foreach (int i in tran)
                            {
                                if (tran.Count() > 0 && tsi2.Contains(tran.ElementAt(contador)))
                                {
                                    int counter = 0;
                                    if (vector.Contains(tsi2)) {
                                        foreach (List<int> elemento in vector)
                                        {
                                            if (elemento == tsi2)
                                            {

                                            }
                                            counter++;
                                        }
                                    } t1.AddLast(vector.ElementAt(counter).ElementAt(0));
                                }
                                contador++;
                            }
                            
                        }

                        HashSet<Hashtable> a = new HashSet<Hashtable>();
                        a.Add(alfabeto);
                        int contadores = 0;
                        foreach (Object key in alfabeto.Keys)
                        {
                            if (key is System.Collections.Hashtable)
                            {

                            }
                            else
                            {
                                if (key is String)
                                {
                                    if (key.Equals(s))
                                    {
                                        break;
                                    }
                                }
                            }
                            contadores++;
                        }
                        tablaTemp[t0,contadores] = t1;

                    }
                }
            }

            LinkedList<Estado> finales = new LinkedList<Estado>();
            int q00 = estadoInicial;

            foreach (List<int> i in vector)
            {
                if (i.Contains(estadoInicial))
                {
                    int counter = 0;
                    if (vector.Contains(i))
                    {
                        foreach (List<int> elemento in vector)
                        {
                            if (elemento == i)
                            {

                            }
                            counter++;
                        }
                    }
                    q00 = vector.ElementAt(counter).ElementAt(0);
                }

                foreach (Estado ii in estadoFinal)
                {
                    if (i.Contains(ii.getId()))
                    {
                        int counter = 0;
                        if (vector.Contains(i))
                        {
                            foreach (List<int> elemento in vector)
                            {
                                if (elemento == i)
                                {

                                }
                                counter++;
                            }
                        }
                        Estado nuevo = new Estado(vector.ElementAt(counter).ElementAt(0));
                        finales.AddLast(nuevo);
                    }
                }
            }

            estadoInicial = q00;
            numestados = vector.Count();
            estadoFinal = finales;
            tabtrans = tablaTemp;

        }
    }
}
