using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float Power = 10;
    public int BulletsNumber = 20;
    [Tooltip("Time required to pass before being able to fire again. Set to 0f to instantly fire again")]
    public float FireTimeout = 0.1f;

    public Rigidbody Bullet;
    public Transform FireOrigin;

    private Queue<Rigidbody> _bullets = new Queue<Rigidbody>();

    public void Fire()
    {
        Rigidbody bullet;
        if (_bullets.Count < BulletsNumber)
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

        bullet.velocity = transform.TransformDirection(Vector3.forward * Power);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
