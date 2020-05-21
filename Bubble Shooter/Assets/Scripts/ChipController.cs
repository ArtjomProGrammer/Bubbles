using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    public class ChipController : MonoBehaviour
    {
        public List<GameObject> chipsSorted = new List<GameObject>();
        public GameObject spawnContainer;

        public List<GameObject> gridContainer = new List<GameObject>();

        void Start()
        {
            // spawn HexGrid/Containers and set the correct positions
            for (int i = 0; i < 10; i++)
            {
                // 5er row
                if (i % 2 == 0)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        GameObject GO = Instantiate(spawnContainer, transform.position, Quaternion.identity);
                        GO.transform.parent = transform;
                        GO.transform.position = new Vector3(transform.position.x + (.7f * x) + .35f, 4 - (i * .6f), 0);
                        gridContainer.Add(GO);
                    }
                }
                // 6er row
                else
                {
                    for (int x = 0; x < 6; x++)
                    {
                        GameObject GO = Instantiate(spawnContainer, transform.position, Quaternion.identity);
                        GO.transform.parent = transform;
                        GO.transform.position = new Vector3(transform.position.x + (.7f * x), 4 - (i * .6f), 0);
                        gridContainer.Add(GO);
                    }
                }
            }

            // spawn Chips
            for (int i = 0; i < 27; i++)
            {
                // we save the random.range to rename the gameobject to its origin.
                // because we are using its name value as our power of two placeholder~
                int tempRange = Random.Range(0, 5);
                GameObject GO = Instantiate(chipsSorted[tempRange], transform.GetChild(i).position, Quaternion.Euler(0, 0, 90));
                GO.transform.parent = transform.GetChild(i);
                // we tell the container that this position is already used
                transform.GetChild(i).GetComponent<GridContainer>().FreeContainer = false;
                GO.GetComponent<Rigidbody2D>().isKinematic = true;
                GO.gameObject.name = chipsSorted[tempRange].name;
            }
        }
    }
}