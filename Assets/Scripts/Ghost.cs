        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        public class Ghost : MonoBehaviour
        {
            private Collider2D coll;
            private SpriteRenderer sprite;
            private Animator anim;
            public bool ghost;

            void Awake()
            {
                coll = GetComponent<Collider2D>();
                sprite = GetComponent<SpriteRenderer>();
                anim = GetComponent<Animator>();
            }

            public void Init()
            {
                ghost = true;
                InvokeRepeating("DisableForOneSecond", 0f, 7f);
            }

            void DisableForOneSecond()
            {
                StartCoroutine(DisappearSequence());
            }

            void Update()
            {
                coll = GetComponent<Collider2D>();
            }

            IEnumerator DisappearSequence()
            {
                anim.SetBool("Dis", true);
                coll.enabled = false;

                yield return new WaitForSeconds(1f);

                sprite.enabled = false;

                StartCoroutine(EnableAfterDelay(3f));
            }

            IEnumerator EnableAfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                sprite.enabled = true;
                anim.SetBool("Dis", false);
                yield return new WaitForSeconds(0.3f);
                coll.enabled = true;
            }

            public void CleanUp()
            {
                StopCoroutine("DisappearSequence"); 
                StopCoroutine("EnableAfterDelay"); 
                CancelInvoke("DisableForOneSecond");
                anim.SetBool("Dis", false);
                ghost = false;
            }
        }
