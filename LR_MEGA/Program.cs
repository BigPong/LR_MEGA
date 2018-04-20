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
                int betTime;
                int betCount = 0;

                int prizeMoney = rand.Next(100, 500) * 1500;
                Console.WriteLine("你目前持有的金錢為" + money + "元" + "\r\n");
                Console.WriteLine("本期總獎金為" + prizeMoney + "元" + "\r\n");
                Console.WriteLine("請選擇下注單數 每單250  或是輸入0略過本次下注" + "\r\n");
                while (true)
                {
                    try
                    {
                        // 不能轉換即到catch
                        Int32.TryParse(Console.ReadLine(), out betTime);
                        break;
                    }
                    catch
                    {
                        // 王八蛋
                        Console.WriteLine("請正確輸入" + "\r\n");
                    }
                }
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
                prize = new int[betTime, 6];
                // 直到玩家輸入正確才Break  否則都到Catch
                while (true)
                {
                    Console.WriteLine("請簽下這期大樂透號碼(6碼 1~42 使用空格間隔數字即可)");
                    //變數初始化與玩家輸入
                    string Input = Console.ReadLine();
                    strPrize = new string[6];
                    int count = 0;
                    int i = 0;

                    //字串處理  遇到空格時就將char儲存到下一個陣列
                    while (i < 6)
                    {
                        //到字串尾端就跳開
                        if (count >= Input.Length)
                        {
                            break;
                        }
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
                            //不能轉換即到catch
                            Int32.TryParse(strPrize[j], out prize[betCount, j]);
                            //非1~42即到Catch
                            if (prize[betCount, j] >= 1 && prize[betCount, j] <= 42)
                            {
                            }
                            else
                            {
                                //好東西2號  同上
                                Console.WriteLine("錯誤：非範圍內數字");
                                throw new Exception();
                            }
                        }

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
                        //王八蛋
                        Console.WriteLine("\r\n" + "請正確輸入" + "\r\n");
                    }
                    if (betCount == betTime)
                    {
                        break;
                    }
                }

                betCount = 0;
                score = new int[betTime];
                List<int> numList = new List<int>();

                for (int i = 0; i < 42; i++)
                {
                    numList.Add(i);
                }

                for (int i = 0; i < 6; i++)
                {
                    int r = rand.Next(1, numList.Count);
                    num[i] = numList[r];
                    numList.Remove(r);

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
                    Console.WriteLine("\r\n第" + (i + 1) + "號：" + num[i] + "\r\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
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
