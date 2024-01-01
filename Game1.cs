using System;
using System.Diagnostics;
using Asteroids3D.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Model playerShip;
        private Texture2D otherTexture;

        private static float gameWidth = 1920f;
        private static float gameHeight = 1024f;

        // World/view management
        
        private Matrix view = Matrix.CreateLookAt(new Vector3(20, 20, 10), new Vector3(20, 20, 0), -Vector3.UnitY);
        //private Matrix shipProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), gameWidth / gameHeight, 0.1f, 100f);
        //private Matrix asteroidProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70), gameWidth / gameHeight, 0.1f, 100f);
        //private Matrix asteroidProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

        // Game Variables

        //Ship - TODO make this a class
        private Vector3 shipPosition;
        private Matrix shipWorldView = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private float shipAngle;
        private float shipMovementXSpeed = 0.05f;
        private float shipMovementYSpeed = 0;
        private float shipMovementZSpeed = 0;
        
        private Asteroid asteroid;

        public Game1()
        {
            // Window controls
            // Useful: https://industrian.net/tutorials/changing-display-resolution/#:~:text=MonoGame's%20default%20resolution%20is%20800x480,it%20by%20using%20the%20GraphicsDeviceManager.
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int) gameWidth;
            _graphics.PreferredBackBufferHeight = (int) gameHeight;
            _graphics.ApplyChanges();
            //_graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playerShip = Content.Load<Model>("Ship");
            Model asteroidModel = Content.Load<Model>("LargeAsteroid");
            //otherTexture = Content.Load<Texture2D>("GreenShipTexture");

            // TODO: use this.Content to load your game content here

            shipPosition = new Vector3(20, 28, 0);
            shipAngle = 0;

            asteroid = new Asteroid(new Vector3(20, 10, 0), Matrix.CreateTranslation(new Vector3(0, 0, 0)), asteroidModel, 0, 0f, 0.05f, 0);
        }    

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (shipAngle < 0.30f)
                {
                    shipAngle += 0.02f;
                }
                shipPosition += new Vector3(shipMovementXSpeed, shipMovementYSpeed, shipMovementZSpeed);
            } 
            else if (Keyboard.GetState().IsKeyDown(Keys.Right)) 
            {
                if (shipAngle > -0.30f)
                {
                    shipAngle -= 0.02f;
                }
                shipPosition -= new Vector3(shipMovementXSpeed, shipMovementYSpeed, shipMovementZSpeed);
            } 
            else 
            {
                if (shipAngle > 0)
                {
                    shipAngle -= 0.01f;
                } 
                else if (shipAngle < 0)
                {
                    shipAngle += 0.01f;
                }
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //TODO: make it fire
                Debug.WriteLine("Fire the cannon!");
            }

            // Update world matrix relative to new position
            shipWorldView = Matrix.CreateRotationY(shipAngle) * Matrix.CreateTranslation(shipPosition);

            // Asteroid update           
            asteroid.Update();

            /* Temporary Debugging */
            //Debug.WriteLine("Ship position: X:" + shipPosition.X + " Y:" + shipPosition.Y + " Z:" + shipPosition.Z);
            //Debug.WriteLine("Asteroid position: X:" + asteroidPosition.X + " Y:" + asteroidPosition.Y + " Z:" + asteroidPosition.Z);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            DrawModel(playerShip, shipWorldView, view, projection);
            DrawModel(asteroid.model, asteroid.worldView, view, projection);
            
            base.Draw(gameTime);
        }
        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                //Multiple textures and effects can be applied to a single model per draw
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    //This is an alternate texture that can be used
                    //effect.Texture = otherTexture;

                    //This is a basic lighting effect
                    effect.EnableDefaultLighting();

                    /**
                    effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    //Ambient light colour can be set with this - it's always good to have some ambient light
                    effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.EmissiveColor = new Vector3(1, 0, 0);
                    */

                    effect.FogEnabled = true;
                    effect.FogColor = Color.Black.ToVector3(); // For best results, make this color whatever your background is.
                    effect.FogStart = 9.75f;
                    effect.FogEnd = 10.25f;
                }

                mesh.Draw();
            }
        }
    }

}
