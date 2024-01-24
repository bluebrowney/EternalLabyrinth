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
    class Skeltri : Enemy, PixelPerfectable, Moveable
    {
        readonly int width = 64;
        readonly int height = 64;

        Dictionary<string, Ability> abilities;

        public Skeltri(int x, int y, bool iFR) : base(Game1.textures[T.Skeltri], new Rectangle(x, y, 64, 64), Color.White, iFR)
        {
            moveRange = 500;
            attackRange = 300;
            speed = 0.5;
            health = 4;
            hitBox = new Rectangle(rect.X - rect.Width / 4, rect.Y, rect.Width / 2, rect.Height);
            timeStamps = new Dictionary<string, int>();
            movement.gravityOn = true;
            timeStamps["INRANGE"] = 0;
            LoadAbilities();
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(1, 0, 0, 64, 64, 1, text.Width, true, false));
            defaultAnimation = "IDLE";

            animations.Add("WALK", new Animation(1, 6 * 64, 0, 64, 64, 2, text.Width, true, false));

            animations.Add("ATTACKTHROW", new Animation(4, 64, 0, 64, 64, 3, text.Width, false, false));

            animations.Add("ATTACKSTOMP", new Animation(3, 4 * 64, 0, 64, 64, 2, text.Width, false, false));

            animations.Add("DEATH", new Animation(7, 64, 64, 64, 64, 6, text.Width, false, true));
        }

        public override void LoadAbilities()
        {
            abilities = new Dictionary<string, Ability>();

            abilities.Add("SkeltriShard", new SkeltriShard(this));

            abilities.Add("SkeltriStomp", new SkeltriStomp(this));
        }

        public void Die()
        {
            movement.stopMovement();
            PlayAnimation("DEATH");
            animations[currentAnimation].PauseAt(animations[currentAnimation].numberOfFrames - 1);
        }

        public void Idle()
        {
            movement.velocity.X = 0;
            movement.acceleration.X = 0;
            PlayAnimation("IDLE");
        }

        public void Walk()
        {
            if (!animations["WALK"].isPlaying())
                PlayAnimation("WALK");
            if (Game1.entities[0].rect.X > rect.X)
            {
                movement.velocityX(speed);
                setFacingRight(true);
            }
            else
            {
                movement.velocityX(-speed);
                setFacingRight(false);
            }
        }

        public void AttackThrow()
        {
            //What should occur if the enitiy is told to do this action
            PlayAnimation("ATTACKTHROW");
            timeStamps["ATTACKTHROW"] = Game1.Time;
        }

        public void AttackStomp()
        {
            //What should occur if the enitiy is told to do this action
            PlayAnimation("ATTACKSTOMP");
            timeStamps["ATTACKSTOMP"] = Game1.Time;
        }

        public override void Update()
        {
            base.Update();
            
            if (health < 0 && !timeStamps.ContainsKey("DEATH"))
            {
                Die();
                timeStamps["DEATH"] = Game1.Time;
            }
            else if (!timeStamps.ContainsKey("DEATH"))
            {
                if(!timeStamps.ContainsKey("FROZEN")) 
                {
                    int playerLocX = (Game1.entities[0].rect.Center.X - this.rect.Center.X);
                    int playerLocY = (Game1.entities[0].rect.Center.Y - this.rect.Center.Y);

                    playerLoc = (int)Math.Sqrt((Math.Pow(playerLocX, 2)) + Math.Pow(playerLocY, 2));

                    if (Math.Abs(playerLoc) < attackRange)
                    {
                        if (playerLocX < 0)
                        {
                            setFacingRight(false);
                        }
                        else
                        {
                            setFacingRight(true);
                        }

                        timeStamps["INRANGE"] += 1;
                        if (timeStamps["INRANGE"] % 120 == 0)
                        {
                            int choice = Game1.rand.Next(0, 100);
                            if (choice < 20 && !timeStamps.ContainsKey("ATTACKSTOMP"))
                            {
                                AttackStomp();
                            }
                            else
                            {
                                AttackThrow();
                            }
                        }
                        else if (!timeStamps.ContainsKey("ATTACKTHROW") && !timeStamps.ContainsKey("ATTACKSTOMP"))
                        {
                            Idle();
                        }

                        if (timeStamps.ContainsKey("ATTACKSTOMP") && Game1.Time - timeStamps["ATTACKSTOMP"] == animations["ATTACKSTOMP"].duration)
                        {
                            abilities["SkeltriStomp"].Use();
                            timeStamps.Remove("ATTACKSTOMP");
                        }

                        if (timeStamps.ContainsKey("ATTACKTHROW") && Game1.Time - timeStamps["ATTACKTHROW"] == animations["ATTACKTHROW"].duration / 2)
                        {
                            abilities["SkeltriShard"].Use();
                        }
                        else if (timeStamps.ContainsKey("ATTACKTHROW") && Game1.Time - timeStamps["ATTACKTHROW"] == animations["ATTACKTHROW"].duration)
                        {
                            timeStamps.Remove("ATTACKTHROW");
                        }

                    }
                    else if (playerLoc < moveRange)
                    {
                        timeStamps.Remove("ATTACKSTOMP");
                        timeStamps.Remove("ATTACKTHROW");
                        Walk();
                    }
                    else
                    {
                        Idle();
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
                    Tile tile = (Tile)Game1.entities[i];
                    if (!tile.foreground)
                    {
                        HelperMethods.environmentCollisions(this, tile);
                    }
                }
                if (Game1.entities[i] is RockWall)
                {
                    if (HelperMethods.pixelCollision(this, Game1.entities[i]) && !Game1.entities[i].timeStamps.ContainsKey("BREAK"))
                    {
                        if (HelperMethods.angleToObject(this, Game1.entities[i]) > 40 && HelperMethods.angleToObject(this, Game1.entities[i]) < 140)
                        {
                            movement.velocity.Y = 7;
                            movement.velocity.X -= 2;
                        }
                    }
                }
            }

            hitBox.X = movement.updatePosX();
            hitBox.Y = movement.updatePosY();

            rect.X = hitBox.X - rect.Width / 4;
            rect.Y = hitBox.Y;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isFacingRight)
            {
                spriteBatch.Draw(text, rect, currentFrame, col, 0, new Vector2(0), SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(text, rect, currentFrame, col, 0, new Vector2(0), SpriteEffects.FlipHorizontally, 0f);
            }
            //spriteBatch.Draw(Game1.white, hitBox, Color.Green * 0.2f);
        }
    }
}
