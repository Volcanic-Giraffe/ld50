using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseArcanoidLevelController : MonoBehaviour
{
    [SerializeField] GameObject Hero;
    [SerializeField] Camera MainCamera;
    [SerializeField] GameObject InputPane;
    [SerializeField] GameObject BlockPrefab;
    [SerializeField] int blockLinesDistance;

    private int lastLine = 0;
    private int blockSizeX;
    private int blockSizeY;

    void Start()
    {
        if(!MainCamera) MainCamera = Camera.main;
        blockSizeX = (int)(BlockPrefab.GetComponent<BoxCollider>().size.x * BlockPrefab.transform.localScale.x);
        blockSizeY = (int)(BlockPrefab.GetComponent<BoxCollider>().size.y * BlockPrefab.transform.localScale.y);
    }
    
    void Update()
    {
        // TODO Cinemachine, deadzone, etc
        var heroPos = Hero.transform.position;
        MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, heroPos.y, MainCamera.transform.position.z);
        InputPane.transform.position = new Vector3(InputPane.transform.position.x, heroPos.y, InputPane.transform.position.z);

        var currLine = Math.Abs(Math.Round((heroPos.y-20) / (blockLinesDistance * blockSizeY)));
        if (currLine > lastLine) SpawnNewLine((int)currLine);
        

    }

    private void SpawnNewLine(int currLine)
    {
        lastLine = currLine;
        
        for (int i = -5; i < 5; i+= 1)
        {
            if (UnityEngine.Random.value > 0.7f) continue;
            var block = Instantiate(BlockPrefab);
            block.transform.position = new Vector3(i * blockSizeX, -currLine * blockLinesDistance*blockSizeY, 0);
        }
    }
}
