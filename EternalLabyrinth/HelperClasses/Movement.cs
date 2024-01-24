using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
namespace EternalLabyrinth
{
    public class Movement
    {
        public bool gravityOn;
        public int boundTop, boundBottom, boundLeft, boundRight;
        public Coordinate position;
        public Coordinate velocity;
        public Coordinate acceleration;


        public Movement(Rectangle rect)
        {
            position = new Coordinate(rect.X, rect.Y);
            velocity = new Coordinate();
            acceleration = new Coordinate();
            boundTop = 0 - 5 * Game1.h;
            boundBottom = 5 * Game1.h;
            boundRight = 0 - 5 * Game1.w;
            boundLeft = 5 * Game1.w;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        //UpdateMethod for motion
        public Coordinate updatePos()
        {
            if(gravityOn)
            {
                acceleration.Y = -0.5;
            }
            if (position.X + velocity.X > boundLeft)
            {
                velocity.X = 0;
                acceleration.X = 0;
            }
            if (position.X + velocity.X < boundRight)
            {
                velocity.X = 0;
                acceleration.X = 0;
            }
            if (position.Y - velocity.Y > boundBottom)
            {
                velocity.Y = 0;
                acceleration.Y = 0;
            }
            if (position.Y - velocity.Y < boundTop)
            {
                velocity.Y = 0;
                acceleration.Y = 0;
            }
            position.X += velocity.X;
            position.Y -= velocity.Y;
            velocity.X += acceleration.X;
            velocity.Y += acceleration.Y;
            return position;
        }

        public int updatePosX()
        {
            position.X += velocity.X;
            velocity.X += acceleration.X;
            return (int)position.X;
        }

        public int futurePosX()
        {
            return (int)(position.X + velocity.X);
        }

        public int updatePosY()
        {
            if (gravityOn)
            {
                acceleration.Y = -0.25;
            }
            position.Y -= velocity.Y;
            velocity.Y += acceleration.Y;
            return (int)position.Y;
        }

        public int futurePosY()
        {
            return (int)(position.Y - velocity.Y);
        }

        public void backTrackX()
        {
            position.X -= velocity.X;
            velocity.X -= acceleration.X;
        }

        public void backTrackY()
        {
            position.Y += velocity.Y;
            velocity.Y -= acceleration.Y;
        }



        //Action functions
        public void stopMovement()
        {
            velocity.X = 0;
            acceleration.X = 0;
            velocity.Y = 0;
            acceleration.Y = 0;
        }




        //Motion setting functions
        public void poistionX(double mag)
        {
            position.X = mag;
        }

        public void positionY(double mag)
        {
            position.Y = mag;
        }

        public void velocityX(double mag)
        {
            velocity.X = mag;
        }

        public void velocityY(double mag)
        {
            velocity.Y = mag;
        }

        public void accelerationX(double mag)
        {
            acceleration.X = mag;
        }

        public void accelerationY(double mag)
        {
            acceleration.Y = mag;
        }

        //public void poistionX(double mag, bool force)
        //{
        //    position.X = mag;
        //    isRestrictingX = false;
        //}

        //public void positionY(double mag, bool force)
        //{
        //    position.Y = mag;
        //    isRestrictingY = false;
        //}

        //public void velocityX(double mag, bool force)
        //{
        //    velocity.X = mag;
        //    isRestrictingX = false;
        //}

        //public void velocityY(double mag, bool force)
        //{
        //    velocity.Y = mag;
        //    isRestrictingY = false;
        //}

        //public void accelerationX(double mag, bool force)
        //{
        //    acceleration.X = mag;
        //    isRestrictingX = false;
        //}

        //public void accelerationY(double mag, bool force)
        //{
        //    acceleration.Y = mag;
        //    isRestrictingY = false;
        //}

        //Coordinate subclass helper
        public class Coordinate
        {
            public double X;
            public double Y;

            public Coordinate()
            {
                X = 0;
                Y = 0;
            }

            public Coordinate(double num)
            {
                X = num;
                Y = num;
            }

            public Coordinate(double x, double y)
            {
                X = x;
                Y = y;
            }

            public int getX()
            {
                return (int)X;
            }

            public int getY()
            {
                return (int)Y;
            }
        }
    }
}
