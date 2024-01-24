using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace EternalLabyrinth
{
    class RockWall : Ability, PixelPerfectable, Moveable
    {
        Player user;

        public RockWall(Player user) : base(Game1.textures[T.RockWall], new Rectangle(user.rect.X, user.rect.Y, 32, 70), Color.White, user.isFacingRight)
        {
            this.user = user;
            currentAnimation = "USING";
            defaultAnimation = "USING";
            motion = new Movement(hitBox);
        }

        public override void LoadAnimations()
        {
            animations.Add("EMERGE", new Animation(10, 0, 0, 32, 70, 6, text.Width, false, false));
            animations.Add("USING", new Animation(10, 32 * 5, 0, 32, 70, 1, text.Width, true, false));
            animations.Add("BREAK", new Animation(10, 0, 70, 32, 70, 8, text.Width, false, false));
        }

        public override void Update()
        {
            base.Update();

            if (timeStamps.ContainsKey("EMERGE") && Game1.Time == timeStamps["EMERGE"])
            {
                PlayAnimation("USING");
                timeStamps["USING"] = 0;
                timeStamps.Remove("EMERGE");
                user.timeStamps.Remove("ROCKWALL");
                motion.velocity.Y = 2;
                motion.gravityOn = true;
            }

            if(timeStamps.ContainsKey("USING") && user.timeStamps.ContainsKey("ATTACK") && !timeStamps.ContainsKey("BLOCKING") && !timeStamps.ContainsKey("MOVEING"))
            {
                if (user.isFacingRight)
                {
                    motion.velocity.X = 3;
                }
                else
                {
                    motion.velocity.X = -3;
                }
                motion.velocity.Y = 0;
                motion.acceleration.Y = -0.01;
                user.timeStamps.Remove("ROCKWALL");
                timeStamps["MOVEING"] = 0;
                motion.gravityOn = false;
            }

            if(timeStamps.ContainsKey("BLOCKING"))
            {
                if(timeStamps["BLOCKING"] == Game1.Time)
                {
                    PlayAnimation("BREAK");
                    //user.timeStamps.Remove("ROCKWALL");
                    timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                }
            }

            if(!timeStamps.ContainsKey("BREAK") && !timeStamps.ContainsKey("EMERGE"))
            {
                for (int i = Game1.entities.Count - 1; i >= 0; i--)
                {
                    if (Game1.entities[i] is Tile)
                    {
                        Tile temp = (Tile)Game1.entities[i];
                        if (!temp.foreground && HelperMethods.pixelCollision(this, temp) && !timeStamps.ContainsKey("BLOCKING"))
                        {
                            if(timeStamps.ContainsKey("MOVEING"))
                            {
                                motion.stopMovement();
                                motion.gravityOn = false;
                                PlayAnimation("BREAK");
                                user.timeStamps.Remove("ROCKWALL");
                                timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                            }
                            else
                            {
                                timeStamps["BLOCKING"] = Game1.Time + 60 * 5;
                                motion.gravityOn = false;
                                motion.stopMovement();
                                user.timeStamps.Remove("ROCKWALL");
                            }
                            
                        }
                    }

                    if (Game1.entities[i] is IceGlazier.IceHold)
                    {
                        if (rect.Intersects(Game1.entities[i].hitBox) && !timeStamps.ContainsKey("BLOCKING"))
                        {
                            if (timeStamps.ContainsKey("MOVEING"))
                            {
                                motion.stopMovement();
                                motion.gravityOn = false;
                                PlayAnimation("BREAK");
                                user.timeStamps.Remove("ROCKWALL");
                                timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                            }
                            else
                            {
                                timeStamps["BLOCKING"] = Game1.Time + 60 * 5;
                                motion.gravityOn = false;
                                motion.stopMovement();
                                user.timeStamps.Remove("ROCKWALL");
                            }

                        }
                    }

                    if (Game1.entities[i] is Enemy && !timeStamps.ContainsKey("BLOCKING"))
                    { 
                        if((Game1.entities[i] is Skull))
                        {
                            if(Game1.entities[i].timeStamps.ContainsKey("AWAKENEING") || Game1.entities[i].timeStamps.ContainsKey("ATTACKING"))
                            {
                                if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                                {
                                    motion.stopMovement();
                                    PlayAnimation("BREAK");
                                    timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                                    timeStamps.Remove("USING");
                                    user.timeStamps.Remove("ROCKWALL");
                                    Enemy temp = (Enemy)Game1.entities[i];
                                    temp.takeDamage(3);
                                }
                            }
                        }
                        else
                        {
                            if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                            {
                                motion.stopMovement();
                                PlayAnimation("BREAK");
                                timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                                timeStamps.Remove("USING");
                                user.timeStamps.Remove("ROCKWALL");
                                Enemy temp = (Enemy)Game1.entities[i];
                                temp.takeDamage(5);
                            }
                        }
                    }
                    if(timeStamps.ContainsKey("BLOCKING"))
                    {
                        if ((Game1.entities[i] is Skull))
                        {
                            if (Game1.entities[i].timeStamps.ContainsKey("AWAKENEING") || Game1.entities[i].timeStamps.ContainsKey("ATTACKING"))
                            {
                                if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                                {
                                    motion.stopMovement();
                                    PlayAnimation("BREAK");
                                    timeStamps["BREAK"] = Game1.Time + animations["BREAK"].duration;
                                    timeStamps.Remove("USING");
                                    user.timeStamps.Remove("ROCKWALL");
                                    Enemy temp = (Enemy)Game1.entities[i];
                                    temp.takeDamage(3);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if(timeStamps.ContainsKey("BREAK") && Game1.Time == timeStamps["BREAK"])
                {
                    timeStamps.Clear();
                    user.timeStamps.Remove("ROCKWALL");
                    timeStamps["COOLDOWN"] = Game1.Time + 5 * 60;
                    Game1.entities.Remove(this);
                }
            }
            

            hitBox.X = motion.updatePosX();
            hitBox.Y = motion.updatePosY();

            rect.X = hitBox.X;
            rect.Y = hitBox.Y;
        }

        public override void Use()
        {
            if(!timeStamps.ContainsKey("COOLDOWN"))
            {
                motion.stopMovement();
                motion.gravityOn = false;
                PlayAnimation("EMERGE");
                timeStamps["EMERGE"] = Game1.Time + animations["EMERGE"].duration;

                if (user.isFacingRight)
                {
                    motion.position.X = user.hitBox.X + user.hitBox.Width + 15;
                }
                else
                {
                    motion.position.X = user.hitBox.X - 15 - user.hitBox.Width;
                }

                motion.position.Y = user.hitBox.Bottom - rect.Height;

                Game1.entities.Add(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //spriteBatch.Draw(Game1.white, hitBox, Color.Green * 0.5f);
        }

        public Movement getMotion()
        {
            return motion;
        }
    }
}
