using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using StarterAssets;

public class CustomFirstPersonController : FirstPersonController
{
	[Header("Player")]
	[Tooltip("Time required to pass before being able to fire again. Set to 0f to instantly fire again")]
	public float FireTimeout = 0.1f;

	[Tooltip("Gun GameObject")]
	public Gun Gun;

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
		if(_input is CustomInputs)
        {
			_fireTimeoutDelta = FireTimeout;
			_customInput = _input as CustomInputs;
		}
	}

	private new void Update()
    {
		base.Update();
		if(_customInput != null) Fire();
    }

	private void Fire()
    {
		// Fire
		if (_customInput.fire && _fireTimeoutDelta <= 0.0f)
		{
			Gun.Fire();
			// reset the fire timeout timer
			_fireTimeoutDelta = FireTimeout;
		}

		// fire timeout
		if (_fireTimeoutDelta >= 0.0f)
		{
			_fireTimeoutDelta -= Time.deltaTime;
		}

		_customInput.fire = false;
	}
}
