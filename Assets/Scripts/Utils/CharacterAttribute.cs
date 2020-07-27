using System;

namespace A_FPS_RPG
{
    [Serializable]
    public struct CharacterAttribute
    {
        public float hp;
        public float maxhp;
        public float mp;
        public float maxmp;

        public float moveSpeed;
        public float jumpHight;
        public float jumpTimeDuration;

        public CharacterAttribute(float hp = 100f, float maxhp = 100f, float mp = 100f,
            float maxmp = 100f, float moveSpeed = 2f, float jumpHight = 2f, float jumpTimeDuration = 0.5f)
        {
            this.hp = hp;
            this.maxhp = maxhp;
            this.mp = mp;
            this.maxmp = maxmp;
            this.moveSpeed = moveSpeed;
            this.jumpHight = jumpHight;
            this.jumpTimeDuration = jumpTimeDuration;
        }
    }
}
