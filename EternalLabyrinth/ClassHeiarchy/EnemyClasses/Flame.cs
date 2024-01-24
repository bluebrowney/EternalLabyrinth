using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace EternalLabyrinth
{
    class Flame : Enemy, PixelPerfectable, Moveable
    {
        //texture frames are 32 x 32
        int floatingInterval;
        Dictionary<string, Ability> abilities;

        public Flame(int x, int y, bool iFR) : base(Game1.textures[T.Flame], new Rectangle(x, y, 32, 32), Color.White, iFR)
        {
            defaultAnimation = "IDLE";
            currentAnimation = "IDLE";

            floatingInterval = Game1.rand.Next(120, 1000);
            movement.velocity.Y = ((double)Game1.rand.Next(-2, 2)) / 10.0;
            timeStamps["FLOAT"] = 0;

            LoadAbilities();
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(8, 0, 0, 32, 32, 5, text.Width, true, false));
            animations.Add("ATTACK", new Animation(8, 0, 32, 32, 32, 5, text.Width, false, false));
            animations.Add("DEATH", new Animation(8, 0, 64, 32, 32, 7, text.Width, false, false));
        }

        public override void LoadAbilities()
        {
            abilities = new Dictionary<string, Ability>();

            abilities.Add("FlamingBullet", new FlamingBullet(this));
        }

        public override void Update()
        {
            base.Update();

            timeStamps["FLOAT"] += 1;

            if (timeStamps["FLOAT"] % floatingInterval == 0)
            {
                movement.velocity.Y *= -1;
            }

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (this == Game1.entities[i] || i >= Game1.entities.Count)
                {
                    continue;
                }

                if (Game1.entities[i] is Tile)
                {
                    if (!((Tile)Game1.entities[i]).foreground && !((Tile)Game1.entities[i]).hazard)
                    {
                        if (HelperMethods.environmentCollisions(this, Game1.entities[i]))
                        {
                        }
                    }
                }

                if(Game1.entities[i] is IceGlazier.IceThrow)
                {
                    if(HelperMethods.pixelCollision(this, Game1.entities[i]))
                    {
                        PlayAnimation("DEATH");
                        timeStamps["DEATH"] = 0;
                        animations[currentAnimation].PauseAt(animations[currentAnimation].numberOfFrames - 1);
                    }
                }

                if(Game1.entities[i] is Player)
                {
                    if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                    {
                        if (!timeStamps.ContainsKey("CONTACT"))
                        {
                            timeStamps["CONTACT"] = 0;
                        }
                        else
                        {
                            timeStamps["CONTACT"] += 1;
                            if (timeStamps["CONTACT"] % 20 == 0)
                            {
                                ((Player)Game1.entities[i]).takeDamage(1);
                            }
                        }
                    }
                    else
                    {
                        timeStamps.Remove("CONTACT");
                    }
                }
            }

            if(animations["DEATH"].paused)
            {
                Game1.entities.Remove(this);
            }

            if (!timeStamps.ContainsKey("DEATH"))
            {
                if (Game1.Time % 180 == 0 && HelperMethods.euclidianDistance(this, Game1.entities[0]) <= 400)
                {
                    PlayAnimation("ATTACK");
                    timeStamps["ATTACK"] = Game1.Time;
                }

                if (timeStamps.ContainsKey("ATTACK") && Game1.Time - timeStamps["ATTACK"] == 20)
                {
                    abilities["FlamingBullet"].Use();
                    timeStamps.Remove("ATTACK");
                }

                hitBox.X = movement.updatePosX();
                hitBox.Y = movement.updatePosY();

                rect.X = hitBox.X;
                rect.Y = hitBox.Y;
            }
        }
    }
}
