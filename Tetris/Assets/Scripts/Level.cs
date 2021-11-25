using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private Grid2D grid;

    // Test basic grid
    IEnumerator FillGrid ()
    {
        GameObject test = GameObject.Find("TestBlock");
        for (int i = 0; i < grid.XLength; i++)
        {
            for (int j = 0; j < grid.YLength; j++)
            {
                grid[i, j] = Instantiate(test);
                yield return new WaitForSeconds(0.050f);
            }
        }
        StartCoroutine("EmptyGrid");
    }

    IEnumerator EmptyGrid()
    {
        for (int i = 0; i < grid.XLength; i++)
        {
            for (int j = 0; j < grid.YLength; j++)
            {
                if (grid.Has(i, j))
                {
                    grid.Delete(i,j);
                }
                yield return new WaitForSeconds(0.025f);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid2D(10, 20);
        StartCoroutine("FillGrid");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
