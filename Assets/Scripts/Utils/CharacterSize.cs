using System;

namespace A_FPS_RPG
{
    [Serializable]
    public struct CharacterSize
    {
        public float radius;
        public float height;

        public CharacterSize(float radius, float height)
        {
            this.radius = radius;
            this.height = height;
        }
    }
}
