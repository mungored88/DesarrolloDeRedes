using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
	Transform _target;
	Transform _position;

	public Seek(Transform position, Transform target)
	{
		_target = target;
		_position = position;
	}

	public Vector3 GetDirection()
	{
		Vector3 dir = (_target.position - _position.position).normalized;
		return dir;
	}

	

	public Vector3 GetDirectionWaypoint(Transform _waypointTarget)
	{
		return Vector3.zero;
	}
}
