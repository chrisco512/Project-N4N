using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NutsForNutsGAME
{
    class ImageLibrary
    {
        private const bool DEBUG_MODE = false;

        private static Texture2D _pixel, _oval;

        public static Color[] DEFAULT_TRANSPARENT_COLORS = {new Color(0, 0, 128), new Color(0, 255, 255)};
        public static Dictionary<string, Texture2D> STORED_IMAGES = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> TEMP_STORED_IMAGES = new Dictionary<string, Texture2D>();


        public static Texture2D DuplicateTexture(GraphicsDevice graphicsDevice, Texture2D texture)
        {
            Texture2D dupe = new Texture2D(graphicsDevice, texture.Width, texture.Height);
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            dupe.SetData(data);
            return dupe;
        }

        public static Texture2D ConvertTransparentPixels(Texture2D texture)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; i++)
                for(int j = 0; j < DEFAULT_TRANSPARENT_COLORS.Length; j++)
                    if (data[i] == DEFAULT_TRANSPARENT_COLORS[j])
                        data[i] = new Color(0, 0, 0, 0);

            texture.SetData(data);

            return texture;
        }
        
        public static Texture2D SwapPalette(Texture2D texture, Color colorFrom, Color colorTo)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; i++)
                if (data[i] == colorFrom)
                    data[i] = colorTo;

            texture.SetData(data);

            return texture;
        }

        public static Texture2D Tint(Texture2D texture, Color color, float ratio)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            
            for (int i = 0; i < data.Length; i++)
                if (data[i].A > 0)
                {
                    int r = (int) (data[i].R * ratio + color.R);
                    int g = (int) (data[i].G * ratio + color.G);
                    int b = (int) (data[i].B * ratio + color.B);

                    data[i] = new Color(r, g, b, data[i].A);
                }

            texture.SetData(data);

            return texture;
        }

        private static Texture2D MakePixel(GraphicsDevice graphicsDevice)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(graphicsDevice, 1, 1);
                _pixel.SetData(new[] { Color.White });
            }

            return _pixel;
        }


        public static void FillRectangle(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Rectangle rect, Color fillColor)
        {
            _pixel = MakePixel(graphicsDevice);
            spriteBatch.Draw(_pixel, rect, fillColor);
        }

        public static void DrawRectangle(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Rectangle rect, Color fillColor)
        {
            DrawRectangle(graphicsDevice, spriteBatch, rect, fillColor, 1, 0f);
        }

        public static void DrawRectangle(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Rectangle rect, Color fillColor, int border, float depth)
        {
            _pixel = MakePixel(graphicsDevice);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, border, rect.Height), null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Right, rect.Top, border, rect.Height), null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Top, rect.Width, border), null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
            spriteBatch.Draw(_pixel, new Rectangle(rect.Left, rect.Bottom, rect.Width, border), null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public static void StoreImage(String id, Texture2D texture, bool bPersistent)
        {
            if (bPersistent)
            {
                if (!STORED_IMAGES.ContainsKey(id))
                    STORED_IMAGES.Add(id, texture);
            }
            else
            {
                if (!TEMP_STORED_IMAGES.ContainsKey(id))
                    TEMP_STORED_IMAGES.Add(id, texture);
            }
        }

        public static Texture2D MakeOval()
        {
            if(_oval == null)
                _oval = ImageLibrary.RetrieveImage("Oval");

            return _oval;
        }

        public static void DrawOval(SpriteBatch spriteBatch, Rectangle rect, Color fillColor)
        {
            DrawOval(spriteBatch, rect, fillColor, 0f);
        }

        public static void DrawOval(SpriteBatch spriteBatch, Rectangle rect, Color fillColor, float depth)
        {
            spriteBatch.Draw(MakeOval(), rect, null, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public static void StoreImage(string id, Texture2D texture)
        {
            StoreImage(id, texture, false);
        }

        public static void UncacheImage(String id)
        {
            if (STORED_IMAGES.ContainsKey(id))
                STORED_IMAGES.Remove(id);

            if (TEMP_STORED_IMAGES.ContainsKey(id))
                TEMP_STORED_IMAGES.Remove(id);
        }

        public static void ClearTempImages()
        {
            if (TEMP_STORED_IMAGES != null)
                TEMP_STORED_IMAGES.Clear();
        }

        public static void Clear()
        {
            if (STORED_IMAGES != null)
                STORED_IMAGES.Clear();

            if (TEMP_STORED_IMAGES != null)
                TEMP_STORED_IMAGES.Clear();
        }

        public static Texture2D RetrieveImage(String id)
        {
            if(STORED_IMAGES.ContainsKey(id))
                return STORED_IMAGES[id];

            if (TEMP_STORED_IMAGES.ContainsKey(id))
                return TEMP_STORED_IMAGES[id];

            return null;
        }

        public static Texture2D GetSubImage(GraphicsDevice graphicsDevice, Texture2D texture, Rectangle rect)
        {
            Texture2D dupe = new Texture2D(graphicsDevice, rect.Width, rect.Height);
            Color[] data = new Color[texture.Width * texture.Height];
            Color[] subData = new Color[rect.Width * rect.Height];
            texture.GetData(data);

            for (int i = 0; i < rect.Width; i++)
                for (int j = 0; j < rect.Height; j++)
                    subData[i + j * rect.Width] = data[i + rect.X + (j + rect.Y) * texture.Width];

            dupe.SetData(subData);
            return dupe;
        }

        /*
        public static Texture2D GetSubImage(GraphicsDevice graphicsDevice, Texture2D texture, Rectangle rect)
        {
            int newWidth = rect.Width + 2;
            int newHeight = rect.Height + 2;
            Texture2D dupe = new Texture2D(graphicsDevice, rect.Width + 2, rect.Height + 2);
            Color[] data = new Color[texture.Width * texture.Height];
            Color[] subData = new Color[newWidth * newHeight];
            texture.GetData(data);

            for (int i = 0; i < rect.Width; i++)
                for (int j = 0; j < rect.Height; j++)
                    subData[(i + 1) + (j + 1) * newWidth] = data[i + rect.X + (j + rect.Y) * texture.Width];

            for (int i = 0; i < newWidth; i++)
                for (int j = 0; j < newHeight; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                            subData[0] = subData[1 + 1 * newWidth];
                        else if (j == newHeight - 1)
                            subData[j * newWidth] = subData[1 + (j - 1) * newWidth];
                        else
                            subData[j * newWidth] = subData[1 + j * newWidth];
                    }
                    else if (j == 0)
                    {
                        if (i == newWidth - 1)
                            subData[i] = subData[i - 1 + 1 * newWidth];
                        else
                            subData[i] = subData[i + 1 * newWidth];
                    }
                    else if (i == newWidth - 1)
                    {
                        if (j == newHeight - 1)
                            subData[i + j * newWidth] = subData[i - 1 + (j - 1) * newWidth];
                        else
                            subData[i + j * newWidth] = subData[i - 1 + j * newWidth];
                    }
                    else if (j == newHeight - 1)
                    {
                        subData[i + j * newWidth] = subData[i + (j - 1) * newWidth];
                    }
                }
            
            dupe.SetData(subData);
            return dupe;
        }
        */

    }
}
