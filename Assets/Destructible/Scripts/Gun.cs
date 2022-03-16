using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Rigidbody Bullet;
    public Transform FireOrigin;

    private Queue<Rigidbody> _bullets = new Queue<Rigidbody>();

    public void Fire()
    {
        Rigidbody bullet;
        if (_bullets.Count < 10)
        {
            bullet = Instantiate(Bullet, FireOrigin.position, transform.rotation);
        } else
        {
            bullet = _bullets.Dequeue();
            bullet.rotation = transform.rotation;
            bullet.position = FireOrigin.position;
            bullet.GetComponent<Bullet>().Fire();
        }
        _bullets.Enqueue(bullet);

        bullet.velocity = transform.TransformDirection(Vector3.forward * 15);
    }
}
