using MainSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MainSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        //public ActionResult Index()
        //{
        //    List<string> dp = Database.Datapoints();

        //    //ViewBag.datapoints = dp;
        //    string a = "";
        //    StringBuilder sb = new StringBuilder("", 50);
        //    foreach (string i in dp)
        //    {
        //        sb.Append(i);
        //    }

        //    a = "[" + sb + "]";
        //    ViewBag.datapoints = a.ToString();
        //    return View();
        //}
        public ActionResult Index()
        {
            List<GetData> datas = Database.ShowData();
            Session["UserData"] = Database.UserData(datas);
            return View();
        }
        public ActionResult JSON()
        {
            List<GetData> l = Database.ShowData().OrderBy(x => x.Date).ToList();
            List<GetData> n = new List<GetData>();
            List<GetData> final = l.GroupBy(x => x.Date).Where(x => x.Count() == 1).SelectMany(x => x).OrderBy(x => x.Date).ToList();

            var a = l.GroupBy(x => x.Date).Where(x => x.Count() > 1).SelectMany(x => x).OrderBy(x => x.Date).ToList();

            if (a.Count() != 0)
            {
                

                int i = a.Count() - 1;
                float price1 = float.Parse(a[i].Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                while (i >= 0)
                {
                    if (i != 0)
                    {
                        if (a[i].Date == a[i - 1].Date)
                        {

                            price1 = price1 + float.Parse(a[i - 1].Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                            i -= 1;


                        }

                        else
                        {

                            a[i].Profit = "(" + Convert.ToString(price1) + ") " + "PtaNahi";
                            n.Add(a[i]);
                            price1 = float.Parse(a[i - 1].Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                            i -= 1;


                        }
                    }
                    else
                    {
                        a[i].Profit = "(" + Convert.ToString(price1) + ") " + "PtaNahi";
                        n.Add(a[i]);
                        i -= 1;

                    }

                }
                final.AddRange(n);
            }
            
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);//from 1970/1/1 00:00:00 to now

            //DateTime dtNow = DateTime.Now;

            //TimeSpan result = dtNow.Subtract(dt);

            //int seconds = Convert.ToInt32(result.TotalSeconds);
            //double a = seconds;
            
            final = final.OrderBy(x => x.Date).ToList();
            List<datapoints> dataPoints = new List<datapoints>();
            float price = 0;
            foreach (GetData g in final)
            {
                price = price + float.Parse(g.Profit.Split(' ').ToArray()[0].Trim('(', ')'));
                DateTime date1 = Convert.ToDateTime(g.Date);
                TimeSpan result1 = date1.Subtract(dt);
                double seconds1 = Convert.ToDouble(result1.TotalSeconds * 1000);
                // d.Add(new datapoints(1,1));
                dataPoints.Add(new datapoints( seconds1,price));

            }            
            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
        }


        public ActionResult GetData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetData(GetData g, HttpPostedFileBase file)
        {
            g.Comments = Database.checkComment(g.Comments);
            g.post_date = TempData["postdate"].ToString();
            if (ModelState.IsValid)
            {
                
               
                if (file != null)
                {

                    string pic = System.IO.Path.GetFileName(file.FileName);
                    g.chartImage = pic;
                    string path = System.IO.Path.Combine(
                                   Server.MapPath("~/Images/data"), pic);
                    file.SaveAs(path);
                }
                else
                {
                    string a = Database.checkImage(g.ID);
                    
                    g.chartImage = a;
                   
                }
                if (g.ID == null)
                {
                    Database.SaveData(g);
                }
                else
                {
                    Database.EditData(g);
                }
                return View("Index");
            }
            return View();
        }
        public ActionResult ShowData()
        {
            List<GetData> datas = Database.ShowData();

            Session["UserData"]= Database.UserData(datas);
            return View(datas);
        }
        public ActionResult PopUp(string id)
        {
            List<GetData> datas = Database.ShowData();
            GetData data = datas.Select(x => x).Where(x => x.ID == id).ToList()[0];

            return PartialView(data);
        }
       
        public ActionResult ViewTrade(string id1)
        {
            if (id1 == null)
            {
                id1 = "1";
            }
            List<GetData> datas = Database.ShowData();
            GetData data = datas.Select(x => x).Where(x => x.ID == id1).ToList()[0];
            Session["UserData"] = Database.UserData(datas);
            //float b = float.Parse((data.Quantity * data.StartPrice).ToString("0.00"));
            //var d = data.Profit.Split(' ').ToArray()[0].Trim('(', ')');
            //float a = 100 * float.Parse(data.Profit.Split(' ').ToArray()[0].Trim('(', ')'));
            //var t = a / b;
            return View(data);
        }
        public ActionResult ViewTradeRedirect(string id = "1")
        {
            Database.IncreaseCount(id);


            return RedirectToAction("ViewTrade",new { id1 = id});
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                id = "1";
            }
            List<GetData> datas = Database.ShowData();
            GetData data = datas.Select(x => x).Where(x => x.ID == id).ToList()[0];
            
            return View("GetData", data);
        }
        public ActionResult Delete(string id)
        {
          
            if (id == null)
            {
                return RedirectToAction("ShowData");
            }
            Database.DeleteData(id);

            return RedirectToAction("ShowData");
        }
        public void Believe(string id)
        {
            Database.Believe(id);
        }
        public void Like(string id)
        {
            Database.Like(id);
        }
        public void Dislike(string id)
        {
            Database.Dislike(id);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser U)
        {

            return View();
        }
        public ActionResult stats()
        {
            List<GetData> datas = Database.ShowData();

            UserData u = Database.UserData(datas);
            ViewData["LongTrades"] = Database.UserData(datas.Select(x => x).Where(x => x.BuySell == "Buy").ToList());
            ViewData["ShortTrades"] = Database.UserData(datas.Select(x => x).Where(x => x.BuySell == "Sell").ToList());
            return View(u);
        }
        [HttpPost]
        public ActionResult stats(string No)
        {
            List<GetData> datass = Database.ShowData().OrderByDescending(x => x.Date).ToList();
            List<GetData> datas = new List<GetData>();
            if (No == "1")
            {
                int j = 0;
                while (Convert.ToDateTime(datass[j].Date) >= DateTime.Today.AddMonths(-1))
                {
                    datas.Add(datass[j]);
                    j += 1;
                }
            }
            else if (No == "3")
            {
                int j = 0;
                while (j < datass.Count()&&Convert.ToDateTime(datass[j].Date) >= DateTime.Today.AddMonths(-3))
                {
                    datas.Add(datass[j]);
                    j += 1;
                }
            }
            else if (No == "12")
            {
                int j = 0;
                while (j < datass.Count() && Convert.ToDateTime(datass[j].Date) >= DateTime.Today.AddMonths(-12))
                {
                    datas.Add(datass[j]);
                    j += 1;
                }
            }
            else if (No == "15")
            {
                int j = 0;
                for (j = 0; j < 15; j += 1)
                {
                    datas.Add(datass[j]);

                }
            }
            else if (No == "25")
            {
                int j = 0;
                for(j = 0;j<25;j+=1)
                {
                    datas.Add(datass[j]);
                    
                }
            }
            else
            {
                datas.AddRange(datass);
            }
            UserData u = Database.UserData(datas);
            ViewData["LongTrades"] = Database.UserData(datas.Select(x => x).Where(x => x.BuySell == "Buy").ToList());
            ViewData["ShortTrades"] = Database.UserData(datas.Select(x => x).Where(x => x.BuySell == "Sell").ToList());
            return View(u);
        }
            public ActionResult ProfitChart()
        {
            List<GetData> datas = Database.ShowData();
            Session["UserData"] = Database.UserData(datas);
            return View();
        }
    }
}