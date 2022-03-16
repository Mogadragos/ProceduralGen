using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

using StarterAssets;


public class CustomInputs : StarterAssetsInputs
{
    [Header("Character Input Values")]
    public bool fire;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnFire(InputValue value)
    {
        FireInput(value.isPressed);
    }
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif

    public void FireInput(bool newFireState)
    {
        fire = newFireState;
    }
}
