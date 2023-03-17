using UnityEngine;

/// <Summary>
/// TerrainGeneratorの設定クラス
/// </Summary>
public class TerrainGeneratorConfiguration : MonoBehaviour {
    [Tooltip("HeightMapを生成する対象のTerrainを指定する")]
    public TerrainData field;

    [Tooltip("X軸の原点座標を指定する")]
    public float xOrigin = 0.0f;

    [Tooltip("Y軸の原点座標を指定する")]
    public float yOrigin = 0.0f;

    [Tooltip("地形の変化の激しさをを指定する")]
    public float scale = 0.03f;

    [Tooltip("高さの補正用パラメータを指定する")]
    public float heightMultiply = 1f;

    [Tooltip("地形生成に使用する乱数シードを指定する")]
    public int seed = 42;

    [Tooltip("地形生成用のノイズを重ね合わせる回数を指定する")]
    public int multipleTimes = 2;
}
