using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Ground Tile", menuName = "Custom Tiles/Ground Tile")]
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
            
            if (bottomTileSprite == rightWallSprite)
                return topRightCornerSprite;
            if (bottomTileSprite == leftWallSprite)
                return topLeftCornerSprite;
            
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
        
        bool keepLookingUp = true;
        bool keepLookingDown = true;
        
        while (true)
        {
            
            if (keepLookingUp)
            {
                bool groundTop = HasGroundTile(tilemap, position + new Vector3Int(0, i, 0));
                bool groundTopRight = HasGroundTile(tilemap, position + new Vector3Int(1, i, 0));
                bool groundTopLeft = HasGroundTile(tilemap, position + new Vector3Int(-1, i, 0));
                
                if (groundTop && !groundTopRight)
                    return rightWallSprite;
                if (groundTop && !groundTopLeft)
                    return leftWallSprite;
                if (!groundTop && groundTopRight)
                    return rightWallSprite;
                if (!groundTop && groundTopLeft)
                    return leftWallSprite;
                
                // if there is nothing up, stop looking
                if (!groundTop && !groundTopRight && !groundTopLeft)
                    keepLookingUp = false;
            }

            if (keepLookingDown)
            {
                bool groundBottom = HasGroundTile(tilemap, position + new Vector3Int(0, -i, 0));
                bool groundBottomRight = HasGroundTile(tilemap, position + new Vector3Int(1, -i, 0));
                bool groundBottomLeft = HasGroundTile(tilemap, position + new Vector3Int(-1, -i, 0));
                
                if (groundBottom && !groundBottomRight)
                    return rightWallSprite;
                if (groundBottom && !groundBottomLeft)
                    return leftWallSprite;
                if (!groundBottom && groundBottomRight)
                    return rightWallSprite;
                if (!groundBottom && groundBottomLeft)
                    return leftWallSprite;
                
                // if there is nothing down, stop looking
                if (!groundBottom && !groundBottomRight && !groundBottomLeft)
                    keepLookingDown = false;
            }
            
            // if there is nothing up and nothing down, there should be no wall here -> can return default sprite
            if (!keepLookingUp && !keepLookingDown)
                return defaultSprite;
            
            i++;
        }
    }
    
    private bool HasGroundTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }
}
