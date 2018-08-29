﻿using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.Ennemy;
using Playmode.Pickable;
using Playmode.Pickable.Types;
using UnityEngine;

namespace Playmode.Pickable
{
    public class MedKitController : PickableController {
    
        [Header("Values")] [SerializeField] private int healthPointRestored = 75;

        public MedKitController()
        {
            Category = PickableCategory.Util;
        }

        protected override void ValidateSerialisedFields()
        {
            if (healthPointRestored <= 0)
                throw new ArgumentException("Medkit can't do damage.");
        }

        protected override void OnCollision(EnnemyController ennemy)
        {
            ennemy.Heal(healthPointRestored);
            
            destroyer.Destroy();
        }
    }
}


