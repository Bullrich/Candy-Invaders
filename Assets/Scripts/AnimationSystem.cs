using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Animation {
    [System.Serializable]
    public class AnimationSystem {
        public AnimationData[] animationData;
        SpriteRenderer spr;

        public void SetUp(SpriteRenderer sp)
        {
            spr = sp;
        }

        public string GetCurrentAnim()
        {
            foreach (AnimationData data in animationData)
                if (data.animSprite == spr.sprite)
                    return data.animName;
            return "";
        }

        public int GetCurrentAnimIndex()
        {
            int index = 0;
            for (int i = 0; i < animationData.Length; i++)
            {
                if (animationData[i].animSprite == spr.sprite)
                    index = i;
            }
            return index;
        }

        public void ChangeSprite(string spName)
        {
            foreach (AnimationData data in animationData)
                if (data.animName == spName)
                    spr.sprite = data.animSprite;
        }

        public void ChangeSprite(int spIndex)
        {
            spr.sprite = animationData[spIndex].animSprite;
        }

        public int getAnimsLength()
        {
            return animationData.Length;
        }
	
            [System.Serializable]
            public struct AnimationData
        {
            public string animName;
            public Sprite animSprite;
            
        }
	}
}