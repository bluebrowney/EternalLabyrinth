using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using static System.Collections.Generic.Dictionary<string, int>;

namespace EternalLabyrinth
{
    class IceGlazier : Ability
    {
        Player user;

        public IceGlazier(Player user) : base(Game1.textures[T.IceGlazier], new Rectangle(user.rect.X, user.rect.Y, 32, 32), Color.White, user.isFacingRight)
        {
            this.user = user;
            timeStamps = new Dictionary<string, int>();
            timeStamps["USAGE"] = 0;
        }

        public class IceHold : StaticObject
        {
            public Enemy inflicted;

            public IceHold(Enemy inflicted) : base(Game1.textures[T.IceHold], new Rectangle(inflicted.rect.X - 10, inflicted.rect.Y - 10, inflicted.rect.Width + 20, inflicted.rect.Height + 10), new Rectangle(0, 0, 64, 64), Color.White)
            {
                timeStamps = new Dictionary<string, int>();
                this.inflicted = inflicted;
                timeStamps["HOLD"] = Game1.Time + 10 * 60;
            }

            public override void Update()
            {
                base.Update();

                if (timeStamps.ContainsKey("SKULLED"))
                {
                    inflicted.animations[inflicted.currentAnimation].UnPause();
                    inflicted.timeStamps.Remove("FROZEN");
                    if (inflicted is Bouldren)
                    {
                        timeStamps.Remove("ROLL");
                        timeStamps.Remove("ROLLSTART");
                    }
                    Game1.entities.Remove(this);
                }

                if(timeStamps["HOLD"] == Game1.Time)
                {
                    inflicted.animations[inflicted.currentAnimation].UnPause();
                    inflicted.timeStamps.Remove("FROZEN");
                    if (inflicted is Bouldren)
                    {
                        timeStamps.Remove("ROLL");
                        timeStamps.Remove("ROLLSTART");
                    }
                    Game1.entities.Remove(this);
                }
                else
                {
                    inflicted.movement.velocity.X = 0;
                    inflicted.movement.acceleration.X = 0;
                    inflicted.animations[inflicted.currentAnimation].Pause();
                    inflicted.timeStamps["FROZEN"] = 0;

                    string[] keys = new string[inflicted.timeStamps.Count];
                    KeyCollection temp = inflicted.timeStamps.Keys;
                    temp.CopyTo(keys, 0);
                    for(int i = 0; i < inflicted.timeStamps.Count; i ++)
                    {
                        inflicted.timeStamps[keys[i]] += 1;
                    }

                    rect.X = inflicted.rect.X - 10;
                    rect.Y = inflicted.rect.Y - 10;

                    hitBox.X = inflicted.rect.X - 10;
                    hitBox.Y = inflicted.rect.Y - 10;
                }

                for (int i = 0; i < Game1.entities.Count; i++)
                {
                    if (this == Game1.entities[i])
                    {
                        continue;
                    }

                    if (Game1.entities[i] is FlamingBullet || Game1.entities[i] is Flame)
                    {
                        Game1.entities.Remove(this);
                    }
                }
            }
        }

        public class IceThrow : Non_StaticObject, PixelPerfectable, Moveable
        {
            Movement motion;
            Player user;

            public IceThrow(Player user) : base(Game1.textures[T.IceGlazier], new Rectangle(user.rect.X, user.rect.Y, 32, 32), Color.White, user.isFacingRight)
            {
                this.user = user;
                defaultAnimation = "FLYING";
                currentAnimation = "FLYING";
                motion = new Movement(hitBox);
                isFacingRight = user.isFacingRight;

                if (user.isFacingRight)
                {
                    motion.velocity.X = 2;
                    motion.velocity.Y = 2;
                    motion.acceleration.Y = -0.05;
                    motion.position.X += user.hitBox.Width + 10;
                }
                else
                {
                    motion.velocity.X = -2;
                    motion.velocity.Y = 2;
                    motion.acceleration.Y = -0.05;
                    motion.position.X -= 10;
                }

                
            }

            public override void Update()
            {
                base.Update();

                for (int i = 0; i < Game1.entities.Count; i++)
                {
                    if(Game1.entities[i] == this)
                    {
                        continue;
                    }

                    if(Game1.entities[i] is Tile)
                    {
                        Tile temp = (Tile)Game1.entities[i];
                        if (!temp.foreground && HelperMethods.pixelCollision(this, temp))
                        {
                            Game1.entities.Remove(this);
                            user.timeStamps.Remove("ICEGLAZIER");
                        }
                    }
                    else if(Game1.entities[i] is Enemy && !(Game1.entities[i] is Smauken) && !(Game1.entities[i] is Flame))
                    {
                        if(HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            Game1.entities.Remove(this);
                            if (!Game1.entities[i].timeStamps.ContainsKey("FROZEN"))
                            {
                                IceHold temp = new IceHold((Enemy)Game1.entities[i]);
                                Game1.entities.Add(temp);
                            }
                            
                        }
                    }
                    else if(Game1.entities[i] is FlamingBullet)
                    {
                        if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            Game1.entities.Remove(Game1.entities[i]);
                            Game1.entities.Remove(this);
                            user.timeStamps.Remove("ICEGLAZIER");
                        }
                    }
                    else if (Game1.entities[i] is RockWall)
                    {
                        if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            Game1.entities.Remove(this);
                            user.timeStamps.Remove("ICEGLAZIER");
                        }
                    }
                }

                hitBox.X = motion.updatePosX();
                hitBox.Y = motion.updatePosY();

                rect.X = hitBox.X;
                rect.Y = hitBox.Y;
            }

            public override void LoadAnimations()
            {
                animations.Add("FLYING", new Animation(8, 0, 0, 32, 32, 4, text.Width, true, false));
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (!isFacingRight)
                {
                    spriteBatch.Draw(text, rect, currentFrame, col, MathHelper.ToRadians(180 - HelperMethods.angle(motion.velocity.X, motion.velocity.Y)), new Vector2(16), SpriteEffects.FlipHorizontally, 0f);
                }
                else
                {
                    spriteBatch.Draw(text, rect, currentFrame, col, MathHelper.ToRadians(180 - HelperMethods.angle(motion.velocity.X, motion.velocity.Y)), new Vector2(16), SpriteEffects.FlipHorizontally, 0f);
                }
            }

            public Movement getMotion()
            {
                return motion;
            }
        }

        public override void Use()
        {
            if(!timeStamps.ContainsKey("COOLDOWN"))
            {
                IceThrow temp = new IceThrow(user);
                Game1.entities.Add(temp);
                timeStamps["USAGE"] += 1;
                if (timeStamps["USAGE"] == 3)
                {
                    timeStamps["COOLDOWN"] = Game1.Time + 60 * 5;
                }
            }
            else
            {
                if(timeStamps["COOLDOWN"] <= Game1.Time)
                {
                    timeStamps.Remove("COOLDOWN");
                    timeStamps["USAGE"] = 0;
                }
            }
        }
    }
}
