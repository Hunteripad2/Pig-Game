using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [HideInInspector] public Tile tile;
    [HideInInspector] public List<Tile> bombedTiles;

    [Header("Bomb")]
    [SerializeField] private int explosionDistance = 2;
    [SerializeField] private float explosionTime = 2f;
    [SerializeField] private AudioSource bombSoundEffect;
    [SerializeField] private SpriteRenderer sprite;

    private void Update()
    {
        if (explosionTime > 0f)
        {
            explosionTime -= Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        bombSoundEffect.Play();
        sprite.color = Color.red;

        bombedTiles = new List<Tile>();
        BombNeighbours(tile, explosionDistance);

        Destroy(gameObject, 0.25f);
        this.enabled = false;
    }

    private void BombNeighbours(Tile tile, int remainingDistance)
    {
        bombedTiles.Add(tile);

        if (tile.character != null)
        {
            tile.character.charController.GetBombed();
        }

        if (remainingDistance > 0)
        {
            for (int i = 0; i < tile.neighbours.Length; i++)
            {
                if (tile.neighbours[i] != null && !bombedTiles.Contains(tile.neighbours[i]))
                {
                    BombNeighbours(tile.neighbours[i], remainingDistance - 1);
                }
            }
        }
    }
}
