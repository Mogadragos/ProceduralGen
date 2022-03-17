using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _body;

    protected virtual void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        Impact();
        if (other.gameObject.CompareTag("Destructible"))
        {
            Debug.Log(other.contacts[0].point); // TODO : break object
        };
    }

    public virtual void Fire()
    {
        _body.detectCollisions = true;
    }

    protected virtual void Impact()
    {
        _body.detectCollisions = false;
        _body.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, -10);
    }
}
