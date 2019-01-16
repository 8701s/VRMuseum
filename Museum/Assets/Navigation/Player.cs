using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    private NavMeshObstacle player;

	void Awake ()
	{
	    player = GetComponent<NavMeshObstacle>();
	}

    public void SetAvoidanceRadius(float radius)
    {
        player.radius = radius;
    }
}
