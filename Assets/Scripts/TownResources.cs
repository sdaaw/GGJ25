using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TownResources : MonoBehaviour
{
    public List<GameObject> townBuildings = new List<GameObject>();

    public TMP_Text townInfoText;

    private float _startBuildingAmount;

    private void Start()
    {
        var entities = gameObject.GetComponentsInChildren<Entity>();
        foreach (var item in entities)
        {
            townBuildings.Add(item.gameObject);
        }
        
        _startBuildingAmount = townBuildings.Count;
    }

    private void Update()
    {
        townInfoText.text = "City integrity: " + townBuildings.Count + " / " + _startBuildingAmount;
    }

}
