using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTagValidator.Models;

namespace ChipTagValidator.Interfaces
{
    public interface IReportPrinter
    {
        public void WriteReport(List<CardModel> cards, string reportName);
    }
}
