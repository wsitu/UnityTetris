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
    private int lastBlock = -1;
    private bool canSpawn = true;
    private bool[] markedLines;

    public void ClearLines()
    {
        bool HasFullLine(int y)
        {
            for (int x = 0; x < grid.XLength; x++)
                if (!grid.Has(x, y))
                    return false;
            return true;
        }
        void MarkLine(int y)
        {
            Color darkGray = new Color(0.25f, 0.25f, 0.25f, 1f);
            for (int x = 0; x < grid.XLength; x++)
                grid[x, y].GetComponent<Renderer>().material.color = darkGray;
        }

        for(int row = 0; row < grid.YLength; row++)
        {
            if (HasFullLine(row))
            {
                MarkLine(row);
                markedLines[row] = true;
                canSpawn = false;
            }
            else
                markedLines[row] = false;
        }
        StartCoroutine("RemoveMarkedLines");
    }

    IEnumerator MovePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveSpeed);
            if (currentBlock == null) continue;
            if (currentBlock.CanMove(currentBlock.x, currentBlock.y - 1))
                    currentBlock.y--;
        }
    }

    IEnumerator PlacePlayer()
    {
        int lastY = grid.YLength;
        while (true)
        {
            yield return new WaitForSeconds(moveSpeed);
            if (currentBlock == null) continue;
            if (!currentBlock.CanMove(currentBlock.x, currentBlock.y - 1))
            {
                if (currentBlock.y >= lastY)
                {
                    currentBlock.PlaceDown();
                    lastY = grid.YLength;
                }
                else
                {
                    lastY = currentBlock.y;
                }

            }
            else
                lastY = grid.YLength;
        }
    }

    IEnumerator RemoveMarkedLines()
    {
        void DropLine(int y, int amount)
        {
            if (amount < 1) return;
            for (int x = 0; x < grid.XLength; x++)
                 grid.Swap(x, y, x, y - amount);
        }
        void RemoveLine(int y)
        {
            for (int x = 0; x < grid.XLength; x++)
                grid.Delete(x, y);
        }

        yield return new WaitForSeconds(0.65f * moveSpeed);
        int cleared = 0;
        for (int y = 0; y < grid.YLength; y++)
        {
            if (markedLines[y])
            {
                RemoveLine(y);
                cleared++;
            }
            else
                DropLine(y, cleared);
        }
        score += cleared;
        if (cleared > 4) score++;
        canSpawn = true;
    }

    void RandomBlock()
    {
        int adjustLength = (lastBlock < 0) ? 0 : 1;
        int notLastBlock = Random.Range(adjustLength, blocks.Length);
        if (notLastBlock <= lastBlock) notLastBlock--;
        lastBlock = notLastBlock;
        GameObject chosen = blocks[notLastBlock];
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
        markedLines = new bool[grid.YLength];
        StartCoroutine("MovePlayer");
        StartCoroutine("PlacePlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBlock == null && canSpawn)
        {
            RandomBlock();
        }
    }
}
