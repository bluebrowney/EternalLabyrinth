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
    //Example of how to use super classes

    //Implementing InputInteractanble allow for entities to be controlled by the player (Forces implementation of DoInputLogic [Check down below])
    //Implemeting Collidable allows for entities to be collidable (WITH PIXEL PERFECT COLLISION) (No forced parameters; however, dont implement for regular collisions)
    public class Player : Non_StaticObject, InputInteractable, PixelPerfectable, Moveable
    {
        //Add Player data e.g health, strength, stamina. then update the constructor accordingly

        public int health;
        public int stamina;
        public int strength;

        public bool advanceLevel;

        public Movement motion;

        private readonly int attackRange = 40;

        Dictionary<string, Ability> abilities;


        public Player(Texture2D t, Rectangle r, Color c, bool iFR) : base(t, r, c, iFR)
        {
            //Setting Starting Aniamtion
            currentAnimation = "IDLE";
            //Default stats for player
            health = 20;
            stamina = 5;
            strength = 2;
            baseColor = Color.White;
            motion = new Movement(rect);
            motion.gravityOn = true;
            hitBox = new Rectangle(r.X + r.Width / 4, r.Y + 5, 51 / 4 * 2, 63 - 5);
            //motion.boundBottom = Game1.FLOOR_HEIGHT+64;

            timeStamps = new Dictionary<string, int>();
            timeStamps["isCollidingY"] = 0;
            LoadAbilities();
        }

        public void LoadAbilities()
        {
            abilities = new Dictionary<string, Ability>();

            abilities.Add("RockWall", new RockWall(this));
            abilities.Add("FireBall", new FireBall(this));
            abilities.Add("IceGlazier", new IceGlazier(this));
            abilities.Add("ElectroCharge", new ElectroCharge(this));
        }

        public override void LoadAnimations()
        {
            //Add Animations (Key, new Animation(FPS, startX, startY, width of frames, height of frames, number of frame, if aniamtion loops, if animatoin restricts))
            animations.Add("IDLE", new Animation(1, 51 * 2, 63, 51, 63, 2, text.Width, true, false));
            defaultAnimation = "IDLE";

            animations.Add("ATTACK", new Animation(7, 3 * 51, 63, 51, 63, 5, text.Width, false, true));

            animations.Add("JUMP", new Animation(1, 51 * 2, 0, 51, 63, 1, text.Width, false, true));

            animations.Add("WALK", new Animation(8, 0, 0, 51, 63, 13, text.Width, true, false));
        }

        //Add public void actions for the entitiy's motion
        public void RockWall()
        {
            if(!abilities["RockWall"].timeStamps.ContainsKey("EMERGE") && !abilities["RockWall"].timeStamps.ContainsKey("USING") && !abilities["RockWall"].timeStamps.ContainsKey("BREAK"))
            {
                if(!abilities["RockWall"].timeStamps.ContainsKey("COOLDOWN"))
                {
                    abilities["RockWall"].Use();
                    timeStamps["ROCKWALL"] = 0;
                    motion.velocity.X = 0;
                    PlayAnimation("IDLE");
                }
            }
        }

        public void IceGlazier()
        {
            abilities["IceGlazier"].Use();
            PlayAnimation("ATTACK");
            timeStamps["ICEGLAZIER"] = Game1.Time;
        }

        public void Attack()
        {
            //What should occur if the enitiy is told to do this action
            PlayAnimation("ATTACK");
            timeStamps["ATTACK"] = Game1.Time;
            timeStamps.Remove("ATTACKED");
        }

        public void Jump()
        {
            PlayAnimation("JUMP");
            animations[currentAnimation].Pause();
            motion.velocityY(8);
        }

        public void Walk()
        {
            if (!animations["WALK"].isPlaying() && timeStamps["isCollidingY"] > 0)
            {
                PlayAnimation("WALK");
            }
            if (isFacingRight)
            {
                if (!timeStamps.ContainsKey("BOULDREN"))
                {
                    motion.velocity.X = 2;
                }
            }
            else
            {
                if (!timeStamps.ContainsKey("BOULDREN"))
                {
                    motion.velocity.X = -2;
                }
            }
        }

        public void Immobilize(int duration)
        {
            PlayAnimation("IDLE");
            motion.stopMovement();
            timeStamps["IMMOBILIZE"] = Game1.Time + duration;
        }

        public void Burn(int duration, double damagePerSecond)
        {
            timeStamps["BURN"] = Game1.Time + duration;
            timeStamps["BURNINGINTERVAL"] = (int)(60 / damagePerSecond);
            timeStamps.Remove("HealthRegen");
        }

        public void Idle()
        {
            if (!currentAnimation.Equals("IDLE"))
            {
                PlayAnimation("IDLE");
            }

            motion.velocity.X = 0;
            motion.acceleration.X = 0;
        }

        public void dealDamage(Enemy e)
        {
            e.takeDamage(strength);
        }

        public void takeDamage(int d)
        {
            health -= d;
            timeStamps.Remove("HealthRegen");
        }

        //Overload update to add calculations specific to entity
        public override void Update()
        {
            base.Update();

            if (health < 20 && !timeStamps.ContainsKey("SpikeDamage") && !timeStamps.ContainsKey("BURN"))
            {
                if (!timeStamps.ContainsKey("HealthRegen"))
                {
                    timeStamps["HealthRegen"] = HelperMethods.TotalSeconds();
                }
                else if (HelperMethods.TotalSeconds() - timeStamps["HealthRegen"] >= 4)
                {
                    timeStamps["HealthRegen"] = HelperMethods.TotalSeconds();
                    health += 1;
                }
            }

            if (timeStamps.ContainsKey("BURN"))
            {
                if (timeStamps["BURN"] == Game1.Time)
                {
                    timeStamps.Remove("BURN");
                }

                if (Game1.Time % timeStamps["BURNINGINTERVAL"] == 0)
                {
                    health -= 1;
                }
            }

            timeStamps["isCollidingY"] = 0;
            timeStamps["isCollidingX"] = 0;

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (health > 0)
                {
                    //this.updateColor(baseColor);
                }
                else
                {
                    //this.updateColor(Color.Green);
                }

                if (this == Game1.entities[i])
                {
                    continue;
                }

                if (Game1.entities[i] is Bouldren)
                {
                    Bouldren temp = (Bouldren)Game1.entities[i];
                    if (temp.timeStamps.ContainsKey("ROLL"))
                    {
                        if (hitBox.Intersects(Game1.entities[i].hitBox) && !temp.timeStamps.ContainsKey("DEATH"))
                        {
                            if (timeStamps["isCollidingX"] != 1)
                            {
                                motion.velocity.X = temp.movement.velocity.X;
                            }
                            timeStamps["BOULDREN"] = 0;
                        }
                        else if (!hitBox.Intersects(Game1.entities[i].hitBox))
                        {
                            timeStamps.Remove("BOULDREN");
                        }
                    }
                    else if (temp.timeStamps.ContainsKey("DEATH") && timeStamps.ContainsKey("BOULDREN"))
                    {
                        timeStamps.Remove("BOULDREN");
                    }
                }

                if (Game1.entities[i] is Tile)
                {
                    Tile temp = (Tile)Game1.entities[i];
                    if (temp.hazard)
                    {
                        if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                        {
                            timeStamps.Remove("HealthRegen");
                            if (!timeStamps.ContainsKey("SpikeDamage"))
                            {
                                timeStamps["SpikeDamage"] = HelperMethods.TotalSeconds();
                            }
                            else if (HelperMethods.TotalSeconds() - timeStamps["SpikeDamage"] == 1)
                            {
                                takeDamage(1);
                                timeStamps["SpikeDamage"] = HelperMethods.TotalSeconds();
                            }
                        }
                        else if (timeStamps.ContainsKey("SpikeDamage") && HelperMethods.TotalSeconds() - timeStamps["SpikeDamage"] >= 2)
                        {
                            timeStamps.Remove("SpikeDamage");
                        }
                    }
                    else if (temp.textString.Equals("death") && this.hitBox.Intersects(Game1.entities[i].rect))
                    {
                        health = 0;
                    }
                    else if (temp.textString.Equals("nextLevel") && this.hitBox.Intersects(Game1.entities[i].rect) && !advanceLevel)
                    {
                        //THIS IS IMPORTANT 
                        //GOES TO NEXT LEVEL 
                        //DO NOT TOUCH
                        advanceLevel = true;
                        //Game1.gState = Game1.GameState.LEVEL2;
                        Game1.gState++;
                    }
                    else if (!temp.textString.Equals("player") && !temp.foreground)
                    {
                        if (HelperMethods.environmentCollisions(this, temp))
                        {
                            animations[currentAnimation].UnPause();
                        }
                    }

                }

                if (Game1.entities[i] is Skeltri)
                {
                    if (timeStamps.ContainsKey("ATTACK"))
                    {
                        if (!timeStamps.ContainsKey("ATTACKED") && HelperMethods.pixelCollision(this, Game1.entities[i]) && !Game1.entities[i].timeStamps.ContainsKey("FROZEN"))
                        {
                            dealDamage((Enemy)Game1.entities[i]);
                            timeStamps["ATTACKED"] = 0;
                        }
                    }
                }

                if(Game1.entities[i] is RockWall)
                {
                    if (!Game1.entities[i].timeStamps.ContainsKey("BREAK"))
                    {
                        if(HelperMethods.environmentCollisions(this, Game1.entities[i]))
                        {
                            animations[currentAnimation].UnPause();
                        }
                    }
                }

                if (Game1.entities[i] is IceGlazier.IceHold)
                {
                    if (HelperMethods.environmentCollisions(this, Game1.entities[i]))
                    {
                        animations[currentAnimation].UnPause();
                    }
                }
            }

            if (timeStamps.ContainsKey("ATTACK"))
            {
                if (Game1.Time - timeStamps["ATTACK"] >= animations["ATTACK"].duration)
                {
                    timeStamps.Remove("ATTACK");
                }
            }

            if (timeStamps.ContainsKey("IMMOBILIZE"))
            {
                if (timeStamps["IMMOBILIZE"] == Game1.Time)
                {
                    timeStamps.Remove("IMMOBILIZE");
                }
            }

            if(abilities["RockWall"].timeStamps.ContainsKey("COOLDOWN") && abilities["RockWall"].timeStamps["COOLDOWN"] <= Game1.Time)
            {
                abilities["RockWall"].timeStamps.Remove("COOLDOWN");
            }

            hitBox.X = motion.updatePosX();
            hitBox.Y = motion.updatePosY();
            rect.X = hitBox.X - rect.Width / 4;
            rect.Y = hitBox.Y - 5;

            //hitBox.X = X;
            //hitBox.Y = Y;
        }

        public Movement getMotion()
        {
            return motion;
        }
        //Logic that should occur based on the users input (called in Game1.cs)
        public void doInputLogic(GameTime gameTime)
        {
            if (Game1.KeyPressed(Keys.D))
            {
                RockWall();
            }

            if (Game1.KeyPressed(Keys.S))
            {
                IceGlazier();
            }


            if (Game1.KeyPressed(Keys.X))
            {
                Attack();
            }

            if (!timeStamps.ContainsKey("IMMOBILIZE"))
            {
                if (Game1.KeyPressed(Keys.Space))
                {
                    if (timeStamps["isCollidingY"] > 0)
                    {
                        Jump();
                    }
                }

                if(!timeStamps.ContainsKey("ROCKWALL"))
                {
                    if (Game1.presIn.Contains(Keys.Right))
                    {
                        if (!timeStamps.ContainsKey("ATTACK"))
                        {
                            Walk();
                        }
                        setFacingRight(true);
                    }


                    if (Game1.presIn.Contains(Keys.Left))
                    {
                        setFacingRight(false);
                        if (!timeStamps.ContainsKey("ATTACK"))
                        {
                            Walk();
                        }
                    }

                    if(!Game1.presIn.Contains(Keys.Right) && !Game1.presIn.Contains(Keys.Left))
                    {
                        motion.velocity.X = 0;
                    }
                }
            }

            if (Game1.presIn.Count == 0 && Game1.oldIn.Count == 0 && timeStamps["isCollidingY"] > 0)
            {
                Idle();
            }
        }
    }
}
