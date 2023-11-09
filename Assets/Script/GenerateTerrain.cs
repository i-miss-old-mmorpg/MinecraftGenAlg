using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;

public class GenerateTerrain : MonoBehaviour
{
    private int width;
    private int depth;
    private int height;
    private float scale = 255f;

    public GameObject snowBlock;

    public GameObject soilBlock; 
    public GameObject forestBlock; 
    public GameObject waterBlock; 

    public GameObject desertBlock;
    public GameObject rockBlock; 
    public GameObject lavaBlock;

    public GameObject brushBlock;
    public GameObject flowerBlock;
    public GameObject mushroomBlock;
    public GameObject toxicMushroomBlock;
    public GameObject chrysanthemumBlock;
    public GameObject treeBlock;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Noise.Seed = MapData.Seed;
        width = MapData.Width;
        depth = MapData.Depth;
        height = MapData.Height;

        float mapCenterX = width / 2f;
        float mapCenterZ = depth / 2f;
        float maxDistanceFromCenter = Mathf.Sqrt(Mathf.Pow(mapCenterX, 2) + Mathf.Pow(mapCenterZ, 2));

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = x - mapCenterX;
                float nz = z - mapCenterZ;

                // Squared Euclidean distance, normalized to [0, 1]
                float distanceFromCenter = Mathf.Min(1f, (nx * nx + nz * nz) / (maxDistanceFromCenter * maxDistanceFromCenter));

                // Original terrain height, normalized to [0, 1]
                float baseHeight = Noise2D(x, z, scale);
                Debug.Log(baseHeight);

                // Apply the reshaping function
                float reshapedHeight = (baseHeight + (1 - distanceFromCenter)) / 2;

                // Convert back to the original height scale
                int y = Mathf.RoundToInt(reshapedHeight * height);

                // Initialize microclimate
                float temperature = Noise2D(x + 100, z + 100, scale);
                float humidity = Noise2D(x + 200, z + 200, scale);

                GameObject blockToUse;

                if (temperature < 0.33f)
                {
                    if (humidity < 0.33f)
                    {
                        // Cold and Dry
                        blockToUse = soilBlock;
                    }
                    else if (humidity < 0.66f)
                    {
                        // Cold and Medium
                        blockToUse = snowBlock;
                    }
                    else
                    {
                        // Cold and Wet
                        blockToUse = snowBlock;
                    }
                }
                else if (temperature < 0.66f)
                {
                    if (humidity < 0.33f)
                    {
                        // Warm and Dry
                        blockToUse = soilBlock;
                    }
                    else if (humidity < 0.66f)
                    {
                        // Warm and Medium
                        blockToUse = forestBlock;
                    }
                    else
                    {
                        // Warm and Wet
                        blockToUse = forestBlock;
                    }
                }
                else
                {
                    if (humidity < 0.33f)
                    {
                        // Hot and Dry
                        blockToUse = desertBlock;
                    }
                    else if (humidity < 0.66f)
                    {
                        // Hot and Medium
                        blockToUse = rockBlock;
                    }
                    else
                    {
                        // Hot and Wet
                        blockToUse = lavaBlock;
                    }
                }

                Vector3 pos = CoordinateOffset(x, y, z);
                Instantiate(blockToUse, pos, Quaternion.identity);
                //Debug.Log($"Temperature: {temperature}, Humidity: {humidity}, Block: {blockToUse.name}");

                // After the block is instantiated
                GeneratePlant(x, y, z, temperature, humidity);

                // Fill with soil block
                for (int soilY = y - 1; soilY >= y - 2; soilY--)
                {
                    pos = CoordinateOffset(x, soilY, z);
                    Instantiate(soilBlock, pos, Quaternion.identity);
                }
            }
        }
    }

    float Noise2D(float x, float z, float scale)
    {
        float total = 0;

        // First layer: Large amplitude, small frequency
        float amplitude1 = 128.0f;
        float frequency1 = 4.0f;
        total += amplitude1 * Noise.Calc2D(x / scale * frequency1, z / scale * frequency1);

        // Second layer: Medium amplitude, medium frequency
        float amplitude2 = 64.0f;
        float frequency2 = 8.0f;
        total += amplitude2 * Noise.Calc2D(x / scale * frequency2, z / scale * frequency2);

        // Third layer: Small amplitude, large frequency
        float amplitude3 = 32.0f;
        float frequency3 = 16.0f;
        total += amplitude3 * Noise.Calc2D(x / scale * frequency3, z / scale * frequency3);

        // Normalize the result
        total = total / (amplitude1 + amplitude2 + amplitude3);

        return total;
    }

    void GeneratePlant(int x, int y, int z, float temperature, float humidity)
    {
        float C1 = 0.5f, C2 = 0.2f, C3 = 0.2f, C4 = 0.1f;

        float nPlant = C1 * Noise2D(32 * x, 32 * z, scale);
        float hPlant = C2 * humidity;
        float tPlant = C3 * ((0.36f - Mathf.Pow((0.4f - temperature), 2)) / 0.36f);
        float rPlant = C4 * Random.Range(0f, 1f); // Assuming Rand(x, y) is a random number between 0 and 1

        float possiblePlant = nPlant + hPlant + tPlant + rPlant;

        // Assuming we generate a plant if possiblePlant is greater than some threshold
        if (possiblePlant > 0.725f)
        {
            GameObject plantBlock;

            if (temperature < 0.33f)
            {
                if (humidity < 0.33f)
                {
                    // Cold and Dry
                    plantBlock = toxicMushroomBlock;
                }
                else if (humidity < 0.66f)
                {
                    // Cold and Medium
                    plantBlock = mushroomBlock;
                }
                else
                {
                    // Cold and Wet
                    plantBlock = chrysanthemumBlock;
                }
            }
            else if (temperature < 0.66f)
            {
                if (humidity < 0.33f)
                {
                    // Warm and Dry
                    plantBlock = flowerBlock;
                }
                else if (humidity < 0.66f)
                {
                    // Warm and Medium
                    plantBlock = brushBlock;
                }
                else
                {
                    // Warm and Wet
                    plantBlock = treeBlock;
                }
            }
            else
            {
                if (humidity < 0.33f)
                {
                    // Hot and Dry
                    plantBlock = brushBlock;
                }
                else if (humidity < 0.66f)
                {
                    // Hot and Medium
                    plantBlock = null;
                }
                else
                {
                    // Hot and Wet
                    plantBlock = null;
                }
            }

            if (plantBlock != null)
            {
                Vector3 pos = CoordinateOffset(x, y, z);
                Instantiate(plantBlock, pos, Quaternion.identity);
            }

        }
    }

    Vector3 CoordinateOffset(float x, float y, float z)
    {
        return new Vector3(x - width / 2, y, z - depth / 2);
    }

}