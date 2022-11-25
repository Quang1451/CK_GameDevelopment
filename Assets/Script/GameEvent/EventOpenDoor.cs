using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private Transform[] PointSpawnZombies;
    [SerializeField] private Vector3 target;
    [SerializeField] private AudioClip clip;
    private bool first = true;
    private AudioSource source;

    void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void StartEvent()
    {
        if(first){
            StartCoroutine(Open());
            first = false;
        }
    }

    IEnumerator Open() {
        while(Door.transform.position.y < target.y) {
            if(!source.isPlaying) {
                AudioManager.Instance.PlayAudio(source,clip);
            }
            Door.transform.position = Vector3.Lerp(Door.transform.position, target, 0.005f * Time.deltaTime);
            yield return null;
        }
    }
}
