using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public float health = 100f;
    public float hunger = 50f;
    public float thirst = 50f;
    public float stamina = 100f;
    public float strength = 10f;
    public float intelligence = 5f;
    public float agility = 7f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    public void RecoverStamina(float amount)
    {
        stamina = Mathf.Min(stamina + amount, 100f);
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        // Добавьте логику смерти игрока
    }
}