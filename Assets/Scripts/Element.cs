
using TMPro;
using UnityEngine;

public delegate long ExecuteStrategy(long cps, int level);

public class Element : MonoBehaviour
{
    public int ID { get; private set; }
    
    public int[] costs;
    public int level { get; private set; }
    public int maxLevel;
    
    public string StrategyString { get; private set; }
    private ExecuteStrategy executeStrategy;
    
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI formulaText;
    private TextMeshProUGUI costText;

    public void Initialize(ElementData data)
    {
        this.ID = data.id;
        this.costs = data.costs;
        this.level = 0;
        this.maxLevel = data.costs.Length;
        
        this.StrategyString = FormulaParser.GetFormulaString(data.id);
        this.executeStrategy = FormulaParser.GetFormula(data.id);
        
        this.levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        this.formulaText = transform.Find("FormulaText").GetComponent<TextMeshProUGUI>();
        this.costText = transform.Find("CostText").GetComponent<TextMeshProUGUI>();
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
        formulaText.text = StrategyString;
        costText.text = level < maxLevel ? $"{costs[level]}$" : "Max Level";       
    }

    public long Execute(long cps) // cps: cash per second
    {
        if (level == 0)
            return cps;
        
        long value = executeStrategy(cps, level);
        Debug.Log(gameObject.name + ":  " + "cps: " + cps + ", value: " + value);
        return value;
    }
}

[CreateAssetMenu(fileName = "ElementData", menuName = "Game/Element Data")]
[System.Serializable]
public class ElementData : ScriptableObject
{
    public int id;
    public int[] costs;
}

/* todo:
    1. next Element 추가해서 다음 element SetActive(true) 할 수 있도록 하기
*/
