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
    class FlamingBullet : Ability, PixelPerfectable
    {
        public class Bullet : Non_StaticObject, Moveable, PixelPerfectable
        {
            Movement motion;
            Flame user;

            public Bullet(Flame user) : base(Game1.textures[T.FlamingBullet], new Rectangle(user.rect.X - 10, user.rect.Y + user.rect.Height / 2, 65, 5), Color.White, user.isFacingRight)
            {
                this.user = user;
                motion = new Movement(hitBox);
                if (isFacingRight)
                {
                    motion.velocity.X = -10;
                }
                else
                {
                    motion.velocity.X = 10;
                }
                defaultAnimation = "FLYING";
                currentAnimation = "FLYING";
                PlayAnimation("FIRE");
                timeStamps["FIRE"] = Game1.Time + animations["FIRE"].duration;
            }

            public override void LoadAnimations()
            {
                animations.Add("FIRE", new Animation(7, 0, 0, 65, 5, 8, text.Width, false, false));
                animations.Add("FLYING", new Animation(7, 65 * 2, 10, 65, 5, 2, text.Width, true, false));
                animations.Add("HIT", new Animation(7, 65, 15, 65, 5, 4, text.Width, false, false));
            }

            public override void Update()
            {
                base.Update();

                for (int i = 0; i < Game1.entities.Count; i++)
                {
                    if (Game1.entities[i] == user || Game1.entities[i] == this || Game1.entities[i] is Enemy)
                    {
                        continue;
                    }

                    if (Game1.entities[i] is Tile)
                    {
                        Tile temp = (Tile)Game1.entities[i];
                        if (temp.foreground)
                        {
                            continue;
                        }
                    }

                    if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                    {
                        if (!timeStamps.ContainsKey("HIT"))
                        {
                            PlayAnimation("HIT");
                            animations["HIT"].PauseAt(animations["HIT"].numberOfFrames - 1);
                            motion.stopMovement();
                            timeStamps["HIT"] = Game1.Time + animations["HIT"].duration;

                            if (Game1.entities[i] is Player)
                            {
                                Player temp = (Player)Game1.entities[0];
                                temp.takeDamage(1);
                                temp.Burn(600, 0.7);
                            }
                        }


                    }
                }

                if (timeStamps.ContainsKey("FIRE") && timeStamps["FIRE"] == Game1.Time)
                {
                    PlayAnimation("FLYING");
                }

                if (timeStamps.ContainsKey("HIT"))
                {
                    if (timeStamps["HIT"] == Game1.Time)
                    {
                        Game1.entities.Remove(this);
                        timeStamps.Remove("HIT");
                    }
                }

                hitBox.X = motion.updatePosX();
                hitBox.Y = motion.updatePosY();

                rect.X = hitBox.X;
                rect.Y = hitBox.Y;
            }

            public Movement getMotion()
            {
                return motion;
            }

            public override void Draw(SpriteBatch spriteBatch)
            {
                if (isFacingRight)
                {
                    spriteBatch.Draw(text, rect, currentFrame, col, 0, new Vector2(0), SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(text, rect, currentFrame, col, 0, new Vector2(0), SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }


        //64 X 5
        Flame user;

        public FlamingBullet(Flame user) : base(Game1.textures[T.FlamingBullet], new Rectangle(user.rect.X, user.rect.Y, 64, 5), Color.White, user.isFacingRight)
        {
            this.user = user;
            animations = new Dictionary<string, Animation>();
        }

        public override void Use()
        {
            Bullet attack = new Bullet(user);
            Game1.entities.Add(attack);
        }
    }
}
