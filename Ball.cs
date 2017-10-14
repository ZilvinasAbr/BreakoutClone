using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout_Clone
{
    class Ball
    {
        public Vector2 Position;
        public float Radius;
        public Texture2D Texture;
        public float Speed;
        public Vector2 MovementDirection;

        public bool Active;

        public void Initialize(Vector2 position, float radius, Texture2D texture, float speed, Vector2 movementDirection)
        {
            Position = position;
            Radius = radius;
            Texture = texture;
            Speed = speed;
            MovementDirection = movementDirection;
            MovementDirection.Normalize();
            Active = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                Position.X += Speed * MovementDirection.X/* * (float)gameTime.ElapsedGameTime.TotalSeconds*/;
                Position.Y += Speed * MovementDirection.Y/* * (float)gameTime.ElapsedGameTime.TotalSeconds*/;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)(Position.X - Radius), (int)(Position.Y - Radius), (int)(Radius * 2), (int)(Radius * 2)), Color.White);
        }
    }
}
