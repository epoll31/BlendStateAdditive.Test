using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BlendStateAdditive.Test
{
    public class Ball
    {
        public static Texture2D Image { get; set; }

        private Vector2 position;
        public ref Vector2 Position => ref position;

        public Color Color { get; private set; }

        public float Rotation { get; private set; }
        public float Speed { get; private set; }

        public Ball(Vector2 position, Color color, float rotation, float speed)
        {
            Position = position;
            Color = color;
            Rotation = rotation;
            Speed = speed;
        }

        public void Update(Viewport viewport)
        {
            if (Position.X < -Image.Width || Position.X - Image.Width > viewport.Width)
            {
                Rotation = (float)Math.PI - Rotation;
            }
            if (Position.Y < -Image.Height || Position.Y - Image.Height > viewport.Height)
            {
                Rotation *= -1;
            }

            Position.X += Speed * (float)Math.Cos(Rotation);
            Position.Y += Speed * (float)Math.Sin(Rotation);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Color);
        }
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ball[] balls;
        Random random;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 4000;
            //graphics.PreferredBackBufferHeight = 4000;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Ball.Image = Content.Load<Texture2D>("TransparentCircle");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            random = new Random();
            balls = new Ball[1500];
            for (int i = 0; i < balls.Length; i++)
            {
                Color color;
                if (i % 3 == 0)
                {
                    color = Color.Red;
                }
                else if (i % 3 == 1)
                {
                    color = Color.Green;
                }
                else
                {
                    color = Color.Blue;
                }
                //color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

                balls[i] = new Ball(new Vector2(random.Next(0, GraphicsDevice.Viewport.Width - Ball.Image.Width), random.Next(0, GraphicsDevice.Viewport.Width - Ball.Image.Width)), color, (float)(2 * random.NextDouble() * Math.PI), 1 * (float)random.NextDouble() + 1);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (Ball ball in balls)
            {
                ball.Update(GraphicsDevice.Viewport);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //spriteBatch.Begin();

            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
