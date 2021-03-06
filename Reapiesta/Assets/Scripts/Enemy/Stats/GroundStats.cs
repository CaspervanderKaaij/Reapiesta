﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoveState { idle, walking, chasing};
[CreateAssetMenu(fileName = "EnemyBaseStats", menuName = "Enemy/EnemyBaseStats/GroundStats", order = 0)]
public class GroundStats : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float mintargetDist;
}
