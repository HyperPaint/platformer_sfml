using System;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFML_Test
{
    public class Person : Entity
    {
        public Person(Vector2f SpawnPoint, int EntityNumber = 0, float dx = 1, float dy = 1) : base(SpawnPoint, EntityNumber, dx, dy)
        {
            isMoving = true;
        }

        public override Sprite Draw()
        {
            Process();

            if (FacingRight)
            {
                if (isMoving)
                {
                    IntRect IR;
                    IR.Left = Frame % 64 / 16 * 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = 32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
                else
                {
                    IntRect IR;
                    IR.Left = 0;
                    IR.Top = EntityNumber * 48;
                    IR.Width = 32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
            }
            else
            {
                if (isMoving)
                {
                    IntRect IR;
                    IR.Left = Frame % 64 / 16 * 32 + 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = -32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
                else
                {
                    IntRect IR;
                    IR.Left = 32;
                    IR.Top = EntityNumber * 48;
                    IR.Width = -32;
                    IR.Height = 48;
                    Sprite.TextureRect = IR;
                }
            }

            Sprite.Position = new Vector2f(GetPosition.X - Global.MyEntity.GetPosition.X + Entity.OffsetX, GetPosition.Y - Global.MyEntity.GetPosition.Y + Entity.OffsetY);

            Frame++;
            return Sprite;
        }

        protected override void Process()
        {
            base.Process();
            if (new Random().Next(1, 500) == 1)
            {
                FacingRight = !FacingRight;
            }
            else if (new Random().Next(1, 500) == 1)
            {
                Jump();
            }
            else if (new Random().Next(1, 500) == 1)
            {
                isMoving = !isMoving;
            }
        }
    }
}
