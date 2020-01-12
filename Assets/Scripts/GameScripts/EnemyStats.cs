using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {

    public float maxHealth = 5f;
    public float health = 5f;
    public int damage = 1;
    public float attackRadius = 2;
    public float damageRadius = 3;
    public int moneyWorth = 30;
    private float moneyModifier = 1;
    private float scoreModifier = 1;
    public GameObject money_pop;
    private Animator _animator;
    Vector3 _scale;
    [SerializeField] GameObject healthbarBack;
    [SerializeField] GameObject healthbarFront;
    [SerializeField] GameObject[] dropItemList;
    private float dropChance = 0.1f;
    void Awake()
    {
        Messenger.AddListener(GameEvent.x2money,x2money);
        Messenger.AddListener(GameEvent.x2score, x2score);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.x2money, x2money);
        Messenger.AddListener(GameEvent.x2score, x2score);
        Managers.game.add_kill();
        Instantiate(money_pop, transform.position, Quaternion.Euler(-90,transform.eulerAngles.y,transform.eulerAngles.z));
    }
    void x2money()
    {
        StartCoroutine(onMoneyModifier());
    }
    IEnumerator onMoneyModifier()
    {
        moneyModifier = 2;
        yield return new WaitForSeconds(20f);
        moneyModifier = 1;
    }
    void x2score()
    {
        StartCoroutine(onScoreModifier());
    }
    IEnumerator onScoreModifier()
    {
        scoreModifier = 2;
        yield return new WaitForSeconds(20f);
        scoreModifier = 1;
    }
    public void hit()
    {
        health -= Managers.gun.damage;
        Managers.game.changeScore(3);
        if (health <= 0)
        {
            death();
        }
        setHealthBar();
    }
    void death()
    {
        Managers.game.enemiesAlive -= 1;
        Managers.game.changeMoney((int)(moneyWorth * moneyModifier));
        Managers.game.changeScore((int)(moneyWorth * scoreModifier));
        //instantiate a random powerup maybe -----------------------------------------------
        if (Random.Range(0, 1f)<dropChance) {
            Instantiate(dropItemList[(int)Random.Range(0, dropItemList.Length)], transform.position, transform.rotation);
        }
    
        Destroy(gameObject);
    }
    void setHealthBar()
    {
        healthbarFront.transform.localScale += new Vector3(-1 * healthbarFront.transform.localScale.x + _scale.x * health / maxHealth, 0, 0);
        //healthbarFront.transform.localScale.Set(_scale.x, _scale.y, _scale.z * health / maxHealth);
    }
	void Start () {
        health = maxHealth;
        _scale = healthbarFront.transform.localScale;
        setHealthBar();
        _animator = GetComponent<Animator>();
        StartCoroutine(spawn());
        damage = 1;
    }
	// Update is called once per frame
    public IEnumerator spawn()
    {
        _animator.SetBool("Spawning",true);
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
        yield return new WaitForSeconds(4.733f); //length of spawn animation
        _animator.SetBool("Spawning", false);
        _animator.SetBool("Idle", true);
        GetComponent<EnemyAttack>().enabled = true;
        GetComponent<EnemyMovement>().enabled = true;
    }
	void Update () {
        
    }
}
