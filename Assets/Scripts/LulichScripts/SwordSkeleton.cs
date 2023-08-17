using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkeleton : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController damage;
    [SerializeField] private RuntimeAnimatorController attack;
    [SerializeField] private RuntimeAnimatorController walk;
    [SerializeField] private RuntimeAnimatorController idle;
    [SerializeField] private Animator skeletonAnimator;

    public void TakeDamage() {
        skeletonAnimator.runtimeAnimatorController = damage;
    }

    public void Attack() {
        skeletonAnimator.runtimeAnimatorController = attack;
    }

    public void Walk() {
        //Добавить логику ходьбы
        skeletonAnimator.runtimeAnimatorController = walk;
    }

    public void SetAnIdleAnimation() {
        skeletonAnimator.runtimeAnimatorController = idle;
    }
}
