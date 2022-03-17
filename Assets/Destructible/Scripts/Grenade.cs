using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Explosions
{
    public static Object[] prefabs = Resources.LoadAll("Explosions");
}

public class Grenade : Bullet
{
    public float ExplodeTimeout = 2f;

    private float _explodeTimeoutDelta;

    protected override void Start()
    {
        _explodeTimeoutDelta = ExplodeTimeout;
        base.Start();
    }

    private void Update()
    {
        if(_explodeTimeoutDelta > -1.0f)
        {
            if (_explodeTimeoutDelta <= 0.0f)
            {
                Impact();
            }

            if (_explodeTimeoutDelta >= 0.0f)
            {
                _explodeTimeoutDelta -= Time.deltaTime;
            }
        }
    }

    public override void Fire()
    {
        base.Fire();
        _explodeTimeoutDelta = ExplodeTimeout;
    }

    protected override void Impact()
    {
        _explodeTimeoutDelta = -2.0f;
        GameObject explosion = Explosions.prefabs[Random.Range(0, Explosions.prefabs.Length)] as GameObject;
        Instantiate(explosion, transform.position, Quaternion.identity);
        base.Impact();
    }
}
