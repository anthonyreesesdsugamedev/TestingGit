using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTerrain : MonoBehaviour {

    public GameObject bType = null;

    private float scalar = 0;

    //array of block terrain
    private GameObject[] blocks;

    private int numOfBlocks = 100;


    //index right and left for leftmost and rightmost blocks
    private int iL = 0;
    private int iR = 0;


    //used to determine position of user, so we can swap terrain accordingly
    public GameObject theUser = null;
    private float origX = 0;

    //used to spawn coins
    public GameObject coin = null;

    private int lCount;
    private int rCount;

    //used to spawn spikes

    public GameObject spike = null;  

    private int spikeRcount;
    private int spikeLcount;


    void Start () {

        //setup coin counter
        lCount = 10;
        rCount = 10;

        //setup spike counter
        spikeLcount = 23;
        spikeRcount = 23;

        //setup block indices

        iR = numOfBlocks - 1;
        iL = 0;

        //store original pos of the ship

        origX = theUser.transform.position.x;


       //populate array
        blocks = new GameObject[numOfBlocks];

        //positioning used for placing blocks directly after one another
        scalar = bType.transform.lossyScale.x * 0.31f;

        for (int i = 0; i < numOfBlocks; i++){

            GameObject bx = GameObject.Instantiate(bType);

            float xPosition = -30f + i * scalar;

            float yPosition = -22f + Mathf.PerlinNoise(xPosition / 122f, 0f) * 14f;

            yPosition += Mathf.PerlinNoise(xPosition / 79f, 0f) * 10f;

            yPosition += Mathf.PerlinNoise(xPosition / 32f, 0f) * 6f;

            yPosition += Mathf.PerlinNoise(xPosition / 22f, 0f) * 4f;

            yPosition += Mathf.PerlinNoise(xPosition / 4f, 0f) * 1f;


            bx.transform.position = new Vector3(xPosition, yPosition);

            bx.transform.SetParent(this.transform);

            blocks[i] = bx;
        }
		
	}

	// Update is called once per frame
	void Update () {


        // if the ship has moved enough, swap in the correct direction
        // if its moved 10 blocks in a direction, spawn a coin in that direction


        if (theUser.transform.position.x - origX > scalar)
        {
            swapRight();
            origX = theUser.transform.position.x;

            rCount--;
            spikeRcount--;
            if(rCount == 0)
            {
                rCount = 10;
                spawnCoin(1);
            }
            if(spikeRcount == 0)
            {
                spikeRcount = 23;
                spawnSpike(1);
            }
        }


        if (origX - theUser.transform.position.x > scalar)
        {
            swapLeft();
            origX = theUser.transform.position.x;

            lCount--;
            spikeLcount--;
            if (lCount == 0)
            {
                lCount = 10;
                spawnCoin(0);
            }
            if(spikeLcount == 0)
            {
                spikeLcount = 23;
                spawnSpike(0);
            }
        }

    }

    void swapRight()
    {
        //taking left most block and putting it on the right side as player moves right
        blocks[iL].transform.position = blocks[iR].transform.position;
        blocks[iL].transform.Translate(Vector3.right * scalar);
        //swap indices
        iR = iL;
        iL++;

        //if we hit end of array, we start at beginning
        if (iL == numOfBlocks)
            iL = 0;

        //calculate new height of block with perlin noise
        float myX = blocks[iR].transform.position.x;
        float myY = applyPerlin(myX);
        blocks[iR].transform.position = new Vector2(myX, myY);
    }

    void swapLeft()
    {
        //taking right most block and putting it on the left side as player moves left
        blocks[iR].transform.position = blocks[iL].transform.position;
        blocks[iR].transform.Translate(Vector3.left * scalar);
        //swap indices
        iL = iR;
        iR--;

        //if we hit beginning, we reset 
        if (iR == -1)
            iR = numOfBlocks -1;

        //calculate new height of block with perlin noise

        float myX = blocks[iL].transform.position.x;
        float myY = applyPerlin(myX);
        blocks[iL].transform.position = new Vector2(myX, myY);
    }

    //takes in x position and apply perlin noise to return y position
    float applyPerlin(float _x)
    {
        float yPosition = -22f + Mathf.PerlinNoise(_x / 122f, 0f) * 14f;

        yPosition += Mathf.PerlinNoise(_x / 79f, 0f) * 10f;

        yPosition += Mathf.PerlinNoise(_x / 32f, 0f) * 6f;

        yPosition += Mathf.PerlinNoise(_x / 22f, 0f) * 4f;

        yPosition += Mathf.PerlinNoise(_x / 4f, 0f) * 1f;

        return yPosition;
    }
    void spawnCoin(int choice)
    {
        if (choice == 1)
        {
            float myX = theUser.transform.position.x;
            float myY = theUser.transform.position.y;
            Instantiate(coin, new Vector2(myX + scalar * 8, myY + scalar), Quaternion.identity);

        }
        else
        {
            float myX = theUser.transform.position.x;
            float myY = theUser.transform.position.y;
            Instantiate(coin, new Vector2(myX - scalar * 8, myY + scalar), Quaternion.identity);

        }
    }


    void spawnSpike(int direction)
    {
        if (direction == 1)
        {
            float myX = theUser.transform.position.x;
            float myY = theUser.transform.position.y;
            Instantiate(spike, new Vector2(myX + scalar * 8, myY + scalar), Quaternion.identity);

        }
        else
        {
            float myX = theUser.transform.position.x;
            float myY = theUser.transform.position.y;
            Instantiate(spike, new Vector2(myX - scalar * 8, myY + scalar), Quaternion.identity);

        }
    }

    void OnGUI()
    {
        bool quit = GUI.Button(new Rect(30, 40, 200, 20), "Quit");

        if (quit)
            Application.Quit();
    }
}
