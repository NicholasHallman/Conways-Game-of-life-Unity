using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : MonoBehaviour
{
    public Material on;
    public Material off;
    public int[,] world = new int[160, 90];
    public GameObject[,] tiles = new GameObject[160, 90];
    public bool init = false;
    public bool isRunning = false;

    // Use this for initialization
    void Start()
    {
        for (int x = -80; x < 80; x++)
        {
            for (int z = -45; z < 45; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tiles[x + 80, z + 45] = tile;
                tile.transform.position = new Vector3(x, 0, z);

            }
        }

        for(int x = 0; x < 160; x++)
        {
            for(int z = 0; z < 90; z++)
            {
                world[x, z] = (int) Mathf.Round( Random.value );
            }
        }
        drawMap(world, tiles);
        init = true;
    }

    void drawMap(int[,] world, GameObject[,] tiles)
    {
        for(int x = 0; x < 160; x++)
        {
            for(int y = 0; y < 90; y++)
            {
                if(world[x,y] == 1)
                {
                    tiles[x, y].GetComponent<Renderer>().sharedMaterial = on;
                }
                else
                {
                    tiles[x, y].GetComponent<Renderer>().sharedMaterial = off;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            if(!isRunning) StartCoroutine(Simulate());
            drawMap(world, tiles);
        }
    }

    IEnumerator Simulate()
    {
        isRunning = true;
        int[,] next = new int[160, 90];

        for (int x = 1; x < 159; x++)
        {
            for(int z = 1; z < 89; z++)
            {
                int totalNeighbors = 0;

                if (world[x - 1, z - 1] == 1) totalNeighbors++;
                if (world[x    , z - 1] == 1) totalNeighbors++;
                if (world[x + 1, z - 1] == 1) totalNeighbors++;
                if (world[x - 1, z    ] == 1) totalNeighbors++;
                if (world[x + 1, z    ] == 1) totalNeighbors++;
                if (world[x - 1, z + 1] == 1) totalNeighbors++;
                if (world[x    , z + 1] == 1) totalNeighbors++;
                if (world[x + 1, z + 1] == 1) totalNeighbors++;

                if (world[x,z] == 1)
                {
                    if (totalNeighbors < 2 || totalNeighbors >= 4) next[x, z] = 0;
                    else next[x, z] = 1;
                }
                else
                {
                    if (totalNeighbors == 3) next[x, z] = 1; 
                    else next[x, z] = 0;
                }
            }
        }
        yield return new WaitForSeconds(0.05f);
        world = next;
        isRunning = false;
    }
}
