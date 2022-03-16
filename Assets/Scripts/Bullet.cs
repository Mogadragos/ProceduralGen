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
                Explode();
                if(other.gameObject.GetComponent<Bullet>()?.Explode() == null)
                {
                    Debug.Log(other.contacts[0].point); // TODO : break object
                }
            }
            _firstContact = false;
        }
    }

    public void Fire()
    {
        _firstContact = true;
        _body.detectCollisions = true;
    }

    public bool Explode()
    {
        GameObject explosion = Explosions.prefabs[Random.Range(0, Explosions.prefabs.Length)] as GameObject;
        Instantiate(explosion, transform.position, Quaternion.identity);
        _body.detectCollisions = false;
        _body.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, -10);
        return true;
    }
}
