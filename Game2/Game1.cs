using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Camera camera;
        bool isX = false;

        Cube cube, cube2;

        RenderTarget2D render;

        List<Mesh> meshes = new List<Mesh>();

        MouseState lastMouseState;
        //private CubeDemo cubeDemo;
        private int LastScrollWheel = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            render = new RenderTarget2D(GraphicsDevice, 1920, 1080, false, SurfaceFormat.Color, DepthFormat.Depth16);

            camera = new FreeCamera(new Vector3(-3.004f, -0.18f, -2.7f), MathHelper.ToRadians(230), MathHelper.ToRadians(0), GraphicsDevice);
            lastMouseState = Mouse.GetState();

            Effect basicEffect = new BasicEffect(GraphicsDevice);
            Effect effect = Content.Load<Effect>("SimpleEffect");

            cube = new Cube(GraphicsDevice, basicEffect, null);
            cube2 = new Cube(GraphicsDevice, basicEffect, null);

            lastMouseState = Mouse.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            updateCamera(gameTime);

            base.Update(gameTime);
            //updateModel(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            Matrix posMatrix = Matrix.CreateTranslation(new Vector3(10, 0, 0));
            GraphicsDevice.SetRenderTarget(render);

            GraphicsDevice.Clear(Color.DimGray);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            Matrix gWVP =  camera.View * camera.Projection;

            GraphicsDevice.SetVertexBuffer(cube.vertexBuffer);
            GraphicsDevice.Indices = cube.indexBuffer;
            //cube.cubeEffect.Parameters["gWVP"].SetValue(gWVP);
            BasicEffect basicEffect = (BasicEffect) cube.cubeEffect;
            basicEffect.World = Matrix.Identity;
            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.DiffuseColor = Color.White.ToVector3();

            GraphicsDevice.RasterizerState = new RasterizerState { CullMode = CullMode.CullClockwiseFace };

            foreach ( var pass in cube.cubeEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cube.indexBuffer.IndexCount / 3);
            }

            GraphicsDevice.SetVertexBuffer(cube2.vertexBuffer);
            GraphicsDevice.Indices = cube2.indexBuffer;
            //cube2.cubeEffect.Parameters["gWVP"].SetValue(posMatrix * gWVP);
            BasicEffect basicEffect2 = (BasicEffect)cube2.cubeEffect;
            basicEffect2.World = posMatrix;
            basicEffect2.View = camera.View;
            basicEffect2.Projection = camera.Projection;
            basicEffect2.DiffuseColor = Color.Black.ToVector3();
            foreach (var pass in cube2.cubeEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cube2.indexBuffer.IndexCount / 3);
            }

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(samplerState:SamplerState.PointWrap);
            _spriteBatch.Draw(render, new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void updateCamera(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            // Rotate Camera
            ((FreeCamera)camera).Rotate(deltaX * .005f, deltaY * .005f);

            Vector3 translation = Vector3.Zero;

            float factor = 5.0f;
            if (keyState.IsKeyDown(Keys.LeftShift))
            {
                factor = 10.0f;
            }

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.Z))
            {
                translation += factor * ((FreeCamera)camera).TransformVector(Vector3.Forward) / 1000;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                translation += factor * ((FreeCamera)camera).TransformVector(Vector3.Backward) / 1000;
            }
            if (keyState.IsKeyDown(Keys.Q))
            {
                translation += factor * ((FreeCamera)camera).TransformVector(Vector3.Left) / 1000;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                translation += factor * ((FreeCamera)camera).TransformVector(Vector3.Right) / 1000;
            }
            // Move 3 units per millisecond, independant of frame rate
            translation *= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            ((FreeCamera)camera).Position += translation;

            // Move the camera
            camera.Update();

            lastMouseState = mouseState;
        }

        void updateModel(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            Vector3 rotChange = new Vector3(0, 0, 0);

            // Determine on which axes
            if (keyState.IsKeyDown(Keys.W))
                rotChange += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.S))
                rotChange += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                rotChange += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.D))
                rotChange += new Vector3(0, -1, 0);

            meshes[0].Rotation += rotChange * .025f;

            // if space isn't down, the ship shouldn't move
            if (!keyState.IsKeyDown(Keys.Space))
                return;

            // Determine what direction to move in
            Matrix rotation = Matrix.CreateFromYawPitchRoll(meshes[0].Rotation.Y, meshes[0].Rotation.X, meshes[0].Rotation.Z);

            // Move in the direction dictated by our rotation matrix
            meshes[0].Position += Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 4;
        }
    }
}
