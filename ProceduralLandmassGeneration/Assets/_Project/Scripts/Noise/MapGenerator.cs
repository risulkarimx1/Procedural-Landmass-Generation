using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth, mapHeight, noiseScale,ocataves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate = false;
    public int seed;
    public Vector2 offset;

    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (ocataves < 0)
            ocataves = 0;
        
    }

    public void GenerateMap()
    {
        var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed, noiseScale,ocataves, persistance, lacunarity,offset);
        MapDisplay disply = FindObjectOfType<MapDisplay>();
        disply.DrawNoiseMap(noiseMap);
    }
}
