using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bubbles
{
    public class Shooting : MonoBehaviour
    {
        private Touch touch;
        private GameObject GO;

        public GameObject chipController;
        public List<GameObject> chipsToFire = new List<GameObject>();

        private void Start()
        {
            chipsToFire = GameObject.Find("ChipsManager").GetComponent<ChipController>().chipsSorted;

            ReloadChip();
        }

        void Update()
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 10);
            //
            //if (hit)
            //{
            //    if (hit.collider.tag == "Wall")
            //    {
            //        Debug.Log(hit.transform.gameObject.name);
            //    }
            //}
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up) * 10, Color.red, 0);


            // shooting when releasing the finger
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    // we save the random.range to rename the gameobject to its origin.
                    // because we are using its name value as our power of two placeholder~

                    GO.AddComponent<ChipsShot>();
                    GO.GetComponent<Rigidbody2D>().AddForce(transform.up * (Time.deltaTime * .1f), ForceMode2D.Impulse);
                    GO.transform.parent = chipController.transform;
                    Invoke("ReloadChip",.5f);
                }
            }

            // rotate canon to touch position
            var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void ReloadChip()
        {
            int tempRange = Random.Range(0, 5);
            GO = Instantiate(chipsToFire[tempRange], transform.position, Quaternion.Euler(0, 0, 90));
            GO.gameObject.name = chipsToFire[tempRange].name;
        }
    }
}