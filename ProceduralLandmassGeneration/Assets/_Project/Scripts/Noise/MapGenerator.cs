using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth, mapHeight, noiseScale;
    public bool autoUpdate = false;


    public void GenerateMap()
    {
        var noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
        MapDisplay disply = FindObjectOfType<MapDisplay>();
        disply.DrawNoiseMap(noiseMap);
    }
}
