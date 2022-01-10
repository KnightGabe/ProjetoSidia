using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent[] animationEvent;

    public void PlayEvent(int id){
        animationEvent[id].Invoke();
    }
}
