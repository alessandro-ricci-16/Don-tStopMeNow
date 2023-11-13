using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundTile : Tile
{
    public Sprite defaultSprite;
    public Sprite topWallSprite;
    public Sprite bottomWallSprite;
    public Sprite rightWallSprite;
    public Sprite leftWallSprite;
    public Sprite topRightCornerSprite;
    public Sprite topLeftCornerSprite;
    public Sprite bottomRightCornerSprite;
    public Sprite bottomLeftCornerSprite;
    
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
        {
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int otherPos = new Vector3Int(position.x + xd, position.y + yd, position.z);
                if (HasGroundTile(tilemap, otherPos))
                    tilemap.RefreshTile(otherPos);
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = GetSprite(position, tilemap);
    }

    private Sprite GetSprite(Vector3Int position, ITilemap tilemap)
    {
        Sprite topTileSprite = tilemap.GetSprite(position + new Vector3Int(0, 1, 0));
        Sprite bottomTileSprite = tilemap.GetSprite(position + new Vector3Int(0, -1, 0));
        Sprite rightTileSprite = tilemap.GetSprite(position + new Vector3Int(1, 0, 0));
        Sprite leftTileSprite = tilemap.GetSprite(position + new Vector3Int(-1, 0, 0));
        
        // HARD BOUNDARIES
        
        // top of the ground: if tile on top is empty
        if (!HasGroundTile(tilemap, position + new Vector3Int(0, 1, 0)))
        {
            if (!HasGroundTile(tilemap, position + new Vector3Int(1, 0, 0)))
                return topRightCornerSprite;
            if (!HasGroundTile(tilemap, position + new Vector3Int(-1, 0, 0)))
                return topLeftCornerSprite;

            if (HasGroundTile(tilemap, position + new Vector3Int(-1, 1, 0)))
                return topLeftCornerSprite;
            if (HasGroundTile(tilemap, position + new Vector3Int(1, 1, 0)))
                return topRightCornerSprite;
            
            return topWallSprite;
        }
        
        // bottom of the ground: if tile on the bottom is empty
        if (!HasGroundTile(tilemap, position + new Vector3Int(0, -1, 0)))
        {
            if (!HasGroundTile(tilemap, position + new Vector3Int(1, 0, 0)))
                return bottomRightCornerSprite;
            if (!HasGroundTile(tilemap, position + new Vector3Int(-1, 0, 0)))
                return bottomLeftCornerSprite;
            
            if (HasGroundTile(tilemap, position + new Vector3Int(-1, -1, 0)))
                return bottomLeftCornerSprite;
            if (HasGroundTile(tilemap, position + new Vector3Int(1, -1, 0)))
                return bottomRightCornerSprite;

            if (topTileSprite == rightWallSprite)
                return bottomRightCornerSprite;
            if (topTileSprite == leftWallSprite)
                return bottomLeftCornerSprite;
            
            return bottomWallSprite;
        }
        
        // left and right walls: if tile on right or left is empty
        if (!HasGroundTile(tilemap, position + new Vector3Int(1, 0, 0)))
            return rightWallSprite;
        if (!HasGroundTile(tilemap, position + new Vector3Int(-1, 0, 0)))
            return leftWallSprite;
        
        // SOFT BOUNDARIES

        int i = 1;
        while (true)
        {
            bool groundTop = HasGroundTile(tilemap, position + new Vector3Int(0, i, 0));
            bool groundTopRight = HasGroundTile(tilemap, position + new Vector3Int(1, i, 0));
            bool groundTopLeft = HasGroundTile(tilemap, position + new Vector3Int(-1, i, 0));

            if (!groundTop && !groundTopLeft && !groundTopRight)
                return defaultSprite;
            if (groundTop && !groundTopRight)
                return rightWallSprite;
            if (!groundTop && groundTopRight)
                return rightWallSprite;
            if (groundTopLeft && !groundTop)
                return leftWallSprite;
            if (!groundTopLeft && groundTop)
                return leftWallSprite;
            
            i++;
        }
        

        // continuation of corners and walls: if tile on top or bottom is corner or wall
        // if (topTileSprite == topRightCornerSprite || bottomTileSprite == bottomRightCornerSprite)
        //     return rightWallSprite;
        // if (topTileSprite == topLeftCornerSprite || bottomTileSprite == bottomLeftCornerSprite)
        //     return leftWallSprite;
        // 
        // if (topTileSprite == topWallSprite && HasGroundTile(tilemap, position + new Vector3Int(0, -1, 0)))
        //     return defaultSprite;
        // if (topTileSprite == defaultSprite && HasGroundTile(tilemap, position + new Vector3Int(0, -1, 0)))
        //     return defaultSprite;
        // if (bottomTileSprite == bottomWallSprite && HasGroundTile(tilemap, position + new Vector3Int(0, 1, 0)))
        //     return defaultSprite;
        // if (bottomTileSprite == defaultSprite && HasGroundTile(tilemap, position + new Vector3Int(0, 1, 0)))
        //     return defaultSprite;
        // 
        // if (topTileSprite == rightWallSprite || bottomTileSprite == rightWallSprite)
        //     return rightWallSprite;
        // if (topTileSprite == leftWallSprite || bottomTileSprite == leftWallSprite)
        //     return leftWallSprite;
        
        return defaultSprite;
    }
    
    private bool HasGroundTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }
    
    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
        [MenuItem("Assets/Create/Custom Tiles/GroundTile")]
        public static void CreateRoadTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Ground Tile", "New Ground Tile", "Asset", "Save Ground Tile", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile>(), path);
        }
    #endif
}
