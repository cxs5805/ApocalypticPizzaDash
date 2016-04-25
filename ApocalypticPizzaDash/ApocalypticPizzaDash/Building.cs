using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApocalypticPizzaDash
{
    class Building
    {
        // attributes
        private int type;
        private Rectangle rect;
        private Dictionary<string, Rectangle> hitboxes;
        private Texture2D image;
        private bool hasPizza;

        public Building(int type, Rectangle rect, Texture2D image)
        {
            this.type = type;
            this.rect = rect;
            this.image = image;
            hitboxes = new Dictionary<string, Rectangle>();
            hasPizza = false;
        }

        // properties
        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public Dictionary<string, Rectangle> Hitboxes
        {
            get { return hitboxes; }
            set { hitboxes = value; }
        }

        public Texture2D Image
        {
            get { return image; }
        }

        public bool HasPizza
        {
            get { return hasPizza; }
            set { hasPizza = value; }
        }

        public void SetHitboxes()
        {
            if(type == 0)
            {
                hitboxes.Add("ladder1", new Rectangle(rect.X + 122, rect.Y + 102, 30, 148));
                hitboxes.Add("ladder2", new Rectangle(rect.X + 92, rect.Y + 38, 28, 66));
                hitboxes.Add("platform1", new Rectangle(rect.X + 88, rect.Y + 102, 64, 8));
                hitboxes.Add("roof1", new Rectangle(rect.X + 0, rect.Y + 38, 170, 12));
                hitboxes.Add("door1", new Rectangle(rect.X + 28, rect.Y + 198, 34, 52));
            }
            else if(type == 1)
            {
                hitboxes.Add("ladder1", new Rectangle(rect.X + 88, rect.Y + 84, 30, 208));
                hitboxes.Add("roof1", new Rectangle(rect.X + 0, rect.Y + 84, 140, 12));
                hitboxes.Add("roof2", new Rectangle(rect.X + 62, rect.Y + 0, 186, 12));
                hitboxes.Add("door1", new Rectangle(rect.X + 18, rect.Y + 240, 34, 52));
                hitboxes.Add("door2", new Rectangle(rect.X + 194, rect.Y + 240, 34, 52));
            }
            else if(type == 2)
            {
                hitboxes.Add("ladder1", new Rectangle(rect.X + 32, rect.Y + 102, 30, 148));
                hitboxes.Add("ladder2", new Rectangle(rect.X + 94, rect.Y + 38, 30, 66));
                hitboxes.Add("platform1", new Rectangle(rect.X + 32, rect.Y + 102, 94, 8));
                hitboxes.Add("roof1", new Rectangle(rect.X + 14, rect.Y + 38, 170, 12));
                hitboxes.Add("door1", new Rectangle(rect.X + 124, rect.Y + 198, 34, 52));
            }
            else if(type == 3)
            {
                hitboxes.Add("ladder1", new Rectangle(rect.X + 20, rect.Y + 48, 30, 212));
                hitboxes.Add("ladder2", new Rectangle(rect.X + 196, rect.Y + 48, 30, 212));
                hitboxes.Add("roof1", new Rectangle(rect.X + 2, rect.Y + 48, 250, 12));
                hitboxes.Add("door1", new Rectangle(rect.X + 112, rect.Y + 208, 34, 52));
            }
            else if(type == 4)
            {
                hitboxes.Add("ladder1", new Rectangle(rect.X + 94, rect.Y + 26, 30, 76));
                hitboxes.Add("roof1", new Rectangle(rect.X + 0, rect.Y + 26, 202, 12));
                hitboxes.Add("platform1", new Rectangle(rect.X + 12, rect.Y + 100, 126, 8));
                hitboxes.Add("door1", new Rectangle(rect.X + 28, rect.Y + 50, 34, 52));
            }
            else if(type == 5)
            {
                hitboxes.Add("roof1", new Rectangle(rect.X + 2, rect.Y + 26, 202, 12));
                hitboxes.Add("platform1", new Rectangle(rect.X + 4, rect.Y + 100, 156, 8));
                hitboxes.Add("door1", new Rectangle(rect.X + 110, rect.Y + 50, 34, 52));
            }
        }
    }
}
