using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HeliDrop
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static Random random = new Random();
        int timeElapsed;
        public static int totalTime = 60;
        public static int score = 0;
        public static int runnersSafe = 0;
        int timeSince;

        //Runner Variables
        private Vector2 runnerVelocity;
        private Vector2 maxRunnerVelocity;
        public static int runnersRunning = 1;
        private static int runnerLimit = 65;
        Runner[] runners = new Runner[runnerLimit];

        private static LinkedList<Runner> runnerList = new LinkedList<Runner>();
        private static LinkedList<Runner> runnersToDelete = new LinkedList<Runner>();

        //Heli
        Heli heli;

        //ParticleSystem
        ParticleEngine particleEngine;

        //Debug Display
        DebugInfo debugInfo;

        //Shader
        private RenderTarget2D tempRenderTarget;
        private EffectParameter effectParameter1;
        private Effect effect1;
        private static bool hit = false;
        private float count = 1.0f;
        private bool increasing = true;

        //Menu Stuff
        enum gameState { play, menu, end };
        gameState gamestate = gameState.menu;
        SpriteFont menuFont;
        SpriteFont menuFont2;


        Texture2D background;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            Runner firstRunner = new Runner();
            LinkedListNode<Runner> firstNode = new LinkedListNode<Runner>(firstRunner);
            runnerList.AddLast(firstNode);

            heli = new Heli();
            debugInfo = new DebugInfo();

        }

        protected override void Initialize()
        {

            base.Initialize();
            runnerVelocity = new Vector2(0, 0);
            maxRunnerVelocity = new Vector2(5, 0);

            foreach (Runner mRunner in runnerList)
            {
                mRunner.Initialize();
                mRunner.position = new Vector2(1275, GetRandomNumber(mRunner.srcHorizon, mRunner.srcForeground));
            }

            heli.Initialize();
            heli.position = new Vector2(100, 200);

        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Particle Engine
            Texture2D texture = Content.Load<Texture2D>("exhaust");
            particleEngine = new ParticleEngine(texture, new Vector2(400, 240), 1);

            //Runners
            foreach (Runner mRunner in runnerList)
            {
                mRunner.LoadContent(Content, "run_cycle");
            }

            //Heli
            heli.LoadContent(Content, "HeliAnimation6");
            debugInfo.LoadContent(Content, "SpriteFont1");

            //Shader
            tempRenderTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);    //ideally only do this once, not every frame
            effect1 = Content.Load<Effect>("Effect1");

            effectParameter1 = effect1.Parameters["brightness"];

            //Menu fonts
            menuFont = Content.Load<SpriteFont>("spriteFont1");
            menuFont2 = Content.Load<SpriteFont>("spriteFont2");

        }

        protected override void UnloadContent()
        {
        }

        public static void removeRunner(Runner runnerToDelete)
        {
            runnersRunning -= 1;
            runnerLimit -= 1;
            runnersToDelete.AddLast(runnerToDelete);
        }

        protected override void Update(GameTime gameTime)
        {
            if (gamestate == gameState.menu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                    gamestate = gameState.play;
            }
            else if (gamestate == gameState.play)
            {
                effectParameter1.SetValue(count);
                foreach (Runner mRunner in runnersToDelete)
                {
                    runnerList.Remove(mRunner);
                }

                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

                //Runners
                timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (runnersRunning < runnerLimit)
                {
                    if (timeElapsed > 500)
                    {
                        Runner newRunner = new Runner();
                        newRunner.Initialize();
                        newRunner.position = new Vector2(1300, GetRandomNumber(newRunner.srcHorizon, newRunner.srcForeground));
                        newRunner.LoadContent(Content, "run_cycle");
                        LinkedListNode<Runner> newNode = new LinkedListNode<Runner>(newRunner);
                        runnerList.AddLast(newRunner);
                        runnersRunning += 1;
                        timeElapsed = 0;
                    }
                }

                timeSince += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSince >= 1000)
                {
                    totalTime -= 1;
                    timeSince = 0;
                }
                if (totalTime == -1 || runnersRunning == 0)
                {
                    gamestate = gameState.end;
                }

                if ((runnersRunning == 1 && gameTime.TotalGameTime.Seconds > 40) || totalTime <= 5)
                {
                    if (count < 4f && increasing)
                        count += .1f;
                    else
                        increasing = false;

                    if (count > 1 && !increasing)
                        count -= .1f;
                    else
                        increasing = true;
                }
                if (totalTime == 0)
                {
                    count = 10;
                }
                effectParameter1.SetValue(count);


                foreach (Runner mRunner in runnerList)
                {
                    mRunner.m_bIsRunning = true;

                    if (mRunner.velocity.X < -maxRunnerVelocity.X)
                        mRunner.velocity.X -= 0.2f;

                    //Dubug - Make the runner stand still for hit detection testing
                    //runners[i].velocity = new Vector2(0, 0);

                    mRunner.m_eDestSprEff = SpriteEffects.FlipHorizontally;
                    mRunner.m_bIsRunning = true;
                    mRunner.velocity.X -= 15f;

                    //Udpate
                    mRunner.Update(gameTime);

                }

                //Heli Movement
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    heli.velocity.Y -= 10f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    heli.velocity.Y += 10f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    heli.m_eDestSprEff = SpriteEffects.None;
                    heli.velocity.X -= 10f;
                    heli.emitterXOffset = 19;
                    particleEngine.velocityDirection = .3f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    heli.m_eDestSprEff = SpriteEffects.FlipHorizontally;
                    heli.velocity.X += 10f;
                    heli.emitterXOffset = 18;
                    particleEngine.velocityDirection = -.3f;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    heli.dropCrate();
                }

                heli.position.Y = MathHelper.Clamp(heli.position.Y, heli.srcHorizon, 720);
                heli.Update(gameTime);

                //Particle Engine
                particleEngine.EmitterLocation = heli.emitterLocation;
                particleEngine.Update(gameTime);

                //
                debugInfo.Update(gameTime);


            }
            else
            {
                //game over
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                    this.Exit();
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            if (gamestate == gameState.menu)
            {
                GraphicsDevice.Clear(Color.Olive);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                spriteBatch.DrawString(menuFont2, "PRISON BREAK!", new Vector2(420, 300), Color.Black);
                spriteBatch.DrawString(menuFont, "Press Enter to Play!", new Vector2(520, 450), Color.Black);
                spriteBatch.End();
            }
            else if (gamestate == gameState.play)
            {
                GraphicsDevice.Clear(Color.Olive);

                //Shader
                RenderTargetBinding[] tempBinding = GraphicsDevice.GetRenderTargets();
                GraphicsDevice.SetRenderTarget(tempRenderTarget);

                GraphicsDevice.Clear(Color.Olive);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);


                // Draw the runners.
                foreach (Runner mRunner in runnerList)
                {
                    mRunner.Draw(spriteBatch);
                }

                //Particle Engine
                particleEngine.Draw(spriteBatch);

                //HEli
                heli.Draw(spriteBatch);

                //Show Debug Info
                debugInfo.Draw(spriteBatch);


                spriteBatch.End();

                //SHader
                GraphicsDevice.SetRenderTargets(tempBinding);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                effect1.CurrentTechnique.Passes[0].Apply();


                spriteBatch.Draw(tempRenderTarget, Vector2.Zero, Color.White);
                spriteBatch.End();


                base.Draw(gameTime);
            }
            else
            {
                //end
                GraphicsDevice.Clear(Color.Olive);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                if (runnersRunning == 0)
                {
                    spriteBatch.DrawString(menuFont2, "Game Over - You Win!", new Vector2(360, 300), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(menuFont2, "Game Over - You Lose!", new Vector2(360, 300), Color.Black);
                }
                spriteBatch.DrawString(menuFont, "Score: " + Game1.score, new Vector2(370, 450), Color.Black);
                spriteBatch.DrawString(menuFont, "Escapees safe: " + Game1.runnersSafe, new Vector2(540, 450), Color.Black);
                spriteBatch.DrawString(menuFont, "Escapees caught: " + Runner.runnersCaught, new Vector2(790, 450), Color.Black);
                spriteBatch.End();
            }
        }

        public int GetRandomNumber(int minimum, int maximum)
        {
            return random.Next(minimum, maximum);
        }

        private void clearRunnersToDelete(Runner mRunner)
        {
            if (runnersToDelete.Contains(mRunner))
                runnersToDelete.Remove(mRunner);
        }
    }
}
