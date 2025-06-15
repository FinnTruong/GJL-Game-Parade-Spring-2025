using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum IslandType
{
    Spring,
    Summer,
    Autumn,
    Winter
}
public struct IslandData
{
    public IslandType islandType;
}

public class Island : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
