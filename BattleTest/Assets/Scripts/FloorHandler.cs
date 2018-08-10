using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;
using UltimateIsometricToolkit;
using UnityEditor;


public class FloorHandler : MonoBehaviour
{

    [System.Serializable]
    public class TileInfo
    {
        public GameObject go;
        public Vector3 isoPos;
        public bool walkablePerm;
        public bool passable;
        public bool standable;
    }

    [System.Serializable]
    public class GridInfo
    {
        public GameObject go;
        public Vector3 isoPos;
        public GameObject[] neighbour = new GameObject[4];
    }

    public GameObject gridPrefab;
    public GameObject activePlayer;
    public Vector3 playerIsoPos;
    public List<TileInfo> Map;
    public List<GridInfo> Grid;

    public int playerMS;
    public float playerJH;


    // Use this for initialization
    void Start()
    {
        Map = new List<TileInfo>();
        Grid = new List<GridInfo>();
        List<GameObject> allTiles = new List<GameObject>();
        var walkTiles = GameObject.FindGameObjectsWithTag("FloorTile_walkable");
        var noWalkTiles = GameObject.FindGameObjectsWithTag("FloorTile_unwalkable");
        allTiles.AddRange(walkTiles);
        allTiles.AddRange(noWalkTiles);
        foreach (GameObject o in allTiles)
        {
            TileInfo temp = new TileInfo
            {
                go = o,
                isoPos = o.GetComponent<IsoTransform>().Position,
                walkablePerm = (o.CompareTag("FloorTile_walkable")) ? true : false,
                passable = (o.CompareTag("FloorTile_walkable")) ? true : false,
                standable = (o.CompareTag("FloorTile_walkable")) ? true : false
            };
            Map.Add(temp);

            if (o.CompareTag("FloorTile_walkable"))
            {
                var grid = Instantiate(gridPrefab);
                grid.GetComponent<IsoTransform>().Position = o.transform.GetComponent<IsoTransform>().Position;
                grid.GetComponent<IsoTransform>().Translate(new Vector3(0, 0.01f, 0));
                //grid.GetComponent<IsoTransform>().Position.y += 0.01f;
                grid.transform.parent = o.transform;
                grid.name = o.GetComponent<IsoTransform>().Position.x + "/" +
                            o.GetComponent<IsoTransform>().Position.y + "/" + o.GetComponent<IsoTransform>().Position.z;
                grid.AddComponent<GridSender>();
                GridInfo temp1 = new GridInfo
                {
                    go = grid,
                    isoPos = o.GetComponent<IsoTransform>().Position
                };
                Grid.Add(temp1);
                grid.SetActive(false);
            }
        }
        foreach (GridInfo g in Grid)
        {
            //Neighbours

            try
            {
                g.neighbour[0] = Grid.Find(x => x.isoPos.x == g.isoPos.x + 1).go;
            }
            catch
            {
                g.neighbour[0] = null;
            }
            try
            {
                g.neighbour[1] = Grid.Find(x => x.isoPos.x == g.isoPos.x - 1).go;
            }
            catch
            {
                g.neighbour[1] = null;
            }
            try
            {
                g.neighbour[2] = Grid.Find(x => x.isoPos.z == g.isoPos.z + 1).go;
            }
            catch
            {
                g.neighbour[2] = null;
            }
            try
            {
                g.neighbour[3] = Grid.Find(x => x.isoPos.z == g.isoPos.z - 1).go;
            }
            catch
            {
                g.neighbour[3] = null;
            }


            
            
            //var xP = (Grid.Find(x => x.isoPos.x == g.isoPos.x + 1) != null)
            //    ? Mathf.Abs(Grid.Find(x => x.isoPos.x == g.isoPos.x + 1).isoPos.y - g.isoPos.y)
            //    : Single.PositiveInfinity;
        }

        UpdateMovementGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GridClick(float x, float y, float z)
    {
        Debug.Log(x + "/" + y + "/" + z);

    }

    public void UpdateMovementGrid()
    {
        playerIsoPos = activePlayer.GetComponent<IsoTransform>().Position;
        var inReach = Grid.FindAll(x => Mathf.Abs(x.isoPos.x - playerIsoPos.x) + Mathf.Abs(x.isoPos.z - playerIsoPos.z) <= playerMS);// && Mathf.Abs(x.isoPos.y - playerIsoPos.y + 0.5f) <= playerJH);
        
        //var onSameX = inReach.FindAll(x => x.isoPos.x == playerIsoPos.x);
        //foreach (GridInfo g in onSameX)
        //{
        //    g.go.SetActive(true);
        //}

        foreach (var c in inReach)
        {
            //if(Mathf.Abs(c.isoPos.x - playerIsoPos.x) + Mathf.Abs(c.isoPos.z - playerIsoPos.z) == playerMS) c.go.SetActive(true); // Outline
            //c.go.SetActive(true); // All
   
            
        }

        List<GridInfo> aList = new List<GridInfo>(); //targets that met the hDif criteria
        List<GridInfo> bList = new List<GridInfo>(); //targets that did not meet the hDif criteria
        List<GridInfo> cList = new List<GridInfo>(); //checked currents

        var current = inReach.Find(x => x.isoPos.x == playerIsoPos.x && x.isoPos.z == playerIsoPos.z);
        var target = inReach.Find(x => x.isoPos.x == current.isoPos.x + 1 && x.isoPos.z == current.isoPos.z);

        //if (inReach.Find(x => x.isoPos.x == current.isoPos.x + 1 && x.isoPos.y == current.isoPos.y) != null && !aList.Contains(current))
        //{
        //    if (current.hDif.x <= playerJH) aList.Add(current);
        //    else bList.Add(current);
        //}

        int e = 1; //exceptions
        int n = 0; //for index

        while(n < aList.Count + e)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 && j != 0 || i == 0 && j == 0) continue;
                    try
                    {
                        target = inReach.Find(x => x.isoPos.x == current.isoPos.x + i && x.isoPos.z == current.isoPos.z + j);
                        Debug.Log("Tried & found target " + target.go.name);
                    }
                    catch
                    {
                        target = null;
                        Debug.Log("Target is null");
                    }

                    if (target != null && !aList.Contains(target) && !cList.Contains(target))
                    {
                        if (HeightDifferenceCheck(current, target) <= playerJH) aList.Add(target);
                        else bList.Add(current);;
                    }
                }
            }
            cList.Add(current);
            try
            {
                current = aList[n];
                n++;
            }
            catch
            {
                Debug.Log("No more aList elements.");
                break;
            }
        }
        foreach (GridInfo g in aList)
        {
            g.go.SetActive(true);
        }
    }

    private float HeightDifferenceCheck(GridInfo a, GridInfo b)
    {
        float hDif = Mathf.Abs(a.isoPos.y - b.isoPos.y);
        Debug.Log("The height difference is " + hDif);
        return hDif;
    }
}