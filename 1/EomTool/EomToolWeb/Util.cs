using EomTool.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EomToolWeb
{
    public class Util
    {
        public static List<CampAffId> ExtractCampAffIds(string[] idPairs)
        {
            List<CampAffId> campAffIds = new List<CampAffId>();
            foreach (var idPair in idPairs.Distinct())
            {
                var ids = idPair.Split(new char[] { ',', '_' }); // pid,affid
                if (ids.Length >= 2)
                {
                    var campAffId = new CampAffId(Convert.ToInt32(ids[0]), Convert.ToInt32(ids[1]));
                    if (ids.Length >= 3)
                        campAffId.CostCurrId = Convert.ToInt32(ids[2]);
                    campAffIds.Add(campAffId);
                }
            }
            return campAffIds;
        }
    }
}