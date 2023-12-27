using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "New Tile", menuName = "Custom Tiles/Simple Tile")]
public class SimpleTile : Tile
{
    public Sprite defaultSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = defaultSprite;
    }
    
}