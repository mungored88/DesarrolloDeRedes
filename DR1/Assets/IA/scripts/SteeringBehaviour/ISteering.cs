using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteering
{

	Vector3 GetDirection();
	Vector3 GetDirectionWaypoint(Transform _waypointTarget);
}
