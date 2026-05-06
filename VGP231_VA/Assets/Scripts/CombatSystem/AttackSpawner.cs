using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpawner : MonoBehaviour
{
    public GameObject attackPrefab;
    public Transform spawn;
    public float timer = 4;

    public List<MovingAttack> targets = new List<MovingAttack>();
    public Queue<MovingAttack> attacks = new Queue<MovingAttack>();
    public Queue<float> attackTimers = new Queue<float>();
    public float[] timers = new float[5];
    
void Start()
    {
        attacks = new Queue<MovingAttack>(targets);
        attackTimers = new Queue<float>(timers);
        timer = attackTimers.Peek();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (attacks.Count > 0)
        {
            //MovingAttack projectile = Instantiate(attackPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity).GetComponent<MovingAttack>();
            //projectile.end = spawn;
            //projectile.Attack();
            timer = attackTimers.Peek();
            attacks.Peek().gameObject.SetActive(true);
            attacks.Dequeue();
            attackTimers.Dequeue();
        }
    }


}
