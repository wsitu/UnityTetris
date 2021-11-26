using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{

    public Color blockColor;
    public GameObject[] topRow = new GameObject[4];
    public GameObject[] bottomRow = new GameObject[4];

    public Grid2D levelGrid;
    public Grid2D moveGrid;

    private List<GameObject> bricks = new List<GameObject>();

    void Start()
    {
        moveGrid = new Grid2D(levelGrid.XLength, levelGrid.YLength);

        GameObject[][] layout = new GameObject[2][];
        layout[1] = topRow;
        layout[0] = bottomRow;
        for (int y = 0; y < layout.GetLength(0); y++)
        {
            for (int x = 0; x < layout[y].Length; x++)
            {
                if (layout[y][x] == null) continue;
                GameObject brick = Instantiate(layout[y][x]);
                bricks.Add(brick);
                brick.GetComponent<Renderer>().material.color = blockColor;
                moveGrid[x, y] = brick;
            }
        }
    }

    void Update()
    {
        
    }
}
