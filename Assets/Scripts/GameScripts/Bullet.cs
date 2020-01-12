using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bloodSplat;
    [SerializeField] private GameObject bloodSplatDirectional;
    [SerializeField] private GameObject dirtSplat;
    [SerializeField]
    private float bulletSpeed = 5;
    RaycastHit objectHit;


    private CapsuleCollider _collider;
    // Use this for initialization
    void Start()
    {
        Physics.IgnoreLayerCollision(11, 12);
        Physics.IgnoreLayerCollision(11, 13);
        
        _collider = gameObject.GetComponent<CapsuleCollider>() as CapsuleCollider;
        Destroy(gameObject, 3f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out objectHit, 2*bulletSpeed * Time.deltaTime))
        {
            if (objectHit.collider.gameObject.tag == "Enemy")
            {
                Instantiate(bloodSplatDirectional, transform.position+transform.forward * bulletSpeed * Time.deltaTime, transform.rotation);
                objectHit.collider.gameObject.GetComponent<EnemyStats>().hit();
            }
            if (objectHit.collider.gameObject.tag == "Wall" )
            { Instantiate(dirtSplat, transform.position+ transform.forward* bulletSpeed * Time.deltaTime, transform.rotation); }
            if (objectHit.collider.gameObject.tag != "Item" && objectHit.collider.gameObject.tag != "Trigger")
            {
                DestroyObject(this.gameObject);
            }
        }
    }
}
