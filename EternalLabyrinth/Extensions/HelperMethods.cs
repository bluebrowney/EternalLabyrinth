using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace EternalLabyrinth
{
    public static class HelperMethods
    {
        //Time Helper Methods
        public static int TotalSeconds()
        {
            //return gameTime.TotalGameTime.Seconds + gameTime.TotalGameTime.Minutes * 60 + gameTime.TotalGameTime.Hours * 60 * 60 + gameTime.TotalGameTime.Days * 60 * 60 * 24;
            return Game1.Time / 60;
        }

        //public static int TotalMilliseconds(GameTime gameTime)
        //{
        //    return gameTime.TotalGameTime.Milliseconds + gameTime.TotalGameTime.Seconds * 1000 + gameTime.TotalGameTime.Minutes * 1000 * 60 + gameTime.TotalGameTime.Hours * 1000 * 60 * 60;
        //}

        //Collision Helper Methods
        public static CollidingSide willCollide(Object self, Object other)
        {

            if (self is Moveable)
            {
                Moveable tempSelf = (Moveable)self;
                Movement selfMotion = tempSelf.getMotion();

                if (other is Moveable)
                {
                    Moveable tempOther = (Moveable)other;
                    Movement otherMotion = tempOther.getMotion();

                    self.hitBox.X = selfMotion.futurePosX();
                    other.hitBox.X = otherMotion.futurePosX();
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        if (self.hitBox.Left > other.hitBox.Left)
                        {
                            return CollidingSide.LEFT;
                        }
                        else
                        {
                            return CollidingSide.RIGHT;
                        }
                    }
                    self.hitBox.Y = selfMotion.futurePosY();
                    other.hitBox.Y = otherMotion.futurePosY();
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        if (self.hitBox.Top > other.hitBox.Top)
                        {
                            return CollidingSide.TOP;
                        }
                        else
                        {
                            return CollidingSide.BOTTOM;
                        }
                    }

                    return CollidingSide.NOCOLLISION;
                }
                else
                {
                    //self.hitBox.Y = selfMotion.futurePosY();
                    //if (self.hitBox.Intersects(other.hitBox))
                    //{
                    //    //self.col = Color.Red; //WORKING
                    //    CollisionData coldata = new CollisionData();
                    //    if (self.hitBox.Top < other.hitBox.Top) 
                    //    {
                    //        //self.col = Color.Red;
                    //        coldata.side = CollidingSide.TOP;
                    //    }
                    //    else if(self.hitBox.Bottom > other.hitBox.Bottom)
                    //    {
                    //        //self.col = Color.Blue;
                    //        coldata.side = CollidingSide.BOTTOM;
                    //    }
                    //    coldata.isColliding = true;
                    //    self.hitBox.X = selfMotion.futurePosX();
                    //    return coldata;
                    //}

                    //self.hitBox.X = selfMotion.futurePosX();
                    //if (self.hitBox.Intersects(other.hitBox)) //NOT REACHING IF STATEMENT
                    //{
                    //    CollisionData coldata = new CollisionData();
                    //    self.col = Color.Red;
                    //    if (self.hitBox.Right < other.hitBox.Right)
                    //    {
                    //        self.col = Color.Red;
                    //        coldata.side = CollidingSide.RIGHT;
                    //    }
                    //    else if (self.hitBox.Left > other.hitBox.Left)
                    //    {
                    //        self.col = Color.Blue;
                    //        coldata.side = CollidingSide.LEFT;
                    //    }
                    //    coldata.isColliding = true;
                    //    self.hitBox.Y = selfMotion.futurePosY();
                    //    return coldata;
                    //}

                    //return new CollisionData(CollidingSide.NOCOLLISION, false);

                    self.hitBox.Y = selfMotion.futurePosY();
                    self.hitBox.X = selfMotion.futurePosX();
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        //self.col = Color.Red; //WORKING
                        if (self.hitBox.Top < other.hitBox.Bottom || self.hitBox.Bottom > other.hitBox.Top)
                        {
                            if (self.hitBox.Top < other.hitBox.Top)
                            {
                                //self.col = Color.Red;
                                return CollidingSide.TOP;
                            }
                            else if (self.hitBox.Bottom > other.hitBox.Bottom)
                            {
                                //self.col = Color.Blue;
                                return CollidingSide.BOTTOM;
                            }
                        }
                        // ------------------------------------------------------------------ONLY PARTIALLY WORKING, TALK TO ZAAIM, PAUL WORK ONLY ------------------------------------------------------------------------------------
                        //else if(self.hitBox.Right > other.hitBox.Left || self.hitBox.Left < other.hitBox.Right)
                        //{
                        //    if (self.hitBox.Right < other.hitBox.Right)
                        //    {
                        //        self.col = Color.Red;
                        //        coldata.side = CollidingSide.RIGHT;
                        //    }
                        //    else if (self.hitBox.Left > other.hitBox.Left)
                        //    {
                        //        self.col = Color.Blue;
                        //        coldata.side = CollidingSide.LEFT;
                        //    }
                        //}
                    }
                    return CollidingSide.NOCOLLISION;
                }
            }
            else
            {
                if (other is Moveable)
                {
                    Moveable tempOther = (Moveable)other;
                    Movement otherMotion = tempOther.getMotion();

                    other.hitBox.X = otherMotion.futurePosX();
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        //self.col = Color.Red;
                        if (self.hitBox.Left > other.hitBox.Left)
                        {
                            return CollidingSide.LEFT;
                        }
                        else
                        {
                            return CollidingSide.RIGHT;
                        }
                    }
                    other.hitBox.Y = otherMotion.futurePosY();
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        //self.col = Color.Red;
                        if (self.hitBox.Top > other.hitBox.Top)
                        {
                            return CollidingSide.TOP;
                        }
                        else
                        {
                            return CollidingSide.BOTTOM;
                        }
                    }

                    return CollidingSide.NOCOLLISION;
                }
                else
                {
                    if (self.hitBox.Intersects(other.hitBox))
                    {
                        return CollidingSide.NOCOLLISION;
                    }
                    else
                    {
                        return CollidingSide.NOCOLLISION;
                    }
                }
            }
        }

        //Checks if colliding pixels
        public static bool pixelCollision(Object self, Object other)
        {
            if (other is PixelPerfectable)
            {
                if (self.rect.Intersects(other.rect))
                {
                    PixelPerfectable tempS = (PixelPerfectable)self;
                    PixelPerfectable tempO = (PixelPerfectable)other;

                    Color[] colorDataS = tempS.getColorData();
                    Color[] colorDataO = tempO.getColorData();

                    int top, bottom, left, right;

                    top = Math.Max(self.rect.Top, other.rect.Top);
                    bottom = Math.Min(self.rect.Bottom, other.rect.Bottom);
                    left = Math.Max(self.rect.Left, other.rect.Left);
                    right = Math.Min(self.rect.Right, other.rect.Right);
                    

                    for (int y = top; y < bottom; y++)
                    {
                        for (int x = left; x < right; x++)
                        {
                            int num1 = (y - self.rect.Top) * (self.rect.Width) + (x - self.rect.Left);
                            int num2 = (y - other.rect.Top) * (other.rect.Width) + (x - other.rect.Left);

                            Color S = new Color(0, 0, 0, 255);
                            Color O = new Color(0, 0, 0, 255);

                            if (num1 < colorDataS.Length)
                                S = colorDataS[num1];
                            if (num2 < colorDataO.Length)
                                O = colorDataO[num2];
                            //Game1.testData[(y - other.rect.Top) * (other.rect.Width) + (x - other.rect.Left)] = colorDataO[(y - other.rect.Top) * (other.rect.Width) + (x - other.rect.Left)];

                            if (S.A == 255 && O.A == 255)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        
        }


        //Checks For Player Movement
        public static bool environmentCollisions(Player player, Object collider)
        {
            bool ans = false;
            //Checking X Movement
            Rectangle dummyHitbox = player.hitBox;
            //if(dummyHitbox.Intersects(collider.hitBox))
            //{
            //    player.motion.position.Y = collider.hitBox.Top + player.hitBox.Height;
            //    return true;
            //}
            dummyHitbox.X = player.motion.futurePosX();
            if(dummyHitbox.Intersects(collider.hitBox))
            {
                player.motion.velocityX(0);
                if (!player.timeStamps.ContainsKey("isCollidingX"))
                {
                    player.timeStamps["isCollidingX"] = 1;
                }
                else
                {
                    player.timeStamps["isCollidingX"] = 1;
                }
                ans = true;
            }

            //Checking Y Movement
            dummyHitbox = player.hitBox;
            dummyHitbox.Y = player.motion.futurePosY();
            if (dummyHitbox.Intersects(collider.hitBox))
            {
                if(!player.timeStamps.ContainsKey("isCollidingY"))
                {
                    player.timeStamps["isCollidingY"] = 1;
                }
                else
                {
                    player.timeStamps["isCollidingY"] = 1;
                }

                player.motion.velocityY(0);
                ans = true;
            }

            return ans;
        }

        //Checks For Player Movement
        public static bool environmentCollisions(Enemy enemy, Object collider)
        {
            if(collider is Tile)
            {
                if (((Tile)collider).hazard)
                    return false;
            }

            bool ans = false;
            Rectangle dummyHitboxX = enemy.hitBox;
            Rectangle dummyHitboxY = enemy.hitBox;
            //Checking Y Movement
            dummyHitboxY = enemy.hitBox;
            dummyHitboxY.Y = enemy.movement.futurePosY();
            if (dummyHitboxY.Intersects(collider.hitBox))
            {
                if (!enemy.timeStamps.ContainsKey("isCollidingY"))
                {
                    enemy.timeStamps["isCollidingY"] = 1;
                }
                else
                {
                    enemy.timeStamps["isCollidingY"] = 1;
                }
                enemy.movement.velocityY(0);
                ans = true;
            }

            //Checking X Movement
            dummyHitboxX = enemy.hitBox;
            dummyHitboxX.X = enemy.movement.futurePosX();
            if (dummyHitboxX.Intersects(collider.hitBox))
            {
                if (!enemy.timeStamps.ContainsKey("isCollidingX"))
                {
                    enemy.timeStamps["isCollidingX"] = 1;
                }
                else
                {
                    enemy.timeStamps["isCollidingX"] = 1;
                }
                enemy.movement.velocityX(0);
                ans = true;
            }

            return ans;
        }

        //USE LATER FOR ANGLE CALCULATIONS
        public static int angleToObject(Object self, Object other)
        {
            double dx = (other.rect.X + other.rect.Width / 2) - (self.rect.X + self.rect.Width / 2);
            double dy = (self.rect.Y + self.rect.Height / 2) - (other.rect.Y + other.rect.Height / 2);
            double angle = 0;
            angle = MathHelper.ToDegrees((float)Math.Atan2(dy, dx));
            if (angle < 0)
            {
                angle = 360 + angle;
            }
            if (angle > 45 && angle < 135)
            {
            }
            else if (angle >= 135 && angle <= 225)
            {
            }
            else if (angle > 225 && angle < 315)
            {
            }

            return (int)angle;
        }

        public static int angleToObject(Object self, Object other, int displaceFromCenter)
        {
            double dx = (other.hitBox.Center.X) - (self.hitBox.Center.X);
            double dy = (self.hitBox.Center.Y) - (other.hitBox.Center.Y - displaceFromCenter);
            double angle = 0;
            angle = MathHelper.ToDegrees((float)Math.Atan2(dy, dx));
            if (angle < 0)
            {
                angle = 360 + angle;
            }
            if (angle > 45 && angle < 135)
            {
            }
            else if (angle >= 135 && angle <= 225)
            {
            }
            else if (angle > 225 && angle < 315)
            {
            }

            return (int)angle;
        }

        public static int angle(double dx, double dy)
        {
            double angle = 0;
            angle = MathHelper.ToDegrees((float)Math.Atan2(dy, dx));
            if (angle < 0)
            {
                angle = 360 + angle;
            }
            if (angle > 45 && angle < 135)
            {
            }
            else if (angle >= 135 && angle <= 225)
            {
            }
            else if (angle > 225 && angle < 315)
            {
            }

            return (int)angle;
        }

        public static double euclidianDistance(Object self, Object other)
        {
            return Math.Sqrt(Math.Abs(Math.Pow(self.rect.Center.X - other.rect.Center.X, 2)) + Math.Abs(Math.Pow(self.rect.Center.Y - other.rect.Center.Y, 2)));
        }

        public static void follow1D(Object self, Object other)
        {
            int otherLocX = (other.rect.Center.X - self.rect.Center.X);
            int otherLocY = (other.rect.Center.Y - self.rect.Center.Y);

            int otherLoc = (int)Math.Sqrt((Math.Pow(otherLocX, 2)) + Math.Pow(otherLocY, 2));
        }

        public static double linearDistanceX(Object self, Object other)
        {
            return Math.Abs(other.rect.Center.X - self.rect.Center.X);
        }

        public static double linearDistanceY(Object self, Object other)
        {
            return Math.Abs(other.rect.Center.Y - self.rect.Center.Y);
        }
    }
}
