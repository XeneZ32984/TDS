using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRandomizer : StateMachineBehaviour
{
    // Константа с ключом смерти персонажа
    private const string DeathIdKey = "DeathId";

    // Константа с числом анимаций
    private const int AnimationCount = 3;

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        // Запускаем случайную анимацию из заданных
        animator.SetInteger(DeathIdKey, Random.Range(0, AnimationCount));
    }
}