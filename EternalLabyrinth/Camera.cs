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
    public class Camera
    {
        public int panTimer;
        public int startX, endX, zoomX;
        public bool moveRight;
        public Vector2 endPos;

        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        private Vector2 center;
        private Viewport viewport;

        private float zoom = 1.8f;
        private float rotation = 0;

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void Update(Vector2 position)
        {

            if (Game1.pan && Game1.gState != Game1.GameState.START)
            {
                if (moveRight)
                {
                    endX = 120 * 32 - 640;
                    if (Zoom > 1.01f)
                    {
                        Zoom -= .01f;
                    }
                    else if (zoomX == 0)
                    {
                        zoomX = (int)center.X;
                    }
                    if (startX + 7 < endX)
                    {
                        startX += 7;
                        center = new Vector2(startX, Game1.h / 2);
                    }
                    else
                    {
                        moveRight = false;
                        center = new Vector2(endX, Game1.h / 2);
                        panTimer = Game1.Time;
                    }

                }
                else
                {
                    if (Game1.Time - panTimer > 90)
                    {
                        Game1.pan = false;
                        Zoom = 1.8f;
                        zoomX = 0;
                    }
                }
                //else
                //{
                //    startX = Game1.w / 2;
                //    if (Zoom < 1.81f && endX <= startX + 500)
                //    {
                //        Zoom += .01f;
                //    }
                //    if (endX - 7 > startX)
                //    {
                //        endX -= 7;
                //        center = new Vector2(endX, Game1.h / 2);
                //    }
                //    else
                //    {
                //        center = new Vector2(startX, Game1.h / 2);
                //        Game1.pan = false;
                //        Zoom = 1.8f;
                //        zoomX = 0;
                //    }
                //}


            }
            else
            {
                center = new Vector2(position.X, position.Y);

                if (center.X < 356)
                {
                    center.X = 356;
                }
                else if (center.X > 925)
                {
                    center.X = 925;
                }

                if (center.Y < 180)
                {
                    center.Y = 180;
                }
                else if (center.Y > 460)
                {
                    center.Y = 460;
                }
            }
            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                                                             Matrix.CreateRotationZ(Rotation) *
                                                             Matrix.CreateScale(Zoom) *
                                                             Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }

    }
}
