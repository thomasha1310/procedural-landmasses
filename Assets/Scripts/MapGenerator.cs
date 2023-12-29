using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    }

    public DrawMode drawMode;

    public int width;
    public int seed;
    public float scale;
    [Range(1, 8)]
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    [Range(1, 3)]
    public float lacunarity;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate = true;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateSquareNoiseMap(width, seed, scale, octaves, persistence, lacunarity, offset);

        Color[] colorMap = new Color[width * width];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float currentHeight = noiseMap[i, j];
                for (int k = 0; k < regions.Length; k++)
                {
                    if (currentHeight <= regions[k].height)
                    {
                        colorMap[i * width + j] = regions[k].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, width));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, width));
        }
    }

    public void OnValidate()
    {
        if (width < 1)
        {
            width = 1;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}