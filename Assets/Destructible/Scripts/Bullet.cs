using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _firstContact = true;
    private Rigidbody _body;

    protected virtual void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(_firstContact)
        {
            if (other.gameObject.CompareTag("Destructible"))
            {
                Impact();
                Debug.Log(other.contacts[0].point); // TODO : break object
            }
            _firstContact = false;
        }
    }

    public virtual void Fire()
    {
        _firstContact = true;
        _body.detectCollisions = true;
    }

    protected virtual void Impact()
    {
        _body.detectCollisions = false;
        _body.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, -10);
    }
}
