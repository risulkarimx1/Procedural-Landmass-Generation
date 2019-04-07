using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth, mapHeight, noiseScale,ocataves;
    public float persistance, lacunarity;
    public bool autoUpdate = false;


    public void GenerateMap()
    {
        var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale,ocataves, persistance, lacunarity);
        MapDisplay disply = FindObjectOfType<MapDisplay>();
        disply.DrawNoiseMap(noiseMap);
    }
}
