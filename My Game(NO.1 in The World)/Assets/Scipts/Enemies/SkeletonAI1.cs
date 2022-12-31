using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class SkeletonAI1 : MonoBehaviour
    {
        private NavMeshAgent nav;
        public GameObject player;
        public Animator anim;

        private PlayerData data;
        public float minDamage;
        public float maxDamage;

        public GameObject damagePop;

        public Slider healthSlider;

        public float maxHealth;
        public float health;

        public float knockBack;
        public float followDist;
        public float maxWanderX, minWanderX, maxWanderZ, minWanderZ;

        public bool isWandering;

        public Transform wanderTarget;


        private bool isAttacking = false;
        private Rigidbody rigid;

        private float updateTime = 0;

        public void Start()
        {
            player = GameObject.Find("Player");
            nav = GetComponent<NavMeshAgent>();
            data = FindObjectOfType<PlayerData>();
            healthSlider.maxValue = maxHealth;
            health = maxHealth;
            healthSlider.value = health;
            rigid = GetComponent<Rigidbody>();

            maxWanderX = transform.position.x + 20;
            minWanderX = transform.position.x - 20;
            maxWanderZ = transform.position.z + 20;
            minWanderZ = transform.position.z - 20;
        }

        public void Update()
        {
            updateTime += Time.deltaTime;

            float dist = Vector3.Distance(this.transform.position, player.transform.position);
            if (dist <= 4)
            {
                if (!isAttacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                anim.SetBool("Attack", false);
                isAttacking = false;
            }

            if (health <= 0)
            {
                die();
            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            healthSlider.value = health;
            rigid.AddForce(-transform.forward * knockBack);
            var pop = Instantiate(damagePop, transform.position, transform.rotation);
            pop.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
            pop.transform.SetParent(transform);
            pop.transform.LookAt(player.transform);
            Destroy(pop, 0.5f);
            StopAllCoroutines();
            nav.enabled = false;
            anim.enabled = false;
            Invoke("ReEnableComponents", 1);
        }

        void die()
        {
            anim.SetBool("Die", true);
            nav.enabled = false;
            Destroy(gameObject, 3);
        }

        public void ReEnableComponents()
        {
            nav.enabled = true;
            anim.enabled = true;
        }

        IEnumerator Attack()
        {
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetBool("Attack", true);
                yield return new WaitForSeconds(1.2f);
                data.TakeDamage(Random.Range(minDamage, maxDamage));
                isAttacking = false;
            }
        }

        private void LateUpdate()
        {
            float dist = Vector3.Distance(this.transform.position, player.transform.position);
            if (dist <= followDist)
            {
                transform.LookAt(player.transform);
                if (updateTime > 2 && nav.enabled)
                {
                    nav.destination = player.transform.position;
                    updateTime = 0;
                }
            }
            else
            {
                if (!isWandering)
                {
                    StartCoroutine(wander());
                }
                else
                {
                    nav.destination = wanderTarget.transform.position;
                }
            }
        }

        IEnumerator wander()
        {
            if (!isWandering)
            {
                isWandering = true;
                var Xpos = Random.Range(minWanderX, maxWanderX);
                var Zpos = Random.Range(minWanderZ, maxWanderZ);
                var position = new Vector3(Xpos, transform.position.y, Zpos);
                
                wanderTarget.position = position;
                
                yield return new WaitForSeconds(5);
                isWandering = false;
            }
        }
    }
}
