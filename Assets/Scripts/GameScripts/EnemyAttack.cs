using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
    private float dist;
    private Transform target;
    private float attackDuration=0.6f;
    private bool attacking;
    private Animator _animator;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attacking = false;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        _animator = GetComponent<Animator>();
    }
	// Update is called once per frame
	void Update () {
        dist = Vector3.Distance(target.position, transform.position);
        if (dist <= GetComponent<EnemyStats>().attackRadius && attacking == false)
        {
            StartCoroutine(attack());
        }
        if (dist >= GetComponent<EnemyStats>().damageRadius) { StopAllCoroutines(); gameObject.GetComponent<MeshRenderer>().material.color = Color.green; attacking = false; _animator.SetBool("Attacking", false); }
        if (attacking == true) { gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(gameObject.GetComponent<MeshRenderer>().material.color, Color.red, 2*Time.deltaTime/ attackDuration); }
    }
    public IEnumerator attack()
    {
        //_animator.speed = 1.5f;
        _animator.SetBool("Attacking", true);
        attacking = true;
        //gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green,Color.red,0.01f);
        yield return new WaitForSeconds(attackDuration);
        if (dist <= GetComponent<EnemyStats>().damageRadius)
        {
            Managers.player.changeHealth(-1*GetComponent<EnemyStats>().damage);
            GameObject.Find("Game managers").GetComponent<UIController>().splat();
            
        }
        attacking = false;
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        _animator.SetBool("Attacking", false);
        //_animator.speed = 1.0f;
    }
}
