using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            List<SplineChartData> SplineData = new List<SplineChartData>
            {
                new SplineChartData { Days = "Yanvar", MaxTemp = 15, AvgTemp = 10, MinTemp = 2 },
                new SplineChartData { Days = "Febral", MaxTemp = 22, AvgTemp = 18, MinTemp = 12 },
                new SplineChartData { Days = "Mart", MaxTemp = 32, AvgTemp = 28, MinTemp = 22 },
                new SplineChartData { Days = "Aprel", MaxTemp = 31, AvgTemp = 28, MinTemp = 23 },
                new SplineChartData { Days = "May", MaxTemp = 29, AvgTemp = 26, MinTemp = 19 },
                new SplineChartData { Days = "Iyun", MaxTemp = 24, AvgTemp = 20, MinTemp = 13 },
                new SplineChartData { Days = "Iyul", MaxTemp = 18, AvgTemp = 15, MinTemp = 8 },
                new SplineChartData { Days = "Avgust", MaxTemp = 18, AvgTemp = 23, MinTemp = 13 },
                new SplineChartData { Days = "Sentabr", MaxTemp = 6, AvgTemp = 23, MinTemp = 3 },
                new SplineChartData { Days = "Oktabr", MaxTemp = 23, AvgTemp = 15, MinTemp = 8 },
                new SplineChartData { Days = "Noyabr", MaxTemp = 13, AvgTemp = 11, MinTemp = 17 },
                new SplineChartData { Days = "Dekabr", MaxTemp = 12, AvgTemp = 21, MinTemp = 10 },
            };
            ViewBag.SplineData = SplineData;
            return View();
        }

        public class SplineChartData
        {
            public string Days;
            public double MaxTemp;
            public double AvgTemp;
            public double MinTemp;
        }
    }
}
