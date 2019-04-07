using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D textureFromColorMap(Color[] colormap, int width, int height)
    {
        var texture = new Texture2D(width,height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colormap);
        texture.Apply();
        return texture;
    }

    public static Texture2D textureFromHeightMap(float[,] heightMap)
    {
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var texture = new Texture2D(width, height);
        var colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return textureFromColorMap(colorMap, width, height);
    }
}
