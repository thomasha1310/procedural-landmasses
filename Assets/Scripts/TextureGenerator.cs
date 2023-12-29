using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width)
    {
        Texture2D texture = new Texture2D(width, width);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);

        Color[] colorMap = new Color[width * width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                colorMap[i * width + j] = Color.Lerp(Color.black, Color.white, heightMap[i, j]);
            }
        }

        return TextureFromColorMap(colorMap, width);
    }
}
