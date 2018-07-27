using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySwordTrail : MonoBehaviour
{
    [Header("Sword Trail Effect")]
    public GameObject swordTrailEffect;
    public GameObject swordEffectInstantiationPosition;
    public float defaultRotation = -45;

    void PlaySwordEffect()
    {
        GameObject swordTrailInstance = Instantiate(swordTrailEffect,
            swordEffectInstantiationPosition.transform.position,
              Quaternion.identity);

        int totalChildren = swordTrailInstance.transform.childCount;
        for (int i = 0; i < totalChildren; i++)
        {
            ParticleSystem childParticle = swordTrailInstance.transform.GetChild(i)
                .GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = childParticle.main;
            float rotation = defaultRotation + gameObject.transform.rotation.eulerAngles.y;
            main.startRotation = Mathf.Deg2Rad * rotation;
        }
    }
}
