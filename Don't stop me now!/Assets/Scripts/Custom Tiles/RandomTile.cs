using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Random Tile", menuName = "Custom Tiles/Random Tile")]
public class RandomTile : Tile
{
    public Sprite[] sprites;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        int index = Random.Range(0, sprites.Length);
        tileData.sprite = sprites[index];
    }
}
