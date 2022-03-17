using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

using StarterAssets;


public class CustomInputs : StarterAssetsInputs
{
    [Header("Character Input Values")]
    public bool fire;
    public bool weaponNext;
    public bool weaponPrev;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnFire(InputValue value)
    {
        FireInput(value.isPressed);
    }
    public void OnWeaponChange(InputValue value)
    {
        float val = value.Get<float>();
        if (val < -10) WeaponChangeInput(false);
        else if(val > 10) WeaponChangeInput(true);
    }
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif

    public void FireInput(bool newFireState)
    {
        fire = newFireState;
    }
    public void WeaponChangeInput(bool direction)
    {
        if (direction) weaponNext = true;
        else weaponPrev = true;
    }
}
