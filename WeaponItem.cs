﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class WeaponItem : Item
    {
        private int damageMultiplier_;
        public WeaponItem(String name, String description, int damageMultiplier) : base(name, description)
        {
            damageMultiplier_ = damageMultiplier;
        }
        public override int GetDamageMultiplier()
        {
            return damageMultiplier_;
        }

        public override bool IsWeapon()
        {
            return true;
        }
        public override string ToString()
        {
            return $"(weapon) {Name}, {Description} Damage Multiplier: {damageMultiplier_}";


        }

    }
}
