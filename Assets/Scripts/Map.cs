using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int width = 20;
    public int height = 100;
    public GameObject hexPrefab;
    public static Map Instance;

    public IceHex[,] map;

    float xOffset = 0.882f;
    float yOffset = 0.764f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        map = new IceHex[width, height];
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {

                var xPos = x * xOffset;

                if (y % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }

                var hexGO = Instantiate(hexPrefab, new Vector3(xPos, y * yOffset, 0),
                    Quaternion.identity);

                hexGO.name = "Hex_" + x + "_" + y;

                var iceHex = hexGO.GetComponent<IceHex>();

                iceHex.pressureThreshold = Mathf.Lerp(0.12f, 0.04f, (float) y / width);
                iceHex.x = x;
                iceHex.y = y;

                hexGO.transform.SetParent(this.transform);

                hexGO.isStatic = true;

                map[x, y] = iceHex;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
