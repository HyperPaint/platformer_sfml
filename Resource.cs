using System;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFML_Test
{
    public static class Resource
    {
        private static Sprite sprite = new Sprite( new Texture("Texture/block.png") );
        public const int BlockSize = 32;
        private static IntRect IR = new IntRect(0, 0, BlockSize, BlockSize);

        public static Sprite GetBlock(int index)
        {
            IR.Left = index * BlockSize;
            sprite.TextureRect = IR;
            return sprite;
        }
    }
}
