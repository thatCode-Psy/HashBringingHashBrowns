﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fighter : MonoBehaviour, ControllerInterface {
    [Header("Stats")]
    public int Health = 0;
    public int Strength = 0;
    public int Defense = 0;
    public int expNeeded = 0;

    [Header("UI Objects")]
    public GameObject battleCanvas;
    public GameObject evolveCanvas;
    public Text textBox;
    public Text healthText;
    public Text expText;
    public Slider playerHealth;

    [Header("Enemy Objects")]
    public GameObject enemyPrefab;
    public GameObject[] enemySprites;
    public string[] enemyNames;

    [Header("Cooldown")]
    public float actionCooldown = 7f;

    [Header("Audio")]
    public AudioSource deathSFX;
    public AudioSource healSFX;
    public AudioSource attackSFX;
    public AudioSource counterSFX;
    public AudioSource defendSFX;
    public AudioSource levelUpSFX;
    public AudioSource rushSFX;

    private FighterAI enemy;

    private string currentAction = "";
    private string battleFeedback = "";
    private string previousAction = "";
    private string enemyPreviousAction = "";

    private bool readyToTakeAction = false;
    private bool needsNewTarget = true;
    private bool levelUp = false;
    private bool pause = false;
    private bool enemySuccussfulCounter = false;
    private bool playLevelUp = false;

    private int attackAmount = 0;
    private int defenseAmount = 0;
    private int maxHealth = 0;
    private int exp = 0;
    private int spriteIndex = 0;
    private int graphNodeIndex = 13;
    private int repeatActions = 0;
    private int enemyRepeatActions = 0;
    private int turnCount = 0;

    private float timeSinceAction = 3f;
    private float healthSliderSpeed = 1f;
    private float evolveTimer = 0f;
    private float deathCooldown = 2f;

    void Start() {
        attackAmount = (int)(2.5f * Strength);
        defenseAmount = (int)(1.5f * Defense);
        maxHealth = Health;
        exp = 0;

        healthText.text = "HP: " + Health + " / " + maxHealth;
        expText.text = "EXP: " + exp + "/" + expNeeded;

        ControllerStateMachine.Instance.SetGame(this);
    }

    void Update() {
        if(!pause) {
            if(levelUp && readyToTakeAction) {
                if(evolveTimer < 3f) {
                    if(!playLevelUp) { levelUpSFX.Play(); playLevelUp = true; }
                    textBox.fontSize = 36;
                    textBox.text = "LEVEL UP!\nHealth, Strength, and Defense have increased!\nOh, what is this? Your pokabomination is evolving!";
                    evolveTimer += Time.deltaTime;
                } else {
                    playLevelUp = false;
                    evolveTimer = 0f;
                    if (!evolveCanvas.activeInHierarchy) {
                        evolveCanvas.SetActive(true);
                    }
                }
            } else if(needsNewTarget && readyToTakeAction) { // spawn a new target for the player
                if (enemy) { Destroy(enemy.gameObject); }
                SpawnNewEnemy();
                turnCount = 0;
                readyToTakeAction = false;
                needsNewTarget = false;
            } else if (Alive()) { // check if player is alive and if they do not need a new target
                if(!readyToTakeAction) {
                    if (!pause) {
                        timeSinceAction += Time.deltaTime;
                    }

                    // if time since action is greater than the action cooldown then the player is ready to take action
                    if (timeSinceAction >= actionCooldown) {
                       if (!needsNewTarget) { // if needsNewTarget is false then update textBox as normal
                            textBox.fontSize = 24;
                            textBox.text = "Select an ability to use:\nAttack for highest damage\nDefend to reduce damage\nRush to attack first with less damage\nCounter" +
                                " to counter your opponents rush\nHeal to heal yourself.";
                       }

                        readyToTakeAction = true;
                        timeSinceAction = 0f;
                    }
                }

                // if the player has selected an action and is ready to take action then update the enemies action and execute their actions
                if(currentAction != "" && readyToTakeAction && !needsNewTarget) {
                    enemy.PickRandomAction();
                    ExecuteAction(enemy, currentAction);

                    previousAction = currentAction;
                    enemyPreviousAction = enemy.Action();

                    currentAction = "";
                    readyToTakeAction = false;
                    turnCount++;
                }
            } else if (!Alive()) {
                if(!readyToTakeAction) {
                    if (!pause) {
                        timeSinceAction += Time.deltaTime;
                    }

                    if (timeSinceAction >= deathCooldown) {
                        readyToTakeAction = true;
                    }
                } else {
                    ControllerStateMachine.Instance.StopGame();
                    SceneManager.LoadScene("GameSelect");
                }
            }

            // update the players health bar to represent their current health
            if (playerHealth.value > ((float)Health / (float)maxHealth) + 0.01f) {
                playerHealth.value -= Time.deltaTime * healthSliderSpeed;
            } else if (playerHealth.value < ((float)Health / (float)maxHealth) - 0.01f) {
                playerHealth.value += Time.deltaTime * healthSliderSpeed;
            }

            /* IN HERE FOR EVOLVING TESTING PURPOSES */
            /*
            if (exp < expNeeded) {
                ExpGain(100);
            }
            */

            healthText.text = "HP: " + Health + " / " + maxHealth; // update player health text
            expText.text = "EXP: " + exp + "/" + expNeeded; // update player exp text
        }
    }

    // function for left button
    public void Left() { UpdateAction("Rush"); }

    // function for right button
    public void Right() { UpdateAction("Rush"); }

    // function for up button
    public void Up() { UpdateAction("Attack"); }

    // function for down button
    public void Down() { UpdateAction("Heal"); }

    // function for a button
    public void A() { UpdateAction("Defend"); }

    // function for b button
    public void B() { UpdateAction("Counter"); }

    // function for pause button
    public void Pause() { pause = !pause; }

    // function to handle the execution of actions between the player and the target
    public void ExecuteAction(FighterAI target, string action) {
        // get targets action and setup battleFeedback to give the information from the actions
        string targetsAction = target.Action();
        battleFeedback = "You used " + currentAction.ToLower() + " and " + target.nickname + " used " + targetsAction.ToLower() + ".\n";

        // check for repeat player actions
        if (previousAction == currentAction) { repeatActions++; }
        else { repeatActions = 0; }

        // check for repeat enemy actions
        if (enemyPreviousAction == targetsAction) { enemyRepeatActions++; }
        else { enemyRepeatActions = 0; }

        graphNodeIndex = 13; // for random conversation if it is not overwritten by a situation

        if(repeatActions >= 2) { graphNodeIndex = 19; }
        else if(targetsAction == "Heal" && enemyRepeatActions >= 2) { graphNodeIndex = 18; }
        else if((targetsAction == "Defend" || targetsAction == "Counter") && (enemyRepeatActions >= 2)) { graphNodeIndex = 20; }
        else if(turnCount >= 10) { graphNodeIndex = 9; }

        if (action == "Attack") { // check if player used attack
            attackSFX.Play();
            if (enemy.GetHealth() > 7 && enemy.GetHealth() <= 10) { graphNodeIndex = 5; }

            // check for if target used rush or heal because they take priority over attack
            if(targetsAction == "Rush") {
                rushSFX.Play();
                battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 1.5f) + " damage to you.\n";
                TakeDamage((int)(target.AttackAmount() / 1.5f));
            } else if(targetsAction == "Heal") {
                healSFX.Play();
                battleFeedback += target.nickname + " healed for 4 HP.\n";
                target.TakeDamage(-4); // take damage from target with a negative value to increase targets health
            }

            if(Alive()) { // check that player is still alive
                int damage = attackAmount;

                if (targetsAction == "Counter") {
                    graphNodeIndex = 16;
                    battleFeedback += target.nickname + " was unsuccessful in countering. ";
                    damage *= 2; // attack does twice as much if target counters
                } else if(targetsAction == "Defend") {
                    defendSFX.Play();
                    graphNodeIndex = 15;
                    battleFeedback += target.nickname + " defended. ";
                    damage -= target.DefenseAmount(); // attack does less if target defends
                }

                damage = Mathf.Clamp(damage, 0, attackAmount * 2); // clamp the min to 0 in case targets defense is higher than your attack damage
                battleFeedback += "You dealt " + damage + " damage to " + target.nickname + ".\n";
                target.TakeDamage(damage);

                if(target.Alive() && targetsAction == "Attack") { // check if target is still alive and if they used the attack command
                    battleFeedback += target.nickname + " dealt " + target.AttackAmount() + " damage to you.\n";
                    TakeDamage(target.AttackAmount());
                }
            }
        } else if(action == "Rush") { // check if player used rush
            if(enemySuccussfulCounter) { graphNodeIndex = 8; }

            if (targetsAction == "Counter") {
                counterSFX.Play();
                graphNodeIndex = 17;
                battleFeedback += target.nickname + " was successful in countering and dealt " + target.AttackAmount() * 2 + " damage to you.\n";
                TakeDamage(target.AttackAmount() * 2); // deal damage to the player since the oppenent successfully countered
            } else if(targetsAction == "Defend") {
                rushSFX.Play();
                defendSFX.Play();
                int damage = (int)((attackAmount / 1.5f) - target.DefenseAmount());
                damage = Mathf.Clamp(damage, 0, attackAmount * 2); // clamp the min to 0 in case targets defense is higher than your attack damage
                target.TakeDamage(damage);
                battleFeedback += target.nickname + " defended. You dealt " + damage + " damage to " + target.nickname + ".\n";
            } else {
                rushSFX.Play();
                battleFeedback += "You dealt " + (int)(attackAmount / 1.5f) + " damage to " + target.nickname + ".\n";
                target.TakeDamage((int)(attackAmount / 1.5f));

                if(target.Alive()) { // check if target is still alive
                    if(targetsAction == "Attack") {
                        attackSFX.Play();
                        battleFeedback += target.nickname + " dealt " + target.AttackAmount() + " damage to you.\n";
                        TakeDamage(target.AttackAmount());
                    } else if(targetsAction == "Rush") {
                        battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 1.5f) + " damage to you.\n";
                        TakeDamage((int)(target.AttackAmount() / 1.5f));
                    } else if(targetsAction == "Heal") {
                        healSFX.Play();
                        battleFeedback += target.nickname + " healed for 4 HP.\n";
                        target.TakeDamage(-4); // take damage from target with a negative value to increase targets health
                    }
                }
            }
        } else if(action == "Defend") { // check if player used defend
            defendSFX.Play();
            graphNodeIndex = 7;

            if(targetsAction == "Defend" || targetsAction == "Counter") {
                battleFeedback += "Nothing happened.\n";
            } else if(targetsAction == "Heal") {
                healSFX.Play();
                battleFeedback += "You defended.\n" + target.nickname + " healed for 4 HP.\n";
                target.TakeDamage(-4); // take damage from target with a negative value to increase targets health
            } else {
                int targetDamage = target.AttackAmount();
                battleFeedback += "You defended. ";

                if(targetsAction == "Attack") {
                    attackSFX.Play();
                    targetDamage -= defenseAmount;
                } else if(targetsAction == "Rush") {
                    rushSFX.Play();
                    targetDamage = (int)((targetDamage / 1.5f) - defenseAmount);
                }

                targetDamage = Mathf.Clamp(targetDamage, 0, target.AttackAmount());
                battleFeedback += target.nickname + " dealt " + targetDamage + " damage to you.\n";
                TakeDamage(targetDamage);
            }
        } else if(action == "Counter") { // check if player used counter
            if(Health < (maxHealth / 2)) { graphNodeIndex = 4; }

            if(targetsAction == "Defend" || targetsAction == "Counter") {
                graphNodeIndex = 6;
                if (targetsAction == "Defend") { defendSFX.Play(); }
                battleFeedback += "Nothing happened.\n";
            } else if(targetsAction == "Rush") {
                counterSFX.Play();
                //graphNodeIndex = ;
                battleFeedback += "You successfully countered and dealt " + attackAmount * 2 + " damage to " + target.nickname + ".\n";
                target.TakeDamage(attackAmount * 2); // deal damage to the target since player successfully countered
            } else {
                graphNodeIndex = 6;
                /*int pick = Random.Range(0, 2);
                if (pick == 0) { graphNodeIndex = 6; }
                else { graphNodeIndex = 14; }*/
                battleFeedback += "You were unsuccessful in countering. ";

                if (targetsAction == "Attack") {
                    attackSFX.Play();
                    battleFeedback += target.nickname + " dealt " + (target.AttackAmount() * 2) + " damage to you.\n";
                    TakeDamage(target.AttackAmount() * 2);
                } else if(targetsAction == "Heal") {
                    healSFX.Play();
                    battleFeedback += target.nickname + " healed for 7 HP.\n";
                    target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
                }
            }
        } else if(action == "Heal") {
            // check rush first because it has priority over heal
            if (targetsAction == "Rush") {
                rushSFX.Play();
                battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 2) + " damage to you.\n";
                TakeDamage((int)(target.AttackAmount() / 1.5f));

                if(Alive()) { // check if player is still alive
                    healSFX.Play();
                    battleFeedback += "You healed for 6 HP.\n";
                    Health += 6;
                    Health = Mathf.Clamp(Health, 0, maxHealth);
                }
            } else {
                healSFX.Play();
                battleFeedback += "You healed for 6 HP.\n";
                Health += 6;
                Health = Mathf.Clamp(Health, 0, maxHealth);

                if(targetsAction == "Attack") {
                    attackSFX.Play();
                    battleFeedback += target.nickname + " dealt " + target.AttackAmount() + " damage to you.\n";
                    TakeDamage(target.AttackAmount());
                } else if(targetsAction == "Defend") {
                    battleFeedback += target.nickname + " defended.\n";
                } else if(targetsAction == "Counter") {
                    battleFeedback += target.nickname + " was unsuccessful in countering.\n";
                } else if(targetsAction == "Heal") {
                    battleFeedback += target.nickname + " healed for 4 HP.\n";
                    target.TakeDamage(-4); // take damage from target with a negative value to increase targets health
                }
            }
        }
        

        if(!Alive()) { // check if the player was defeated during the battle
            deathSFX.Play();
            battleFeedback += "You have been defeated.\n";
        }

        if (!target.Alive()) { // check if target was defeated during the battle
            deathSFX.Play();
            battleFeedback += "You have defeated " + target.nickname + " and recieved " + target.ExpAmount() + " EXP!\n";
            ExpGain(target.ExpAmount());
            enemySprites[spriteIndex].SetActive(false);
            needsNewTarget = true; // update that the player needs a new target
        }

        if(action == "Rush" && targetsAction == "Counter") { enemySuccussfulCounter = true; }
        else { enemySuccussfulCounter = false; }

        textBox.text = battleFeedback; // update the textBox with the battle feedback
    }

    // function to take damage from the player
    public void TakeDamage(int damage) {
        healthSliderSpeed = (damage * 0.05f) + 0.05f;

        Health -= damage;
        Health = Mathf.Clamp(Health, 0, maxHealth);
    }

    // function to add exp to the player
    public void ExpGain(int amount) {
        exp += amount;
        exp = Mathf.Clamp(exp, 0, expNeeded);

        if(exp == expNeeded) {
            levelUp = true;
        }
    }

    // function to spawn a new enemy and link the player to the enemy
    public void SpawnNewEnemy() {
        // instantiate the new enemy and update all of the enemies information
        GameObject newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        newEnemy.name = "Enemy";
        enemy = newEnemy.GetComponent<FighterAI>();
        enemy.nickname = enemyNames[Random.Range(0, enemyNames.Length)];

        // output that a new enemy has appeared
        textBox.fontSize = 44;
        textBox.text = enemy.nickname + " has appeared!";
        timeSinceAction = actionCooldown / 2f; // speed up delay between enemy appearing and being able to play

        // set an enemy sprite active for the new enemy
        spriteIndex = Random.Range(0, enemySprites.Length);
        enemySprites[spriteIndex].SetActive(true);
    }

    // returns the current action
    public string Action() { return currentAction; }
    
    // returns if the player is alive
    public bool Alive() { return (Health != 0); }

    // function to use with onClick() to update the players current action
    public void UpdateAction(string a) {
        if (readyToTakeAction && !pause) {
            currentAction = a;
        }
    }

    // function to update fighter after evolve
    public void StopEvolve() {
        levelUp = false;
        evolveCanvas.SetActive(false);

        attackAmount++;
        defenseAmount++;
        maxHealth += Random.Range(2, 5);
        Health = maxHealth;
        exp = 0;
        expNeeded += 5;
        expNeeded = Mathf.Clamp(expNeeded, 0, 100);
    }

    public List<int> GetPossibleDialogueNodes(){
        return new List<int> { graphNodeIndex };
    }
}
