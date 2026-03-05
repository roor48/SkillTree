using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI cpsText;
    
    private List<Element> elements = new();

    private BigNumber cash;

    private bool STOP = false;
    
    public void StartCycle()
    {
        cash = new BigNumber(1000);
        STOP = false;
        RefreshUI();
        StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        yield return null;

        var wait = new WaitForSeconds(1f);
        while (!STOP)
        {
            yield return wait;

            BigNumber cash = CalculateCPS();
            Debug.Log("cash total: " + cash);
            this.cash += cash;
            RefreshUI();
        }
    }

    private BigNumber CalculateCPS()
    {
        BigNumber cps = new BigNumber(); // cash per second
            
        foreach (var element in elements)
        {
            cps = element.Execute(cps);
        }

        return cps;
    }
    
    public void AddElement(Element element)
    {
        int position = -1;
        for (int i = 0; i < elements.Count; i++)
        {
            if (element.ID < elements[i].ID)
            {
                position = i;
                break;
            }
        }

        if (position == -1)
        {
            elements.Add(element);
        }
        else
        {
            elements.Insert(position, element);
        }
    }

    public void OnBuy(int id)
    {
        if (elements[id].IsMaxLevel()) 
            return;
        BigNumber cost = elements[id].GetUpgradeCost();
        if (cost == null || cash < cost)
            return;

        elements[id].Upgrade();
        cash -= cost;
        
        // ui 변경
        RefreshUI();
        
        // add upgrade effect
        elements[id].EnableChildren();
    }
    
    private void RefreshUI()
    {
        cashText.text = $"Currency: {cash}$";
        cpsText.text = $"Cash/s: {CalculateCPS()}$";
    }
}
