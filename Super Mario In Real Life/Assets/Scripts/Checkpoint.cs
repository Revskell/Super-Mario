using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private AudioClip sound = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Checkpoints.AddPoint(transform.position, sound);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
