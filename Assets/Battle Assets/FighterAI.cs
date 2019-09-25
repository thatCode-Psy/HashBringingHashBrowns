using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAI : MonoBehaviour {
    [HideInInspector] public Slider healthBar;
    [HideInInspector] public Text nameText;
    [HideInInspector] public Text healthText;
    [HideInInspector] public string nickname = "ENEMY";

    private string[] actions = { "Attack", "Rush", "Defend", "Counter", "Heal" };
    private string currentAction = "";

    private int Health = 0;
    private int Strength = 0;
    private int Defense = 0;
    private int EXP = 0;
    private int attackAmount = 0;
    private int defenseAmount = 0;
    private int maxHealth = 0;

    private float healthSliderSpeed = 1f;

    // Start is called before the first frame update
    void Awake() {
        healthBar = GameObject.Find("Enemy Health Bar").GetComponent<Slider>();
        healthText = GameObject.Find("Enemy Health").GetComponent<Text>();
        nameText = GameObject.Find("Enemy Name").GetComponent<Text>();

        Health = Random.Range(20, 50);
        Strength = Random.Range(1, 4);
        Defense = Random.Range(1, 4);
        EXP = Random.Range(20, 40);
        
        attackAmount = 3 * Strength;
        defenseAmount = 2 * Defense;
        maxHealth = Health;

        healthSliderSpeed = 1f;
        healthText.text = "HP: " + Health + " / " + maxHealth;
    }

    void Update() {
        // update the enemy's health bar to represent their current health
        if (healthBar.value > ((float)Health / (float)maxHealth) + .01f) {
            healthBar.value -= Time.deltaTime * healthSliderSpeed;
        } else if (healthBar.value < ((float)Health / (float)maxHealth) - .01f) {
            healthBar.value += Time.deltaTime * healthSliderSpeed;
        }

        healthText.text = "HP: " + Health + " / " + maxHealth; // update enemy health text
        nameText.text = nickname; // update enemy name
    }

    // function to pick a random action
    public void PickRandomAction() {
        int randomIndex = Random.Range(0, actions.Length);
        currentAction = actions[randomIndex];
    }

    // function to take damage or heal the enemy
    public void TakeDamage(int damage) {
        healthSliderSpeed = Mathf.Abs(damage * 0.05f) + 0.05f;

        Health -= damage;
        Health = Mathf.Clamp(Health, 0, maxHealth);
    }

    // return the current action of the enemy
    public string Action() { return currentAction; }

    // return the attack stat of the enemy
    public int AttackAmount() { return attackAmount; }

    // return the defense stat of the enemy
    public int DefenseAmount() { return defenseAmount; }

    // return the remaining HP of the enemy
    public int GetHealth() { return Health; }

    // return the exp amount of the enemy
    public int ExpAmount() { return EXP; }

    // return if the enemy is alive
    public bool Alive() { return (Health != 0); }
}
