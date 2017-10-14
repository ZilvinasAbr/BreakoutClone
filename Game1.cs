using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Breakout_Clone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 480;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Block> blocks;
        Pad pad;
        Ball ball;
        List<Wall> walls;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            pad = new Pad();
            blocks = new List<Block>();
            ball = new Ball();
            walls = new List<Wall>();

            base.Initialize(); ;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Texture2D ballTexture = Content.Load<Texture2D>("ball");
            Texture2D padTexture = Content.Load<Texture2D>("pad");
            Texture2D blockTexture = Content.Load<Texture2D>("block");

            ball.Initialize(new Vector2(400-5, 360), 10, ballTexture, 5, new Vector2(1f, 1f));
            pad.Initialize(new Vector2(400-75, 480-20), 150, 10, padTexture, 10);

            Vector2 startingPosition = new Vector2(400-8*42, 60f);

            for(int i = 0; i < 16; i++)
            {
                for(int j = 0; j < 12; j++)
                {
                    Block block = new Block();
                    block.Initialize(blockTexture, new Vector2(startingPosition.X+i*42, startingPosition.Y+j*22), 40, 20);
                    blocks.Add(block);
                }
            }

            Wall upper = new Wall();
            Wall left = new Wall();
            Wall right = new Wall();
            upper.Initialize(new Rectangle(0, -10, WINDOW_WIDTH, 10));
            left.Initialize(new Rectangle(-10, 0, 10, WINDOW_HEIGHT));
            right.Initialize(new Rectangle(WINDOW_WIDTH, 0, 10, WINDOW_HEIGHT));
            walls.Add(upper);
            walls.Add(left);
            walls.Add(right);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            UpdatePlayer(gameTime);
            ball.Update(gameTime);
            UpdateBlocks(gameTime);
            UpdateCollision();

            base.Update(gameTime);
        }

        protected void UpdatePlayer(GameTime gameTime)
        {
            pad.Update(gameTime);

            //Use the Keyboard/Dpad
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (!ball.Active)
                    ball.Active = true;
                pad.Position.X -= pad.Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (!ball.Active)
                    ball.Active = true;
                pad.Position.X += pad.Speed;
            }

            pad.Position.X = MathHelper.Clamp(pad.Position.X, 0, GraphicsDevice.Viewport.Width - pad.Width);
        }

        protected void UpdateBlocks(GameTime gameTime)
        {
            for(int i=blocks.Count -1; i>= 0; i--)
            {
                blocks[i].Update(gameTime);
                if(blocks[i].Active == false)
                {
                    blocks.RemoveAt(i);
                }

            }
        }

        protected void UpdateCollision()
        {
            var intersectionPoint = new Point();
            if (RectangleIntersectsCircle(ball.Position, ball.Radius, pad.Rectangle, out intersectionPoint))
            {
                float ratio = (intersectionPoint.X - pad.Rectangle.X) / (float)(pad.Rectangle.Width);
                float radians = (float)System.Math.PI * ratio;
                
                float movementX = (float)System.Math.Cos(radians);
                float movementY = (float)System.Math.Sin(radians);
                ball.MovementDirection.X = -movementX;
                ball.MovementDirection.Y = -movementY;
                //ball.MovementDirection = getMovementDirection(pad.Rectangle, intersectionPoint, ball.MovementDirection);
                //ball.MovementDirection.X *= -1;
                //ball.MovementDirection.Y *= -1;
            }

            foreach(Block b in blocks)
            {
                if (RectangleIntersectsCircle(ball.Position, ball.Radius, b.Rectangle, out intersectionPoint))
                {
                    ball.MovementDirection = getMovementDirection(b.Rectangle, intersectionPoint, ball.MovementDirection);
                    //ball.MovementDirection.X *= -1;
                    //ball.MovementDirection.Y *= -1;
                    b.Active = false;
                    break;//only one intersection per time unit
                }
            }

            foreach(Wall w in walls)
            {
                if(RectangleIntersectsCircle(ball.Position,ball.Radius,w.Rectangle, out intersectionPoint))
                {
                    ball.MovementDirection = getMovementDirection(w.Rectangle, intersectionPoint, ball.MovementDirection);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach(Block b in blocks)
            {
                b.Draw(spriteBatch);
            }
            pad.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected bool RectangleIntersectsCircle(Vector2 circlePos,
            float circleRadius, Rectangle rect, out Point intersectionPoint)
        {
            // clamp(value, min, max) - limits value to the range min..max

            // Find the closest point to the circle within the rectangle
            
            float closestX = MathHelper.Clamp(circlePos.X, rect.X, rect.X+rect.Width);
            float closestY = MathHelper.Clamp(circlePos.Y, rect.Y, rect.Y+rect.Height);// rect.Y-rect.Height/2 and rect.Y+rect.Height-rect.Height/2, because somehow it detects collision somehow in offset

            // Calculate the distance between the circle's center and this closest point
            float distanceX = circlePos.X - closestX;
            float distanceY = circlePos.Y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            if(distanceSquared < (circleRadius * circleRadius))
            {
                intersectionPoint = new Point((int)closestX, (int)closestY);
                return true;
            }
            intersectionPoint = new Point();
            return false;
        }

        protected Vector2 getMovementDirection(Rectangle rect, Point point, Vector2 movementDirection)
        {
            Vector2 result = movementDirection;
            if((point.X == rect.X && point.Y == rect.Y) || 
               (point.X == rect.X && point.Y == rect.Y+rect.Height) ||
               (point.X == rect.X+rect.Width && point.Y == rect.Y) ||
               (point.X == rect.X+rect.Width && point.Y == rect.Y+rect.Height))
            {
                result.X *= -1;
                result.Y *= -1;
                return result;
            }
            if (point.X == rect.X || point.X == rect.X + rect.Width)
            {
                result.X *= -1;
                return result;
            }else if(point.Y == rect.Y || point.Y == rect.Y + rect.Height)
            {
                result.Y *= -1;
                return result;
            }
            return result;
        }

        /*protected bool RectangleIntersectsCircle(Vector2 circlePos,
            float circleRadius, Rectangle rect)
        {
            Vector2 circleDistance = new Vector2();
            circleDistance.X = System.Math.Abs(circlePos.X - rect.X);
            circleDistance.Y = System.Math.Abs(circlePos.Y - rect.Y);

            if (circleDistance.X > (rect.Width / 2 + circleRadius)) { return false; }
            if (circleDistance.Y > (rect.Height / 2 + circleRadius)) { return false; }

            if (circleDistance.X <= (rect.Width / 2)) { return true; }
            if (circleDistance.Y <= (rect.Height / 2)) { return true; }

            float cornerDistance_sq = (circleDistance.X - rect.Width / 2)*
                (circleDistance.X - rect.Width / 2) +
                (circleDistance.Y - rect.Height / 2)*
                (circleDistance.Y - rect.Height / 2);

            return (cornerDistance_sq <= (circleRadius*circleRadius));
        }*/
    }
}
