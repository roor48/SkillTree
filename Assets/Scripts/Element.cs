
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate BigNumber ExecuteStrategy(BigNumber cps);

public class Element : MonoBehaviour
{
    public int ID { get; private set; }
    
    public BigNumber[] costs;
    private int maxLevel;
    private int level;

    private string strategyString;
    private ExecuteStrategy executeStrategy;
    
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI formulaText;
    private TextMeshProUGUI costText;

    private List<Element> childElements;

    public void Initialize(ElementData data)
    {
        this.ID = data.id;
        this.costs = data.costs;
        this.level = 0;
        this.maxLevel = data.costs.Length;
        
        this.strategyString = FormulaParser.GetFormulaString(data.id);
        this.executeStrategy = FormulaParser.GetFormula(data.id);
        
        this.levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        this.formulaText = transform.Find("FormulaText").GetComponent<TextMeshProUGUI>();
        this.costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
        
        childElements = new List<Element>();
    }

    public void AddChild(Element element)
    {
        childElements.Add(element);
    }

    public void EnableChildren()
    {
        foreach (Element element in childElements)
        {
            element.transform.parent.gameObject.SetActive(true);
        }
    }

    public bool IsMaxLevel()
    {
        return level >= maxLevel;
    }
    
    public BigNumber GetUpgradeCost()
    {
        if (IsMaxLevel())
            return null; // Max Level
        
        return costs[level];
    }
    
    public void Upgrade()
    {
        if (level >= maxLevel)
            return;
        
        level += 1;
        
        RefreshText();
    }

    public void RefreshText()
    {
        levelText.text = $"{level} / {maxLevel}";
        formulaText.text = strategyString;
        costText.text = level < maxLevel ? $"{costs[level]}$" : "Max Level";       
    }

    public BigNumber Execute(BigNumber cps) // cps: cash per second
    {
        if (level == 0)
            return cps;

        BigNumber value = cps;
        for (int i = 0; i < level; i++)
        {
            value = executeStrategy(value);
        }
        return value;
    }
}

[CreateAssetMenu(fileName = "ElementData", menuName = "Game/Element Data")]
[System.Serializable]
public class ElementData : ScriptableObject
{
    public int id;
    public BigNumber[] costs;
    public int[] childrenIds;

    private void OnValidate()
    {
        foreach (var cost in costs)
        {
            cost.Normalize();
        }
    }
}

/* todo:
    1. 숫자 계산 클래스 생성
        Sexvigintillion범위 안에 들어오면 단위로 표현, 아니면 e표현
*/
