using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HeliDrop
{
    class Heli
    {
        // Source Data
        private Texture2D srcSpriteSheet;
        private Texture2D srcShadowSprite;
        private Rectangle srcLocation;
        private Vector2 srcOrigin;
        public int srcHorizon = 225;
        public int srcForeground = 325;

        // DestinationData
        public Vector2 position;
        public Color destColor;
        public float m_fDestRotation;
        public float destScale;
        public float destDepth;
        public SpriteEffects m_eDestSprEff;

        // Game Data
        public Vector2 velocity;
        private float MAX_VEL = 1280 / 6.0f;

        //Particle Variables
        public Vector2 emitterLocation;
        public int emitterXOffset = 18;
        public int emitterYOffset = -35;

        // Animation Data
        private int m_iCurrentCel;
        private int m_iNumberOfCels;
        private int m_iMsUntilNextCel;
        private int m_iMsPerCel;
        public bool m_bIsRunning = true;

        //Shadow
        HeliShadow heliShadow;

        //Crates
        Crate crate;
        private static bool crateDropped = false;
        
        //Debug Ingfo
        public static float heliX;
        public static float heliY;
        public static float heliDepth;
        public static float heliScale;
        public static Vector2 heliVelocity;
        public Heli()
        {
            destColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;

            heliShadow = new HeliShadow();
            crate = new Crate();

            m_iNumberOfCels = 10;
            m_iCurrentCel = 0;
            m_iMsPerCel = 10;
            m_iMsUntilNextCel = m_iMsPerCel;
        }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            destScale = .6f;
            destDepth = 0;
            srcLocation = new Rectangle(0, 0, 215, 69);
            srcOrigin = new Vector2(57, 105);
            velocity = new Vector2(0, 0);
            emitterLocation = new Vector2(position.X, position.Y - srcSpriteSheet.Height);

            //Shadow
            heliShadow.Initialize();
            heliShadow.position.X = 87;

            //Crate
            crate.Initialize();           
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            srcSpriteSheet = pContent.Load<Texture2D>(fileName);

            //Shadow
            heliShadow.LoadContent(pContent, "Shadow");

            //Crate
            crate.LoadContent(pContent, "crate");
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            UpdateHeliPosition(gameTime);
            UpdateEmitterLocation(gameTime);
            UpdateDepth(gameTime);
            UpdateScale(gameTime);
            UpdateDebugInfo(gameTime);

            //Heli Shadow
            updateHeliShadow();
            heliShadow.Update(gameTime);

            //Crate
            if (crateDropped == true)
            {
                crate.Update(gameTime);
            }
        }

        public void UpdateDebugInfo(GameTime gameTime)
        {
            heliDepth = this.destDepth;
            heliScale = this.destScale;
            heliX = this.position.X;
            heliY = this.position.Y;
            heliVelocity = this.velocity;
        }

        public void updateHeliShadow()
        {
            //Update the shadow variables
            heliShadow.velocity = this.velocity;
            heliShadow.destDepth = this.destDepth;
            heliShadow.destScale = this.destScale;
        }

        public void UpdateDepth(GameTime gameTime)
        {
            destDepth = (position.Y - srcHorizon) / (720 - srcHorizon);
        }

        public void UpdateEmitterLocation(GameTime gameTime)
        {
            emitterLocation.X = position.X + emitterXOffset + ((emitterXOffset * (1 * destDepth)));
            emitterLocation.Y = position.Y + emitterYOffset + ((emitterYOffset * (2 * destDepth)));
        }

        public void UpdateScale(GameTime gameTime)
        {
            destScale = .4f + (destDepth * 1f);
        }

        public void UpdateHeliPosition(GameTime gameTime)
        {
            velocity *= 0.92f; // friction
            velocity.X = MathHelper.Clamp(velocity.X, -MAX_VEL, +MAX_VEL);
            velocity.Y = MathHelper.Clamp(velocity.Y, -MAX_VEL, +MAX_VEL);
            position.X += (float)(velocity.X * (destScale * 3) * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (float)(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y = MathHelper.Clamp(position.Y, srcHorizon, 720 - (720 - srcForeground));
            if (position.X <= -25)
                position.X = 1275;
            else if (position.X >= 1275)
                position.X = -25;
        }

        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(srcSpriteSheet, position, srcLocation, destColor,
                    m_fDestRotation, srcOrigin, destScale, m_eDestSprEff, destDepth);

            //Shadow
            heliShadow.Draw(pBatch);

            //Crate
            if (crateDropped == true)
            {
                crate.Draw(pBatch);
            }
        }

        public void dropCrate()
        {
            crateDropped = true;
            crate.velocity.X = this.velocity.X;
            crate.position.X = heliShadow.position.X + 45;
            crate.position.Y = this.position.Y;
            crate.destDepth = this.destDepth;
            crate.destScale = this.destScale / 1.5f;
            crate.ground = heliShadow.position.Y;
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