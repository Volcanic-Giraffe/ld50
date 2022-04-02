using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var solidExpected = Math.Min(columnsCount, Random.Range(solidPerRowMin, solidPerRowMax));

            var blocks = new List<string>();

            for (int i = 0; i < solidExpected; i++)
            {
                blocks.Add("solid");
            }
            
            for (int col = solidExpected; col < columnsCount; col++)
            {
                
                if (Random.value <= blockChance)
                {
                    blocks.Add("regular");
                }
                else
                {
                    blocks.Add("empty");
                }
            }

            blocks = blocks.Shuffle().ToList();
            
            for (int col = 0; col < columnsCount; col++)
            {
                float blockX = col;
                float blockY = row * rowDist;

                var blockType = blocks[col];

                
                if (blocks[col] != "empty")
                {
                    var blockToMakeGO = blockGO;
                
                    if (blocks[col] == "solid")
                    {
                        blockToMakeGO = blockSolidGO;
                    }
                    
                    var block = Instantiate(blockToMakeGO, blockContainer, true);
                    block.transform.localPosition = new Vector3(blockX, blockY, 0f);
                }
            }
        }
    }
}
