using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public String userName;
    public String gameDate;

    public float support;
    public float efficiency;
    public float order;
    public float satisfaction;
    
    public int population;
    public int energy;
    public int wood;
    public int stone;
    public int iron;

    void Start()
    {
        userName = "KONKUK";
        gameDate = "2022-1";

        population = 10;
        energy = 20;
        wood = 50;
        stone = 50;
        iron = 50;
    }
}
