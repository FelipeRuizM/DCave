using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontRunAway : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private AudioSource epicMusic;
    [SerializeField] private AudioSource winningMusic;

    private DontRunAway Instance;

    //void Awake()
    //{
    //    Messenger.AddListener("BOSS_DIED", OnBossDeath);
    //}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Messenger.AddListener("BOSS_DIED", OnBossDeath);
        } else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            door.SetActive(true);
            epicMusic.Play();
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void OnBossDeath()
    {
        epicMusic.Stop();
        StartCoroutine("Wait2Sec");
        
    }

    IEnumerator Wait2Sec()
    {
        yield return new WaitForSeconds(2f);
        winningMusic.Play();
    }
}
