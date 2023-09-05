using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ShootArrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnpoint;
    [SerializeField] private TextMeshProUGUI firePowerText;
    [SerializeField] private Transform sceneUtilsTransform; // in order to unchild the arrow when shooting
    [SerializeField] private AudioSource shootingArrowAudio;

    private float maxFirePower = 50f;
    private float firePowerSpeed = 40f;
    private float firePower = 0;
    private float reloadDelay = 1f;
    private bool isChargingBow = false;
    private bool isReloading = false;

    private GameObject currentArrow;

    [SerializeField] private Animator animator;

    public void OnBowFullCharged()
    {
        animator.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isChargingBow = true;
            AddArrow();
        }

        if (isChargingBow)
        {
            // keep increasing until maxFirePower is reached
            if (firePower < maxFirePower)
            {
                firePower += Time.deltaTime * firePowerSpeed;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Shoot();
                firePower = 0;
                isChargingBow = false;
            }

            firePowerText.text = "Fire Power = " + String.Format("{0:.##}", firePower);
        }
    }

    void Shoot()
    {
        shootingArrowAudio.Play();
        currentArrow.GetComponent<Arrow>().Shoot(firePower);
        currentArrow.GetComponent<Arrow>().transform.SetParent(sceneUtilsTransform, true);
        currentArrow.GetComponent<Rigidbody>().useGravity = true;
        animator.SetTrigger("OnShot");
        animator.speed = 0.5f;
        StartCoroutine("Reload");
    }

    void AddArrow()
    {
        currentArrow = Instantiate(arrowPrefab, arrowSpawnpoint);
        currentArrow.GetComponent<Rigidbody>().useGravity = false;
        animator.SetTrigger("OnPulling");
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        isReloading = false;
    }
}