namespace Dungeon.Players
{
    public class Experience
    {
        public int Xp { get; set; }
        public int Level { get; set; }

        public int LevelDamageBonus { get; set; } 

        public Experience() 
        {
            Xp = 0;
            Level = 0;
            LevelDamageBonus = 0;
        } 

    }
}

