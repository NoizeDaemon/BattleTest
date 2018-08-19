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
    public GameHandler gameHandler;
    public bool initComplete;
    public bool calcComplete;

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
        public sbyte pop; //-1 for enemies, 0 for nothing, 1 for players
        public List<int> path;
        public List<float> pathDif;
    }

    public class Path
    {
        public GridInfo target;
        public List<char> dir;
        public List<float> h;
    }

    public class aStarNode
    {
        public GridInfo tile;
        public float h; //heuristic
        public float g; //movecost
        public float f; //sum
        public GridInfo parent;
    }
    public List<GameObject> gg;
    public List<GameObject> bg;
    public GameObject gridPrefab;
    public IsoTransform activePlayerIsoTransform;
    public Vector3 playerIsoPos;

    public float walkSpeed;
    private Animator playerAnim;
    public List<TileInfo> Map;
    public List<GridInfo> Grid;

    private List<GridInfo>[] pathfinding;
    private List<GridInfo> inReach, aList, bList; //inReach = every floortile in movement distance, aList = inReach checked for height, minimal movement distance, bList = -,,- any move distance
    private GridInfo origin, current, target;

    public int playerMS;
    public float playerJH;


    // Use this for initialization
    void Start()
    {
        gg = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        bg = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
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

        foreach(GameObject p in gg)
        {
            try
            {
                GridInfo g = Grid.Find(x => x.isoPos + new Vector3(0, 0.5f, 0) == p.GetComponent<IsoTransform>().Position);
                g.pop = 1;
            }
            catch
            {
                Debug.Log("Character " + p.name + " is not standing on a tile!");
            }
        }
        foreach (GameObject p in bg)
        {
            try
            {
                GridInfo g = Grid.Find(x => x.isoPos + new Vector3(0, 0.5f, 0) == p.GetComponent<IsoTransform>().Position);
                g.pop = -1;
            }
            catch
            {
                Debug.Log("Character " + p.name + " is not standing on a tile!");
            }
        }
        initComplete = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMovementGrid(GameHandler.CharInfo chara)
    {
        GameObject activePlayer = chara.go;
        playerMS = chara.ms;
        playerJH = chara.jh;
        Debug.Log("Started UpdateMovementGrid");
        playerIsoPos = activePlayer.GetComponent<IsoTransform>().Position;
        inReach = Grid.FindAll(x => Mathf.Abs(x.isoPos.x - playerIsoPos.x) + Mathf.Abs(x.isoPos.z - playerIsoPos.z) <= playerMS);// && Mathf.Abs(x.isoPos.y - playerIsoPos.y + 0.5f) <= playerJH);
        if (gg.Contains(chara.go)) inReach = inReach.FindAll(x => x.pop >= 0);
        else if (bg.Contains(chara.go)) inReach = inReach.FindAll(x => x.pop <= 0);
        origin = inReach.Find(x => x.isoPos.x == playerIsoPos.x && x.isoPos.z == playerIsoPos.z);

        aList = new List<GridInfo>(); //targets that met the hDif criteria
        List<GridInfo> cList = new List<GridInfo>(); //checked currents
        bList = new List<GridInfo>(); //alternatives

        pathfinding = new List<GridInfo>[playerMS];
        pathfinding[0] = inReach.FindAll(x => CarthesianCoords(x).magnitude == 1 && Mathf.Abs(HeightDifferenceCheck(origin, x)) <= playerJH);
        foreach(GridInfo g in pathfinding[0])
        {
            g.path = new List<int>();
            if (CarthesianCoords(g).x == 1) g.path.Add(0);
            else if (CarthesianCoords(g).x == -1) g.path.Add(2);
            else if (CarthesianCoords(g).y == 1) g.path.Add(3);
            else if (CarthesianCoords(g).y == -1) g.path.Add(1);
            g.pathDif = new List<float>();
            g.pathDif.Add(HeightDifferenceCheck(origin, g));
        }
        cList.Add(origin);
        aList.AddRange(pathfinding[0]);

        for(int p = 0; p < playerMS - 1; p++)
        {
            pathfinding[p + 1] = new List<GridInfo>();
            foreach(GridInfo g in pathfinding[p])
            {
                current = g;
                Vector2 currentPos = CarthesianCoords(current);
                //Debug.Log("Started checking for: " + currentPos);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i != 0 && j != 0 || i == 0 && j == 0) continue;

                        target = CarthesianFind(inReach, currentPos.x + i, currentPos.y + j);

                        if (target != null)
                        {
                            if (Mathf.Abs(HeightDifferenceCheck(current, target)) <= playerJH)
                            {
                                if (!aList.Contains(target) && !cList.Contains(target))
                                {
                                    target.path = new List<int>();
                                    target.path.AddRange(current.path);
                                    if (i == 1) target.path.Add(0);
                                    else if (i == -1) target.path.Add(2);
                                    else if (j == 1) target.path.Add(3);
                                    else if (j == -1) target.path.Add(1);
                                    target.pathDif = new List<float>();
                                    target.pathDif.AddRange(current.pathDif);
                                    target.pathDif.Add(HeightDifferenceCheck(current, target));
                                    aList.Add(target);
                                }
                                else
                                {
                                    GridInfo newTarget = new GridInfo
                                    {
                                        go = target.go,
                                        isoPos = target.isoPos                                        
                                    };
                                    newTarget.path = new List<int>();
                                    try
                                    {
                                        newTarget.path.AddRange(current.path);
                                    }
                                    catch
                                    {
                                        //Debug.Log("Nothing to add yet.");
                                    }
                                    if (i == 1) newTarget.path.Add(0);
                                    else if (i == -1) newTarget.path.Add(2);
                                    else if (j == 1) newTarget.path.Add(3);
                                    else if (j == -1) newTarget.path.Add(1);
                                    newTarget.pathDif = new List<float>();
                                    try
                                    {
                                        newTarget.pathDif.AddRange(current.pathDif);
                                    }
                                    catch
                                    {
                                        //Debug.Log("Nothing to add yet.");
                                    }
                                    newTarget.pathDif.Add(HeightDifferenceCheck(current, newTarget));
                                    bList.Add(newTarget);
                                }
                                    
                                pathfinding[p + 1].Add(target);
                            }
                            //else Debug.Log("HeightDifferenceCheck failed for: " + (currentPos.x + i) + "/" + (currentPos.y + j));
                        }
                        //else if (target == null) Debug.Log("Target is null: " + (currentPos.x + i) + "/" + (currentPos.y + j));
                        //else if (aList.Contains(target)) Debug.Log("Target is already in aList: " + (currentPos.x + i) + "/" + (currentPos.y + j));
                        //else if (cList.Contains(target)) Debug.Log("Target is already in cList: " + (currentPos.x + i) + "/" + (currentPos.y + j));
                    }
                }
            }
        }
        chara.movable = aList;
        calcComplete = true;
        //foreach (GridInfo g in aList)
        //{
        //    if(g.standable) g.go.SetActive(true);
        //}
        //gameHandler.canBeCancelled = true;
        //gameHandler.actionButton[0].interactable = true;
    }

    public void DisplayMovementGrid(GameHandler.CharInfo chara)
    {
        foreach (GridInfo g in chara.movable)
        {
            if (g.pop == 0) g.go.SetActive(true);
        }
    }

    public IEnumerator MoveClick(GameObject clickedGo)
    {
        Debug.Log("Coroutine started.");
        Debug.Log("Clicked GO: " + clickedGo.name);
        List<GridInfo> newList = gameHandler.activeChar.movable;
        GridInfo c = newList.Find(x => x.go == clickedGo);
        activePlayerIsoTransform = gameHandler.activeChar.go.GetComponent<IsoTransform>();
        playerAnim = gameHandler.activeChar.go.GetComponent<Animator>();
        origin = Grid.Find(x => x.isoPos.x == activePlayerIsoTransform.Position.x && x.isoPos.z == activePlayerIsoTransform.Position.z);
        //for (int i = 0; i < c.path.Count; i++) Debug.Log("The " + i + " element of cPath is " + c.path[i]);
        //List<GridInfo> alts = bList.FindAll(x => x.go == c.go && x.path.Count == c.path.Count);
        //for(int n = 0; n < alts.Count; n++)
        //{
        //    Debug.Log("The " + n + " altPath has the following directions:");
        //    for (int i = 0; i < alts[n].path.Count; i++) Debug.Log("The " + i + " element of the " + n + " Path is " + alts[n].path[i]);
        //}



        foreach(GridInfo g in newList)
        {
            if (g != c) g.go.SetActive(false);
        }
        Vector3 v = new Vector3(0, 0, 0);
        Vector3 newPos = new Vector3(0, 0, 0);
        Vector3 roundPos = new Vector3(0, 0, 0);
        //float startTime = 0;
        
        playerAnim.SetBool("isWalking", true);
        for (int i = 0; i < c.path.Count; i++)
        {
            Debug.Log(c.path[i]);
            switch (c.path[i])
            {
                case 3:
                    playerAnim.SetBool("directionUp", true);
                    playerAnim.SetBool("directionLeft", true);
                    v = new Vector3(0, 0, 1);
                    //Debug.Log("West");
                    break;
                case 0:
                    playerAnim.SetBool("directionUp", true);
                    playerAnim.SetBool("directionLeft", false);
                    v = new Vector3(1, 0, 0);
                    //Debug.Log("North");
                    break;
                case 1:
                    playerAnim.SetBool("directionUp", false);
                    playerAnim.SetBool("directionLeft", false);
                    v = new Vector3(0, 0, -1);
                    //Debug.Log("East");
                    break;
                case 2:
                    playerAnim.SetBool("directionUp", false);
                    playerAnim.SetBool("directionLeft", true);
                    v = new Vector3(-1, 0, 0);
                    //Debug.Log("South");
                    break;
            }
            if (i >= 1 && c.path[i - 1] != c.path[i])
            {
                yield return new WaitForSeconds(0.3f);
            }
            if (i == c.path.Count - 1) c.go.GetComponent<SpriteRenderer>().enabled = false;
            //startTime = Time.time;
            //Debug.Log(newPos);
            if(c.pathDif[i] == 0)
            {
                newPos = activePlayerIsoTransform.Position + v;
                while (roundPos != newPos)
                {
                    activePlayerIsoTransform.Translate(v * walkSpeed);
                    roundPos = new Vector3(Mathf.Round(activePlayerIsoTransform.Position.x * 100) / 100, activePlayerIsoTransform.Position.y, Mathf.Round(activePlayerIsoTransform.Position.z * 100) / 100);
                    yield return null;
                }
                activePlayerIsoTransform.Position = newPos;
                //yield return new WaitUntil(() => activePlayer.GetComponent<IsoTransform>().Position == newPos);
            }
            else
            {
                newPos = activePlayerIsoTransform.Position + v * 0.4f;
                while (roundPos != newPos)
                {
                    activePlayerIsoTransform.Translate(v * walkSpeed);
                    roundPos = new Vector3(Mathf.Round(activePlayerIsoTransform.Position.x * 100) / 100, activePlayerIsoTransform.Position.y, Mathf.Round(activePlayerIsoTransform.Position.z * 100) / 100);
                    yield return null;
                }
                activePlayerIsoTransform.Position = newPos;
                newPos = activePlayerIsoTransform.Position + v * 0.2f + new Vector3(0, c.pathDif[i], 0);
                while (roundPos != newPos)
                {
                    activePlayerIsoTransform.Translate((v * 0.2f + new Vector3(0, c.pathDif[i], 0)) * walkSpeed);
                    roundPos = new Vector3(Mathf.Round(activePlayerIsoTransform.Position.x * 100) / 100, Mathf.Round(activePlayerIsoTransform.Position.y * 100) / 100, Mathf.Round(activePlayerIsoTransform.Position.z * 100) / 100);
                    yield return null;
                }
                activePlayerIsoTransform.GetComponent<IsoTransform>().Position = newPos;
                newPos = activePlayerIsoTransform.Position + v * 0.4f;
                while (roundPos != newPos)
                {
                    activePlayerIsoTransform.Translate(v * walkSpeed);
                    roundPos = new Vector3(Mathf.Round(activePlayerIsoTransform.Position.x * 100) / 100, activePlayerIsoTransform.Position.y, Mathf.Round(activePlayerIsoTransform.Position.z * 100) / 100);
                    yield return null;
                }
                activePlayerIsoTransform.Position = newPos;

            }
            

        }
        playerAnim.SetBool("isWalking", false);
        c.go.GetComponent<SpriteRenderer>().enabled = true;
        c.go.SetActive(false);
        origin.pop = 0;
        //UpdateMovementGrid();
    }

    //CALCULATORY FUNCTIONS

    private float HeightDifferenceCheck(GridInfo a, GridInfo b)
    {
        float hDif;
        try
        {
            hDif = b.isoPos.y - a.isoPos.y;
        }
        catch
        {
            hDif = Single.PositiveInfinity;
        }
        //Debug.Log("The height difference is " + hDif);
        return hDif;
    }

    private GridInfo CarthesianFind(List<GridInfo> parent, float xC, float zC)
    {
        GridInfo tile = null;
        tile = parent.Find(x => x.isoPos.x == playerIsoPos.x + xC && x.isoPos.z == playerIsoPos.z + zC);
        return tile;
    }

    private Vector2 CarthesianCoords(GridInfo g)
    {
        Vector2 coords = new Vector2(g.isoPos.x - playerIsoPos.x, g.isoPos.z - playerIsoPos.z);
        return coords;
    }
}