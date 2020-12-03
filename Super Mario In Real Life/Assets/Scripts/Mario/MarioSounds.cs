using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioSounds : MonoBehaviour
{
    [SerializeField] private AudioClip step = null;
    [SerializeField] private AudioClip jump1 = null;
    [SerializeField] private AudioClip jump2 = null;
    [SerializeField] private AudioClip jump3 = null;
    [SerializeField] private AudioClip longJump = null;
    [SerializeField] private AudioClip punch1 = null;
    [SerializeField] private AudioClip punch2 = null;
    [SerializeField] private AudioClip punch3 = null;

    public void Step() { PlaySound(step); }
    public void Jump1() { PlaySound(jump1); }
    public void Jump2() { PlaySound(jump2); }
    public void Jump3() { PlaySound(jump3); }
    public void LongJumpSound() { PlaySound(longJump); }
    public void PunchSound1() { PlaySound(punch1); }
    public void PunchSound2() { PlaySound(punch2); }
    public void PunchSound3() { PlaySound(punch3); }
    public void FinishPunch() { }

    private void PlaySound(AudioClip clip) { AudioSource.PlayClipAtPoint(clip, transform.position); }

}
