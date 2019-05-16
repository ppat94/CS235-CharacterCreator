using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationsManager : MonoBehaviour
{

    List<string> animationNames = new List<string>() { "Idle", "Walk", "T-Pose" };

    public GameObject gameobject;
    public TMPro.TMP_Dropdown myDropdown;
    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    public void DropdownIndexChanged(int index)
    {
        switch(index)
        {
            case 0:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Idle", typeof(RuntimeAnimatorController));
                BaseButton.ResetCamPos();
                break;
            case 1:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Walk", typeof(RuntimeAnimatorController));
                BaseButton.SetCamPosWalking();
                break;
            case 2:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Vol_ArmRaise", typeof(RuntimeAnimatorController));
                BaseButton.ResetCamPos();
                break;
            default:
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Chal_Idle", typeof(RuntimeAnimatorController));
                BaseButton.ResetCamPos();
                break;

        }
    }

    void Start()
    {
        PopulateList();
        myDropdown = GetComponent<TMPro.TMP_Dropdown>();
        animator = gameobject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }   

    void PopulateList()
    {
        myDropdown.AddOptions(animationNames);
    }
}
