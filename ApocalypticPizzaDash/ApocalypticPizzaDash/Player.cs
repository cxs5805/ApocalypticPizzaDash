using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApocalypticPizzaDash
{
    class Player : Character
    {

        // jumping attributes
        private const float GRAVITY = 1.3f;
        private const float INITL_JUMP_V = -20f;
        private float ySpeed;
        private bool isUp, isClimbing;

        // when player gets hit, he'll be invincible to attack for some time
        // (to be implemented in milestone 3)
        private bool invincible;

        // keyboard-state attributes
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        // attacking attributes
        private Rectangle attackBox;

        public Player(Texture2D image, Rectangle rect, int health):base(image, rect, health)
        {
            isUp = false;
            isClimbing = false;
        }

        // properties
        public bool IsUp
        {
            get { return isUp; }
            set { isUp = value; }
        }

        public bool IsClimbing
        {
            get { return isClimbing; }
            set { isClimbing = value; }
        }

        public Rectangle AttackBox
        {
            get { return attackBox; }
            set { attackBox = value; }
        }

        /// <summary>
        /// Moves the player
        /// </summary>
        public void Move(KeyboardState kState, int screenWidth, int minHeight, int maxHeight, List<Building> buildings)
        {
            // storing param into attribute for use later
            kbState = kState;

            // press and hold A to move left
            if (kState.IsKeyDown(Keys.A) && kState.IsKeyUp(Keys.D))
            {
                Dir = Direction.MoveLeft;

                // setting leftmost edge of window as boundary
                if(!(Rect.X <= 0))
                {
                    Rect = new Rectangle(Rect.X - 4, Rect.Y, Rect.Width, Rect.Height);
                }
            }
            // press and hold D to move right
            else if (kState.IsKeyDown(Keys.D) && kState.IsKeyUp(Keys.A))
            {
                Dir = Direction.MoveRight;

                // setting rightmost edge of window as another boundary
                if(!(Rect.X + Rect.Width >= screenWidth))
                {
                    Rect = new Rectangle(Rect.X + 4, Rect.Y, Rect.Width, Rect.Height);
                }
            }
            // player faces last direction walked
            else if (kState.IsKeyUp(Keys.A) && kState.IsKeyUp(Keys.D))
            {
                if (Dir == Direction.MoveLeft)
                {
                    Dir = Direction.FaceLeft;
                }
                else if (Dir == Direction.MoveRight)
                {
                    Dir = Direction.FaceRight;
                }
            }
            // if player hits left and right at the same time, face the last direction walked
            else if (kState.IsKeyDown(Keys.A) && kState.IsKeyDown(Keys.D))
            {
                if(Dir == Direction.MoveLeft)
                {
                    Dir = Direction.FaceLeft;
                }
                else if (Dir == Direction.MoveRight)
                {
                    Dir = Direction.FaceRight;
                }
            }
            // Update Velocity
            bool isOnBuilding = false;
            if (isUp)
            {
                // decrementing by acceleration due to gravity
                ySpeed += GRAVITY;

                // Logic for solid platforms
                Rectangle collision = new Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height + 1);
                for (int i = 0; i < buildings.Count && !isOnBuilding; i++)
                {
                    for (int j = 0; j < buildings[i].Hitboxes.Count && !isOnBuilding; j++)
                    {
                        if (buildings[i].Hitboxes.ContainsKey("roof" + j.ToString()))
                        {
                            if (collision.Intersects(buildings[i].Hitboxes["roof" + j.ToString()]) && ySpeed > 0)
                            {
                                Rect = new Rectangle(Rect.X, buildings[i].Hitboxes["roof" + j.ToString()].Y - Rect.Height, Rect.Width, Rect.Height);
                                isOnBuilding = true;
                                isUp = false;
                                ySpeed = 0;
                            }
                        }
                    }
                }

                // if the player is below the ground and falling...
                if (Rect.Y > minHeight && ySpeed > 0)
                {
                    // ...reset the height and ySpeed to default values when grounded
                    Rect = new Rectangle(Rect.X, minHeight, Rect.Width, Rect.Height);
                    isUp = false;
                    ySpeed = 0;
                }
                else
                {
                    // moving player up before falling
                    Rect = new Rectangle(Rect.X, Rect.Y + (int)ySpeed, Rect.Width, Rect.Height);
                }

            }
            if (!isOnBuilding)
            {
                if (Rect.Y < minHeight)
                {
                    isUp = true;
                }
            }

            //jumping controls
            if (SingleKeyPress(Keys.K) && !isUp)
            {
                // when the player first jumps, it starts moving upward with an
                // initial velocity in the opposite direction of gravity
                ySpeed = INITL_JUMP_V;

                // ensuring that the player stays in the air
                isUp = true;
            }
            
            // saving current keyboard state as the previous one
            prevKBState = kbState;
        }

        //player's attack method
        public bool Attack(KeyboardState kState)
        {
            // assign current keyboard state to param
            kbState = kState;

            // attack by pressing the "J" key while on the ground
            if (SingleKeyPress(Keys.J) && !IsUp && !isClimbing)
            {
                // set height and width of attack hitbox
                int hitboxWidth = 18;
                int hitboxHeight = 10;

                // draw hitbox of attack depending on direction player is facing
                if (Dir == Direction.FaceRight || Dir == Direction.MoveRight)
                {
                    AttackBox = new Rectangle(Rect.X + 29, Rect.Y + 26, hitboxWidth, hitboxHeight);
                }
                else if (Dir == Direction.FaceLeft || Dir == Direction.MoveLeft)
                {
                    AttackBox = new Rectangle(Rect.X - (hitboxWidth - 1), Rect.Y + 26, hitboxWidth, hitboxHeight);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Climb(KeyboardState kState, int ladderX)
        {
            // climbing controls
            if (kState.IsKeyDown(Keys.W))
            {
                Rect = new Rectangle(ladderX, Rect.Y - 2, Rect.Width, Rect.Height);
                isClimbing = true;
            }
            else if (kState.IsKeyDown(Keys.S) && isClimbing)
            {
                Rect = new Rectangle(ladderX, Rect.Y + 2, Rect.Width, Rect.Height);
                if(Rect.Y >= 356)
                {
                    isClimbing = false;
                }
                return true;
            }
            return false;
        }

        public bool SingleKeyPress(Keys key)
        {
            // only true if it's the first frame in which the param key is pressed
            if(kbState.IsKeyDown(key) && prevKBState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
