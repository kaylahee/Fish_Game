using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    public GameObject tilemapPrefab;
    public Transform player;

    private List<GameObject> tilemaps = new List<GameObject>();
    private float spawnPos = 0;
    private float tilemapLength = 20f;

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > spawnPos - tilemapLength * 3)
        {
            SpawnTilemap();
        }
    }

    void SpawnTilemap()
    {
        GameObject newTilemap = Instantiate(tilemapPrefab, new Vector3(spawnPos, 0, 0), Quaternion.identity);
        tilemaps.Add(newTilemap);
        spawnPos += tilemapLength;

        if (tilemaps.Count > 5)
        {
            Destroy(tilemaps[0]);
            tilemaps.RemoveAt(0);
        }
    }
}
