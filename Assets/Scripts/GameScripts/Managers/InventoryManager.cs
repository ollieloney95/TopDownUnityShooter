using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    public Dictionary<string, int> _items;
    public int ammo { get; private set; }
    public int inventorySize = 12;
    [SerializeField] public Sprite[] inventoryItemListSprites;
    [SerializeField] public string[] inventoryItemListNames;
    [SerializeField] public GameObject[] inventorySlots;

    // Use this for initialization
    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        _items = new Dictionary<string, int>();
        status = ManagerStatus.Started; 
    }

    // Update is called once per frame
    void Update()
    {
        refreshInventoryUI();
    }
    public void onSLot(int slotNum)
    {
        int index = 1;
        foreach (KeyValuePair<string, int> item in _items)
        {
            if (index == slotNum)
            {
                _items[item.Key] -= 1;
                if(_items[item.Key] <= 0) { _items.Remove(item.Key); }
                Messenger.Broadcast(item.Key);
                return;
            }
            index++;
        }
    }
    void refreshInventoryUI()
    {
        int i = 0;
            //inventorySlots[i].GetComponent<Image>().overrideSprite = inventoryItemListSprites[inventoryItemListNames[_items[i]]];
        
        foreach (KeyValuePair<string, int> item in _items)
        {
            inventorySlots[i].GetComponent<Image>().overrideSprite = inventoryItemListSprites[System.Array.IndexOf(inventoryItemListNames, item.Key)];
            inventorySlots[i].GetComponentInChildren<Text>().text = ""+item.Value;
            i++;
        }
        for (i=_items.Count;i<inventorySize;i++)
        {
            inventorySlots[i].GetComponent<Image>().overrideSprite = inventoryItemListSprites[2];
            inventorySlots[i].GetComponentInChildren<Text>().text = "";
        }
    }

    public void AddItem(string itemToAdd)
    {
        if (_items.ContainsKey(itemToAdd))
        {
            _items[itemToAdd] += 1;
            Debug.Log("added 1 more of:" + itemToAdd);
        }
        else
        {
            if (_items.Count<inventorySize) {
                _items[itemToAdd] = 1;
                StartCoroutine(addedItem(itemToAdd));
                Debug.Log("added:" + itemToAdd); 
            }
            else
            {
                StartCoroutine(inventoryFull(itemToAdd));
                Debug.Log("inventory too full for:" + itemToAdd);
            }
        }
    }
    IEnumerator inventoryFull(string item)
    {
        //display image, make text equal item
        yield return new WaitForEndOfFrame();
    }
    IEnumerator addedItem(string item)
    {
        //display image, make text equal item
        yield return new WaitForEndOfFrame();
    }

}
