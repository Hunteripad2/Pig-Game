using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [HideInInspector] public Tile[,] tiles;
    [HideInInspector] public List<Character> characters;
    [HideInInspector] private Transform charFolder;
    [HideInInspector] private Transform itemFolder;

    [Header("Map")]
    [SerializeField] private int mapSizeX = 17;
    [SerializeField] private int mapSizeY = 9;

    [Header("Tiles")]
    [SerializeField] private float zeroPosX = -8.5f;
    [SerializeField] private float zeroPosY = -3.35f;
    [SerializeField] private float nextPosX = 0.99f;
    [SerializeField] private float nextPosY = 0.85f;
    [SerializeField] private float offsetPosX = 0.11f;

    [Header("Characters")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 playerPosition;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Vector2[] enemyPositions;

    [Header("Items")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int itemsToSpawn = 10;

    private void Start()
    {
        charFolder = GameObject.FindGameObjectWithTag("CharFolder").transform;
        itemFolder = GameObject.FindGameObjectWithTag("ItemFolder").transform;

        GenerateTiles();
        GenerateCharacters();
        GenerateItems();
    }

    private void GenerateTiles()
    {
        tiles = new Tile[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = new Tile(new Vector3(zeroPosX + nextPosX * x + offsetPosX * y, zeroPosY + nextPosY * y, 0f), mapSizeY - y);

                if (x % 2 == 1 && y % 2 == 1)
                {
                    tiles[x, y].passable = false;
                }
            }
        }

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (x > 0 && tiles[x - 1, y].passable)
                {
                    tiles[x, y].neighbours[(int)CharController.MoveDirection.left] = tiles[x - 1, y];
                }
                if (x < mapSizeX - 1 && tiles[x + 1, y].passable)
                {
                    tiles[x, y].neighbours[(int)CharController.MoveDirection.right] = tiles[x + 1, y];
                }
                if (y > 0 && tiles[x, y - 1].passable)
                {
                    tiles[x, y].neighbours[(int)CharController.MoveDirection.down] = tiles[x, y - 1];
                }
                if (y < mapSizeY - 1 && tiles[x, y + 1].passable)
                {
                    tiles[x, y].neighbours[(int)CharController.MoveDirection.up] = tiles[x, y + 1];
                }
            }
        }
    }

    private void GenerateCharacters()
    {
        characters = new List<Character>();

        characters.Add(new Character(tiles[(int)playerPosition.x, (int)playerPosition.y], true, playerPrefab));

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            characters.Add(new Character(tiles[(int)enemyPositions[i].x, (int)enemyPositions[i].y], false, enemyPrefabs[i]));
        }

        foreach (Character character in characters)
        {
            GameObject newCharacter = Instantiate(character.prefab, character.tile.position, Quaternion.identity, charFolder);
            newCharacter.GetComponent<CharController>().characterData = character;
        }
    }

    private void GenerateItems()
    {
        while (itemsToSpawn > 0)
        {
            int randomX = Random.Range(0, mapSizeX);
            int randomY = Random.Range(0, mapSizeY);
            Tile tile = tiles[randomX, randomY];

            if (tile.passable && tile.character == null)
            {
                tile.item = Instantiate(itemPrefab, tile.position, Quaternion.identity, itemFolder);
                itemsToSpawn--;
            }
        }
    }
}
