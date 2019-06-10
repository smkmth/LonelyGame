using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public Animator[] anims;
public void Start(){
SetBool("cast", true);
}


    public void SetBool(string boolToSet, bool isTrue)
    {
        foreach (Animator anim in anims)
        {
            anim.SetBool(boolToSet, isTrue);
        }

    }
    public void SetTrigger(string triggerToSet)
    {
        foreach (Animator anim in anims)
        {
            anim.SetTrigger(triggerToSet);
        }
    }
}
