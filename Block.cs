using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout_Clone
{
    class Block
    {
        public Rectangle Rectangle;
        public Texture2D Texture;
        public Vector2 Position
        {
            get { return new Vector2(Rectangle.X, Rectangle.Y); }
        }

        public int Width
        {
            get { return Rectangle.Width; }
        }

        public int Height
        {
            get { return Rectangle.Height; }
        }

        public bool Active;

        public void Initialize(Texture2D texture, Vector2 position, int width, int height)
        {
            Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
            Texture = texture;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
