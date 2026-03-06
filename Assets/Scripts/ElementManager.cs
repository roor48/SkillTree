using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour
{
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private ElementData[] elementDatas;
    [SerializeField] private Transform[] elementPositions;
    
    private Dictionary<int, Element> elements;
    private GameController gameController;
    public void InitElementManager(GameController gameController)
    {
        this.gameController = gameController;
        elements = new Dictionary<int, Element>();
        
        foreach (var elementData in elementDatas)
        {
            foreach (var cost in elementData.costs)
            {
                cost.Normalize();
            }
        }
        
        CreateElement();
        MakeTree();
        AddToGameController();
    }

    private void CreateElement()
    {
        if (elementDatas == null || elementPositions == null ||
            elementDatas.Length != elementPositions.Length)
        {
            throw new Exception("ElementDatas and ElementPositions must be set and have the same length.");
        }

        foreach (var d in elementDatas)
        {
            GameObject obj = Instantiate(elementPrefab, elementPositions[d.id]);
            obj.name = "Element_" + d.id;
            
            Element element = obj.GetComponent<Element>();
            element.Initialize(d);
            element.RefreshText();
            if (!elements.TryAdd(d.id, element))
            {
                Debug.LogError("Element_" + d.id + " already exists.");
            }

            Button button = element.GetComponent<Button>();
            button.onClick.AddListener(() => gameController.OnBuy(d.id));
            
            elementPositions[d.id].gameObject.SetActive(d.id == 0);
        }
    }

    private void MakeTree()
    {
        foreach (var element in elements.Values)
        {
            ElementData data = elementDatas[element.ID];
            foreach (int childID in data.childrenIds)
            {
                if (elements.TryGetValue(childID, out Element childElement))
                {
                    element.AddChild(childElement);
                }
                else
                {
                    Debug.LogError("Child element with ID " + childID + " not found for element " + element.ID);
                }
            }
        }
    }

    private void AddToGameController()
    {
        foreach (var element in elements.Values)
        {
            gameController.AddElement(element);
        }
    }
}
