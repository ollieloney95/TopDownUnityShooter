using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(GunManager))]
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(ShopManager))]
[RequireComponent(typeof(StatsManager))]
[RequireComponent(typeof(InputManager))]

public class Managers : MonoBehaviour {

    public static PlayerManager player { get; private set; }
    public static InventoryManager inventory { get; private set; }
    public static GunManager gun { get; private set; }
    public static GameManager game { get; private set; }
    public static ShopManager shop { get; private set; }
    public static StatsManager stats { get; private set; }
    public static InputManager input { get; private set; }
    public static MissionManager missions { get; private set; }

    private List<IGameManager> _startsequence;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
        inventory = GetComponent<InventoryManager>();
        gun = GetComponent<GunManager>();
        game = GetComponent<GameManager>();
        shop = GetComponent<ShopManager>();
        stats = GetComponent<StatsManager>();
        input = GetComponent<InputManager>();
        missions = GetComponent<MissionManager>();

        _startsequence = new List<IGameManager>();
        _startsequence.Add(player);
        _startsequence.Add(inventory);
        _startsequence.Add(gun);
        _startsequence.Add(game);
        _startsequence.Add(shop);
        _startsequence.Add(stats);
        _startsequence.Add(input);
        _startsequence.Add(missions);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach(IGameManager manager in _startsequence)
        {
            manager.Startup();
        }
        yield return null;

        int numModules = _startsequence.Count;
        int numready = 0;

        while (numready < numModules)
        {
            int lastReady = numready;
            numready = 0;

            foreach(IGameManager manager in _startsequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numready++;
                }
            }

            if(numready > lastReady)
            {
                Debug.Log("Progress: " + numready + "/" + numModules);
                yield return null;
            }
            Debug.Log("All managers started up");
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
