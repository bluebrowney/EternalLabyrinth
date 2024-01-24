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
    class Skull : Enemy, PixelPerfectable, Moveable
    {
        //16 x 16
        int damage = 8;
        int angle = 0;

        public Skull(int x, int y) : base(Game1.textures[T.Skull], new Rectangle(x, y, 24, 24), Color.White, true)
        {
            currentAnimation = "IDLE";
            defaultAnimation = "IDLE";

            int side = Game1.rand.Next(0, 2);
            if(side == 0)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }

            movement.gravityOn = true;
            speed = 1;
            timeStamps["INRANGE"] = 0;
            col = Color.DimGray;
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(3, 0, 0, 24, 24, 1, text.Width, true, false));
            animations.Add("AWAKEN", new Animation(3, 24, 0, 24, 24, 3, text.Width, false, false));
            animations.Add("ATTACKING", new Animation(7, 0, 24, 24, 24, 4, text.Width, true, false));
            animations.Add("DEATH", new Animation(10, 0, 48, 24, 24, 5, text.Width, false, false));
            animations.Add("NOTHING", new Animation(8, 24, 72, 24, 24, 1, text.Width, true, false));
        }

        public void Awaken()
        {
            movement.gravityOn = false;
            movement.velocity.Y = 1;
            PlayAnimation("AWAKEN");
            if(Game1.entities[0].rect.X < rect.X)
            {
                isFacingRight = true;
            }
            else
            {
                isFacingRight = false;
            }
            timeStamps["AWAKENING"] = Game1.Time + animations["AWAKEN"].duration;
        }

        public void Attack()
        {
            PlayAnimation("ATTACKING");
            timeStamps["ATTACKING"] = 0;
        }

        public void Die()
        {
            PlayAnimation("DEATH");
            animations[currentAnimation].PauseAt(animations[currentAnimation].numberOfFrames - 1);
            timeStamps["DEATH"] = Game1.Time + animations["DEATH"].duration;
            movement.stopMovement();
            movement.velocity.Y = -0.3;
            movement.acceleration.Y = -0.1;
        }

        public override void Update()
        {
            base.Update();


            if (!timeStamps.ContainsKey("FROZEN"))
            {
                if (!timeStamps.ContainsKey("ATTACKING") && !timeStamps.ContainsKey("DEATH"))
                {
                    if (HelperMethods.euclidianDistance(this, Game1.entities[0]) < 100)
                    {
                        timeStamps["INRANGE"] += 1;
                        if (timeStamps["INRANGE"] % 60 == 0 && !timeStamps.ContainsKey("AWAKENING"))
                        {
                            int awakenChance = Game1.rand.Next(0, 100);
                            if (awakenChance < 50)
                            {
                                Awaken();
                            }
                        }
                    }

                    if (timeStamps.ContainsKey("AWAKENING"))
                    {
                        if (timeStamps["AWAKENING"] == Game1.Time)
                        {
                            Attack();
                        }
                        else
                        {
                            if (col.R + 5 <= 255)
                            {
                                col.R += 5;
                                col.G += 5;
                                col.B += 5;
                            }
                            else if (col.R != 255)
                            {
                                col.R += (byte)(255 - col.R);
                                col.G += (byte)(255 - col.R);
                                col.B += (byte)(255 - col.R);
                            }
                        }
                    }
                }
                else if (timeStamps.ContainsKey("DEATH"))
                {
                    if (animations[currentAnimation].paused)
                    {
                        movement.gravityOn = true;
                    }
                }
                else
                {
                    if (Game1.entities[0].rect.X < rect.X)
                    {
                        isFacingRight = true;
                    }
                    else
                    {
                        isFacingRight = false;
                    }

                    angle = HelperMethods.angleToObject(this, Game1.entities[0]);
                    movement.velocity.X = speed * Math.Cos(MathHelper.ToRadians(angle));
                    movement.velocity.Y = speed * Math.Sin(MathHelper.ToRadians(angle));

                    if (HelperMethods.pixelCollision(this, Game1.entities[0]))
                    {
                        timeStamps.Remove("ATTACKING");
                        ((Player)Game1.entities[0]).takeDamage(damage);
                        Die();
                    }

                }
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
                        if(HelperMethods.environmentCollisions(this, Game1.entities[i]))
                        {
                            if(timeStamps.ContainsKey("DEATH"))
                            {
                                PlayAnimation("NOTHING");
                            }
                        }
                    }
                }

                if (Game1.entities[i] is IceGlazier.IceHold)
                {
                    IceGlazier.IceHold temp = (IceGlazier.IceHold)Game1.entities[i];
                    if(temp.inflicted != this)
                    {
                        if (timeStamps.ContainsKey("ATTACKING") && !timeStamps.ContainsKey("DEATH"))
                        {
                            if (hitBox.Intersects(Game1.entities[i].hitBox))
                            {
                                timeStamps.Remove("ATTACKING");
                                Die();
                                Game1.entities[i].timeStamps["SKULLED"] = 0;
                                continue;
                            }
                        }
                    }
                }

                if (Game1.entities[i] is RockWall)
                {
                    if(timeStamps.ContainsKey("ATTACKING") && !timeStamps.ContainsKey("DEATH"))
                    {
                        if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            timeStamps.Remove("ATTACKING");
                            Die();
                            
                        }
                    }
                }

                if (Game1.entities[i] is AttackAbility)
                {
                    if (timeStamps.ContainsKey("ATTACKING") && !timeStamps.ContainsKey("DEATH"))
                    {
                        if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            timeStamps.Remove("ATTACKING");
                            Die();

                        }
                    }
                }

                if (Game1.entities[i] is Enemy)
                {
                    if(!(Game1.entities[i] is Skull) && !(Game1.entities[i] is Smauken))
                    {
                        if (timeStamps.ContainsKey("ATTACKING"))
                        {
                            if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                            {
                                Enemy temp = (Enemy)Game1.entities[i];
                                if (!temp.timeStamps.ContainsKey("DEATH"))
                                {
                                    timeStamps.Remove("ATTACKING");
                                    temp.takeDamage(1);
                                    Die();
                                }
                            }
                        }
                    }
                }
            }

            hitBox.X = movement.updatePosX();
            hitBox.Y = movement.updatePosY();

            rect.X = hitBox.X;
            rect.Y = hitBox.Y;
        }
    }
}
