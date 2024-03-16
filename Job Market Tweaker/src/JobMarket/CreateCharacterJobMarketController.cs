using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JobMarketTweaker
{
    public static class CreateCharacterJobMarketController
    {
        public enum PerkType
        {
            CEO = 0,
            StarDesigner = 1,　//レジェンド
            Inexhaustible = 2, //疲れ知らず
            ErrorFree = 3,
            Loyal = 4,        //逃げないやつ
            Talented = 5,    //スキル学習速度アップ
            Luck = 6,        //結果がしばしば向上する
            Sporty = 7,        //移動速度アップ
            Orderly = 8,    //汚さない
            NatureLover = 9, //自然愛好家
            MedicalMiracle = 10, //風邪引かない
            Inuit = 11,    //寒さに強い
            Modest = 12, //Officeの良さに関係しない
            IronBladder = 13, //トイレに行かない
            Leadership = 14, //主導の開発時にボーナス
            AllRounder = 15, //全てのマイナースキルの上限値が上がる
            Messy = 16, //汚い場所気にしない
            Philanthropist = 17, //密集していても気にならない
            Greedy = 18, //給料が高い　!BadPerk
            Immounocompromised = 19, //病気になりやすい　!BadPerk
            StressAverse = 20, //クランスタイム中に、休みだす　!BadPerk
            Unfocused = 21, //バグが増える　!BadPerk
            Untalented = 22, //スキル学習速度ダウン　!BadPerk
            PixelArtist = 23, //レトロゲームを作るとき、グラフィックにボーナス
            PortingSpecialist = 24, //ポーティングにTechボーナス
            Imaginative = 25, //Sequelsゲームの開発時にGameplayボーナス
            EngineExpert = 26, //エンジン開発時に速度ボーナス
            Unlucky = 27, //結果の向上が見られなくなる
            Workaholic = 28, //委託業務をより早くこなす
            Efficient = 29, //すべての仕事時間が早くなる
                            //それ移行は未使用だが、39まで存在している。
        }

        private static List<PerkType> disabledPerks = new List<PerkType>();
        private static List<PerkType> enabledPerks = new List<PerkType>();

        //disabledPerksのプロパティ
        public static List<PerkType> DisabledPerks
        {
            get { return disabledPerks; }
        }

        //enabledPerksのプロパティ
        public static List<PerkType> EnabledPerks
        {
            get { return enabledPerks; }
        }


        public static void InitLimitedPerksList()
        {
            disabledPerks.Clear();
            enabledPerks.Clear();
        }

        // Perkを無効化するメソッド
        public static void AddDisabledPerk(PerkType perk)
        {
            disabledPerks.Add(perk);
        }

        // Perkを有効化するメソッド
        public static void AddEnabledPerk(PerkType perk)
        {
            enabledPerks.Add(perk);
        }

        public static bool IsAllPerksDisabled()
        {
            if(disabledPerks.Count == Enum.GetNames(typeof(PerkType)).Length)
            {
                //全てのPerkが無効になった場合の処理
                return true;
            }
            return false;
        }

        // 与えられたrandomPerkIndexが有効なPerkかどうかを判断し、無効ならば再選択するメソッド
        public static PerkType SetPerk(int randomPerkIndex, charArbeitsmarkt instance)
        {
            PerkType selectedPerk = (PerkType)randomPerkIndex;
            try {
                // 無効なPerkが選ばれた場合は、有効なPerkが選ばれると呼び出す
                while (disabledPerks.Contains(selectedPerk))
                {
                    // ここでrandomPerkIndexを再選択するロジックを書く
                    randomPerkIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(PerkType)).Length);
                    selectedPerk = (PerkType)randomPerkIndex;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("SetPerk : " + e);
            }

            return selectedPerk;
        }

        public static int SetEnabledPerk(int value)
        {
            int result = -1;
            try
            {
                result = (int)enabledPerks[value];
            }
            catch (Exception e)
            {
                Debug.LogError("SetLimitedPerk : " + e);
            }

            return result;
        }
    }
}
