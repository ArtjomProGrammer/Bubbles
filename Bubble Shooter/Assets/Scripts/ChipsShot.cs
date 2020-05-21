using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    public class ChipsShot : MonoBehaviour
    {
        private Vector2 collisionPos;
        private float nearestDis = 10;
        private GameObject nearestGrid;
        private List<GameObject> twinks = new List<GameObject>();
        private List<GameObject> chips = new List<GameObject>();
        private int twinkCount;
        private bool checkForTwinks = false;
        private bool createCollider = false;
        private bool isMerged = false;
        private int nextPowerOfTwo;
        private GameObject tempMergePos;

        private void Start()
        {
            chips = GameObject.Find("ChipsManager").GetComponent<ChipController>().chipsSorted;

            if(isMerged)
                CheckForContainers();
        }

        private void Update()
        {
            // create collider to check if there are nearby chips with the same number
            if (createCollider == true)
            {
                createCollider = false;

                SearchForTwinks();

                MergeTogether();
            }
        }

        void CheckForContainers()
        {
            Collider2D[] getContainers = Physics2D.OverlapCircleAll(transform.position, .5f, LayerMask.GetMask("Grid"));

            for (int i = 0; i < getContainers.Length; i++)
            {
                if (Vector2.Distance(collisionPos, getContainers[i].transform.position) < nearestDis &&
                    getContainers[i].GetComponent<GridContainer>().FreeContainer)
                {
                    nearestDis = Vector2.Distance(collisionPos, getContainers[i].transform.position);
                    nearestGrid = getContainers[i].gameObject;
                }
            }
            nearestGrid.GetComponent<GridContainer>().FreeContainer = false;
            transform.parent = nearestGrid.transform;
            LeanTween.move(gameObject, nearestGrid.transform, .1f).setOnComplete(DestroyThis);
        }

        void DestroyThis()
        {
            Destroy(this);
        }

        void SearchForTwinks()
        {
            // while we find new chips with the same number, we repeat creating gizmos until every twink chip is found
            do
            {
                twinkCount = twinks.Count;
                for (int x = 0; x < twinks.Count; x++)
                {
                    Collider2D[] getTwinks = Physics2D.OverlapCircleAll(twinks[x].transform.position, .8f, LayerMask.GetMask("Chip"));

                    for (int i = 0; i < getTwinks.Length; i++)
                    {
                        if (getTwinks[i].gameObject.name == gameObject.name && !twinks.Contains(getTwinks[i].gameObject))
                        {
                            if (twinks[0] != getTwinks[i].gameObject)
                                twinks.Add(getTwinks[i].gameObject);
                        }
                    }
                }
                Debug.Log(twinks.Count);

            } while (twinkCount != twinks.Count);

            if (twinks.Count == 1)
                CheckForContainers();
        }


        // get next power of two value
        void NextPowerOfTwo()
        {
            int twinkAmount = twinks.Count;
            twinkAmount *= int.Parse(gameObject.name);
            if (Mathf.IsPowerOfTwo(twinkAmount))
                nextPowerOfTwo = twinkAmount;
            else
                nextPowerOfTwo = Mathf.NextPowerOfTwo(twinkAmount);
            SwitchPowerOfTwo();
        }

        void SwitchPowerOfTwo()
        {
            switch (nextPowerOfTwo)
            {
                case 4:
                    InstantiateNewChip(chips[1]);
                    break;
                case 8:
                    InstantiateNewChip(chips[2]);
                    break;
                case 16:
                    InstantiateNewChip(chips[3]);
                    break;
                case 32:
                    InstantiateNewChip(chips[4]);
                    break;
                case 64:
                    InstantiateNewChip(chips[5]);
                    break;
                case 128:
                    InstantiateNewChip(chips[6]);
                    break;
                case 256:
                    InstantiateNewChip(chips[7]);
                    break;
                case 512:
                    InstantiateNewChip(chips[8]);
                    break;
                case 1024:
                    InstantiateNewChip(chips[9]);
                    break;
                case 2048:
                    InstantiateNewChip(chips[10]);
                    break;
            }
        }

        void InstantiateNewChip(GameObject value)
        {
            GameObject GO = Instantiate(value, transform.position, Quaternion.Euler(0, 0, 90));
            GO.name = value.name;
            GO.GetComponent<Rigidbody2D>().isKinematic = true;

            for (int i = 0; i < twinks.Count; i++)
            {
                Destroy(twinks[i]);
            }
        }

        // merging together
        void MergeTogether()
        {
            if (twinks.Count > 1)
            {
                int twinkAmount = twinks.Count;
                twinkAmount *= int.Parse(gameObject.name);
                if (Mathf.IsPowerOfTwo(twinkAmount))
                    nextPowerOfTwo = twinkAmount;
                else
                    nextPowerOfTwo = Mathf.NextPowerOfTwo(twinkAmount);

                for (int x = 0; x < twinks.Count; x++)
                {
                    Collider2D[] getTwinks = Physics2D.OverlapCircleAll(twinks[x].transform.position, .8f, LayerMask.GetMask("Chip"));

                    for (int i = 0; i < getTwinks.Length; i++)
                    {
                        if (int.Parse(getTwinks[x].name) == nextPowerOfTwo && !twinks.Contains(getTwinks[x].gameObject))
                        {
                            tempMergePos = getTwinks[x].gameObject;
                        }
                        else
                        {
                            tempMergePos = twinks[twinks.Count - 1];
                        }
                    }
                }

                Debug.Log("twinks: " + twinks.Count);
                for (int i = 0; i < twinks.Count; i++)
                {
                    LeanTween.move(twinks[i], tempMergePos.transform.position, .1f);
                }
                Invoke("InvokedPowerOfTwoStart", .15f);
            }
        }

        void InvokedPowerOfTwoStart()
        {
            NextPowerOfTwo();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Chip"))
            {
                createCollider = true;
                twinks.Add(gameObject);

                // we check the position when it collides and add some Y value to make it fly to "possible" empty grid spaces above
                collisionPos = transform.position + new Vector3(0, .3f);

                GetComponent<Collider2D>().sharedMaterial = null;
                Destroy(GetComponent<Rigidbody2D>());
            }
        }
    }
}