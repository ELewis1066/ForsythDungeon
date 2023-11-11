using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class Creature
    {
        private string Name;
        private int Health;
        protected Random random;

        public Creature(string name, int health)
        {
            Name = name;
            Health = health;
            random = new Random();
        }

        public virtual int GetAttackDamage()
        {
            return (random.Next(10, 50));
        }
        public int GetHealth()
        {
            return Health;
        }
        public string GetName()
        {
            return Name;
        }
        public void SetHealth(int amount)
        {
            Health = amount;
        }
        public bool TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual Item? OnDeathDrop() { return null; }

        public virtual bool TakeSpellDamage(string spell, int damage)
        {
            Console.WriteLine($"{Name} takes {damage} from {spell}");

            return TakeDamage(damage);
        }
    }
}
