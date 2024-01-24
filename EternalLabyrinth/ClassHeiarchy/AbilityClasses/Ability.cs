using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace EternalLabyrinth
{
    public class Ability : Non_StaticObject
    {
        public Movement motion;
        //duration of Ability in seconds
        public int duration;
        public bool isCalled;
        

        public Ability(Texture2D t, Rectangle r, Color c, bool iFR) : base(t, r, c, iFR)
        {
            motion = new Movement(rect);
            isCalled = false;
            hitBox = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            timeStamps = new Dictionary<string, int>();
        }

        public override void LoadAnimations() { }

        public virtual void Use() { }
    }
}
