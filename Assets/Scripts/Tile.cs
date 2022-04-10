using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile(Vector3 position, int layer)
    {
        this.position = position;
        this.layer = layer;
        this.neighbours = new Tile[4];
    }

    readonly public Vector3 position;
    readonly public int layer;
    public bool passable = true;
    public Tile[] neighbours;
    public Character character;
    public GameObject item;
}
