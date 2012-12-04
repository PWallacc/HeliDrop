using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HeliDrop
{
    class Crate
    {

        // Source Data
        private Texture2D srcSpriteSheet;
        private Rectangle srcLocation;
        private Vector2 srcOrigin;

        //
        public float ground = 0f;
        private Vector2 landingPosition;

        // DestinationData
        public Vector2 position;
        public Vector2 velocity;
        public Color destColor;
        public float m_fDestRotation;
        public float destScale;
        public float destDepth;
        public SpriteEffects m_eDestSprEff;

        // Game Data
        float MAX_VEL = 1280 / 6.0f;

        //Particle Variables
        public Vector2 emitterLocation;
        public int emitterXOffset = 18;
        public int emitterYOffset = -35;

        //Gravity
        private static float gravity = 9.81f;

        public Crate()
        {
            destColor = Color.White;
            m_eDestSprEff = SpriteEffects.None;
        }

        public void Initialize()
        {
            m_fDestRotation = 0.0f;
            destScale = .6f;
            srcLocation = new Rectangle(0, 0, 75, 75);
            srcOrigin = new Vector2(57, 135);
            emitterLocation = new Vector2(position.X, position.Y - srcSpriteSheet.Height);

        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            srcSpriteSheet = pContent.Load<Texture2D>(fileName);
        }

        public void Update(GameTime gameTime)
        {
            if (position.Y >= (ground + (destScale * 100)))
            {
                landingPosition = new Vector2(position.X, position.Y);
                velocity.X = 0;
                velocity.Y = 0;
            }
            else
            {
                velocity.X = MathHelper.Clamp(velocity.X, -1280, +1280);
                velocity.Y = 150.81f;
            }
            position.X += (float)(velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (float)(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);


        }


        public void Draw(SpriteBatch pBatch)
        {
            pBatch.Draw(srcSpriteSheet, position, srcLocation, destColor,
                        m_fDestRotation, srcOrigin, destScale, m_eDestSprEff, destDepth);
        }
    }
}

