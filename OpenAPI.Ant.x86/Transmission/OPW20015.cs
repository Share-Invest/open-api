using AxKHOpenAPILib;

using Newtonsoft.Json;

namespace ShareInvest.Transmission;

class OPW20015 : Constructor
{
    internal override IEnumerable<string> OnReceiveTrData(AxKHOpenAPI axAPI, _DKHOpenAPIEvents_OnReceiveTrDataEvent e)
    {
        Dictionary<string, string> dic = [];

        if (Id == null || Value == null)
        {
            yield break;
        }

        if (Multiple != null)
        {
            for (int i = 0; i < axAPI.GetRepeatCnt(e.sTrCode, e.sRQName); i++)
            {
                dic = new Dictionary<string, string>
                {
                    { nameof(Entities.Kiwoom.Opw20015.Classification), Value[1] }
                };

                for (int j = 0; j < Multiple.Length; j++)
                {
                    dic[Multiple[j]] = axAPI.GetCommData(e.sTrCode, e.sRQName, i, Multiple[j]).Trim();
                }
                yield return JsonConvert.SerializeObject(dic);
            }
        }

        if (Single != null)
        {
            for (int i = 0; i < Single.Length; i++)
            {
                dic[Single[i]] = axAPI.GetCommData(e.sTrCode, e.sRQName, 0, Single[i]).Trim();
            }

            if (dic.Count > 0)
            {
                yield return JsonConvert.SerializeObject(dic);
            }
        }
    }
}