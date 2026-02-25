using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateZpzTable10FilialCollector
    {
        public List<ConsolidateZpzTable10Filial> Collect(string yymm)
        {
            List<ConsolidateZpzTable10Filial> result = new List<ConsolidateZpzTable10Filial>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_Zpz2025_Table10_SvodFilial");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateZpzTable10Filial
                        {
                            RegionName = row["RegionName"].ToString(),
                            _1 = Convert.ToDecimal(row["_1"]),
                            _11 = Convert.ToDecimal(row["_11"]),
                            _12 = Convert.ToDecimal(row["_12"]),
                            _13 = Convert.ToDecimal(row["_13"]),
                            _14 = Convert.ToDecimal(row["_14"]),
                            _2 = Convert.ToDecimal(row["_2"]),
                            _21 = Convert.ToDecimal(row["_21"]),
                            _22 = Convert.ToDecimal(row["_22"]),
                            _23 = Convert.ToDecimal(row["_23"]),
                            _24 = Convert.ToDecimal(row["_24"]),
                            _3 = Convert.ToDecimal(row["_3"]),
                            _31 = Convert.ToDecimal(row["_31"]),
                            _32 = Convert.ToDecimal(row["_32"]),
                            _33 = Convert.ToDecimal(row["_33"]),
                            _34 = Convert.ToDecimal(row["_34"]),
                            _4 = Convert.ToDecimal(row["_4"]),
                            _41 = Convert.ToDecimal(row["_41"]),
                            _411 = Convert.ToDecimal(row["_411"]),
                            _412 = Convert.ToDecimal(row["_412"]),
                            _413 = Convert.ToDecimal(row["_413"]),
                            _414 = Convert.ToDecimal(row["_414"]),
                            _42 = Convert.ToDecimal(row["_42"]),
                            _421 = Convert.ToDecimal(row["_421"]),
                            _422 = Convert.ToDecimal(row["_422"]),
                            _423 = Convert.ToDecimal(row["_423"]),
                            _424 = Convert.ToDecimal(row["_424"]),
                            _43 = Convert.ToDecimal(row["_43"]),
                            _431 = Convert.ToDecimal(row["_431"]),
                            _432 = Convert.ToDecimal(row["_432"]),
                            _433 = Convert.ToDecimal(row["_433"]),
                            _434 = Convert.ToDecimal(row["_434"]),
                            _44 = Convert.ToDecimal(row["_44"]),
                            _441 = Convert.ToDecimal(row["_441"]),
                            _442 = Convert.ToDecimal(row["_442"]),
                            _443 = Convert.ToDecimal(row["_443"]),
                            _444 = Convert.ToDecimal(row["_444"]),
                            _45 = Convert.ToDecimal(row["_45"]),
                            _451 = Convert.ToDecimal(row["_451"]),
                            _452 = Convert.ToDecimal(row["_452"]),
                            _453 = Convert.ToDecimal(row["_453"]),
                            _454 = Convert.ToDecimal(row["_454"]),
                            _46 = Convert.ToDecimal(row["_46"]),
                            _461 = Convert.ToDecimal(row["_461"]),
                            _462 = Convert.ToDecimal(row["_462"]),
                            _463 = Convert.ToDecimal(row["_463"]),
                            _464 = Convert.ToDecimal(row["_464"]),
                            _5 = Convert.ToDecimal(row["_5"]),
                            _51 = Convert.ToDecimal(row["_51"]),
                            _511 = Convert.ToDecimal(row["_511"]),
                            _512 = Convert.ToDecimal(row["_512"]),
                            _513 = Convert.ToDecimal(row["_513"]),
                            _514 = Convert.ToDecimal(row["_514"]),
                            _52 = Convert.ToDecimal(row["_52"]),
                            _521 = Convert.ToDecimal(row["_521"]),
                            _522 = Convert.ToDecimal(row["_522"]),
                            _523 = Convert.ToDecimal(row["_523"]),
                            _524 = Convert.ToDecimal(row["_524"]),
                            _53 = Convert.ToDecimal(row["_53"]),
                            _531 = Convert.ToDecimal(row["_531"]),
                            _532 = Convert.ToDecimal(row["_532"]),
                            _533 = Convert.ToDecimal(row["_533"]),
                            _534 = Convert.ToDecimal(row["_534"]),
                            _54 = Convert.ToDecimal(row["_54"]),
                            _541 = Convert.ToDecimal(row["_541"]),
                            _542 = Convert.ToDecimal(row["_542"]),
                            _543 = Convert.ToDecimal(row["_543"]),
                            _544 = Convert.ToDecimal(row["_544"]),
                            _55 = Convert.ToDecimal(row["_55"]),
                            _551 = Convert.ToDecimal(row["_551"]),
                            _552 = Convert.ToDecimal(row["_552"]),
                            _553 = Convert.ToDecimal(row["_553"]),
                            _554 = Convert.ToDecimal(row["_554"]),
                            _56 = Convert.ToDecimal(row["_56"]),
                            _561 = Convert.ToDecimal(row["_561"]),
                            _562 = Convert.ToDecimal(row["_562"]),
                            _563 = Convert.ToDecimal(row["_563"]),
                            _564 = Convert.ToDecimal(row["_564"]),
                            _6 = Convert.ToDecimal(row["_6"]),
                            _61 = Convert.ToDecimal(row["_61"]),
                            _62 = Convert.ToDecimal(row["_62"]),
                            _63 = Convert.ToDecimal(row["_63"]),
                            _64 = Convert.ToDecimal(row["_64"]),
                            _65 = Convert.ToDecimal(row["_65"]),
                            _66 = Convert.ToDecimal(row["_66"]),
                            _67 = Convert.ToDecimal(row["_67"]),
                            _7 = Convert.ToDecimal(row["_7"]),
                            _71 = Convert.ToDecimal(row["_71"]),
                            _72 = Convert.ToDecimal(row["_72"]),
                            _73 = Convert.ToDecimal(row["_73"]),
                            _74 = Convert.ToDecimal(row["_74"]),
                            _75 = Convert.ToDecimal(row["_75"]),
                            _76 = Convert.ToDecimal(row["_76"]),
                            _77 = Convert.ToDecimal(row["_77"]),
                            _78 = Convert.ToDecimal(row["_78"]),
                            _8 = Convert.ToDecimal(row["_8"]),
                            _81 = Convert.ToDecimal(row["_81"]),
                            _82 = Convert.ToDecimal(row["_82"]),
                            _83 = Convert.ToDecimal(row["_83"]),
                            _84 = Convert.ToDecimal(row["_84"]),
                            _85 = Convert.ToDecimal(row["_85"]),
                            _86 = Convert.ToDecimal(row["_86"])
                        });
                    }
                }
            }
            return result;
        }
    }
}