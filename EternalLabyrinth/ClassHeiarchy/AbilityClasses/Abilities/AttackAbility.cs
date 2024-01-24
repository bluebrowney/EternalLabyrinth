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
    public class AttackAbility : Ability, PixelPerfectable, Moveable
    {
        public int maxDamage;
        public int velocity;
        public int duration;
        public AttackAbility(Rectangle r, Color c, bool iFR, int duration, int maxDamage) : base(Game1.textures[T.FireBall], r, c, iFR)
        {
            this.maxDamage = maxDamage;
            this.duration = duration;
            velocity = 4;
            maxDamage = 4;
            duration = 2;
            currentAnimation = "IDLE";

        }

        public void callAbility(int playerX, int playerY, bool iFR)
        {
            isCalled = true;
            rect.X = playerX;
            rect.Y = playerY + 40;
            isFacingRight = iFR;
            motion = new Movement(rect);
            Game1.entities.Add(this);
        }
        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(10, 0, 0, 32, 32, 10, text.Width, true, false));

        }
        public override void Update()
        {
            base.Update();

            rect.X = motion.updatePosX();
            hitBox = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            if (isCalled)
            {
                if (isFacingRight)
                    motion.velocityX(velocity);
                else
                    motion.velocityX(-1 * velocity);

                if (!timeStamps.ContainsKey("Fireball"))
                {
                    timeStamps["Fireball"] = HelperMethods.TotalSeconds();
                }
                else if (HelperMethods.TotalSeconds() - timeStamps["Fireball"] == duration)
                {
                    isCalled = false;
                    motion.velocityX(0);
                    timeStamps.Remove("Fireball");
                    Game1.entities.Remove(this);

                }
            }

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (Game1.entities[i] is Enemy)
                {
                    if (Game1.entities[i].hitBox.Intersects(hitBox) && !timeStamps.ContainsKey("HIT"))
                    {
                        isCalled = false;
                        timeStamps["HIT"] = 0;
                        Enemy e = (Enemy)Game1.entities[i];
                        e.takeDamage(maxDamage);
                        Game1.entities.Remove(this);
                    }
                }
            }

        }

        public Movement getMotion()
        {
            return motion;
        }
    }
}
