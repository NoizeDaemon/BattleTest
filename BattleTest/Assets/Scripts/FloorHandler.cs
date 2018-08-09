using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;
using UltimateIsometricToolkit;

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

    public class GridInfo
    {
        public GameObject go;
        public Vector3 isoPos;
        public SpriteRenderer rend;
        public Vector4 hDif;
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
                    isoPos = o.GetComponent<IsoTransform>().Position,
                    rend = grid.GetComponent<SpriteRenderer>()
                };
                Grid.Add(temp1);
                grid.SetActive(false);
            }
        }
        foreach (GridInfo g in Grid)
        {
            var xP = (Grid.Find(x => x.isoPos.x == g.isoPos.x + 1) != null)
                ? Mathf.Abs(Grid.Find(x => x.isoPos.x == g.isoPos.x + 1).isoPos.y - g.isoPos.y)
                : -1;

            var xN = (Grid.Find(x => x.isoPos.x == g.isoPos.x - 1) != null)
                ? Mathf.Abs(Grid.Find(x => x.isoPos.x == g.isoPos.x - 1).isoPos.y - g.isoPos.y)
                : -1;

            var zP = (Grid.Find(x => x.isoPos.z == g.isoPos.z + 1) != null)
                ? Mathf.Abs(Grid.Find(x => x.isoPos.z == g.isoPos.z + 1).isoPos.y - g.isoPos.y)
                : -1;

            var zN = (Grid.Find(x => x.isoPos.x == g.isoPos.z - 1) != null)
                ? Mathf.Abs(Grid.Find(x => x.isoPos.x == g.isoPos.z - 1).isoPos.y - g.isoPos.y)
                : -1;

            g.hDif = new Vector4(xP, xN, zP, zN);
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
            c.go.SetActive(true);
            Debug.Log(c.go.name);
        }

    }
}