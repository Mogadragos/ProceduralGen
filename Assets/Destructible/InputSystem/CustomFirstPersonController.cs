using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using StarterAssets;

public class CustomFirstPersonController : FirstPersonController
{
    [Header("Player")]

    [Tooltip("Gun GameObject")]
    public Gun[] Guns;

    private Gun _currentGun;
    private int _currentGunIndex = 0;

    // timeout deltatime
    private float _fireTimeoutDelta;
    private CustomInputs _customInput = null;

    public void Reset()
    {
        RotationSpeed = 2f;
        TopClamp = 89.0f;
        BottomClamp = -89.0f;
    }

    private new void Start()
    {
        base.Start();
        if (_input is CustomInputs)
        {
            _currentGun = Guns[_currentGunIndex];
            _fireTimeoutDelta = _currentGun.FireTimeout;
            _customInput = _input as CustomInputs;
        }
    }

    private new void Update()
    {
        base.Update();
        if (_customInput != null)
        {
            Fire();
            ChangeWeapon();
        }
    }

    private void Fire()
    {
        // Fire
        if (_customInput.fire && _fireTimeoutDelta <= 0.0f)
        {
            _currentGun.Fire();
            // reset the fire timeout timer
            _fireTimeoutDelta = _currentGun.FireTimeout;
        }

        // fire timeout
        if (_fireTimeoutDelta >= 0.0f)
        {
            _fireTimeoutDelta -= Time.deltaTime;
        }

        _customInput.fire = false;
    }

    private void ChangeWeapon()
    {
        if(_customInput.weaponChange)
        {
            _customInput.weaponChange = false;
            if (_customInput.weaponNext)
            {
                _customInput.weaponNext = false;
                if (_currentGunIndex < Guns.Length - 1)
                {
                    _currentGunIndex++;
                }
            }
            else if (_customInput.weaponPrev)
            {
                _customInput.weaponPrev = false;
                if (_currentGunIndex > 0)
                {
                    _currentGunIndex--;
                }
            }
            _currentGun.Hide();
            _currentGun = Guns[_currentGunIndex];
            _currentGun.Show();
            _fireTimeoutDelta = 0.0f;
        }
    }
}
