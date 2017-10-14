using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout_Clone
{
    class Pad
    {
        public Rectangle Rectangle;
        public Texture2D Texture;
        public Vector2 Position;

        public int Width
        {
            get { return Rectangle.Width; }
        }

        public int Height
        {
            get { return Rectangle.Height; }
        }

        public float Speed;

        public void Initialize(Vector2 position, int width, int height, Texture2D texture, float speed)
        {
            Position = position;
            Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
            Texture = texture;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color.White);
        }
    }
}
