using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // made changes in param
    public enum  DrawMode
    {
        NoiseMode,ColorMap,Mesh
    }

    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetails;

    public int  noiseScale,ocataves;

    public float HeightMultiplier;
    public AnimationCurve meshHeightCurve;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public bool autoUpdate = false;
    public int seed;
    public Vector2 offset;

    public TerrainType[] Regions;

    void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;
        if (ocataves < 0)
            ocataves = 0;
        
    }

    public void GenerateMap()
    {
        var noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale,ocataves, persistance, lacunarity,offset);

        Color [] colorMaps = new Color[mapChunkSize* mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight < Regions[i].height)
                    {
                        colorMaps[y * mapChunkSize + x] = Regions[i].color;
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
            disply.DrawTexture(TextureGenerator.textureFromColorMap(colorMaps, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            disply.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap,HeightMultiplier,meshHeightCurve,levelOfDetails),
                TextureGenerator.textureFromColorMap(colorMaps, mapChunkSize, mapChunkSize));
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
