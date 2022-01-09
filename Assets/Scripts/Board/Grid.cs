using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Tile[,] grid;

    public List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();
        for (int i = -1; i <= 1; i++)
        {
            Tile result = grid[Mathf.Clamp(tile.gridIndex.x + i, 0, grid.GetLength(0) - 1), tile.gridIndex.y];
            if (!neighbours.Contains(result) && !result.Equals(tile))
                neighbours.Add(result);

            result = grid[tile.gridIndex.x, Mathf.Clamp(tile.gridIndex.y + i, 0, grid.GetLength(1) - 1)];
            if (!neighbours.Contains(result) && !result.Equals(tile))
                neighbours.Add(result);
        }
        return neighbours;
    }
}
