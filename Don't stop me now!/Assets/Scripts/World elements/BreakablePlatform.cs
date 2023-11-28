using System;
using System.Collections;
using System.Collections.Generic;
using Ice_Cube.States;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BreakablePlatform : MonoBehaviour
{
    private Tilemap _tilemap;
    public float msTileBreakTime = 3;
    [FormerlySerializedAs("Parameters")] public IceCubeParameters parameters;

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IceCubeStateManager sm = other.gameObject.GetComponent<IceCubeStateManager>();
            if (sm == null)
            {
                Debug.LogError("Cannot get IceCubeStateManager from player");
                return;
            }
            else if (_tilemap == null)
            {
                Debug.LogError("Cannot get tilemap component");
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
                if (iceCubeState == IceCubeStatesEnum.IsDashing && roundedNorm.x != 0)
                {
                    cellPosition += Vector3Int.right * MathF.Sign(roundedNorm.x);
                    StartBreakingPlatform(cellPosition);
                    break;
                }

                
                Debug.Log("Relative velocity: " + other.relativeVelocity + " Normal: " + roundedNorm.y);
                // Break the platform if ground pounding against a floor
                if ((iceCubeState == IceCubeStatesEnum.IsGroundPounding ||
                     (other.relativeVelocity.y <= -parameters.groundPoundSpeed - Mathf.Epsilon &&
                      other.relativeVelocity.y >= -parameters.groundPoundSpeed + Mathf.Epsilon)) && roundedNorm.y != 0)
                {
                    cellPosition += Vector3Int.up * MathF.Sign(roundedNorm.y);
                    StartBreakingPlatform(cellPosition);
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
    /// <remarks>
    /// This function recursively removes tiles from the platform, starting from the specified cell position.
    /// It traverses adjacent cells (up, down, left, right) and removes tiles if present.
    /// The recursion continues until no more tiles are found or an empty cell is encountered.
    /// </remarks>
    private IEnumerator BreakPlatform(Vector3Int startCellPosition)
    {
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

            _tilemap.SetTile(currentCellPosition, null);

            // Aggiungi le celle adiacenti allo stack
            queue.Enqueue(currentCellPosition + Vector3Int.right);
            queue.Enqueue(currentCellPosition + Vector3Int.left);
            queue.Enqueue(currentCellPosition + Vector3Int.up);
            queue.Enqueue(currentCellPosition + Vector3Int.down);

            if (msTileBreakTime > 0)
                yield return new WaitForSeconds(msTileBreakTime / 1000);
        }
    }

    private void StartBreakingPlatform(Vector3Int startCellPosition)
    {
        Debug.Log("Breaking platform");
        StartCoroutine(BreakPlatform(startCellPosition));
    }
}