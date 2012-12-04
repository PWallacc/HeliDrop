using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace HeliDrop
{
    class DebugInfo
    {
        //Frame Rate Variables
        SpriteFont font1;

        private float shadowVelocityX;
        private float shadowDepth;
        private float shadowScale;
        private float shadowX;
        private float shadowY;

        private float heliVelocityX;
        private float heliDepth;
        private float heliScale;
        private float heliX;
        private float heliY;

        private int runners;
        private int runnersCaught;

        private int time;
        private int score;
        public int safe;


        public DebugInfo()
        {
        }

        public void Initialize()
        {
            shadowVelocityX = 0;
            shadowDepth = 0;
            shadowScale = 0;
            shadowX = 0;
            shadowY = 0;

            heliVelocityX = 0;
            heliDepth = 0;
            heliScale = 0;
            heliX = 0;
            heliY = 0;

            runners = 0;
            runnersCaught = 0;

            time = 0;
            score = 0;
            safe = 0;
        }

        public void LoadContent(ContentManager pContent, String fileName)
        {
            font1 = pContent.Load<SpriteFont>(fileName);

        }

        public void Update(GameTime gameTime)
        {
            shadowVelocityX = (float)Math.Round(HeliShadow.shadowVelocity.X,2);
            shadowDepth = (float)Math.Round(HeliShadow.shadowDepth,2);
            shadowScale = (float)Math.Round(HeliShadow.shadowScale, 2);
            shadowX = (float)Math.Round(HeliShadow.shadowX, 2);
            shadowY = (float)Math.Round(HeliShadow.shadowY, 2);

            heliVelocityX = (float)Math.Round(Heli.heliVelocity.X,2);
            heliDepth = (float)Math.Round(Heli.heliDepth,2);
            heliScale = (float)Math.Round(Heli.heliScale,2);
            heliX = (float)Math.Round(Heli.heliX, 2);
            heliY = (float)Math.Round(Heli.heliY, 2);

            runners = Game1.runnersRunning;
            runnersCaught = Runner.runnersCaught;
            time = Game1.totalTime;
            score = Game1.score;
            safe = Game1.runnersSafe;



        }
        public void Draw(SpriteBatch pBatch)
        {
            // Draw the string
            pBatch.DrawString(font1, string.Format("Heli Velocity={0}", heliVelocityX.ToString()), new Vector2(20, 20), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Heli Depth={0}", heliDepth.ToString()), new Vector2(20, 40), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Heli Scale={0}", heliScale.ToString()), new Vector2(20, 60), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Heli X={0}", heliX.ToString()), new Vector2(20, 80), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Heli Y={0}", heliY.ToString()), new Vector2(20, 100), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);

            // Draw the string
            pBatch.DrawString(font1, string.Format("Shadow Velocity={0}", shadowVelocityX.ToString()), new Vector2(350, 20), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Shadow Depth={0}", shadowDepth.ToString()), new Vector2(350, 40), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Shadow Scale={0}", shadowScale.ToString()), new Vector2(350, 60), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Shadow X={0}", shadowX.ToString()), new Vector2(350, 80), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Shadow Y={0}", shadowY.ToString()), new Vector2(350, 100), Color.Black,
               0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            
            // Draw the string
            pBatch.DrawString(font1, string.Format("Runners={0}", runners.ToString()), new Vector2(700, 20), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Runners Caught={0}", runnersCaught.ToString()), new Vector2(700, 40), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Runners Safe={0}", safe.ToString()), new Vector2(700, 60), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Time Remaining={0}", time.ToString()), new Vector2(700, 80), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
            pBatch.DrawString(font1, string.Format("Score={0}", score.ToString()), new Vector2(700, 100), Color.Black,
                0, new Vector2(10.0f, 20.0f), 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}
