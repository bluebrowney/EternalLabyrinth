using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace EternalLabyrinth
{
    public class Enemy : Non_StaticObject, PixelPerfectable, Moveable
    {

        public int health;
        public int strength;
        public double speed;

        public int attackRange;
        public int moveRange;

        public int playerLoc;

        public Movement movement;

        public Enemy(Texture2D t, Rectangle r, Color c, bool iFR) : base(t, r, c, iFR)
        {
            currentAnimation = "IDLE";
            col = c;
            baseColor = c;
            movement = new Movement(hitBox);
        }

        public virtual void LoadAbilities() { }

        public override void Update()
        {
            base.Update();

            timeStamps["isCollidingY"] = 0;
            timeStamps["isCollidingX"] = 0;
        }

        public void dealDamage(Player p)
        {
            p.takeDamage(strength);
        }

        public void takeDamage(int d)
        {
            health -= d;
        }

        public Movement getMotion()
        {
            return movement;
        }
    }
}