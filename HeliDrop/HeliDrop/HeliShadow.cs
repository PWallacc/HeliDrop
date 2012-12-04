using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HeliDrop
{
    class HeliShadow
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

        // Game Data
        private Vector2 heliVelocity;
        float MAX_VEL = 1280 / 6.0f;

        //Particle Variables
        public Vector2 emitterLocation;
        public int emitterXOffset = 18;
        public int emitterYOffset = -35;


        //Debug Ingfo
        public static float shadowX;
        public static float shadowY;
        public static float shadowDepth;
        public static float shadowScale;
        public static Vector2 shadowVelocity;

        public HeliShadow()
        {
            destColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;
        }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            destScale = .6f;
            destDepth = 0;
            srcLocation = new Rectangle(0, 0, 200, 75);
            srcOrigin = new Vector2(0, 0);
            heliVelocity = new Vector2(0, 0);
            emitterLocation = new Vector2(position.X, position.Y - srcSpriteSheet.Height);
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            srcSpriteSheet = pContent.Load<Texture2D>(fileName);
        }

        public void Update(GameTime gameTime)
        {
            UpdatePosition(gameTime);
            UpdateDepth(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);

            //
            UpdateDebugInfo(gameTime);
        }

        public void UpdateDebugInfo(GameTime gameTime)
        {
            shadowDepth = this.destDepth;
            shadowScale = this.destScale;
            shadowX = this.position.X;
            shadowY = this.position.Y;
            shadowVelocity = this.velocity;
        }

        public void UpdateDepth(GameTime gameTime)
        {
            destDepth = (position.Y - srcHorizon) / (720 - srcHorizon);
        }

        public void UpdateScale(GameTime gameTime)
        {
            destScale = .4f + (destDepth * 1f);
        }

        public void UpdateColor(GameTime gameTime)
        {
            float greyValue = 0.75f + (destDepth * 0.25f);
            //destColor = new Color(greyValue, greyValue, greyValue, 10);
            destColor = new Color(
            256,
            256,
            256,
            20);
        }

        public void UpdatePosition(GameTime gameTime)
        {
            velocity.X = MathHelper.Clamp(velocity.X, -MAX_VEL, +MAX_VEL);
            velocity.Y = MathHelper.Clamp(velocity.Y, -MAX_VEL, +MAX_VEL);
            position.X += (float)(velocity.X * (destScale * 3.0) * gameTime.ElapsedGameTime.TotalSeconds);
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
        }
    }
}
