using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
    [SerializeField] string itemType;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Managers.inventory.AddItem(itemType);
            if (Managers.inventory._items.ContainsKey(itemType) || Managers.inventory._items.Count < Managers.inventory.inventorySize)
            {
                Destroy(gameObject);
            }
        }
    }
}
