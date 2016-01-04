using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakBoll
{
    class ClassSprite
    {
        public Texture2D texture;  // 2D 紋理圖
        public Vector2 position;   // 2D 紋理圖 的位置
        private Vector2 screenSize; // 視窗寬高
        public Vector2 velocity = Vector2.Zero;   // 2D 紋理圖 的位移速度

        public ClassSprite(Texture2D texture, Vector2 position, Vector2 screenSize)
        {
            this.texture = texture;
            this.position = position;
            this.screenSize = screenSize;
        }

        // 移動
        public bool Move()
        {
            // 右緣 碰到 視窗右邊了
            if (position.X + texture.Width + velocity.X > screenSize.X)
                velocity.X = -velocity.X;

            // 下緣 碰到 視窗底邊了
            else if (position.Y + texture.Height + velocity.Y > screenSize.Y)
                // velocity.Y = -velocity.Y;
                return true;

            // 左緣 碰到 視窗左邊了
            else if (position.X + velocity.X < 0)
                velocity.X = -velocity.X;

            // 上緣 碰到 視窗上邊了
            else if (position.Y + velocity.Y < 0)
                velocity.Y = -velocity.Y;

            position += velocity;
            return false;
        }

        public bool Collides(ClassSprite other)  //回傳當圖片碰到板子的時候的反應
        {
            return (this.position.X + texture.Width > other.position.X &&
                    this.position.X < other.position.X + other.texture.Width &&
                    this.position.Y + texture.Height > other.position.Y &&
                    this.position.Y < other.position.Y + other.texture.Height);
        }
    }
}
