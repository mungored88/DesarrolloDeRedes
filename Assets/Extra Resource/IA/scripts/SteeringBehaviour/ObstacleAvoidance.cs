using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ISteering
{
	Transform _from;
	Transform _target;
	float _radius;
	float _avoidWeight;
	LayerMask _mask;

	public ObstacleAvoidance(Transform from, Transform target, float radius, float avoidWeight, LayerMask mask)
	{
		_avoidWeight = avoidWeight;
		_target = target;
		_radius = radius;
		_mask = mask;
		_from = from;
	}

	public Vector3 GetDirection()
	{
		Vector3 dir = (_target.position - _from.position).normalized;
		Collider[] obstacles = Physics.OverlapSphere(_from.position, _radius, _mask);
		int count = obstacles.Length;
		if (count > 0)
		{
			float currDistance = 0;
			int currIndex = 0;
			currDistance = Vector3.Distance(obstacles[0].transform.position, _from.transform.position);
			for (int i = 1; i < count; i++)
			{
				var newDistance = Vector3.Distance(obstacles[i].transform.position, _from.transform.position);
				if (newDistance < currDistance)
				{
					currDistance = newDistance;
					currIndex = i;
				}
			}

			var dirToObs = (_from.position - obstacles[currIndex].transform.position).normalized * ((_radius - currDistance) / _radius) * _avoidWeight;
			dir += dirToObs;
		}
		return dir.normalized;
	}

	public Vector3 GetDirectionWaypoint(Transform _waypointTarget)
	{
		Vector3 dir = (_waypointTarget.position - _from.position).normalized;
		Collider[] obstacles = Physics.OverlapSphere(_from.position, _radius, _mask);
		int count = obstacles.Length;
		if (count > 0)
		{
			float currDistance = 0;
			int currIndex = 0;
			currDistance = Vector3.Distance(obstacles[0].transform.position, _from.transform.position);
			for (int i = 1; i < count; i++)
			{
				var newDistance = Vector3.Distance(obstacles[i].transform.position, _from.transform.position);
				if (newDistance < currDistance)
				{
					currDistance = newDistance;
					currIndex = i;
				}
			}

			var dirToObs = (_from.position - obstacles[currIndex].transform.position).normalized * ((_radius - currDistance) / _radius) * _avoidWeight;
			dir += dirToObs;
		}
		return dir.normalized;
	}
}
