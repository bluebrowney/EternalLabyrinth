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
    class Waterne : Enemy, PixelPerfectable, Moveable
    {
        Enemy revived;

        public Waterne(int x, int y, bool iFR) : base(Game1.textures[T.Waterne], new Rectangle(x, y, 32, 32), Color.White, iFR)
        {
            defaultAnimation = "IDLE";
            currentAnimation = "IDLE";

            timeStamps["IDLE"] = 0;
            movement.gravityOn = true;
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(3, 0, 0, 32, 32, 2, text.Width, true, false));
            animations.Add("WALK", new Animation(8, 0, 0, 32, 32, 9, text.Width, true, false));
            animations.Add("FELL", new Animation(6, 9 * 32, 0, 32, 32, 2, text.Width, false, false));
            animations.Add("DEATH", new Animation(7, 0, 32, 32, 32, 5, text.Width, false, false));
            animations.Add("ATTACK", new Animation(5, 32 * 5, 32, 32, 32, 6, text.Width, false, false));
            animations.Add("ABSORB", new Animation(6, 0, 64, 32, 32, 7, text.Width, false, false));
        }

        public void Idle()
        {
            timeStamps.Remove("WALK");
            if (!animations["IDLE"].isPlaying())
            {
                PlayAnimation("IDLE");
            }

            movement.velocity.X = 0;
        }

        public void Walk(Object o)
        {
            if (!timeStamps.ContainsKey("WALK"))
            {
                PlayAnimation("WALK");
            }
            timeStamps["WALK"] = 0;
            if (o.rect.X < rect.X)
            {
                isFacingRight = true;
                movement.velocity.X = -0.5;
            }
            else
            {
                isFacingRight = false;
                movement.velocity.X = 0.5;
            }
        }

        public void Attack()
        {
            PlayAnimation("ATTACK");
            movement.velocity.X = 0;
            timeStamps["ATTACK"] = Game1.Time + animations["ATTACK"].duration;
        }

        public override void Update()
        {
            base.Update();

            if(animations["DEATH"].paused)
            {
                Game1.entities.Remove(this);
            }

            if(!timeStamps.ContainsKey("FROZEN") && !timeStamps.ContainsKey("DEATH"))
            {
                if (timeStamps.ContainsKey("LEAVE"))
                {
                    if (timeStamps["LEAVE"] == Game1.Time)
                    {
                        timeStamps["COOLDOWN"] = Game1.Time + 60 * 40;
                        timeStamps.Remove("LEAVE");
                    }
                }
                if (!timeStamps.ContainsKey("REVIVE"))
                {
                    if (timeStamps.ContainsKey("COOLDOWN") && timeStamps["COOLDOWN"] == Game1.Time)
                    {
                        timeStamps.Remove("COOLDOWN");
                    }

                    if (HelperMethods.linearDistanceX(this, Game1.entities[0]) < 300 && HelperMethods.linearDistanceX(this, Game1.entities[0]) > 20)
                    {
                        timeStamps.Remove("ATTACK");
                        Walk(Game1.entities[0]);
                    }
                    else if (HelperMethods.linearDistanceX(this, Game1.entities[0]) <= 20)
                    {
                        movement.velocity.X = 0;
                        if (Game1.entities[0].rect.Center.X < rect.Center.X)
                        {
                            isFacingRight = true;
                        }
                        else
                        {
                            isFacingRight = false;
                        }
                        timeStamps.Remove("WALK");
                        if (!timeStamps.ContainsKey("ATTACK"))
                        {
                            timeStamps["IDLE"] += 1;
                            Idle();

                            if (timeStamps["IDLE"] % 120 == 0)
                            {
                                timeStamps["IDLE"] += 1;
                                Attack();
                            }
                        }
                        else
                        {
                            if (timeStamps["ATTACK"] == Game1.Time)
                            {
                                timeStamps.Remove("ATTACK");
                                timeStamps.Remove("HIT");
                            }
                        }



                        if (timeStamps.ContainsKey("ATTACK"))
                        {
                            if (HelperMethods.pixelCollision(this, Game1.entities[0]) && !timeStamps.ContainsKey("HIT"))
                            {
                                timeStamps["HIT"] = 0;
                                ((Player)Game1.entities[0]).takeDamage(1);
                            }
                        }
                    }
                    else
                    {
                        timeStamps.Remove("ATTACK");
                        Idle();
                    }
                }
                else
                {
                    if (timeStamps["REVIVE"] == 0)
                    {
                        if (HelperMethods.pixelCollision(this, revived) && !timeStamps.ContainsKey("ABSORB") && !timeStamps.ContainsKey("UNDEAD"))
                        {
                            timeStamps["ABSORB"] = Game1.Time + animations["ABSORB"].duration;
                            PlayAnimation("ABSORB");
                            animations["ABSORB"].PauseAt(animations["ABSORB"].numberOfFrames - 1);
                        }
                        else if (timeStamps.ContainsKey("ABSORB"))
                        {
                            if (Game1.Time == timeStamps["ABSORB"])
                            {
                                revived.col = Color.SkyBlue;
                                revived.PlayAnimation("DEATH");
                                revived.animations["DEATH"].Reverse();
                                revived.timeStamps.Remove("DEATH");
                                if (revived is Skull)
                                {
                                    revived.timeStamps.Remove("AWAKENING");
                                }
                                revived.animations["DEATH"].currentFrame = animations["DEATH"].numberOfFrames - 1;
                                revived.animations["DEATH"].PauseAt(-1);
                                revived.animations["DEATH"].UnPause();
                                timeStamps["UNDEAD"] = Game1.Time + revived.animations["DEATH"].duration;
                                timeStamps["REVIVE"] = Game1.Time + 60 * 20;
                                timeStamps.Remove("ABSORB");
                            }
                            movement.velocity.X = 0;
                        }
                        else
                        {
                            Walk(revived);
                        }
                    }
                    else
                    {
                        if (timeStamps.ContainsKey("UNDEAD"))
                        {
                            if (Game1.Time == timeStamps["UNDEAD"])
                            {
                                revived.timeStamps.Remove("DEATH");
                                revived.health = 20;
                            }
                            else if (timeStamps["UNDEAD"] < Game1.Time)
                            {
                                if (revived.timeStamps.ContainsKey("DEATH"))
                                {
                                    revived.col = Color.White;
                                    movement.position.X = revived.movement.position.X;
                                    PlayAnimation("ABSORB");
                                    animations["ABSORB"].Reverse();
                                    animations["ABSORB"].UnPause();
                                    animations["ABSORB"].PauseAt(-1);
                                    timeStamps["LEAVE"] = Game1.Time + animations["ABSORB"].duration;
                                    timeStamps.Remove("REVIVE");
                                    timeStamps.Remove("UNDEAD");
                                    revived = null;
                                }

                                if (timeStamps.ContainsKey("REVIVE") && Game1.Time == timeStamps["REVIVE"])
                                {
                                    revived.col = Color.White;
                                    revived.PlayAnimation("DEATH");
                                    revived.animations["DEATH"].PauseAt(animations["DEATH"].numberOfFrames);
                                    revived.timeStamps["DEATH"] = 0;

                                    movement.position.X = revived.movement.position.X;
                                    PlayAnimation("ABSORB");
                                    animations["ABSORB"].Reverse();
                                    animations["ABSORB"].UnPause();
                                    animations["ABSORB"].PauseAt(-1);
                                    timeStamps["LEAVE"] = Game1.Time + animations["ABSORB"].duration;
                                    timeStamps.Remove("REVIVE");
                                    timeStamps.Remove("UNDEAD");
                                    revived = null;
                                }
                            }
                        }
                    }
                }
            }

            

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (this == Game1.entities[i] || i >= Game1.entities.Count)
                {
                    continue;
                }

                if (!timeStamps.ContainsKey("COOLDOWN") && Game1.entities[i] is Enemy && HelperMethods.linearDistanceX(this, Game1.entities[i]) < 200 && !timeStamps.ContainsKey("REVIVE") && !timeStamps.ContainsKey("LEAVE"))
                {
                    if (!(Game1.entities[i] is Flame) && !(Game1.entities[i] is Smauken) && !(Game1.entities[i] is Waterne))
                    {
                        if (Game1.entities[i].timeStamps.ContainsKey("DEATH"))
                        {
                            timeStamps["REVIVE"] = 0;
                            revived = (Enemy)Game1.entities[i];
                        }
                    }
                }

                if (Game1.entities[i] is Tile)
                {
                    Tile tile = (Tile)Game1.entities[i];
                    if (!tile.foreground)
                    {
                        HelperMethods.environmentCollisions(this, Game1.entities[i]);
                    }
                }

                if(Game1.entities[i] is AttackAbility)
                {
                    if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                    {
                        PlayAnimation("DEATH");
                        timeStamps["DEATH"] = 0;
                        movement.velocity.X = 0;
                        animations[currentAnimation].PauseAt(animations[currentAnimation].numberOfFrames - 1);
                    }
                }

                if (Game1.entities[i] is RockWall)
                {
                    HelperMethods.environmentCollisions(this, Game1.entities[i]);
                }
            }

            hitBox.X = movement.updatePosX();
            hitBox.Y = movement.updatePosY();

            rect.X = hitBox.X;
            rect.Y = hitBox.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(Game1.font, (HelperMethods.linearDistanceX(this, Game1.entities[0])).ToString(), new Vector2(10, 350), Color.White);
        }
    }
}
