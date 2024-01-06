using System;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(Tilemap))]
public class BreakablePlatform : MonoBehaviour
{
    private Tilemap _tilemap;
    public IceCubeParameters parameters;
    [SerializeField] private GameObject spriteSplitterPrefab;

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IceCubeStateManager sm = other.gameObject.GetComponent<IceCubeStateManager>();
            if (sm is null)
            {
                Debug.LogError("Cannot get IceCubeStateManager from player");
                return;
            }

            IceCubeStatesEnum iceCubeState = sm.GetCurrentState().GetEnumState();

            ContactPoint2D[] contactPoints = new ContactPoint2D[other.contactCount];
            other.GetContacts(contactPoints);

            foreach (var contact in contactPoints)
            {
                Vector2 roundedNorm = new Vector2(Mathf.Round(contact.normal.x), Mathf.Round(contact.normal.y));
                // Get the position of the contact point in the tilemap
                Vector3Int cellPosition = _tilemap.WorldToCell(contact.point);

                // Break the platform if dashing against a wall
                if (roundedNorm.x != 0 && iceCubeState == IceCubeStatesEnum.IsDashing)
                {
                    cellPosition += Vector3Int.right * MathF.Sign(roundedNorm.x);
                    BreakPlatform(cellPosition, other.transform.position);
                    break;
                }
                
                // Break the platform if ground pounding against a floor
                if (roundedNorm.y != 0 && (iceCubeState == IceCubeStatesEnum.IsGroundPounding ||
                                           Mathf.Approximately(other.relativeVelocity.y, -parameters.groundPoundSpeed)))
                {
                    cellPosition += Vector3Int.up * MathF.Sign(roundedNorm.y);
                    BreakPlatform(cellPosition, other.transform.position);
                    break;
                }
            }
        }
    }

    // Grazie sommo per non farci usare le ricorsioni (scusa Maristella so che ti piacciono)    
    /// <summary>
    /// Breaks the platform starting from the specified cell position and recursively removes adjacent tiles.
    /// </summary>
    /// <param name="startCellPosition">The starting position of the cell to break.</param>
    /// <param name="iceCubePosition">The position of the iceCube.</param>
    /// <remarks>
    /// This function recursively removes tiles from the platform, starting from the specified cell position.
    /// It traverses adjacent cells (up, down, left, right) and removes tiles if present.
    /// The recursion continues until no more tiles are found or an empty cell is encountered.
    /// </remarks>
    private void BreakPlatform(Vector3Int startCellPosition, Vector3 iceCubePosition)
    {
        EventManager.TriggerEvent(EventNames.BreakingPlatform);
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        queue.Enqueue(startCellPosition);

        while (queue.Count > 0)
        {
            Vector3Int currentCellPosition = queue.Dequeue();

            // Verify if it has already been visited to avoid infinite loops
            if (!visited.Add(currentCellPosition))
                continue;

            var tile = _tilemap.GetTile(currentCellPosition);
            if (tile is null)
                continue;
            
            BreakTile(currentCellPosition, iceCubePosition);

            // Adds adjacent tiles into the queue
            queue.Enqueue(currentCellPosition + Vector3Int.right);
            queue.Enqueue(currentCellPosition + Vector3Int.left);
            queue.Enqueue(currentCellPosition + Vector3Int.up);
            queue.Enqueue(currentCellPosition + Vector3Int.down);
        }
    }

    private void BreakTile(Vector3Int tilePosition, Vector3 iceCubePosition)
    {
        GameObject newTile = Instantiate(spriteSplitterPrefab, tilePosition, Quaternion.identity);
        var tileSplitter = newTile.GetComponent<SpriteSplitter>();
        
        tileSplitter.tileSprite = _tilemap.GetSprite(tilePosition);
        tileSplitter.forcePosition = iceCubePosition;
        
        // Remove tile from tilemap
        _tilemap.SetTile(tilePosition, null);
    }
}
