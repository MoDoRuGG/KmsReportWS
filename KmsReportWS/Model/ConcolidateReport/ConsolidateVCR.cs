﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateVCR
    {
        public string RowNum { get; set; }
        public decimal ExpertWithEducation { get; set; }
        public decimal ExpertWithoutEducation { get; set; }
        public decimal Total { get; set; }
    }
}