using UnityEngine;

// Creates a 2d grid representing GameObject and their location
public class Grid2D 
{
    private GameObject[,] grid;

    public Grid2D(int xSize, int ySize)
    {
        this.grid = new GameObject[xSize, ySize];
    }

    public GameObject this [int xIndex, int yIndex]
    {
        get => this.grid[xIndex, yIndex];
        set
        {
            if (value != null)
            {
                value.transform.position = new Vector3(xIndex, yIndex, 0);
            }
            this.grid[xIndex, yIndex] = value;
        }
    }
    
    public void Delete(int xIndex, int yIndex)
    {
        GameObject.Destroy(this[xIndex, yIndex]);
        this[xIndex, yIndex] = null;
    }

    public bool Has(int xIndex, int yIndex)
    {
        return this[xIndex, yIndex] != null;
    }

    public int XLength
    {
        get => this.grid.GetLength(0);
    }

    public int YLength
    {
        get => this.grid.GetLength(1);
    }
}
