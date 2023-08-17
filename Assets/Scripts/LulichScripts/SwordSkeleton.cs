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

    private void TakeDamage() {
        skeletonAnimator.runtimeAnimatorController = damage;
    }

    private void Attack() {
        skeletonAnimator.runtimeAnimatorController = attack;
    }

    private void Walk() {
        //Добавить логику ходьбы
        skeletonAnimator.runtimeAnimatorController = walk;
    }

    private void SetAnIdleAnimation() {
        skeletonAnimator.runtimeAnimatorController = idle;
    }
}
