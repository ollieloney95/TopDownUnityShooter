using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class Device : MonoBehaviour {


    public UnityEvent m_MyEvent;
    // Use this for initialization
    [SerializeField] int price;
    [SerializeField] private GameObject purchaseButtonPopup;
    [SerializeField] private GameObject purchaseButtonPopupText;
    [SerializeField] private Sprite canBuySprite;
    [SerializeField] private string canBuyText;
    [SerializeField] private GameObject tooExpensivePopup;
    [SerializeField] private GameObject tooExpensivePopupText;
    [SerializeField] private Sprite tooExpensiveSprite;
    [SerializeField] private Sprite maxedOutSprite;
    [SerializeField] private string tooExpensiveText;
    [SerializeField] private int shop_id;     // note 1 for perks, 2 for gun,   3 for door
    public bool buying;
    
    void Awake()
    {
        Messenger.AddListener(GameEvent.ONBUY, onBuy);
        purchaseButtonPopup = GameObject.Find("PurchaseButton");
        purchaseButtonPopupText = GameObject.Find("PurchaseButtonText");
        tooExpensivePopup = GameObject.Find("TooExpensivePopup");
        tooExpensivePopupText = GameObject.Find("TooExpensivePopupText");
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ONBUY, onBuy);
    }
    void onBuy()
    {
        StartCoroutine(buyLag());
    }
    IEnumerator buyLag()
    {
        buying = true;
        yield return new WaitForSeconds(0.5f);
        buying = false;
        Managers.gun.refreshStats();
    }
   
    void Start () {
        purchaseButtonPopup.SetActive(false);
        tooExpensivePopup.SetActive(false);
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (shop_id == 1 && price > 1600)
            {
                //maxed out 
                purchaseButtonPopup.SetActive(true);
                purchaseButtonPopup.transform.GetChild(0).gameObject.GetComponent<Image>().overrideSprite = maxedOutSprite;
                purchaseButtonPopupText.GetComponent<Text>().text = "maxed out";
            }
            else //still levels to buy
            {
                if (Managers.game.currentMoney < price)
                {
                    tooExpensivePopup.SetActive(true);
                    tooExpensivePopup.transform.GetChild(0).gameObject.GetComponent<Image>().overrideSprite = tooExpensiveSprite;
                    tooExpensivePopupText.GetComponent<Text>().text = tooExpensiveText;
                }
                else
                {
                    purchaseButtonPopup.SetActive(true);
                    purchaseButtonPopup.transform.GetChild(0).gameObject.GetComponent<Image>().overrideSprite = canBuySprite;
                    purchaseButtonPopupText.GetComponent<Text>().text = canBuyText;
                }
                if (Managers.game.currentMoney >= price && buying == true)
                {
                    m_MyEvent.Invoke();
                    Managers.game.changeMoney(-1 * price);
                    price = price * 2;
                    buying = false;
                }
            }
        }
    }
    void OnTriggerExit()
    {
        purchaseButtonPopup.SetActive(false);
        tooExpensivePopup.SetActive(false);
    }
}
