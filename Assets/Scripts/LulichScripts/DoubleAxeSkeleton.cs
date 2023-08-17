using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAxeSkeleton : MonoBehaviour
{
    [SerializeField] private Animator sekeletonAnimator;
    [SerializeField] private int attacksAnimationsNumber;

    public void TakeDamage() {
        sekeletonAnimator.Play("Damage00");
    }

    public void Attack() {
        int rand = Random.Range(0, attacksAnimationsNumber - 1);
        sekeletonAnimator.Play("Attack0"+rand);
    }

    public void Walk() {
        //Прописать логику хотьбы
        ChangeIsWalkingStatus(true);
    }

    private void ChangeIsWalkingStatus(bool value) {
        sekeletonAnimator.SetBool("isWalking", value);
    }
}
