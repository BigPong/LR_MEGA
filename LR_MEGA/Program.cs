using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LR_MEGA
{
    class Program
    {
        static void Main(string[] args)
        {
            int money = 15000;
            while (true)
            {
                // 初始化字色
                Console.ForegroundColor = ConsoleColor.White;

                // 宣告參數
                Random rand = new Random();// 亂數
                int[] num = new int[6];// 儲存當期得獎數字的陣列
                int[,] prize;// 玩家簽下的號碼
                int[] score;// 玩家中獎數
                string[] strPrize;// 字串處理用
                int betTime;// 玩家下注數
                int betCount = 0;// 計數用
                string FirstInput;// 輸入用

                // 決定該期獎金及顯示畫面
                int prizeMoney = rand.Next(100, 500) * 1500;
                Console.WriteLine("你目前持有的金錢為" + money + "元" + "\r\n");
                Console.WriteLine("本期總獎金為" + prizeMoney + "元" + "\r\n");
                Console.WriteLine("請選擇下注單數 每單250  或是輸入0略過本次下注");
                Console.WriteLine("輸入/h可以觀看說明" + "\r\n");

                // 輸入
                FirstInput = Console.ReadLine();

                // 判斷是否為/h跟說明
                if (FirstInput == "/h")
                {
                    Console.WriteLine("\r\n大樂透規則說明：");
                    Console.WriteLine("每次都會隨機決定獎金，玩家可以自行決定下單數量");
                    Console.WriteLine("頭獎為總獎金的77.5%  貳獎為11%  參獎為7%  肆獎為4.5%  伍獎則是固定500元");
                    Console.WriteLine("重複獲獎會拿兩次錢  並不會平分  因為我累了懶得調ˊˋ");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                while (true)
                {
                    try
                    {
                        // 不能轉換即到catch
                        Int32.TryParse(FirstInput, out betTime);
                        break;
                    }
                    catch
                    {
                        // 王八蛋
                        Console.WriteLine("請正確輸入" + "\r\n");
                    }
                }

                // 玩家不下注或輸入錯誤即重來
                if (betTime == 0)
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    money -= betTime * 250;
                    Console.Clear();
                    Console.WriteLine("你目前持有的金錢為" + money + "元" + "\r\n");
                    Console.WriteLine("本期總獎金為" + prizeMoney + "元" + "\r\n");
                }

                // 設定陣列大小
                prize = new int[betTime, 6];

                // 直到玩家輸入正確才Break  否則都到Catch
                while (true)
                {
                    Console.WriteLine("請簽下這期大樂透號碼(6碼 1~42 使用空格間隔數字即可 ex:1 2 3 4 5 6)");

                    // 變數初始化與玩家輸入
                    string Input = Console.ReadLine();
                    strPrize = new string[6];// 切割字串用
                    int count = 0;// 計算字數
                    int i = 0;// 計算切割數量

                    // 字串處理  遇到空格時就將char儲存到下一個陣列
                    while (i < 6)
                    {
                        // 到字串尾端就跳開
                        if (count >= Input.Length)
                        {
                            break;
                        }

                        // 遇到空格切下一字
                        if (Input[count] == ' ')
                        {
                            i++;
                            count++;
                            continue;
                        }
                        strPrize[i] += Input[count];
                        count++;
                    }

                    try
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            // 不能轉換即到catch
                            Int32.TryParse(strPrize[j], out prize[betCount, j]);

                            // 非1~42即到Catch
                            if (prize[betCount, j] >= 1 && prize[betCount, j] <= 42) ;
                            else
                            {
                                Console.WriteLine("錯誤：非範圍內數字");
                                throw new Exception();
                            }
                        }

                        // 演算棒
                        for (int j = 0; j < 6 - 1; j++)
                        {
                            for (int k = j + 1; k < 6; k++)
                            {
                                if (prize[betCount, j] == prize[betCount, k])
                                {
                                    Console.WriteLine("錯誤：數字重複");
                                    throw new Exception();
                                }
                            }
                        }
                        betCount++;
                    }
                    catch
                    {
                        Console.WriteLine("\r\n" + "請正確輸入" + "\r\n");
                    }

                    // 簽完單數即跳出
                    if (betCount == betTime)
                    {
                        break;
                    }
                }

                // 宣告得獎號碼及儲存用變數
                betCount = 0;
                score = new int[betTime];// 各單得獎狀況
                List<int> numList = new List<int>();// 號碼List

                // 登記號碼範圍
                for (int i = 0; i < 42; i++)
                {
                    numList.Add(i);
                }

                // 亂數選出得獎號碼並判斷
                for (int i = 0; i < 6; i++)
                {
                    int r = rand.Next(1, numList.Count);
                    num[i] = numList[r];
                    numList.Remove(r);

                    // 偷懶只用巢狀判斷  其實還可以減少對比次數  不過有點麻煩
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = 0; k < betTime; k++)
                        {
                            if (num[i] == prize[k, j])
                            {
                                score[k]++;
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                        }
                    }

                    // 顯示得獎號碼
                    Console.WriteLine("\r\n第" + (i + 1) + "號：" + num[i] + "\r\n");
                    Thread.Sleep(500);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // 開獎  幾單開幾次  剩下看ConsoleWrite就懂
                while (betCount < betTime)
                {
                    Console.Write("第" + (betCount + 1) + "張樂透：");
                    switch (score[betCount])
                    {
                        case 6:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("頭獎");
                            Console.WriteLine("獎金" + (int)(prizeMoney * .775f) + "元");
                            money += (int)(prizeMoney * .775f);
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("貳獎");
                            Console.WriteLine("獎金" + (int)(prizeMoney * .11f) + "元");
                            money += (int)(prizeMoney * .11f);
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("參獎");
                            Console.WriteLine("獎金" + (int)(prizeMoney * .07f) + "元");
                            money += (int)(prizeMoney * .07f);
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("肆獎");
                            Console.WriteLine("獎金" + (int)(prizeMoney * .045f) + "元");
                            money += (int)(prizeMoney * .045f);
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("伍獎");
                            Console.WriteLine("獎金" + 500 + "元");
                            money += 500;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("沒得獎  下次再試試吧");
                            break;
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    betCount++;
                }

                // 判斷使用者輸入是否為n 是即跳出迴圈
                Console.WriteLine("輸入n離開或按Enter繼續");
                if (Console.ReadLine() == "n")
                {
                    break;
                }
                Console.Clear();
            }
        }
    }
}
