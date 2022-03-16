using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Explosions
{
    public static Object[] prefabs = Resources.LoadAll("Explosions");
}

public class Bullet : MonoBehaviour
{
    private bool _firstContact = true;
    private Rigidbody _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(_firstContact)
        {
            if (other.gameObject.CompareTag("Destructible"))
            {
                GameObject explosion = Explosions.prefabs[Random.Range(0, Explosions.prefabs.Length)] as GameObject;
                Instantiate(explosion, transform.position, Quaternion.identity);
                _body.detectCollisions = false;
                _body.velocity = Vector3.zero;
                transform.position = new Vector3(0, 0, -10);
            }
            _firstContact = false;
        }
    }

    public void Fire()
    {
        _firstContact = true;
        _body.detectCollisions = true;
    }
}
