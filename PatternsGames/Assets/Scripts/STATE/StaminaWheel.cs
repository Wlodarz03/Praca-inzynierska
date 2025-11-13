using UnityEngine;
using UnityEngine.UI;
public class StaminaWheel : MonoBehaviour
{
    public float stamina;
    [SerializeField] public float maxStamina = 100f;
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;
    [HideInInspector] public bool staminaExhausted;
    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !staminaExhausted)
        {
            if (stamina > 0)
            {
                stamina -= 20 * Time.deltaTime;
            }
            else
            {
                greenWheel.enabled = false;
                staminaExhausted = true;
            }
            redWheel.fillAmount = stamina / maxStamina + 0.07f;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += 20 * Time.deltaTime;
            }
            else
            {
                greenWheel.enabled = true;
                staminaExhausted = false;
            }

            redWheel.fillAmount = stamina / maxStamina;
        }

        greenWheel.fillAmount = stamina / maxStamina;
    }

    public void ConsumeStamina(float amount)
    {
        stamina -= amount;
        if (stamina <= 0f)
        {
            stamina = 0f;
            greenWheel.enabled = false;
            staminaExhausted = true;
        }

        greenWheel.fillAmount = stamina / maxStamina;
        redWheel.fillAmount = stamina / maxStamina + 0.07f;
    }
}
