using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TriggerType
{
    OnEnterTriggerBox,
    OnExitTriggerBox,
    Interact
}
public class GameEventTrigger : MonoBehaviour {

    public TriggerType howEventIsTriggered;

    public bool hasBeenTriggered = false;
    public ScriptableEvent eventToTrigger;
    public AudioSource audioSource;
    public GameObject animatedObject;

    public Ghost ghost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (howEventIsTriggered == TriggerType.OnEnterTriggerBox)
            {
                TriggerEvent();
            }


        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (howEventIsTriggered == TriggerType.OnExitTriggerBox)
            {
                TriggerEvent();

            }

        }

    }

    public void TriggerEvent()
    {
        if (!hasBeenTriggered)
        {
            switch (eventToTrigger.eventType)
            {
                case EventType.Sound:
                    SoundEvent soundEvent = (SoundEvent)eventToTrigger;
                    audioSource.PlayOneShot(soundEvent.audioClip, soundEvent.volumeSetting);
                    break;
                case EventType.Animation:
                    AnimationEvent animationEvent = (AnimationEvent)eventToTrigger;
                    animatedObject.GetComponent<Animator>().SetTrigger(animationEvent.parameterToTrigger);
                    break;
                case EventType.Debug:
                    Debug.Log("Event Triggered");
                    break;
                case EventType.GhostEvent:
                    GhostEvent ghostEvent = (GhostEvent)eventToTrigger;
                    switch (ghostEvent.thisGhostEvent)
                    {
                        case GhostEventType.ActivateGhost:
                            ghost.ghostActive = true;
                            break;
                    }
                    break;

            
            }
            hasBeenTriggered = true;
        }
    }


}
