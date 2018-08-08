using System.Collections;
using System.Collections.Generic;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using UnityEngine;
using UltimateIsometricToolkit;

public class FloorHandler : MonoBehaviour {

    [System.Serializable]
    public class TileInfo
    {
        public GameObject go;
        public Vector3 isoPos;
        public bool walkablePerm;
        public bool walkableTemp;
    }

    public List<TileInfo> MapInfo;
    public Vector3 playerIsoPos;

	// Use this for initialization
	void Start ()
	{
        MapInfo = new List<TileInfo>();
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
                walkablePerm = (o.CompareTag("FloorTile_walkable")) ? true: false,
                walkableTemp = (o.CompareTag("FloorTile_walkable")) ? true: false
            };
            MapInfo.Add(temp);
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
