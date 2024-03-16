using HarmonyLib;
using JobMarketTweaker.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
(input)
>> wochenAmArbeitsmarkt

(output)
Translation:
- English: "weeks on the job market"
- Japanese: "求人市場での週数"

For programming purposes, use:
- "weeksOnJobMarket"
 */

namespace JobMarketTweaker
{
    public partial class Hooks
    {
        [HarmonyPatch]
        internal class OnUpdateJobMarketPatch
        {
            /// <summary>
            /// Used by:
            /// InitNewGame / NEWGAME_BUTTON_OK
            /// WeeklyUpdates
            /// 
            /// dontDeleteがTrueの場合、従業員候補の削除を行わない -> 週次更新時に従業員候補を削除する, InitNewGameは削除しない
            /// 
            /// </summary>
            /// <param name="__instance"></param>
            /// <param name="dontDelete"></param>
            [HarmonyPrefix]
            [HarmonyPatch(typeof(arbeitsmarkt), "ArbeitsmarktUpdaten")]
            public static bool UpdateJobMarketPatch(arbeitsmarkt __instance, bool dontDelete, mainScript ___mS_)
            {
                if (!ConfigManager.IsModEnabled.Value) { return true; }

                //Debug.Log("UpdateJobMarketPatch");
                if (___mS_.multiplayer && ___mS_.mpCalls_.isClient)
                {
                    return false;
                }
                GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
                if (!dontDelete)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i])
                        {
                            charArbeitsmarkt component = array[i].GetComponent<charArbeitsmarkt>();
                            if (component)
                            {
                                component.wochenAmArbeitsmarkt++;
                                if (component.wochenAmArbeitsmarkt > 12 && UnityEngine.Random.Range(0, component.wochenAmArbeitsmarkt * 3) > UnityEngine.Random.Range(0, 100))
                                {
                                    IEnumerator Remove = Traverse.Create(__instance).Method("Remove", new object[] { component }).GetValue<IEnumerator>();
                                    __instance.StartCoroutine(Remove);
                                }
                            }
                        }
                    }
                }
                if (___mS_.globalEvent != 3)
                {
                    //num : 現在の応募者数 -> currentNumberOfApplicants 
                    int currentNumberOfApplicants = array.Length;
                    //num2 : 応募者数の上限 -> maximumApplicantsCount 30
                    int maximumApplicantsCount = ConfigManager.MaximumApplicantsCount.Value;

                    //デフォルトの応募者数 -> defaultadditionalApplicants 3
                    int defaultadditionalApplicants = ConfigManager.AdditionalApplicants.Value;
                    int extraAdditionalApplicants = ConfigManager.ExtraAdditionalApplicants.Value;

                    //num3 : 応募者数の増加量 -> additionalApplicants
                    int additionalApplicants = defaultadditionalApplicants + extraAdditionalApplicants;
                    // サンドボックスモードの場合, 設定により、別で初期化を行う
                    if (___mS_.settings_sandbox && !ConfigManager.IsInSandBoxModeApplied.Value)
                    {
                        additionalApplicants = 3;
                        switch (___mS_.sandbox_arbeitsmarkt)
                        {
                            case 0:
                                maximumApplicantsCount = 30;
                                additionalApplicants += 0;
                                break;
                            case 1:
                                maximumApplicantsCount = 50;
                                additionalApplicants += 3;
                                break;
                            case 2:
                                maximumApplicantsCount = 80;
                                additionalApplicants += 6;
                                break;
                            case 3:
                                maximumApplicantsCount = 100;
                                additionalApplicants += 9;
                                break;
                            case 4:
                                maximumApplicantsCount = 150;
                                additionalApplicants += 14;
                                break;
                        }
                    }

                    if (currentNumberOfApplicants < maximumApplicantsCount)
                    {
                        if (!___mS_.multiplayer)
                        {
                            //0, 1, 2, デフォルトだと、3回しかループしない。
                            for (int j = 0; j < additionalApplicants; j++)
                            {
                                if (UnityEngine.Random.Range(0, 100) > 50 || dontDelete)
                                {
                                    charArbeitsmarkt charArbeitsmarkt = __instance.CreateArbeitsmarktItem();
                                    if (charArbeitsmarkt)
                                    {
                                        charArbeitsmarkt.Create(null);
                                        currentNumberOfApplicants++;
                                    }
                                }
                                if (currentNumberOfApplicants >= maximumApplicantsCount)
                                {
                                    return false;
                                }
                            }
                            return false;
                        }

                        //なにこれ？なんか７回ループしてるけど、なんで？基礎のコード…？後で確認する。
                        //Max = 30で、同様にcurrent = 30出ない限りは、ここには来ないはず。
                        //そしてその場合、1回のみループするだけになる。
                        //k < 7 : Default
                        for (int k = 0; k < 7; k++)
                        {
                            if (UnityEngine.Random.Range(0, 100) > 50 || dontDelete)
                            {
                                charArbeitsmarkt charArbeitsmarkt2 = __instance.CreateArbeitsmarktItem();
                                if (charArbeitsmarkt2)
                                {
                                    charArbeitsmarkt2.Create(null);
                                    currentNumberOfApplicants++;
                                }
                            }
                            if (currentNumberOfApplicants >= maximumApplicantsCount)
                            {
                                break;
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}
