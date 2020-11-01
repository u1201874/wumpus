using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_PathFinder 
{
    // el coste de movimiento en linea recta es 1, pero en diagonal es √2 = 1.4 aprox
    //para no trabajar con floats se multiploca el costo x10 para tener 10 y 14 
    private const int CostoMovimientoRecto = 10;  
    private const int CostoMovimientoDiagonal = 14;

    public static AStar_PathFinder Instance { get; private set; }

    private Grilla<Nodo> grilla;
    private List<Nodo> CaminosPosibles;
    private List<Nodo> CaminosVisitados;

    public AStar_PathFinder(int width, int height)
    {
      
        grilla = new Grilla<Nodo>(width, height, 50f, Vector3.zero, (Grilla<Nodo> g, int x, int y) => new Nodo(g, x, y));

    }

    public Grilla<Nodo> GetGrilla()
    {
        return grilla;

    }



    public List<Nodo> EncontrarCamino(int startX, int startY, int endX, int endY, int type)
    {
        Nodo StartNode = grilla.GetGridObject(startX, startY);
        Nodo endNode = grilla.GetGridObject(endX, endY);

        CaminosPosibles = new List<Nodo> { StartNode };
        CaminosVisitados = new List<Nodo>();
        for (int x = 0; x < grilla.GetWidth(); x++)
        {
            for (int y = 0; y < grilla.GetHeight(); y++)
            {
                Nodo nodo = grilla.GetGridObject(x, y);
                nodo.CostoG = int.MaxValue;
                nodo.CalcularCostoF();
                nodo.Padre = null;
            }
        }
        StartNode.CostoG = 0;
        StartNode.CostoH = CalcularDistanciaEntre(StartNode, endNode);
        StartNode.CalcularCostoF();

        PathfindingDebugStepVisual.Instance.ClearSnapshots();
        PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosibles, CaminosVisitados);

        while (CaminosPosibles.Count >0)
        {

            Nodo NodoActual = EncontrarValorMenor(CaminosPosibles,type);
            if(NodoActual == endNode)
            {

                PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosibles, CaminosVisitados);
                PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grilla, CaminoCalculado(endNode));
                return CaminoCalculado(endNode);
            }
            CaminosPosibles.Remove(NodoActual);
            CaminosVisitados.Add(NodoActual);

            foreach( Nodo nodoVecino in ValorarVecindad(NodoActual))
            {
                if (CaminosVisitados.Contains(nodoVecino)) continue;
                if (!nodoVecino.casillaValida)
                {
                    CaminosVisitados.Add(nodoVecino);
                    continue;
                }
                int CostoGIdeal = NodoActual.CostoG + CalcularDistanciaEntre(NodoActual, nodoVecino);
                if( CostoGIdeal < nodoVecino.CostoG)
                {
                    nodoVecino.Padre = NodoActual;
                    nodoVecino.CostoG = CostoGIdeal;
                    nodoVecino.CostoH = CalcularDistanciaEntre(nodoVecino, endNode);
                    nodoVecino.CalcularCostoF();
                    if(!CaminosPosibles.Contains(nodoVecino))
                    {
                        CaminosPosibles.Add(nodoVecino);
                    }
                }
                PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosibles, CaminosVisitados);
            }
        }

        //una vez se acaben los caminos posibles y no hay solucion
        return null;



    }


    private List<Nodo> ValorarVecindad(Nodo nodoactual)
    {
        List<Nodo> ListaDeVecinos = new List<Nodo>();
        if (nodoactual.x - 1 >= 0)
        {
            // Isquierda
            ListaDeVecinos.Add(GetNodo(nodoactual.x - 1, nodoactual.y));
            // Diagonal Izquierda Abajo
            if (nodoactual.y - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x - 1, nodoactual.y - 1));
            // Diagonal Izquierda Arriba
            if (nodoactual.y + 1 < grilla.GetHeight()) ListaDeVecinos.Add(GetNodo(nodoactual.x - 1, nodoactual.y + 1));
        }
        if (nodoactual.x + 1 < grilla.GetWidth())
        {
            // Derecha
            ListaDeVecinos.Add(GetNodo(nodoactual.x + 1, nodoactual.y));
            // Diagonal Derecha Abajo
            if (nodoactual.y - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x + 1, nodoactual.y - 1));
            // Diagonal Derecha Arriba
            if (nodoactual.y + 1 < grilla.GetHeight()) ListaDeVecinos.Add(GetNodo(nodoactual.x + 1, nodoactual.y + 1));
        }
        // Abajo
        if (nodoactual.y - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y - 1));
        // Arriba
        if (nodoactual.y + 1 < grilla.GetHeight()) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y + 1));

        return ListaDeVecinos;

    }

    public Nodo GetNodo(int x, int y)
    {
        return grilla.GetGridObject(x, y);
    }


    private List<Nodo> CaminoCalculado(Nodo endNode)
    {
        List<Nodo> CaminoSolucion = new List<Nodo>();
        CaminoSolucion.Add(endNode);
        Nodo nodoactual = endNode;
        while (nodoactual.Padre != null)
        {
            CaminoSolucion.Add(nodoactual.Padre);
            nodoactual = nodoactual.Padre;
        }
        CaminoSolucion.Reverse();
        return CaminoSolucion;
     
    }


    private int CalcularDistanciaEntre(Nodo a , Nodo b )
    {
        int DistanciaX = Mathf.Abs(a.x - b.x);
        int Distanciay = Mathf.Abs(a.y - b.y);
        int Resto = Mathf.Abs(DistanciaX - Distanciay);
        return CostoMovimientoDiagonal * Mathf.Min(DistanciaX, Distanciay) + CostoMovimientoRecto * Resto;
    }


    private Nodo EncontrarValorMenor(List<Nodo> ListaNodos, int type)
    {
        Nodo NodoMenor = ListaNodos[0];
        if ( type == 0)
        {
            for(int i=1; i < ListaNodos.Count; i++ )
                {
                    if(ListaNodos[i].CostoF < NodoMenor.CostoF)
                    {
                        NodoMenor = ListaNodos[i];            }
                }
        }
        if (type == 1)
        {
            for (int i = 1; i < ListaNodos.Count; i++)
            {
                if (ListaNodos[i].CostoH < NodoMenor.CostoH)
                {
                    NodoMenor = ListaNodos[i];
                }
            }
        }
        return NodoMenor;
    }


}
