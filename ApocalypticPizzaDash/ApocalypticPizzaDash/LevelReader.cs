using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ApocalypticPizzaDash
{
    class LevelReader
    {
        public List<int> readIn(string lv)
        {
            System.IO.BinaryReader level = new System.IO.BinaryReader(System.IO.File.OpenRead(lv));

            List<int> intholder = new List<int>();

            int temp;

            try
            {
                while (true)
                {
                    temp = (level.ReadInt32());

                    intholder.Add(temp);
                }
            }
            catch
            {

            }

            level.Close();
            return intholder;
        }


        public List<Texture2D> makeList(List<int> grey, Texture2D gal, Texture2D bld1, Texture2D bld2, Texture2D zeke1, Texture2D zeke2)
        {
            int i = grey.Count / 3;

            List<Texture2D> objType = new List<Texture2D>();

            while (i > 0)
            {
                switch (grey[(i * 3) - 3])
                {
                    case 0: //bld1
                        objType.Add(bld1);
                        break;
                    case 1: //bld2
                        objType.Add(bld2);
                        break;
                    case 2: //player
                        objType.Add(gal);
                        break;
                    case 3: //zombie1
                        objType.Add(zeke1);
                        break;
                    case 4: //zombie2
                        objType.Add(zeke2);
                        break;
                    default:
                        objType.Add(null);
                        break;
                }

                i--;
            }

            return objType;
        }

        public List<Rectangle> makeRect(List<int> grey)
        {
            List<Rectangle> objectList = new List<Rectangle>();

            int i = 1;
            int height;
            int width;

            while ((i * 3) - 3 < grey.Count)
            {
                switch (grey[(i * 3) - 3])
                {
                    case 0: //bld1
                        height = 250;
                        width = 184;
                        break;
                    case 1: //bld2
                        height = 292;
                        width = 248;
                        break;
                    case 2: //player
                        height = 46;
                        width = 30;
                        break;
                    case 3: //zombie1
                        height = 42;
                        width = 34;
                        break;
                    case 4: //zombie2
                        height = 42;
                        width = 34;
                        break;
                    case 5: //bld3
                        height = 250;
                        width = 184;
                        break;
                    case 6: //bld4
                        height = 260;
                        width = 252;
                        break;
                    case 7: //bld5
                        height = 298;
                        width = 204;
                        break;
                    case 8: //bld6
                        height = 298;
                        width = 204;
                        break;
                    default:
                        height = 0;
                        width = 0;
                        break;
                }

                if (grey[(i * 3) - 3] > 2 && grey[(i * 3) - 3] < 5)
                {
                    objectList.Add(new Rectangle(grey[(i * 3) - 2], grey[(i * 3) - 1] + 4, width, height));
                }
                else
                {
                    objectList.Add(new Rectangle(grey[(i * 3) - 2], grey[(i * 3) - 1], width, height));
                }

                i++;
            }
            return objectList;

        }
    }
}
