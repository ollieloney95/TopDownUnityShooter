using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float followDist = 2;
    [SerializeField] public float dist;
    //public Transform thisObject;
    public Transform target;
    public UnityEngine.AI.NavMeshAgent navComponent;
    private Animator _animation;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        navComponent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        followDist = 2;
        _animation = GetComponent<Animator>();
        navComponent.Warp(this.transform.position);
    }

    void Update()
    {

        dist = Vector3.Distance(target.position, transform.position);

        if (target)
        {
            _animation.SetBool("Walking",true);
            if (dist > followDist)
            {
                navComponent.SetDestination(target.position);
            }
            else
            {
                navComponent.SetDestination(transform.position);
                transform.LookAt(target.transform);
            }
        }

        else
        {
            if (target == null)
            {
                _animation.SetBool("Walking", false);
                _animation.SetBool("Idle", true);
                target = this.gameObject.GetComponent<Transform>();
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
    
}
