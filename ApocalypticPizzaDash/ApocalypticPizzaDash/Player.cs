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
        private bool allDelivered, isDelivering;

        // when player gets hit, he'll be invincible to attack for some time
        private int invincible;
        private int lives;

        // keyboard-state attributes
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        // attacking attributes
        private Rectangle attackBox;

        public Player(Texture2D image, Rectangle rect, int health, int lives):base(image, rect, health)
        {
            isUp = true;
            isClimbing = false;
            allDelivered = false;
            isDelivering = false;
            invincible = 0;
            this.lives = lives;
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

        public bool IsDelivering
        {
            get { return isDelivering; }
            set { isDelivering = value; }
        }

        public Rectangle AttackBox
        {
            get { return attackBox; }
            set { attackBox = value; }
        }
        
        public bool AllDelivered
        {
            get { return allDelivered; }
            set { allDelivered = value; }
        }

        public int Invincible
        {
            get { return invincible; }
            set { invincible = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
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

            isUp = true;

            if (isUp)
            {
                // decrementing by acceleration due to gravity
                ySpeed += GRAVITY;

                // Logic for solid platforms
                Rectangle collision = new Rectangle(Rect.X + 10, Rect.Y + 40, 14, 8); ;

                if(Dir == Direction.FaceLeft || Dir == Direction.MoveLeft)
                {
                    collision = new Rectangle(Rect.X, Rect.Y + 40, 14, 8);
                }
                


                for (int i = 0; i < buildings.Count && !isOnBuilding && !isClimbing; i++)
                {
                    for (int j = 0; j < buildings[i].Hitboxes.Count && !isOnBuilding; j++)
                    {
                        if (buildings[i].Hitboxes.ContainsKey("roof" + j.ToString()))
                        {
                            if (collision.Intersects(buildings[i].Hitboxes["roof" + j.ToString()]) && ySpeed > 0)
                            {
                                Rect = new Rectangle(Rect.X, buildings[i].Hitboxes["roof" + j.ToString()].Y - Rect.Height, Rect.Width, Rect.Height);
                                isOnBuilding = true;
                                ySpeed = 0;
                                isUp = false;
                            }
                        }
                        if(buildings[i].Hitboxes.ContainsKey("platform" + j.ToString()))
                        {
                            if (collision.Intersects(buildings[i].Hitboxes["platform" + j.ToString()]) && ySpeed > 0)
                            {
                                Rect = new Rectangle(Rect.X, buildings[i].Hitboxes["platform" + j.ToString()].Y - Rect.Height, Rect.Width, Rect.Height);
                                isOnBuilding = true;
                                ySpeed = 0;
                                isUp = false;
                            }
                        }
                    }
                }

                if (!isOnBuilding)
                {
                    if (Rect.Y < minHeight)
                    {
                        isUp = true;
                    }
                }

                // if the player is below the ground and falling...
                if (collision.Y + 2 > minHeight && ySpeed > 0)
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

        /// <summary>
        /// Makes the player attack
        /// </summary>
        public bool Attack(KeyboardState kState)
        {
            // assign current keyboard state to param
            kbState = kState;

            // attack by pressing the "J" key while on the ground
            if (SingleKeyPress(Keys.J) && !IsUp && !isClimbing && !isDelivering)
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

        /// <summary>
        /// Makes the player climb up or down a ladder
        /// </summary>
        public bool Climb(KeyboardState kState, Rectangle ladder)
        {
            // climbing controls when at bottom of ladder
            if (kState.IsKeyDown(Keys.W))
            {
                if (Rect.Intersects(ladder))
                {
                    Rect = new Rectangle(ladder.X, Rect.Y - 2, Rect.Width, Rect.Height);
                    isClimbing = true;
                }
                else
                {
                    isUp = true;
                }
            }
            // climb down
            else if (kState.IsKeyDown(Keys.S))
            {
                if (Rect.Height + Rect.Y < ladder.Y + ladder.Height - 2)
                {
                    Rect = new Rectangle(ladder.X, Rect.Y + 2, Rect.Width, Rect.Height);
                    isClimbing = true;
                }
                else
                {
                    isClimbing = false;
                }
                
                // when player reaches ground, don't change Y value anymore
                if(Rect.Y >= 356)
                {
                    isClimbing = false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delivers a pizza at a door
        /// </summary>
        public bool Deliver(KeyboardState kState, Rectangle doorRect)
        {
            if(kState.IsKeyDown(Keys.W))
            {
                Rect = new Rectangle(doorRect.X, Rect.Y, Rect.Width, Rect.Height);
                return true;
            }
            return false;
        }

        public override void Collision()
        {
            if(invincible == 0)
            {
                CurrentHealth--;
                if(IsColliding && !WasColliding)
                {
                    Color = Color.Red;
                }
            }
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
