using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    public class GridContainer : MonoBehaviour
    {
        // we ask here if this container is free or already taken by another chip
        private bool freeContainer = true;
        public bool FreeContainer
        {
            get
            {
                return freeContainer;
            }
            set
            {
                freeContainer = value;
            }
        }
    }
}
