using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    
    public GameObject gamePanel;
    public TextMeshProUGUI userNameTxt1;
    public TextMeshProUGUI userNameTxt2;
    public TextMeshProUGUI gameDateTxt;
    
    public TextMeshProUGUI supportTxt1;
    public TextMeshProUGUI supportTxt2;
    public TextMeshProUGUI efficiencyTxt;
    public TextMeshProUGUI orderTxt;
    public TextMeshProUGUI satisfactionTxt;

    
    public TextMeshProUGUI populationTxt;
    public TextMeshProUGUI energyTxt;
    public TextMeshProUGUI woodTxt;
    public TextMeshProUGUI stoneTxt;
    public TextMeshProUGUI ironTxt;

    private void LateUpdate()
    {
        userNameTxt1.text = player.userName;
        userNameTxt2.text = player.userName;
        gameDateTxt.text = player.gameDate;

        supportTxt1.text = player.support.ToString("F1");
        supportTxt2.text = player.support.ToString("F1");
        efficiencyTxt.text = player.efficiency.ToString("F1");
        orderTxt.text = player.order.ToString("F1");
        satisfactionTxt.text = player.satisfaction.ToString("F1");

        populationTxt.text = player.population.ToString();
        energyTxt.text = player.energy.ToString();
        woodTxt.text = player.wood.ToString();
        stoneTxt.text = player.stone.ToString();
        ironTxt.text = player.iron.ToString();
    }
}
