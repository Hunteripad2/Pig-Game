using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public Character(Tile tile, bool isPlayer, GameObject prefab)
    {
        this.tile = tile;
        tile.character = this;
        this.isPlayer = isPlayer;
        this.prefab = prefab;
    }

    public Tile tile;
    public CharController charController;
    readonly public bool isPlayer;
    readonly public GameObject prefab;

    public void SetTile(Tile newTile)
    {
        tile.character = null;

        tile = newTile;
        tile.character = this;
    }
}
