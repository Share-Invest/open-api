using Microsoft.AspNetCore.SignalR.Client;

using ShareInvest.Observers;
using ShareInvest.OpenAPI.Entity;

using System.Diagnostics;

namespace ShareInvest;

partial class AnTalk
{
    async Task OnReceiveMessage(RealMsgEventArgs e)
    {
        if (IsAdministrator && HubConnectionState.Connected == Socket?.Hub.State)
        {
            await Socket.Hub.SendAsync(e.Type, e.Key, e.Data);
        }

        if (Enum.TryParse(e.Type, out KiwoomRealType type))
        {
            string[] data;

            switch (type)
            {
                case KiwoomRealType.옵션시세:
                    data = e.Data.Split('\t');

                    if (foQuote.TryGetValue(e.Key, out SingleOpt50001? ct51))
                    {
                        ct51.CompareToPreviousDay = data[2];
                        ct51.CurrentPrice = data[1];
                        ct51.Rate = data[3];
                        ct51.CompareToPreviousSign = data[data.Length == 53 ? 21 : 19];
                        ct51.TradingVolume = data[7];
                    }
                    else
                    {
                        foQuote[e.Key] = new SingleOpt50001
                        {
                            CompareToPreviousDay = data[2],
                            CurrentPrice = data[1],
                            Rate = data[3],
                            CompareToPreviousSign = data[data.Length == 53 ? 21 : 19],
                            TradingVolume = data[7]
                        };
                    }
                    LiquidateInPrinciple(e.Key);
                    break;

                case KiwoomRealType.선물시세:
                    data = e.Data.Split('\t');

                    if (priorityQuote.TryGetValue(e.Key, out var q))
                    {
                        q.CurrentPrice = data[1];
                        q.TopPriorityAskPrice = data[4];
                        q.TopPriorityBidPrice = data[5];
                    }
                    else
                    {
                        priorityQuote[e.Key] = new RealType.PriorityQuote
                        {
                            CurrentPrice = data[1],
                            TopPriorityAskPrice = data[4],
                            TopPriorityBidPrice = data[5]
                        };
                    }
                    break;

                case KiwoomRealType.옵션호가잔량:
                    data = e.Data.Split('\t');

                    if (foQuote.TryGetValue(e.Key, out SingleOpt50001? qt51))
                    {
                        qt51.QuoteTime = data[0];
                        qt51.FirstAskPrice = data[3];
                        qt51.FirstAskBalance = data[4];
                        qt51.ComparedToFirstAskBalance = data[5];
                        qt51.FirstBidPrice = data[7];
                        qt51.FirstBidBalance = data[8];
                        qt51.ComparedToFirstBidBalance = data[9];
                        qt51.SecondAskPrice = data[11];
                        qt51.SecondAskBalance = data[12];
                        qt51.ComparedToSecondAskBalance = data[13];
                        qt51.SecondBidPrice = data[15];
                        qt51.SecondBidBalance = data[16];
                        qt51.ComparedToSecondBidBalance = data[17];
                        qt51.ThirdAskPrice = data[19];
                        qt51.ThirdAskBalance = data[20];
                        qt51.ComparedToThirdAskBalance = data[21];
                        qt51.ThirdBidPrice = data[23];
                        qt51.ThirdBidBalance = data[24];
                        qt51.ComparedToThirdBidBalance = data[25];
                        qt51.FourthAskPrice = data[27];
                        qt51.FourthAskBalance = data[28];
                        qt51.ComparedToFourthAskBalance = data[29];
                        qt51.FourthBidPrice = data[31];
                        qt51.FourthBidBalance = data[32];
                        qt51.ComparedToFourthBidBalance = data[33];
                        qt51.FifthAskPrice = data[35];
                        qt51.FifthAskBalance = data[36];
                        qt51.ComparedToFifthAskBalance = data[37];
                        qt51.FifthBidPrice = data[39];
                        qt51.FifthBidBalance = data[40];
                        qt51.ComparedToFifthBidBalance = data[41];
                        qt51.TotalAskBalance = data[43];
                        qt51.ComparedToTotalAskBalance = data[44];
                        qt51.TotalBidBalance = data[46];
                        qt51.ComparedToTotalBidBalance = data[47];
                        qt51.NetBidBalance = data[50];
                        qt51.TradingVolume = data[51];
                    }
                    else
                    {
                        foQuote[e.Key] = new SingleOpt50001
                        {
                            QuoteTime = data[0],
                            FirstAskPrice = data[3],
                            FirstAskBalance = data[4],
                            ComparedToFirstAskBalance = data[5],
                            FirstBidPrice = data[7],
                            FirstBidBalance = data[8],
                            ComparedToFirstBidBalance = data[9],
                            SecondAskPrice = data[11],
                            SecondAskBalance = data[12],
                            ComparedToSecondAskBalance = data[13],
                            SecondBidPrice = data[15],
                            SecondBidBalance = data[16],
                            ComparedToSecondBidBalance = data[17],
                            ThirdAskPrice = data[19],
                            ThirdAskBalance = data[20],
                            ComparedToThirdAskBalance = data[21],
                            ThirdBidPrice = data[23],
                            ThirdBidBalance = data[24],
                            ComparedToThirdBidBalance = data[25],
                            FourthAskPrice = data[27],
                            FourthAskBalance = data[28],
                            ComparedToFourthAskBalance = data[29],
                            FourthBidPrice = data[31],
                            FourthBidBalance = data[32],
                            ComparedToFourthBidBalance = data[33],
                            FifthAskPrice = data[35],
                            FifthAskBalance = data[36],
                            ComparedToFifthAskBalance = data[37],
                            FifthBidPrice = data[39],
                            FifthBidBalance = data[40],
                            ComparedToFifthBidBalance = data[41],
                            TotalAskBalance = data[43],
                            ComparedToTotalAskBalance = data[44],
                            TotalBidBalance = data[46],
                            ComparedToTotalBidBalance = data[47],
                            NetBidBalance = data[50],
                            TradingVolume = data[51]
                        };
                    }
                    break;

                case KiwoomRealType.선물호가잔량:
                    data = e.Data.Split('\t');

                    if (priorityQuote.TryGetValue(e.Key, out var qb))
                    {
                        qb.TopPriorityAskPrice = data[1];
                        qb.TopPriorityBidPrice = data[2];
                    }
                    else
                    {
                        priorityQuote[e.Key] = new RealType.PriorityQuote
                        {
                            TopPriorityAskPrice = data[1],
                            TopPriorityBidPrice = data[2]
                        };
                    }
                    break;

                case KiwoomRealType.선물옵션우선호가:
                    data = e.Data.Split('\t');

                    if (priorityQuote.TryGetValue(e.Key, out var tq))
                    {
                        tq.CurrentPrice = data[0];
                        tq.TopPriorityAskPrice = data[1];
                        tq.TopPriorityBidPrice = data[2];
                    }
                    else
                    {
                        priorityQuote[e.Key] = new RealType.PriorityQuote
                        {
                            CurrentPrice = data[0],
                            TopPriorityAskPrice = data[1],
                            TopPriorityBidPrice = data[2]
                        };
                    }
                    LiquidateInPrinciple(e.Key);
                    break;

                case KiwoomRealType.장시작시간:
                    var marketOperation = Operation.Get(e.Data.Split('\t')[0]);

                    Delay.Instance.Milliseconds = marketOperation switch
                    {
                        MarketOperation.장시작 => worksWithMarketOperation(),

                        MarketOperation.선옵_장마감전_동시호가_시작 => LiquidateInPrinciple(),

                        MarketOperation.장마감 => await RequestTransmissionAsync(nameof(Opt10081)),

                        _ => 0x259
                    };
                    Cache.MarketOperation = marketOperation;

                    notifyIcon.Text = $"{DateTime.Now:G}\n{Enum.GetName(marketOperation)}";
                    break;
            }
        }

#if DEBUG
        if (!Array.Exists(realTypes, match => match.ToString().Equals(e.Type)))
        {
            Debug.WriteLine(new
            {
                e.Type,
                e.Key,
                e.Data.Split('\t').Length,
                e.Data
            });
        }
#endif
    }

