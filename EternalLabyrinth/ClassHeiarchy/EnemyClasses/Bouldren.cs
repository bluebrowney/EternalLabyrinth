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
    class Bouldren : Enemy, PixelPerfectable, Moveable
    {
        public Bouldren(int x, int y, bool iFR) : base(Game1.textures[T.Bouldren], new Rectangle(x, y, 32, 32), Color.White, iFR)
        {
            currentAnimation = "IDLE";
            defaultAnimation = "IDLE";

            movement.gravityOn = true;
            timeStamps["INRANGE"] = 0;
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(8, 0, 0, 32, 32, 3, text.Width, true, false));
            animations.Add("ROLLSTART", new Animation(8, 32 * 3, 0, 32, 32, 2, text.Width, false, false));
            animations.Add("ROLL", new Animation(8, 0, 32, 32, 32, 2, text.Width, true, false));
            animations.Add("DEATH", new Animation(8, 32 * 2, 32, 32, 32, 5, text.Width, false, false));
        }

        public void Roll()
        {
            PlayAnimation("ROLL");
            timeStamps.Remove("ROLLSTART");
            timeStamps["ROLL"] = 0;
            if (Game1.entities[0].rect.X < rect.X)
            {
                movement.acceleration.X = -0.5;
            }
            else
            {
                movement.acceleration.X = 0.5;
            }
        }

        public void Die()
        {
            PlayAnimation("DEATH");
            animations[currentAnimation].PauseAt(animations[currentAnimation].numberOfFrames - 1);
            timeStamps["DEATH"] = Game1.Time;
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (this == Game1.entities[i] || i >= Game1.entities.Count)
                {
                    continue;
                }
                if (Game1.entities[i] is Tile)
                {
                    Tile tile = (Tile)Game1.entities[i];
                    if (!tile.foreground && !tile.hazard)
                    {
                        HelperMethods.environmentCollisions(this, tile);
                        if(timeStamps["isCollidingX"] == 1)
                        {
                            movement.velocity.X = 0;
                            timeStamps.Remove("ROLL");
                            Die();
                        }
                    }
                }

                if(Game1.entities[i] is RockWall)
                {
                    if (HelperMethods.pixelCollision(this, Game1.entities[i]) && !Game1.entities[i].timeStamps.ContainsKey("BREAK"))
                    {
                        if (HelperMethods.angleToObject(this, Game1.entities[i]) > 30 && HelperMethods.angleToObject(this, Game1.entities[i]) < 150)
                        {
                            movement.velocity.Y = 7;
                            movement.velocity.X -= 2;
                        }
                        else
                        {
                            movement.velocity.X = 0;
                            timeStamps.Remove("ROLL");
                            PlayAnimation("IDLE");
                        }
                    }
                }
            }

            if(!timeStamps.ContainsKey("FROZEN"))
            {
                if (timeStamps.ContainsKey("ROLLSTART"))
                {
                    if (Game1.Time == timeStamps["ROLLSTART"])
                    {
                        Roll();
                    }
                }
                else if (timeStamps.ContainsKey("ROLL"))
                {
                    if (Game1.entities[0].hitBox.X < hitBox.X)
                    {
                        if (movement.velocity.X < -4)
                        {
                            movement.acceleration.X = 0;
                            movement.velocity.X = -4;
                        }
                    }
                    else
                    {
                        if (movement.velocity.X > 4)
                        {
                            movement.acceleration.X = 0;
                            movement.velocity.X = 4;
                        }
                    }

                    if (hitBox.Intersects(Game1.entities[0].hitBox))
                    {
                        //if (Game1.entities[0].rect.X < rect.X)
                        //{
                        //    movement.velocity.X = -3;
                        //}
                        //else
                        //{
                        //    movement.velocity.X = 3;
                        //}

                        Player temp = (Player)Game1.entities[0];
                        if (temp.timeStamps["isCollidingX"] == 1 || temp.timeStamps.ContainsKey("IMMOBILIZE"))
                        {
                            temp.motion.position.X -= movement.velocity.X;
                            movement.velocity.X = 0;
                            timeStamps.Remove("ROLL");
                            Die();
                            temp.takeDamage(5);
                        }
                    }
                }
                else if (Math.Abs(Game1.entities[0].rect.X - rect.X) < 400 && HelperMethods.linearDistanceY(this, Game1.entities[0]) < 70 && !timeStamps.ContainsKey("DEATH"))
                {
                    timeStamps["INRANGE"] += 1;
                    if (timeStamps["INRANGE"] >= 60)
                    {
                        PlayAnimation("ROLLSTART");
                        timeStamps["ROLLSTART"] = Game1.Time + animations["ROLLSTART"].duration;
                    }
                }

                if (timeStamps["isCollidingX"] == 1)
                {
                    movement.velocity.X = 0;
                    timeStamps.Remove("ROLL");
                    Die();
                }
            }
            

            hitBox.X = movement.updatePosX();
            hitBox.Y = movement.updatePosY();

            rect.X = hitBox.X;
            rect.Y = hitBox.Y;
        }
    }
}
