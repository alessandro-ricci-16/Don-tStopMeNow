using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class RandomTile : Tile
{
    public Sprite[] sprites;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        int index = Random.Range(0, sprites.Length);
        tileData.sprite = sprites[index];
    }


#if UNITY_EDITOR
        [MenuItem("Assets/Create/Custom Tiles/Random Tile")]
        public static void CreateRandomTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Random Tile", "New Random Tile", "Asset", "Save Random Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RandomTile>(), path);
        }
    #endif
}
