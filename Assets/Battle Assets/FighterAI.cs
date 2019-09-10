using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAI : MonoBehaviour {
    public string nickname = "ENEMY";
    public Slider healthBar;

    private Fighter target;

    private string[] actions = { "Attack", "Rush", "Defend", "Counter", "Heal" };
    private string currentAction = "";

    private int Health = 0;
    private int Strength = 0;
    private int Defense = 0;
    private int attackAmount = 0;
    private int defenseAmount = 0;
    private int maxHealth = 0;

    // Start is called before the first frame update
    void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();

        Health = Random.Range(20, 50);
        Strength = Random.Range(1, 4);
        Defense = Random.Range(1, 4);
        
        attackAmount = 3 * Strength;
        defenseAmount = 2 * Defense;
        maxHealth = Health;
    }

    void Update() {
        if (healthBar.value > ((float)Health / (float)maxHealth)) {
            healthBar.value -= Time.deltaTime / 10f;
        } else if (healthBar.value < ((float)Health / (float)maxHealth)) {
            healthBar.value += Time.deltaTime / 10f;
        }
    }

    public void PickRandomAction() {
        int randomIndex = Random.Range(0, actions.Length);
        currentAction = actions[randomIndex];
    }

    public void TakeDamage(int damage) {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0, maxHealth);
    }

    public string Action() {
        return currentAction;
    }

    public int AttackAmount() {
        return attackAmount;
    }

    public int DefenseAmount() {
        return defenseAmount;
    }

    public bool Alive() {
        return (Health != 0);
    }
}