    void LiquidateInPrinciple(string code)
    {
        if (balance.TryGetValue(code, out OpenAPI.Balance? bal) && bal.QuantityAvailableForOrder > 0 && OrderStatus.매도 == bal.OrderStatus)
        {
            if ('1' == code[0] || 'A' == code[0])
            {
                return;
            }

            if (foQuote.TryGetValue(code, out SingleOpt50001? t51) && double.TryParse(t51?.FifthBidPrice, out double price) && Math.Abs(price) <= 1e-2)
            {
                axAPI.SendOrderFO(new OpenAPI.OrderFO
                {
                    AccNo = bal.AccNo,
                    Code = code,
                    RQName = code,
                    SlbyTp = "2"
                });
            }
        }
    }

    int LiquidateInPrinciple()
    {
        foreach (var con in from fc in conclusion
                            where fc.Value.Code?.Length == 0x8 && "시장가".Equals(fc.Value.TradedClassification) is false
                            select new
                            {
                                fc.Value.UntradedQuantity,
                                fc.Value.AccNo,
                                fc.Value.Code,
                                OrgOrdNo = string.IsNullOrEmpty(fc.Value.OrgOrdNo) || fc.Value.OrgOrdNo.All(e => '0'.Equals(e)) ? fc.Key : fc.Value.OrgOrdNo
                            })
        {
            axAPI.SendOrderFO(new OpenAPI.OrderFO
            {
                AccNo = con.AccNo,
                Code = con.Code,
                OrdKind = 3,
                OrgOrdNo = con.OrgOrdNo,
                Qty = con.UntradedQuantity,
                RQName = con.Code
            });
        }

        foreach (var bal in from fb in balance
                            where 0x8 == fb.Key.Length && (fb.Key[0] == '1' || fb.Key[0] == 'A')
                            select new
                            {
                                Code = fb.Key,
                                fb.Value.AccNo,
                                fb.Value.QuantityAvailableForOrder,
                                fb.Value.OrderStatus
                            })
        {
            if (conclusion.Any(e => bal.Code.Equals(e.Value.Code) &&
                                    bal.QuantityAvailableForOrder == e.Value.UntradedQuantity &&
                                    bal.OrderStatus != e.Value.OrderStatus &&
                                    "시장가".Equals(e.Value.TradedClassification)))
            {
                continue;
            }
            axAPI.SendOrderFO(new OpenAPI.OrderFO
            {
                AccNo = bal.AccNo,
                Code = bal.Code,
                OrdTp = "3",
                Price = string.Empty,
                Qty = bal.QuantityAvailableForOrder,
                RQName = bal.Code,
                SlbyTp = OrderStatus.매도 == bal.OrderStatus ? "2" : "1"
            });
        }
        return 0x259;
    }

#if DEBUG
    readonly KiwoomRealType[] realTypes =
    [
        KiwoomRealType.종목프로그램매매,
        KiwoomRealType.주식체결,
        KiwoomRealType.주식호가잔량,
        KiwoomRealType.주식우선호가,
        KiwoomRealType.주식예상체결,
        KiwoomRealType.주식당일거래원,
        KiwoomRealType.옵션시세,
        KiwoomRealType.옵션이론가,
        KiwoomRealType.옵션호가잔량,
        KiwoomRealType.선물시세,
        KiwoomRealType.선물이론가,
        KiwoomRealType.선물호가잔량,
        KiwoomRealType.선물옵션우선호가,
        KiwoomRealType.업종지수,
        KiwoomRealType.업종등락
    ];
#endif
}