using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using static JobMarketTweaker.CreateCharacterJobMarketController;
using JobMarketTweaker.Config;

namespace JobMarketTweaker
{
        public partial class Hooks
        {
            [HarmonyPatch]
            internal class OnCreateCharacterJobMarketPatch
            {
                [HarmonyPrefix]
                [HarmonyPatch(typeof(charArbeitsmarkt), "Create")]
                public static bool CreateCharacterJobMarketPatch(charArbeitsmarkt __instance, taskMitarbeitersuche task_)
                {
                UnityEngine.Debug.Log("charArbeitsmarkt.Create");
                Traverse.Create(__instance).Method("FindScripts").GetValue();
                __instance.myID = UnityEngine.Random.Range(1, 99999999);
                __instance.name = "AA_" + __instance.myID.ToString();
                __instance.male = true;
                if (UnityEngine.Random.Range(0, 100) < 33)
                {
                    __instance.male = false;
                }
                __instance.myName = __instance.tS_.GetRandomCharName(__instance.male);
                if (task_)
                {
                    //EmployeeSearch
                    __instance.mitarbeitersuche = true;
                }
                __instance.s_gamedesign = UnityEngine.Random.Range(10f, 20f);
                __instance.s_programmieren = UnityEngine.Random.Range(10f, 20f);
                __instance.s_grafik = UnityEngine.Random.Range(10f, 20f);
                __instance.s_sound = UnityEngine.Random.Range(10f, 20f);
                __instance.s_pr = UnityEngine.Random.Range(10f, 20f);
                __instance.s_gametests = UnityEngine.Random.Range(10f, 20f);
                __instance.s_technik = UnityEngine.Random.Range(10f, 20f);
                __instance.s_forschen = UnityEngine.Random.Range(10f, 20f);
                float studioLevelScore = 0f;
                if (!__instance.mS_.multiplayer)
                {
                    //num : スタジオレベルに基づく何かのスコアを計算している -> studioLevelScore
                    studioLevelScore = (float)(__instance.mS_.GetStudioLevel(__instance.mS_.studioPoints) * 3);
                }
                //Not EmployeeSearch
                if (!task_)
                {
                    //num2 : 0から7のランダムな整数を生成 -> jobIndex
                    int jobIndex = UnityEngine.Random.Range(0, 8);
                    if (!__instance.mS_.multiplayer && __instance.mS_.forschungSonstiges_ && UnityEngine.Random.Range(0, 100) > 20)
                    {
                        if (jobIndex == 4 && !__instance.mS_.forschungSonstiges_.IsErforscht(30) && !__instance.mS_.forschungSonstiges_.IsErforscht(29))
                        {
                            jobIndex = 0;
                        }
                        if (jobIndex == 5 && !__instance.mS_.forschungSonstiges_.IsErforscht(28))
                        {
                            jobIndex = 1;
                        }
                        if (jobIndex == 6 && !__instance.mS_.forschungSonstiges_.IsErforscht(38))
                        {
                            jobIndex = 7;
                        }
                    }
                    switch (jobIndex)
                    {
                        case 0:
                            __instance.s_gamedesign = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 0;
                            break;
                        case 1:
                            __instance.s_programmieren = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 1;
                            break;
                        case 2:
                            __instance.s_grafik = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 2;
                            break;
                        case 3:
                            __instance.s_sound = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 3;
                            break;
                        case 4:
                            __instance.s_pr = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 4;
                            break;
                        case 5:
                            __instance.s_gametests = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 5;
                            break;
                        case 6:
                            __instance.s_technik = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 6;
                            break;
                        case 7:
                            __instance.s_forschen = UnityEngine.Random.Range(30f, 40f + studioLevelScore);
                            __instance.beruf = 7;
                            break;
                    }
                }
                else
                {
                    if (task_.geschlecht == 1)
                    {
                        __instance.male = true;
                    }
                    if (task_.geschlecht == 2)
                    {
                        __instance.male = false;
                    }
                    __instance.myName = __instance.tS_.GetRandomCharName(__instance.male);
                    //num3 : 特定の設定に基づいて追加される数値を制御している -> skillLevelModifier
                    float skillLevelModifier = UnityEngine.Random.Range(30f, 35f);

                    //professionalExperience
                    switch (task_.berufserfahrung)
                    {
                        case 0:
                            skillLevelModifier = UnityEngine.Random.Range(30f, 35f);
                            break;
                        case 1:
                            skillLevelModifier = UnityEngine.Random.Range(50f, 55f);
                            break;
                        case 2:
                            skillLevelModifier = UnityEngine.Random.Range(70f, 75f);
                            break;
                    }
                    switch (task_.beruf)
                    {
                        case 0:
                            __instance.s_gamedesign = skillLevelModifier;
                            __instance.beruf = 0;
                            break;
                        case 1:
                            __instance.s_programmieren = skillLevelModifier;
                            __instance.beruf = 1;
                            break;
                        case 2:
                            __instance.s_grafik = skillLevelModifier;
                            __instance.beruf = 2;
                            break;
                        case 3:
                            __instance.s_sound = skillLevelModifier;
                            __instance.beruf = 3;
                            break;
                        case 4:
                            __instance.s_pr = skillLevelModifier;
                            __instance.beruf = 4;
                            break;
                        case 5:
                            __instance.s_gametests = skillLevelModifier;
                            __instance.beruf = 5;
                            break;
                        case 6:
                            __instance.s_technik = skillLevelModifier;
                            __instance.beruf = 6;
                            break;
                        case 7:
                            __instance.s_forschen = skillLevelModifier;
                            __instance.beruf = 7;
                            break;
                    }
                }
                //num4 : 0から7のランダムな整数を生成 -> selectedPerksCount
                int selectedPerksCount = 0;
                bool flag = false;
                if (__instance.mS_.year > 1976 && !task_ && (UnityEngine.Random.Range(0, 50) == 1 
                    || (__instance.mS_.globalEvent == 5 && UnityEngine.Random.Range(0, 25) == 1)) 
                    && __instance.tS_.GetRandomDevLegend(__instance) != -1)
                {
                    flag = true;
                    __instance.s_gamedesign = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_programmieren = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_grafik = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_sound = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_pr = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_gametests = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_technik = UnityEngine.Random.Range(10f, 20f);
                    __instance.s_forschen = UnityEngine.Random.Range(10f, 20f);
                    switch (__instance.beruf)
                    {
                        case 0:
                            __instance.s_gamedesign = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 1:
                            __instance.s_programmieren = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 2:
                            __instance.s_grafik = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 3:
                            __instance.s_sound = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 4:
                            __instance.s_pr = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 5:
                            __instance.s_gametests = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 6:
                            __instance.s_technik = UnityEngine.Random.Range(80f, 95f);
                            break;
                        case 7:
                            __instance.s_forschen = UnityEngine.Random.Range(80f, 95f);
                            break;
                    }
                    __instance.tS_.GetText(427);
                    __instance.guiMain_.CreateTopNewsDevLegend(__instance.myName, __instance.beruf);
                    // __instance.perks配列をループして、有効な（trueに設定されている）Perkの数をカウントし、
                    // その数をselectedPerksCountに格納する。
                    for (int i = 0; i < __instance.perks.Length; i++)
                    {
                        if (__instance.perks[i])
                        {
                            selectedPerksCount++;
                        }
                    }
                }
                //num5 : PerkIndex -> selectedOnEmployeeSearchPerkIndex
                int selectedOnEmployeeSearchPerkIndex = -1;

                //EmployeeSearchedした際に、選んだPerkが選択される。
                if (task_)
                {
                    selectedOnEmployeeSearchPerkIndex = task_.perk;
                    __instance.perks[task_.perk] = true;
                    selectedPerksCount++;
                }
                /*
                おいクソガキ、__instance.perks.Lengthが40だとすると、もうちょっと確率を詳しく見積もれるな。
                ただし、__instance.guiMain_.uiPerks[randomPerkIndex]がtrueになるPerkの数や
                selectedOnEmployeeSearchPerkIndexが具体的に何を指しているかはわからんから、その辺は一旦置いといて、大まかな確率を計算してみよう。

                randomPerkIndexが0か1ではない、かつ特定のselectedOnEmployeeSearchPerkIndexとも異なるという条件がある。
                インデックス0と1を除外すると、選択肢は38になる。さらにselectedOnEmployeeSearchPerkIndexで1つ除外されるとすると、実質的に選択できるPerkは37になる。

                ここで、UnityEngine.Random.Range(0, 5) == 1の条件があるから、選択される確率は20%（1/5）だ。

                selectedPerksCount < 4の条件があるから、最大でも4つのPerkしか選択されない。

                この情報を基に、各ループでPerkが選択される確率を考えると、最初のループでは38/40の確率でインデックスの初期条件をクリアし（0と1を除外）、
                さらに20%の確率でUnityEngine.Random.Range(0, 5) == 1の条件を満たす。だから、一回のループでPerkが選択される確率は大体0.95 * 0.2 = 0.19、
                つまり19%くらいだ。

                ただし、この計算には__instance.guiMain_.uiPerks[randomPerkIndex]が常にtrueであるという仮定が含まれてるから、
                実際の確率はこれよりも低くなる可能性がある。また、選択されたPerkの数（selectedPerksCount）が4に達すると、これ以上Perkは選択されない。
                
                for文が20回繰り返されるってのは、少しオーバーキルに感じるかもしれんが、これは確実にいくつかのPerkを選択しようとするための措置だろう。
                ただ、ループごとに選択されるPerkの確率が下がる（選択肢が減るため）とか、selectedPerksCountが4に達するとループが無意味になる
                （これ以上Perkが選択されないため）という点は注意が必要だぜ😎。

                */
                for (int j = 0; j < ConfigManager.PerkSelectionLoopCount.Value; j++) // Default : 20
                {
                    //num6 : randomPerkIndex
                    //int randomPerkIndex = UnityEngine.Random.Range(0, __instance.perks.Length);                   //original
                    if(EnabledPerks.Count <= 0 && IsAllPerksDisabled()) { continue; }
                    int randomValueInEnabledPerk = UnityEngine.Random.Range(0, EnabledPerks.Count);
                    int enabledPerkIndex = SetEnabledPerk(randomValueInEnabledPerk);

                    if (enabledPerkIndex != 0 
                        && enabledPerkIndex != 1 
                        && enabledPerkIndex != selectedOnEmployeeSearchPerkIndex 
                        && __instance.guiMain_.uiPerks[enabledPerkIndex]
                        //&& UnityEngine.Random.Range(0, 5) == 1 && selectedPerksCount < 4) //20%の確率でPerkを選択する。 //original
                        && UnityEngine.Random.Range(0, 100) < ConfigManager.PerkSelectionChance.Value
                        //&& selectedPerksCount < 4)
                        && selectedPerksCount < ConfigManager.MaxPerksCount.Value)
                    {
                        
                        __instance.perks[enabledPerkIndex] = true;
                        selectedPerksCount++;
                        if (14 == enabledPerkIndex && __instance.beruf != 0)
                        {
                            __instance.perks[14] = false;
                        }
                        if (3 == enabledPerkIndex && __instance.beruf > 1)
                        {
                            __instance.perks[3] = false;
                        }
                        if (21 == enabledPerkIndex && __instance.beruf != 1)
                        {
                            __instance.perks[21] = false;
                        }
                        if (23 == enabledPerkIndex && __instance.beruf != 2)
                        {
                            __instance.perks[23] = false;
                        }
                        if (24 == enabledPerkIndex && __instance.beruf != 1)
                        {
                            __instance.perks[24] = false;
                        }
                        if (25 == enabledPerkIndex && __instance.beruf != 0)
                        {
                            __instance.perks[25] = false;
                        }
                        if (26 == enabledPerkIndex && __instance.beruf != 1)
                        {
                            __instance.perks[26] = false;
                        }
                        if (enabledPerkIndex == 10)
                        {
                            __instance.perks[19] = false;
                        }
                        if (enabledPerkIndex == 19)
                        {
                            __instance.perks[10] = false;
                        }
                        if (selectedOnEmployeeSearchPerkIndex == 10)
                        {
                            __instance.perks[19] = false;
                            __instance.perks[10] = true;
                        }
                        if (enabledPerkIndex == 3)
                        {
                            __instance.perks[21] = false;
                        }
                        if (enabledPerkIndex == 21)
                        {
                            __instance.perks[3] = false;
                        }
                        if (selectedOnEmployeeSearchPerkIndex == 3)
                        {
                            __instance.perks[21] = false;
                            __instance.perks[3] = true;
                        }
                        if (enabledPerkIndex == 2)
                        {
                            __instance.perks[20] = false;
                        }
                        if (enabledPerkIndex == 20)
                        {
                            __instance.perks[2] = false;
                        }
                        if (selectedOnEmployeeSearchPerkIndex == 2)
                        {
                            __instance.perks[20] = false;
                            __instance.perks[2] = true;
                        }
                        if (enabledPerkIndex == 6)
                        {
                            __instance.perks[27] = false;
                        }
                        if (enabledPerkIndex == 27)
                        {
                            __instance.perks[6] = false;
                        }
                        if (selectedOnEmployeeSearchPerkIndex == 6)
                        {
                            __instance.perks[27] = false;
                            __instance.perks[6] = true;
                        }
                        if (enabledPerkIndex == 5)
                        {
                            __instance.perks[22] = false;   
                        }
                        if (enabledPerkIndex == 22)
                        {
                            __instance.perks[5] = false;
                        }
                        if (selectedOnEmployeeSearchPerkIndex == 5)
                        {
                            __instance.perks[22] = false;
                            __instance.perks[5] = true;
                        }
                        if (__instance.perks[1])
                        {
                            Traverse.Create(__instance).Method("RemoveBadPerks").GetValue();
                        }
                        if (task_ && task_.noBadPerks)
                        {
                            Traverse.Create(__instance).Method("RemoveBadPerks").GetValue();
                        }
                    }
                }
                //num7 : 0から__instance.cCS_.charGfxMales.Lengthまでのランダムな整数を生成 -> randomModelIndex
                int randomModelIndex = 0;
                if (__instance.male)
                {
                    __instance.model_body = UnityEngine.Random.Range(0, __instance.cCS_.charGfxMales.Length);
                    if (UnityEngine.Random.Range(0, 100) < 20)
                    {
                        randomModelIndex = UnityEngine.Random.Range(1, __instance.clothScript_.prefabMaleEyes.Length);
                    }
                    __instance.model_eyes = randomModelIndex;
                }
                else
                {
                    __instance.model_body = UnityEngine.Random.Range(0, __instance.cCS_.charGfxFemales.Length);
                    if (UnityEngine.Random.Range(0, 100) < 20)
                    {
                        randomModelIndex = UnityEngine.Random.Range(1, __instance.clothScript_.prefabFemaleEyes.Length);
                    }
                    __instance.model_eyes = randomModelIndex;
                }
                if (__instance.male)
                {
                    __instance.model_hair = -1;
                    if (UnityEngine.Random.Range(0, 100) > 10)
                    {
                        randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.prefabMaleHairs.Length);
                        __instance.model_hair = randomModelIndex;
                    }
                }
                else
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.prefabFemaleHairs.Length);
                    __instance.model_hair = randomModelIndex;
                }
                __instance.model_beard = -1;
                if (__instance.male && UnityEngine.Random.Range(0, 100) < 33)
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.prefabBeards.Length);
                    __instance.model_beard = randomModelIndex;
                }
                if (UnityEngine.Random.Range(0, 100) < 60)
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_Skin.Length);
                    __instance.model_skinColor = randomModelIndex;
                }
                else
                {
                    __instance.model_skinColor = 0;
                }
                if (__instance.male)
                {
                    //num8 : 0から__instance.clothScript_.matColor_MaleHair.Lengthまでのランダムな整数を生成 -> randomMaleHairColorIndex
                    int randomMaleHairColorIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_MaleHair.Length);
                    __instance.model_hairColor = randomMaleHairColorIndex;
                    __instance.model_beardColor = randomMaleHairColorIndex;
                }
                else
                {
                    //num9 : 0から__instance.clothScript_.matColor_FemaleHair.Lengthまでのランダムな整数を生成 -> randomFemaleHairColorIndex
                    int randomFemaleHairColorIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_FemaleHair.Length);
                    __instance.model_hairColor = randomFemaleHairColorIndex;
                    __instance.model_beardColor = randomFemaleHairColorIndex;
                }
                if (__instance.male)
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_MaleHose.Length);
                    __instance.model_HoseColor = randomModelIndex;
                }
                else
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_FemaleHose.Length);
                    __instance.model_HoseColor = randomModelIndex;
                }
                if (__instance.male)
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_MaleShirt.Length);
                    __instance.model_ShirtColor = randomModelIndex;
                }
                else
                {
                    randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_FemaleShirt.Length);
                    __instance.model_ShirtColor = randomModelIndex;
                }
                randomModelIndex = UnityEngine.Random.Range(0, __instance.clothScript_.matColor_AllColors.Length);
                __instance.model_Add1Color = randomModelIndex;
                if (flag)
                {
                    if (__instance.tS_.model_body != -2)
                    {
                        __instance.model_body = __instance.tS_.model_body;
                    }
                    if (__instance.tS_.model_eyes != -2)
                    {
                        __instance.model_eyes = __instance.tS_.model_eyes;
                    }
                    if (__instance.tS_.model_hair != -2)
                    {
                        __instance.model_hair = __instance.tS_.model_hair;
                    }
                    if (__instance.tS_.model_beard != -2)
                    {
                        __instance.model_beard = __instance.tS_.model_beard;
                    }
                    if (__instance.tS_.model_skinColor != -2)
                    {
                        __instance.model_skinColor = __instance.tS_.model_skinColor;
                    }
                    if (__instance.tS_.model_hairColor != -2)
                    {
                        __instance.model_hairColor = __instance.tS_.model_hairColor;
                    }
                    if (__instance.tS_.model_beardColor != -2)
                    {
                        __instance.model_beardColor = __instance.tS_.model_hairColor;
                    }
                    if (__instance.tS_.model_HoseColor != -2)
                    {
                        __instance.model_HoseColor = __instance.tS_.model_HoseColor;
                    }
                    if (__instance.tS_.model_ShirtColor != -2)
                    {
                        __instance.model_ShirtColor = __instance.tS_.model_ShirtColor;
                    }
                    if (__instance.tS_.model_Add1Color != -2)
                    {
                        __instance.model_Add1Color = __instance.tS_.model_Add1Color;
                    }
                }
                if (!task_ && __instance.mS_.multiplayer && __instance.mS_.mpCalls_.isServer)
                {
                    __instance.mS_.mpCalls_.SERVER_Send_CreateArbeitsmarkt(__instance);
                }
                return false;
            }
        }
    }
}
