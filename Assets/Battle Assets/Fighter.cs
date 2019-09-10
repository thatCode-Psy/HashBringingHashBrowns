using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour {
    [Header("Stats")]
    public int Health = 0;
    public int Strength = 0;
    public int Defense = 0;

    [Header("UI Objects")]
    public Text textBox;
    public Text healthText;
    public Slider playerHealth;

    [Header("Cooldown")]
    public float actionCooldown = 7f;

    [HideInInspector] public string battleFeedback = "";

    private FighterAI enemy;

    private static string[] actions = { "Attack", "Rush", "Defend", "Counter", "Heal" };
    private string currentAction = "";

    private bool readyToTakeAction = false;
    private bool needsNewTarget = true;

    private int attackAmount = 0;
    private int defenseAmount = 0;
    private int maxHealth = 0;

    private float timeSinceAction = 3f;
    private float healthSliderSpeed = 1f;

    // Start is called before the first frame update
    void Awake() {
        attackAmount = (int)(2.5f * Strength);
        defenseAmount = (int)(1.5f * Defense);
        maxHealth = Health;

        healthText.text = "HP: " + Health + " / " + maxHealth;
    }

    // Update is called once per frame
    void Update() {
        if(Alive() && !needsNewTarget) { // check if player is alive and if they do not need a new target
            if(!readyToTakeAction) {
                timeSinceAction += Time.deltaTime;

                // if time since action is greater than the action cooldown then the player is ready to take action
                if (timeSinceAction >= actionCooldown) {
                    textBox.fontSize = 28;
                    textBox.text = "Select an ability to use:\nAttack for highest damage\nDefend to reduce damage\nRush to attack first with less damage\nCounter" +
                        " to counter your opponents rush\nHeal to heal yourself.";
                    readyToTakeAction = true;
                    timeSinceAction = 0f;
                }
            }

            // if the player has selected an action and is ready to take action then update the enemies action and execute their actions
            if(currentAction != "" && readyToTakeAction) {
                enemy.PickRandomAction();
                ExecuteAction(enemy, currentAction);

                currentAction = "";
                readyToTakeAction = false;
            }
        } else if(needsNewTarget) { // spawn or get reference to a new target for the player
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<FighterAI>();
            needsNewTarget = false;
        }

        // update the players health bar to represent their current health
        if (playerHealth.value > ((float)Health / (float)maxHealth) + 0.005f) {
            playerHealth.value -= Time.deltaTime * healthSliderSpeed;
        } else if (playerHealth.value < ((float)Health / (float)maxHealth) - 0.005f) {
            playerHealth.value += Time.deltaTime * healthSliderSpeed;
        }

        healthText.text = "HP: " + Health + " / " + maxHealth; // update player health text
    }

    // function to handle the execution of actions between the player and the target
    public void ExecuteAction(FighterAI target, string action) {
        // get targets action and setup battleFeedback to give the information from the actions
        string targetsAction = target.Action();
        battleFeedback = "You used " + currentAction.ToLower() + " and " + target.nickname + " used " + targetsAction.ToLower() + ".\n";

        if(action == "Attack") { // check if player used attack
            // check for if target used rush or heal because they take priority over attack
            if(targetsAction == "Rush") {
                battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 1.5f) + " damage to you.\n";
                TakeDamage((int)(target.AttackAmount() / 1.5f));
            } else if(targetsAction == "Heal") {
                battleFeedback += target.nickname + " healed for 7 HP.\n";
                target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
            }

            if(Alive()) { // check that player is still alive
                int damage = attackAmount;

                if (targetsAction == "Counter") {
                    battleFeedback += target.nickname + " was unsuccessful in countering. ";
                    damage *= 2; // attack does twice as much if target counters
                } else if(targetsAction == "Defend") {
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
            if(targetsAction == "Counter") {
                battleFeedback += target.nickname + " was successful in countering and dealt " + target.AttackAmount() * 2 + " damage to you.\n";
                TakeDamage(target.AttackAmount() * 2); // deal damage to the player since the oppenent successfully countered
            } else if(targetsAction == "Defend") {
                int damage = (int)((attackAmount / 1.5f) - target.DefenseAmount());
                damage = Mathf.Clamp(damage, 0, attackAmount * 2); // clamp the min to 0 in case targets defense is higher than your attack damage
                target.TakeDamage(damage);
                battleFeedback += target.nickname + " defended. You dealt " + damage + " damage to " + target.nickname + ".\n";
            } else {
                battleFeedback += "You dealt " + (int)(attackAmount / 1.5f) + " damage to " + target.nickname + ".\n";

                if(target.Alive()) { // check if target is still alive
                    if(targetsAction == "Attack") {
                        battleFeedback += target.nickname + " dealt " + target.AttackAmount() + " damage to you.\n";
                        TakeDamage(target.AttackAmount());
                    } else if(targetsAction == "Rush") {
                        battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 1.5f) + " damage to you.\n";
                        TakeDamage((int)(target.AttackAmount() / 1.5f));
                    } else if(targetsAction == "Heal") {
                        battleFeedback += target.nickname + " healed for 7 HP.\n";
                        target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
                    }
                }
            }
        } else if(action == "Defend") { // check if player used defend
            if(targetsAction == "Defend" || targetsAction == "Counter") {
                battleFeedback += "Nothing happened.\n";
            } else if(targetsAction == "Heal") {
                battleFeedback += "You defended.\n" + target.nickname + " healed for 7 HP.\n";
                target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
            } else {
                int targetDamage = target.AttackAmount();
                battleFeedback += "You defended. ";

                if(targetsAction == "Attack") {
                    targetDamage -= defenseAmount;
                } else if(targetsAction == "Rush") {
                    targetDamage = (int)((targetDamage / 1.5f) - defenseAmount);
                }

                targetDamage = Mathf.Clamp(targetDamage, 0, target.AttackAmount());
                battleFeedback += target.nickname + " dealt " + targetDamage + " damage to you.\n";
                TakeDamage(targetDamage);
            }
        } else if(action == "Counter") { // check if player used counter
            if(targetsAction == "Defend" || targetsAction == "Counter") {
                battleFeedback += "Nothing happened.\n";
            } else if(targetsAction == "Rush") {
                battleFeedback += "You successfully countered and dealt " + attackAmount * 2 + " damage to " + target.nickname + ".\n";
                target.TakeDamage(attackAmount * 2); // deal damage to the target since player successfully countered
            } else {
                battleFeedback += "You were unsuccessful in countering. ";

                if (targetsAction == "Attack") {
                    battleFeedback += target.nickname + " dealt " + (target.AttackAmount() * 2) + " damage to you.\n";
                    TakeDamage(target.AttackAmount() * 2);
                } else if(targetsAction == "Heal") {
                    battleFeedback += target.nickname + " healed for 7 HP.\n";
                    target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
                }
            }
        } else if(action == "Heal") {
            // check rush first because it has priority over heal
            if (targetsAction == "Rush") {
                battleFeedback += target.nickname + " dealt " + (int)(target.AttackAmount() / 2) + " damage to you.\n";
                TakeDamage((int)(target.AttackAmount() / 1.5f));

                if(Alive()) { // check if player is still alive
                    battleFeedback += "You healed for 7 HP.\n";
                    Health += 7;
                    Health = Mathf.Clamp(Health, 0, maxHealth);
                }
            } else {
                battleFeedback += "You healed for 7 HP.\n";
                Health += 7;
                Health = Mathf.Clamp(Health, 0, maxHealth);

                if(targetsAction == "Attack") {
                    battleFeedback += target.nickname + " dealt " + target.AttackAmount() + " damage to you.\n";
                    TakeDamage(target.AttackAmount());
                } else if(targetsAction == "Defend") {
                    battleFeedback += target.nickname + " defended.\n";
                } else if(targetsAction == "Counter") {
                    battleFeedback += target.nickname + " was unsuccessful in countering.\n";
                } else if(targetsAction == "Heal") {
                    battleFeedback += target.nickname + " healed for 7 HP.\n";
                    target.TakeDamage(-7); // take damage from target with a negative value to increase targets health
                }
            }
        }
        

        if(!Alive()) { // check if the player was defeated during the battle
            battleFeedback += "You have been defeated.\n";
        }

        if (!target.Alive()) { // check if target was defeated during the battle
            battleFeedback += "You have defeated " + target.nickname + "!\n";
            needsNewTarget = true; // update that the player needs a new target
        }

        textBox.text = battleFeedback; // update the textBox with the battle feedback
    }

    // function to take damage from the player
    public void TakeDamage(int damage) {
        healthSliderSpeed = (damage * 0.05f) + 0.05f;

        Health -= damage;
        Health = Mathf.Clamp(Health, 0, maxHealth);
    }

    // returns the current action
    public string Action() { return currentAction; }
    
    // returns if the player is alive
    public bool Alive() { return (Health != 0); }

    // function to use with onClick() to update the players current action
    public void UpdateAction(string a) {
        if(readyToTakeAction) {
            currentAction = a;
        }
    }
}
