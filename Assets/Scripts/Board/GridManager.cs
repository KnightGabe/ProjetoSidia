using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Grid myGrid;

    public Tile tilePrefab;

    public Vector2 tileOffset;

    private List<Tile> highlightedTiles = new List<Tile>();

    public static GridManager Instance { get { return instance; } }
    private static GridManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateGrid(16, 16);
    }

    public void CreateGrid(int xLength, int yLength)
    {
        myGrid.grid = new Tile[xLength, yLength];
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                myGrid.grid[i, j] = Instantiate(tilePrefab,
                    new Vector3(myGrid.transform.position.x + (j - xLength / 2) * tileOffset.x,
                    myGrid.transform.position.y,
                    myGrid.transform.position.z + (i - yLength / 2) * tileOffset.y),
                    tilePrefab.transform.rotation, myGrid.transform);
                myGrid.grid[i, j].gridIndex = new Vector2Int(i, j);
            }
        }
    }

    public void TileClicked(Tile tile)
    {
        //highlightedTiles = myGrid.GetAdjacentTiles(tile, 3);
        foreach (var item in highlightedTiles)
        {
            item.Dim();
        }
        highlightedTiles.Clear();
        foreach (var item in myGrid.GetAdjacentTiles(tile))
        {
            item.Highlight(Color.green);
            highlightedTiles.Add(item);
        }
        
    }
}
