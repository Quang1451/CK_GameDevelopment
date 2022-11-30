using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private Transform[] PointSpawnZombies;
    [SerializeField] private Vector3 target;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject zb;
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
            InvokeRepeating("SpawnZB",2,3);
        }
    }

    IEnumerator Open() {
        while(Door.transform.position.y < target.y) {
            if(!source.isPlaying) {
                AudioManager.Instance.PlayAudio(source,clip);
            }
            Door.transform.position = Vector3.Lerp(Door.transform.position, target, 0.01f * Time.deltaTime);        
            yield return null;
        }
    }

    void SpawnZB(){
        if(GameObject.FindGameObjectsWithTag("Enemy").Length < 30) {
            GameObject zombie1 = Instantiate(zb, PointSpawnZombies[0].position, Quaternion.identity);
            zombie1.GetComponent<EnemyAI>().SetChasing();
            GameObject zombie2 = Instantiate(zb, PointSpawnZombies[1].position, Quaternion.identity);
            zombie2.GetComponent<EnemyAI>().SetChasing();
        }
    }
}
