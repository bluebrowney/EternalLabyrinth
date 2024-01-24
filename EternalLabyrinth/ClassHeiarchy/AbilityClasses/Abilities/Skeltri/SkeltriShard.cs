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
    class SkeltriShard : Ability, PixelPerfectable
    {
        //readonly int width = 32;
        //readonly int height = 32;

        public readonly int maxDamage = 3;
        //int duration = 180;

        Skeltri user;

        public SkeltriShard(Skeltri user) : base(Game1.textures[T.SkeltriShard], new Rectangle(0, 0, 32, 32), Color.White, user.isFacingRight)
        {
            this.user = user;
        }

        public class Shard : Non_StaticObject, PixelPerfectable, Moveable
        {
            Movement motion;
            int duration;
            int maxDamage = 3;

            public Shard(Skeltri user) : base(Game1.textures[T.SkeltriShard], new Rectangle(user.rect.X, user.rect.Y, 32, 32), Color.White, !user.isFacingRight)
            {
                motion = new Movement(rect);
                if (!isFacingRight)
                {
                    motion.velocity.X = 3;
                }
                else
                {
                    motion.velocity.X = -3;
                }

                //duration = 0;

                hitBox.X = rect.X;
                hitBox.Y = rect.Y;
                currentAnimation = "FLYING";
                defaultAnimation = "HIT";
            }

            public override void LoadAnimations()
            {
                animations.Add("FLYING", new Animation(5, 0, 0, 32, 32, 3, text.Width, true, false));

                animations.Add("HIT", new Animation(12, 0, 32, 32, 32, 6, text.Width, false, false));
            }

            public override void Update()
            {
                base.Update();

                //duration++;

                rect.X = motion.updatePosX();
                rect.Y = motion.updatePosY();

                hitBox.X = rect.X;
                hitBox.Y = rect.Y;

                if (timeStamps.ContainsKey("HIT") && Game1.Time - timeStamps["HIT"] == animations["HIT"].duration - 5)
                {
                    timeStamps.Remove("HIT");
                    motion.position.X = 0;
                    motion.position.Y = 0;
                    Game1.entities.Remove(this);
                    return;
                }

                for (int i = Game1.entities.Count - 1; i >= 0; i--)
                {
                    if (!timeStamps.ContainsKey("HIT"))
                    {
                        if (Game1.entities[i] is Player)
                        {
                            if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                            {
                                motion.stopMovement();
                                PlayAnimation("HIT");
                                timeStamps["HIT"] = Game1.Time;
                                ((Player)Game1.entities[0]).takeDamage(maxDamage);
                            }
                        }

                        if (duration > 500)
                        {
                            motion.stopMovement();
                            PlayAnimation("HIT");
                            timeStamps["HIT"] = Game1.Time;
                        }

                        if (Game1.entities[i] is Tile)
                        {
                            Tile temp = (Tile)Game1.entities[i];
                            if (!temp.foreground && (this.rect.Intersects(Game1.entities[i].rect)))
                            {
                                motion.stopMovement();
                                PlayAnimation("HIT");
                                timeStamps["HIT"] = Game1.Time;
                            }
                        }

                        if (Game1.entities[i] is RockWall)
                        {
                            if (HelperMethods.pixelCollision(this, Game1.entities[i]))
                            {
                                motion.stopMovement();
                                PlayAnimation("HIT");
                                timeStamps["HIT"] = Game1.Time;
                            }
                        }
                    }
                }


            }

            public Movement getMotion()
            {
                return motion;
            }
        }



        public override void Use()
        {
            Shard temp = new Shard(user);
            temp.PlayAnimation("FLYING");
            temp.timeStamps["FLYING"] = Game1.Time;
            Game1.entities.Add(temp);
        }
    }
}
