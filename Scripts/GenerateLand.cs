using UnityEngine;
using System.Collections;

public class Block
{
    public int type;
    public bool vis;
    public GameObject block;

    public Block(int t, bool v, GameObject b)
    {
        type = t;
        vis = v;
        block = b;
    }
}

public class GenerateLand : MonoBehaviour
{

    public static int width = 128;
    public static int depth = 128;
    public static int height = 128;
    public int heightScale = 28;
    public int heightOffset = 100;
    public float detailScale = 25.0f;

    public GameObject grassBlock;
    public GameObject sandBlock;
    public GameObject snowBlock;
    public GameObject cloudBlock;
    public GameObject diamondBlock;

    Block[,,] worldBlocks = new Block[width, height, depth];

    // Use this for initialization
    void Start()
    {
        int seed = (int)Network.time * 10;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int y = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (z + seed) / detailScale) * heightScale)
                               + heightOffset;
                Vector3 blockPos = new Vector3(x, y, z);

                CreateBlock(y, blockPos, true);
                while (y > 0)
                {
                    y--;
                    blockPos = new Vector3(x, y, z);
                    CreateBlock(y, blockPos, false);
                }

            }
        }

        DrawClouds(20, 100);
        DigMines(5, 500);
    }

    void DrawClouds(int numClouds, int cSize)
    {
        for (int i = 0; i < numClouds; i++)
        {
            int xpos = Random.Range(0, width);
            int zpos = Random.Range(0, depth);
            for (int j = 0; j < cSize; j++)
            {
                Vector3 blockPos = new Vector3(xpos, height - 1, zpos);
                GameObject newBlock = (GameObject)Instantiate(cloudBlock, blockPos, Quaternion.identity);
                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(4, true, newBlock);
                xpos += Random.Range(-1, 2);
                zpos += Random.Range(-1, 2);
                if (xpos < 0 || xpos >= width) xpos = width / 2;
                if (zpos < 0 || zpos >= depth) zpos = depth / 2;
            }
        }
    }


    void DigMines(int numMines, int mSize)
    {
        int holeSize = 2;
        for (int i = 0; i < numMines; i++)
        {
            int xpos = Random.Range(10, width - 10);
            int ypos = Random.Range(10, height - 10);
            int zpos = Random.Range(10, depth - 10);
            for (int j = 0; j < mSize; j++)
            {
                for (int x = -holeSize; x <= holeSize; x++)
                    for (int y = -holeSize; y <= holeSize; y++)
                        for (int z = -holeSize; z <= holeSize; z++)
                        {
                            if (!(x == 0 && y == 0 && z == 0))
                            {
                                Vector3 blockPos = new Vector3(xpos + x, ypos + y, zpos + z);
                                if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] != null)
                                    if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].block != null)
                                        Destroy(worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].block);
                                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
                            }
                        }


                xpos += Random.Range(-1, 2);
                zpos += Random.Range(-1, 2);
                ypos += Random.Range(-1, 2);
                if (xpos < holeSize || xpos >= width - holeSize) xpos = width / 2;
                if (ypos < holeSize || ypos >= height - holeSize) ypos = height / 2;
                if (zpos < holeSize || zpos >= depth - holeSize) zpos = depth / 2;
            }
        }

        for (int z = 1; z < depth - 1; z++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {

                    if (worldBlocks[x, y, z] == null)
                    {

                        for (int x1 = -1; x1 <= 1; x1++)
                            for (int y1 = -1; y1 <= 1; y1++)
                                for (int z1 = -1; z1 <= 1; z1++)
                                {
                                    if (!(x1 == 0 && y1 == 0 && z1 == 0))
                                    {
                                        Vector3 neighbour = new Vector3(x + x1, y + y1, z + z1);
                                        DrawBlock(neighbour);
                                    }
                                }
                    }
                }

            }
        }
    }

    void CreateBlock(int y, Vector3 blockPos, bool create)
    {
        if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.x >= depth) return;
        GameObject newBlock = null;
        if (y > 105)
        {
            if (create)
                newBlock = (GameObject)Instantiate(snowBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(1, create, newBlock);
        }
        else if (y > 85)
        {
            if (create)
                newBlock = (GameObject)Instantiate(grassBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(2, create, newBlock);
        }
        else
        {
            if (create)
                newBlock = (GameObject)Instantiate(sandBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(3, create, newBlock);
        }

        if (y > 11 && y < 50 && Random.Range(0, 100) < 10)
        {
            if (create)
                newBlock = (GameObject)Instantiate(diamondBlock, blockPos, Quaternion.identity);

            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block(5, create, newBlock);
        }


    }

    void DrawBlock(Vector3 blockPos)
    {
        //if it is outside the map
        if (blockPos.x < 0 || blockPos.x > width - 1 ||
            blockPos.y < 0 || blockPos.y > height - 1 ||
            blockPos.z < 0 || blockPos.z > depth - 1) return;

        if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null) return;

        if (!worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis)
        {
            GameObject newBlock = null;
            worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = true;
            if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 1)
                newBlock = (GameObject)Instantiate(snowBlock, blockPos, Quaternion.identity);

            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 2)
                newBlock = (GameObject)Instantiate(grassBlock, blockPos, Quaternion.identity);

            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 3)
                newBlock = (GameObject)Instantiate(sandBlock, blockPos, Quaternion.identity);

            else if (worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type == 5)
                newBlock = (GameObject)Instantiate(diamondBlock, blockPos, Quaternion.identity);

            else
                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = false;

            if (newBlock != null)
                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].block = newBlock;
        }

    }

    int NeighbourCount(Vector3 blockPos)
    {
        int nCount = 0;
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                    {
                        if (worldBlocks[(int)blockPos.x + x, (int)blockPos.y + y, (int)blockPos.z + z] != null)
                            nCount++;
                    }
                }
        return (nCount);
    }

    void CheckObscuredNeighbours(Vector3 blockPos)
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                    {
                        Vector3 neighbour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);

                        //if it is outside the map
                        if (neighbour.x < 1 || neighbour.x > width - 2 ||
                            neighbour.y < 1 || neighbour.y > height - 2 ||
                            neighbour.z < 1 || neighbour.z > depth - 2) continue;


                        if (worldBlocks[(int)neighbour.x, (int)neighbour.y, (int)neighbour.z] != null)
                        {
                            if (NeighbourCount(neighbour) == 26)
                            {
                                Destroy(worldBlocks[(int)neighbour.x, (int)neighbour.y, (int)neighbour.z].block);
                                worldBlocks[(int)neighbour.x, (int)neighbour.y, (int)neighbour.z] = null;
                            }
                        }
                    }
                }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 blockPos = hit.transform.position;

                //this is the bottom block.  Don't delete it.
                if ((int)blockPos.y == 0) return;

                worldBlocks[(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
                Destroy(hit.transform.gameObject);

                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        for (int z = -1; z <= 1; z++)
                        {
                            if (!(x == 0 && y == 0 && z == 0))
                            {
                                Vector3 neighbour = new Vector3(blockPos.x + x, blockPos.y + y, blockPos.z + z);
                                DrawBlock(neighbour);
                            }
                        }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0));
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Vector3 blockPos = hit.transform.position;
                Vector3 hitVector = blockPos - hit.point;

                hitVector.x = Mathf.Abs(hitVector.x);
                hitVector.y = Mathf.Abs(hitVector.y);
                hitVector.z = Mathf.Abs(hitVector.z);

                if (hitVector.x > hitVector.z && hitVector.x > hitVector.y)
                    blockPos.x -= (int)Mathf.RoundToInt(ray.direction.x);
                else if (hitVector.y > hitVector.x && hitVector.y > hitVector.z)
                    blockPos.y -= (int)Mathf.RoundToInt(ray.direction.y);
                else
                    blockPos.z -= (int)Mathf.RoundToInt(ray.direction.z);

                CreateBlock((int)blockPos.y, blockPos, true);
                CheckObscuredNeighbours(blockPos);

            }
        }


    }
}