using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum  DrawMode
    {
        NoiseMode,ColorMap
    }

    public DrawMode drawMode;

    public int mapWidth, mapHeight, noiseScale,ocataves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate = false;
    public int seed;
    public Vector2 offset;

    public TerrainType[] Regions;

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

        Color [] colorMaps = new Color[mapWidth*mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight < Regions[i].height)
                    {
                        colorMaps[y * mapWidth + x] = Regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay disply = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMode)
            disply.DrawTexture(TextureGenerator.textureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
        {
            disply.DrawTexture(TextureGenerator.textureFromColorMap(colorMaps, mapWidth,mapHeight));
        }

    }
}

[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
