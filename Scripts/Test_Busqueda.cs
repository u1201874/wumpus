using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;


public class Test_Busqueda : MonoBehaviour
{
    public bool WallMakerMode = true;
    public int Type = 0;
    private int origeny = 0;
    private int origenx = 0;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
 
    private AStar_PathFinder pathfinding;


    private void Start()
    {
        pathfinding = new AStar_PathFinder(20,10);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrilla());
        pathfindingVisual.SetGrid(pathfinding.GetGrilla());

    }

    private void Update()
    {
        if(WallMakerMode == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                pathfinding.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                pathfinding.GetNodo(x, y).CrearPared(!pathfinding.GetNodo(x, y).casillaValida);
                pathfinding.GetGrilla();
                Debug.Log("pared hecha");
            }

        }else
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("destino");
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                pathfinding.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                List<Nodo> Camino = pathfinding.EncontrarCamino(origenx, origeny, x, y,Type);
                if(Camino != null)
                {
                
                    for (int i=0; i < Camino.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(Camino[i].x, Camino[i].y) * 50f + Vector3.one * 25f, new Vector3(Camino[i+1].x, Camino[i+1].y) * 50f + Vector3.one * 25f, Color.blue,10f);

                    }
             
                }else
                { 
                    Debug.Log("efe");

                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                pathfinding.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                origeny = y;
                origenx = x;
                Debug.Log("origen");
            }

        }

    }



    public void Set_Process(int val)
    {
        if (val == 0)
        {
            Type = 0;
        }

        if (val == 1)
        {
            Type = 1;
        }
    }


    public void MakeWallsMode(bool Seleccion)
    {
        WallMakerMode = Seleccion;
    }

}
