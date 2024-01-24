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
    class TestEnemy : Enemy, PixelPerfectable
    {
        public TestEnemy(Texture2D t, Rectangle r, Color c, bool iFR, int h, int s) : base(t, r, c, iFR)
        {
            moveRange = 150;
            attackRange = 32;
            speed = 0.4;
        }

        public override void LoadAnimations()
        {
            animations.Add("IDLE", new Animation(3, 0, 0, 64, 64, 13, text.Width, true, false));
            defaultAnimation = "IDLE";

            animations.Add("WALK", new Animation(3, 0, 1 * 64, 64, 64, 8, text.Width, true, false));

            animations.Add("ATTACK", new Animation(3, 0, 3 * 64, 64, 64, 10, text.Width, false, true));
        }

        public override void Update()
        {
            base.Update();

            Movement.Coordinate temp = movement.updatePos();
            rect.X = temp.getX();
            rect.Y = temp.getY();

            hitBox.X = rect.X + 10;
            hitBox.Y = rect.Y + 30;

            for (int i = 0; i < Game1.entities.Count; i++)
            {
                if (this == Game1.entities[i])
                {
                    continue;
                }

                if (health > 0)
                {
                    updateColor(baseColor);
                }
                else
                {
                    movement.velocityX(0);
                    currentAnimation = "IDLE";
                    updateColor(Color.Green);
                }

                if (health > 0)
                {
                    playerLoc = (Game1.entities[0].rect.Center.X - this.rect.Center.X);

                    if (Math.Abs(playerLoc) < attackRange && !timeStamps.ContainsKey("ATTACK"))
                    {
                        movement.velocityX(0);
                        Attack();
                    }
                    else if (!timeStamps.ContainsKey("ATTACK"))
                    {
                        movement.velocityX(0);
                    }
                    else if (Math.Abs(playerLoc) < moveRange)
                    {
                        currentAnimation = "WALK";
                        Walk();
                    }

                    else if (Math.Abs(movement.velocity.X) != 2)
                    {
                        currentAnimation = "IDLE";
                        movement.velocityX(0);
                    }
                    else if (Math.Abs(movement.velocity.X) == 2)
                    {
                        currentAnimation = "IDLE";
                    }

                }
            }
        }

        public void Attack()
        {
            //What should occur if the enitiy is told to do this action
            PlayAnimation("ATTACK");
            if (HelperMethods.pixelCollision(this, Game1.entities[0]))
            {
                dealDamage((Player)Game1.entities[0]);
            }
        }

        public void Walk()
        {
            if (!timeStamps.ContainsKey("WALK"))
                PlayAnimation("WALK");
            if (playerLoc > 0)
            {
                movement.velocityX(speed);
                setFacingRight(true);
            }
            else if (playerLoc < 0)
            {
                movement.velocityX(-speed);
                setFacingRight(false);
            }
        }
    }
}
