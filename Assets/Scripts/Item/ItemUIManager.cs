using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemUIManager : Singleton<ItemUIManager>
{
    public float Duration = 3.0f;
    public GameObject ItemUI;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;

    public void OnItemUI(string itemName, string itemDescription)
    {
        ItemName.text = itemName;
        ItemDescription.text = itemDescription;
        StartCoroutine(SetActiveItemUI());
    }
    private IEnumerator SetActiveItemUI()
    {
        ItemUI.SetActive(true);
        yield return new WaitForSeconds(Duration);
        ItemUI.SetActive(false);
    }
}
