using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HeliDrop
{
    public class Runner
    {
        // Source Data
        private Texture2D srcSpriteSheet;
        private Rectangle srcLocation;
        private Vector2 srcOrigin;
        public int srcHorizon = 390;
        public int srcForeground = 490;

        // DestinationData
        public Vector2 position;
        public Vector2 velocity;
        public Color destColor;
        public float m_fDestRotation;
        public float destScale;
        public float destDepth;
        public SpriteEffects m_eDestSprEff;

        // Animation Data
        private int m_iCurrentCel;
        private int m_iNumberOfCels;
        private int m_iMsUntilNextCel;
        private int m_iMsPerCel;
        public bool m_bIsRunning;

        // Game Data
        private Vector2 runnerVelocity;
        float MAX_VEL = 1280 / 6.0f;

        //
        private int found = 0;
        private int hitCount = 25;
        public static int runnersCaught = 0;
        public static float runnerDepth;
        public static float runnerX;

        public Runner()
        {
            destColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;
            m_iNumberOfCels = 12;
            m_iCurrentCel = 0;
            m_iMsPerCel = 50;
            m_iMsUntilNextCel = m_iMsPerCel;
        }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            destScale = .3f;
            srcLocation = new Rectangle(0, 0, 128, 128);
            srcOrigin = new Vector2(57, 105);
            runnerVelocity = new Vector2(0, 0);
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            srcSpriteSheet = pContent.Load<Texture2D>(fileName);
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            UpdatePosition(gameTime);
            UpdateFound(gameTime);
            UpdateDepth(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);
            runnerDepth = destDepth;
            runnerX = position.X;

        }

        public void UpdateFound(GameTime gameTime)
        {
            float hitDepth = (HeliShadow.shadowDepth);
            if((this.position.X >= HeliShadow.shadowX - 10f && this.position.X <= HeliShadow.shadowX + 90f) && (this.destDepth >= (hitDepth) && this.destDepth <= (hitDepth + .06f)))
            {
                found += 1;
                if (found == hitCount)
                {
                    Game1.removeRunner(this);
                    runnersCaught += 1;
                    Game1.score += 100;
                }
            }
        }


        public void UpdateDepth(GameTime gameTime)
        {
            destDepth = (position.Y - srcHorizon) / (720 - srcHorizon);
        }

        public void UpdateScale(GameTime gameTime)
        {
            destScale = 0.15f + (destDepth * 0.5f);
        }

        public void UpdateColor(GameTime gameTime)
        {
            float greyValue = 0.75f + (destDepth * 0.25f);
            destColor = new Color(greyValue, greyValue, greyValue);
        }

        public void UpdatePosition(GameTime gameTime)
        {
            velocity *= 0.92f; // friction
            velocity.X = MathHelper.Clamp(velocity.X, -MAX_VEL, +MAX_VEL);
            velocity.Y = MathHelper.Clamp(velocity.Y, -MAX_VEL, +MAX_VEL);
            position.X += (float)(velocity.X * (destScale * 3) * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (float)(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y = MathHelper.Clamp(position.Y, srcHorizon, 720 - (720 - srcForeground));
            if (position.X <= -25)
            {
                Game1.score -= 25;
                Game1.runnersSafe += 1;
                position.X = 1275;
            }
        }
        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(srcSpriteSheet, position, srcLocation, destColor,
                    m_fDestRotation, srcOrigin, destScale, m_eDestSprEff, destDepth);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            float relativeVelocity = Math.Abs(velocity.X / MAX_VEL);
            if (relativeVelocity > 0.05f)
            {
                if (m_iMsUntilNextCel <= 0)
                {
                    m_iCurrentCel++;
                    m_iMsUntilNextCel = (int)(m_iMsPerCel * (2.0f - relativeVelocity));
                }

            }

            m_iMsUntilNextCel -= gameTime.ElapsedGameTime.Milliseconds;
            if ((m_iMsUntilNextCel <= 0) && (m_bIsRunning))
            {
                m_iCurrentCel++;
                m_iMsUntilNextCel = m_iMsPerCel;
            }


            if (m_iCurrentCel >= m_iNumberOfCels)
            {
                m_iCurrentCel = 0;
            }

            srcLocation.X = srcLocation.Width * m_iCurrentCel;
        }

    }
}