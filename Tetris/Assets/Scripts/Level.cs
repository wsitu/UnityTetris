using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] blocks;

    private Grid2D grid;
    private PlayerBlock currentBlock;
    private float moveSpeed = 1.0f;
    private int score = 0;

    public void ClearLines()
    {
        bool HasFullLine(int y)
        {
            for (int x = 0; x < grid.XLength; x++)
                if (!grid.Has(x, y))
                    return false;
            return true;
        }
        void ClearLine(int y)
        {
            for (int x = 0; x < grid.XLength; x++)
                grid.Delete(x, y);
        }
        void DropLine(int y, int amount)
        {
            if (amount < 1) return; 
            for (int x = 0; x < grid.XLength; x++)
                grid.Swap(x, y, x, y - 1);
        }

        int cleared = 0;
        for(int row = 0; row < grid.YLength; row++)
        {
            if (HasFullLine(row))
            {
                ClearLine(row);
                cleared++;
            }
            else
                DropLine(row, cleared);
        }
        score += cleared;
        if (cleared > 4) score++;
    }

    IEnumerator PlacePlayer()
    {
        bool failedMove = false;
        while (currentBlock != null)
        {
            yield return new WaitForSeconds(moveSpeed);
            if (failedMove)
            {
                currentBlock.PlaceDown();
                currentBlock = null;
                failedMove = false;
            }
            else if (currentBlock.CanMove(currentBlock.x, currentBlock.y - 1))
            {
                currentBlock.y--;
                failedMove = false;
            }
            else
            {
                failedMove = true;
            }
        }
    }

    void RandomBlock()
    {
        GameObject chosen = blocks[Random.Range(0, blocks.Length)];
        PlayerBlock player = Instantiate(chosen).GetComponent<PlayerBlock>();
        player.level = this;
        player.levelGrid = grid;
        player.Spawn(0, 0);
        player.x = (int)(grid.XLength / 2 - player.length / 2);
        player.y = (int)grid.YLength - player.length;
        currentBlock = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid2D(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBlock == null)
        {
            RandomBlock();
            StartCoroutine("PlacePlayer");
        }
    }
}
