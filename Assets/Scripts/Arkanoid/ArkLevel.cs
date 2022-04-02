using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArkLevel : MonoBehaviour
{
    [SerializeField] private Transform blockContainer;
    [SerializeField] private GameObject blockGO;
    [SerializeField] private int columnsCount;
    [SerializeField] private int rowsCount;
    [SerializeField] private float rowDist;
    [SerializeField] private float blockChance;

    
    private void Start()
    {
        blockContainer.transform.localPosition = new Vector3(-columnsCount / 2F, -rowsCount *rowDist, 0);
        
        for (int row = 0; row < rowsCount; row++)
        {
            for (int col = 0; col < columnsCount; col++)
            {
                float blockX = col;
                float blockY = row * rowDist;

                if (Random.value <= blockChance)
                {
                    var block = Instantiate(blockGO, blockContainer, true);
                    block.transform.localPosition = new Vector3(blockX, blockY, 0f);
                }
            }
        }
    }
}
