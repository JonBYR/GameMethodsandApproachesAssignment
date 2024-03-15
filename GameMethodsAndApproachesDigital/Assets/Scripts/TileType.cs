using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //informs Unity that these are serializable objects within the inspector
public class TileType
{
    public string name; //name of tile
    public GameObject visualPrefab;
    //public bool walkable = true;
}
