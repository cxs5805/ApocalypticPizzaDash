using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApocalypticPizzaDash
{
    class BossZombie : Character
    {
        // attributes
        private int index;
        public bool isAngry;
        private Random rand;
        private Player player;
        private int speed;
        // design note: for milestone 3, make all frame animation variables attributes and params of constructor

        public BossZombie(Player play, Texture2D image, Rectangle rect, int health, int speed) : base(image, rect, health)
        {
            isAngry = false;
            rand = new Random();
            index = rand.Next(0, 2);
            player = play;
            this.speed = speed;
        }

        /// <summary>
        /// randomizes the bosses' movements
        /// </summary>
        /// <param name="screenWidth"></param>
        public void Move(int screenWidth)
        {
            // moving the boss left
            if (index == 0)
            {
                Dir = Direction.MoveLeft;
                Rect = new Rectangle(Rect.X - speed, Rect.Y, Rect.Width, Rect.Height);
                isAngry = false;

                // when boss hits boundary, it moves right
                if (Rect.X <= 0)
                {
                    Dir = Direction.MoveRight;
                    index = 1;
                }
                else if ((player.Rect.X - Rect.X) <= 400 && (player.Rect.X - Rect.X) >= 100 && player.Rect.Y >= 210)
                {
                    Dir = Direction.MoveRight;
                    index = 1;
                }

                //make boss aggro
                if ((player.Rect.X - Rect.X) <= 500 && player.Rect.Y >= 210)
                {
                    isAngry = true;
                }
            }
            // moving the boss right
            else if (index == 1)
            {
                Dir = Direction.MoveRight;
                Rect = new Rectangle(Rect.X + speed, Rect.Y, Rect.Width, Rect.Height);
                isAngry = false;

                // when boss hits boundary, it moves left
                if (Rect.X + Rect.Width >= screenWidth)
                {
                    Dir = Direction.MoveLeft;
                    index = 0;
                }
                else if ((Rect.X - player.Rect.X) <= 400 && (Rect.X - player.Rect.X) >= 100 && player.Rect.Y >= 210)
                {
                    Dir = Direction.MoveLeft;
                    index = 0;
                }

                //make boss aggro
                if ((Rect.X - player.Rect.X) <= 500 && player.Rect.Y >= 210)
                {
                    isAngry = true;
                }
            }
        }

        /// <summary>
        /// overrides the Collision method such that the boss will not glow red
        /// </summary>
        public override void Collision()
        {
            // upon collision...
            if (isColliding && !wasColliding)
            {
                // zombie's color doesn't change
                Color = Color.White;

                // but health is still decremented
                CurrentHealth--;
            }
        }
    }
}
