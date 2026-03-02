using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    private List<Element> elements = new();

    private long cash;

    private bool STOP = false;
    
    private void Start()
    {
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

            long cash = 0; // cash per second
            
            foreach (var element in elements)
            {
                cash = element.Execute(cash);
            }
            Debug.Log("cash total: " + cash);
            this.cash += cash;
            RefreshUI();
        }
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
        if (elements[id].level >= elements[id].maxLevel) 
            return;
        int cost = elements[id].costs[elements[id].level];
        if (cash < cost)
            return;

        elements[id].Upgrade();
        cash -= cost;
        
        // ui 변경
        RefreshUI();
        // add upgrade effect
    }
    
    private void RefreshUI()
    {
        cashText.text = $"Currency: {cash}$";
    }
}
