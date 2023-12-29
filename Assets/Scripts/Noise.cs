using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateSquareNoiseMap(int width, int seed, float scale, int numOctaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] map = new float[width, width];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[numOctaves];
        for (int i = 0; i < numOctaves; i++)
        {
            float offsetX = prng.Next(-100_000, 100_000);
            float offsetY = prng.Next(-100_000, 100_000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int o = 0; o < numOctaves; o++)
                {
                    float sampleX = i * frequency / scale + octaveOffsets[o].x + offset.x;
                    float sampleY = j * frequency / scale + octaveOffsets[o].y + offset.y;

                    float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlin * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                map[i, j] = noiseHeight;
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[i, j]);
            }
        }

        return map;
    }
}
