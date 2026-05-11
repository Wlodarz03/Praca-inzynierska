using UnityEngine;
using System;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource notificationSound;
    [SerializeField] private AudioSource swordEquipSound;
    [SerializeField] private AudioSource potionDrinkSound;
    [SerializeField] private AudioSource shieldEquipSound;
    [SerializeField] private AudioSource clothingEquipSound;
    [SerializeField] private AudioSource equipItemSound;

    public static SFX instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AudioManager.Instance.PlayMusic(backgroundMusic);
    }

    public void PlayClickSound()
    {
        clickSound.PlayOneShot(clickSound.clip);
    }

    public void PlayNotificationSound()
    {
        notificationSound.PlayOneShot(notificationSound.clip);
    }

    public void PlaySwordEquipSound()
    {
        swordEquipSound.PlayOneShot(swordEquipSound.clip);
    }

    public void PlayPotionDrinkSound()
    {
        potionDrinkSound.PlayOneShot(potionDrinkSound.clip);
    }

    public void PlayShieldEquipSound()
    {
        shieldEquipSound.PlayOneShot(shieldEquipSound.clip);
    }

    public void PlayClothingEquipSound()
    {
        clothingEquipSound.PlayOneShot(clothingEquipSound.clip);
    }

    public void PlayEquipItemSound()
    {
        equipItemSound.PlayOneShot(equipItemSound.clip);
    }

}