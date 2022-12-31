using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class Hitbox : MonoBehaviour
    {

        public bool isInField = false;

        private SkeletonAI1 skel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "skeleton")
            {
                isInField = true;
                skel = other.GetComponent<SkeletonAI1>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "skeleton")
            {
                isInField = false;
                skel = null;
            }
        }

        public void Attack(float damage)
        {
            if (isInField)
            {
                skel.TakeDamage(damage);
            }

        }
    }
}
