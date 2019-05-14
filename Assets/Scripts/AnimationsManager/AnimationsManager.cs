using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationsManager : MonoBehaviour
{

    List<string> animationNames = new List<string>() { "Idle", "Walk", "T-Pose" };

    public GameObject gameobject;
    public Dropdown myDropdown;
    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    public void DropdownIndexChanged(int index)
    {
        switch(index)
        {
            case 0:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Idle", typeof(RuntimeAnimatorController));
                break;
            case 1:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Walk", typeof(RuntimeAnimatorController));
                break;
            case 2:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Vol_ArmRaise", typeof(RuntimeAnimatorController));
                break;
            default:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Idle", typeof(RuntimeAnimatorController));
                break;

        }
    }

    void Start()
    {
        PopulateList();
        myDropdown = GetComponent<Dropdown>();
        animator = gameobject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }   

    void PopulateList()
    {
        myDropdown.AddOptions(animationNames);
    }
}
