using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale,int octaves,float persistance,float lacunarity,Vector2 offset)
    {
        float[,] noiseMap = new float[mapHeight,mapHeight];

        System.Random prng = new System.Random(seed);

        var octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            octaveOffsets[i] =new Vector2(prng.Next(-100000, 100000) + offset.x, prng.Next(-100000, 100000) + offset.y);
        }

        if (scale <= 0)
            scale = .0001f;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    var sampleX = (x-halfWidth) / scale *frequency+octaveOffsets[i].x;
                    var sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;
                    var perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2 -1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                // find min and max to use them for normalize
                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;
                noiseMap[x, y] = noiseHeight;
            }
        }


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
