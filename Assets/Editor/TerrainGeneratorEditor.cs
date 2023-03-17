using UnityEngine;
using UnityEditor;

/// <Summary>
/// エディタ上でTerrainのHeightMapを生成するクラス
/// </Summary>
[CustomEditor(typeof(TerrainGeneratorConfiguration))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var config = target as TerrainGeneratorConfiguration;
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("フィールドの生成"))
        {
            GenerateTerrainOnEditor(config);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("リセット"))
        {
            ResetHeightMap(config);
        }
    }

    /// <Summary>
    /// 設定に応じてTerrainのHeightMapを生成する
    /// </Summary>
    void GenerateTerrainOnEditor(TerrainGeneratorConfiguration config)
    {
        // Terrainの解像度を取得する
        int terrainSizeX = config.field.heightmapResolution;
        int terrainSizeZ = config.field.heightmapResolution;

        // Terrainにセットするハイトマップを作成する
        float[,] newHeightMap = new float[terrainSizeX, terrainSizeZ];

        for (int z = 0; z < terrainSizeZ; z++)
        {
            for (int x = 0; x < terrainSizeX; x++)
            {
                // パーリンノイズの座標を指定して値を取得する
                float xValue = (config.xOrigin + x) * config.scale;
                float yValue = (config.yOrigin + z) * config.scale;
                float value = GetMultiplePerlinNoise(xValue, yValue, terrainSizeX, terrainSizeZ, config);
                float height = config.heightMultiply * value;

                // HeightMapに値をセットする
                newHeightMap[x, z] = height;
            }
        }

        // 生成したHeightMapをセットする
        config.field.SetHeights(0, 0, newHeightMap);
    }

    /// <Summary>
    /// 複数のパーリンノイズをかけ合わせる
    /// </Summary>
    float GetMultiplePerlinNoise(float xPosition, float yPosition, int terrainSizeX, int terrainSizeZ, TerrainGeneratorConfiguration config)
    {
        // 乱数のシードを設定する
        Random.InitState(config.seed);

        // パーリンノイズの値を指定回数だけ乗算する
        float value = Mathf.PerlinNoise(xPosition, yPosition);
        for (int i = 0; i < config.multipleTimes; i++)
        {
            float offsetValue = Random.value;
            float xOffset = offsetValue * terrainSizeX;
            float yOffset = offsetValue * terrainSizeZ;
            float scaleOffset = Random.Range(0.8f, 1.25f);

            float xValue = (xPosition + xOffset) * scaleOffset;
            float yValue = (yPosition + yOffset) * scaleOffset;
            float perlinValue = Mathf.PerlinNoise(xValue, yValue);
            value *= Mathf.Clamp01(perlinValue);
        }

        return value;
    }

    /// <Summary>
    /// TerrainのHeightMapを初期化する
    /// </Summary>
    void ResetHeightMap(TerrainGeneratorConfiguration config)
    {
        int terrainSizeX = config.field.heightmapResolution;
        int terrainSizeZ = config.field.heightmapResolution;

        float[,] newHeightMap = new float[terrainSizeX, terrainSizeZ];
        config.field.SetHeights(0, 0, newHeightMap);
    }
}
