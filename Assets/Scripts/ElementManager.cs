using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour
{
    [SerializeField] private GameObject elementPrefab;
    [SerializeField] private ElementData[] elementDatas;
    [SerializeField] private Transform[] elementPositions;
    private GameController gameController;
    private void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        
        CreateElement();
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
            GameObject obj = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity, elementPositions[d.id]);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "Element_" + d.id;
            
            Element element = obj.GetComponent<Element>();
            element.Initialize(d);
            element.RefreshText();

            Button button = element.GetComponent<Button>();
            button.onClick.AddListener(() => gameController.OnBuy(d.id));
            
            elementPositions[d.id].gameObject.SetActive(d.id == 0);
            gameController.AddElement(element);
        }
    }
}
