using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;

    void Start()
    {
        SpwanBrick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpwanBrick()
    {
        float y = transform.position.y;
        float z = transform.position.z;
        for (int i = -3; i < 4;  i++)
        {
            for (int j = -1; j < 4; j++)
            {
                GameObject obj = Instantiate(brickPrefab);
                obj.transform.position = new Vector3(i * 2, 0.65f + y, j * 2 + z);

            }
        }
    }
}
