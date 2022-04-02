using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArkLevel : MonoBehaviour
{
    [SerializeField] private Transform blockContainer;
    [SerializeField] private GameObject blockGO;
    [SerializeField] private GameObject blockSolidGO;
    [SerializeField] private int columnsCount;
    [SerializeField] private int rowsCount;
    [SerializeField] private float rowDist;
    [SerializeField] private float blockChance;

    [SerializeField] private int solidPerRowMin;
    [SerializeField] private int solidPerRowMax;
    
    private void Start()
    {
        blockContainer.transform.localPosition = new Vector3(-columnsCount / 2F, -rowsCount *rowDist, 0);
        
        for (int row = 0; row < rowsCount; row++)
        {
            var solidCount = 0;
            var solidExpected = Random.Range(solidPerRowMin, solidPerRowMax);
            
            for (int col = 0; col < columnsCount; col++)
            {
                float blockX = col;
                float blockY = row * rowDist;

                var blockToMakeGO = blockGO;
                
                if (Random.value <= 0.5f && solidCount < solidExpected)
                {
                    solidCount += 1;
                    blockToMakeGO = blockSolidGO;
                }
                var block = Instantiate(blockToMakeGO, blockContainer, true);
                block.transform.localPosition = new Vector3(blockX, blockY, 0f);
                
                if (Random.value >= blockChance)
                {
                    Destroy(block);
                }
            }
        }
    }
}
