using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class door : MonoBehaviour{

    public bool opened = false;
    // Use this for initialization
    [SerializeField]
    int price;
    [SerializeField]
    private GameObject purchaseButtonPopup;
    [SerializeField]
    private GameObject purchaseButtonPopupText;
    [SerializeField]
    private Sprite canBuySprite;
    [SerializeField]
    private string canBuyText;
    [SerializeField]
    private GameObject tooExpensivePopup;
    [SerializeField]
    private GameObject tooExpensivePopupText;
    [SerializeField]
    private Sprite tooExpensiveSprite;
    [SerializeField]
    private string tooExpensiveText;
    public bool buying;
    public Transform d_left;
    public Transform d_right;

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
    }
    void Start()
    {
        purchaseButtonPopup.SetActive(false);
        tooExpensivePopup.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
                //hide gameobject,  make opened == true, cost some dollar
                foreach (BoxCollider c in GetComponents<BoxCollider>())
                {
                    c.enabled = false;
                }
                //gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
                Managers.game.changeMoney(-1 * price);
                opened = true;
                Managers.game.refeshSpawns();
                buying = false;
                purchaseButtonPopup.SetActive(false);
                tooExpensivePopup.SetActive(false);
                StartCoroutine(Rotate(1f, d_left,false));
                StartCoroutine(Rotate(1f, d_right, true));
                
            }
        }
    }
    void OnTriggerExit()
    {
        purchaseButtonPopup.SetActive(false);
        tooExpensivePopup.SetActive(false);
    }
    IEnumerator Rotate(float duration,Transform object_to_rotate,bool clockwise)
    {
        float startRotation = object_to_rotate.localEulerAngles.y;
        float endRotation = startRotation + 90.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            if (clockwise == false)
            {
                yRotation = -yRotation;
            }
            object_to_rotate.localEulerAngles = new Vector3(object_to_rotate.localEulerAngles.x, yRotation, object_to_rotate.localEulerAngles.z);
            yield return null;
        }
    }
}


