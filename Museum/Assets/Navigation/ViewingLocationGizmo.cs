using UnityEngine;
using System.Collections;

namespace Assets.Navigation
{
    public class ViewingLocationGizmo : MonoBehaviour {

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, 0.1f, transform.localScale.z));
        }
    }
}
