using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//se hace una clase Nodo que representara cada espacio de la cuadricula en la grilla
public class Nodo  // clase para cada nodo de busqueda (espacio) en la grilla
{
   private Grilla<Nodo> grilla;
   public int x, y ,CostoG, CostoH, CostoF;

    public bool casillaValida;
    public Nodo Padre;  // apuntador al nodo padre del nodo actual

   public Nodo(Grilla<Nodo> grilla, int x, int y) //constructor de la clase nodo
    {
        this.grilla = grilla;
        this.x = x;
        this.y = y;
        casillaValida = true;
    }

    public void CalcularCostoF()
    {
        CostoF = CostoG + CostoH;
    }

    public void CaminoValido(bool casillaValida)
    {
        this.casillaValida = casillaValida;
        grilla.TriggerGridObjectChanged(x,y);
    }


    public override string ToString()
    {
        return x + "," + y;
    }

    public void CrearPared( bool casillaValida)
    {
        this.casillaValida = casillaValida;
        grilla.TriggerGridObjectChanged(x, y);
    }
}
