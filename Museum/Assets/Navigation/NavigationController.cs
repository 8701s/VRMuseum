using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Navigation
{
    public class NavigationController : MonoBehaviour
    {

        public ViewingLocationRuntimeSet ViewingLocations;
        public BotRuntimeSet bots;
        public float LeaveWeight = 0.1f;

        public ViewingLocation getNewLocation([CanBeNull] ViewingLocation oldLocation)
        {
            int[] weights = new int[ViewingLocations.Items.Count];
            int weightsSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                if (oldLocation == ViewingLocations.Items[i])
                {
                    weights[i] = 0;
                } else if (oldLocation!= null && oldLocation.Area == ViewingLocations.Items[i].Area)
                {
                    weights[i] = 6;
                }
                else if (ViewingLocations.Items[i].Area == 0) //Exit
                {
                    if (oldLocation == null)
                    {
                        weights[i] = 0;
                    }
                    else
                    {
                        weights[i] = (int)(bots.Items.Count * LeaveWeight);
                    }
                }
                else
                {
                    weights[i] = 1;
                }
                weightsSum += weights[i];
            }

            int randomRoll = Random.Range(0, weightsSum);

            for (int i = 0; i < weights.Length; i++)
            {
                if (randomRoll < weights[i])
                {
                    return ViewingLocations.Items[i];
                }

                randomRoll -= weights[i];
            }

            Debug.Log("ERROR: Location visited twice!");
            return oldLocation;
        }
    }
}
