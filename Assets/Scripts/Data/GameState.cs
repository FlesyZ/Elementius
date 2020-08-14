using UnityEngine;
using UI;

public enum Elements
{
    None, Brave, Agile, Guard, Origin, Earth, Chaos, Iridescent, Dark = 9
}

namespace Game
{
    public struct GameState
    {
        public int ID;

        // 基本數值
        /// <summary>
        /// 血量
        /// </summary>
        public int hp;

        /// <summary>
        /// 經驗值
        /// </summary>
        public int xp;

        /// <summary>
        /// 流動係數
        /// </summary>
        public float recovery;

        // 基本能力值
        /// <summary>
        /// 力量
        /// </summary>
        public int STR;

        /// <summary>
        /// 敏捷
        /// </summary>
        public int AGI;

        /// <summary>
        /// 智力
        /// </summary>
        public int INT;

        /// <summary>
        /// 幸運
        /// </summary>
        public int LUK;
        
        /// <summary>
        /// 元素屬性相剋判斷
        /// </summary>
        /// <param name="detected">被偵測方</param>
        /// <param name="counteract">相對方</param>
        /// <returns></returns>
        public static short StrongOrWeakDetector(Elements detected, Elements counteract)
        {
            short result;

            switch (detected)
            {
                // None: neither resist or weak
                case Elements.None:
                    result = 0;
                    break;
                // Brave : StrongTo(Agile), WeakTo(Guard, Chaos)
                case Elements.Brave:
                    if (counteract == Elements.Agile)
                        result = 1;
                    else if (counteract == Elements.Guard || counteract == Elements.Chaos)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Agile : StrongTo(Guard), WeakTo(Brave, Chaos)
                case Elements.Agile:
                    if (counteract == Elements.Guard)
                        result = 1;
                    else if (counteract == Elements.Brave || counteract == Elements.Chaos)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Guard : StrongTo(Brave), WeakTo(Agile, Chaos)
                case Elements.Guard:
                    if (counteract == Elements.Brave)
                        result = 1;
                    else if (counteract == Elements.Agile || counteract == Elements.Chaos)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Origin: StrongTo(Earth), WeakTo(Irid)
                case Elements.Origin:
                    if (counteract == Elements.Earth)
                        result = 1;
                    else if (counteract == Elements.Iridescent)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Earth : StrongTo(Origin), WeakTo(Irid)
                case Elements.Earth:
                    if (counteract == Elements.Origin)
                        result = 1;
                    else if (counteract == Elements.Iridescent)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Chaos : StrongTo(Brave, Agile, Guard), WeakTo(Dark)
                case Elements.Chaos:
                    if ((int)counteract >= 1 && (int)counteract <= 3)
                        result = 1;
                    else if (counteract == Elements.Dark)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Irid  : StrongTo(Origin, Earth), Weak(Irid)
                case Elements.Iridescent:
                    if (counteract == Elements.Origin || counteract == Elements.Earth)
                        result = 1;
                    else if (counteract == Elements.Iridescent)
                        result = -1;
                    else
                        result = 0;
                    break;
                // Dark  : StrongTo(Chaos), Weak(Dark)
                case Elements.Dark:
                    if (counteract == Elements.Chaos)
                        result = 1;
                    else if (counteract == Elements.Dark)
                        result = -1;
                    else
                        result = 0;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }

        public static float ValueWithElements(Elements e, StatGeneral stat, bool isAttack)
        {
            float value = stat.STR;

            if (e == Elements.Brave || e == Elements.Agile || e == Elements.Guard)
            {
                value = 0.1f * (value * stat.STR);
                switch ((int)e)
                {
                    case 1:
                        value *= (isAttack) ? 2f : 0.8f;
                        break;
                    case 2:
                        value *= (isAttack) ? 1.5f : 1.5f;
                        break;
                    case 3:
                        value *= (isAttack) ? 0.8f : 2f;
                        break;
                }
            }
            else if (e == Elements.Origin)
            {
                value = 0.1f * (value * stat.INT);
            }
            else if (e == Elements.Earth)
            {
                value = 0.1f * (value * stat.AGI);
            }
            else if (e == Elements.Chaos)
            {
                value = 0.1f * (value * stat.LUK);
            }
            else if (e == Elements.Iridescent || e == Elements.Dark)
            {
                value = 0.5f * (stat.STR * stat.INT);
                if (e == Elements.Iridescent)
                    value *= (isAttack) ? 1f : 2f;
                else
                    value *= (isAttack) ? 2f : 1f;
            }

            return Random.Range(value * 0.9f, value * 1.1f);
        }
    }
}
