﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechTalksWeb.Models;

namespace TechTalksWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string url = "http://techtalksapi:8080/api/techtalks";
            string output = url;

            List<TechTalkDTO> techTalks = new List<TechTalkDTO>();
            try
            {
                var client = new WebClient();
                var response = client.DownloadString(url);
                Console.WriteLine($"Data returned from API call : {response}");
                techTalks.AddRange(JsonConvert.DeserializeObject<List<TechTalkDTO>>(response));

                Console.WriteLine($"Number of records in collecton : {techTalks.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside exceptions block");
                Console.WriteLine(ex.Message);
            }

            var result = new List<TechTalkDTO> 
            {
                new TechTalkDTO {Id = 1, TechTalkName="Docker", CategoryId = 1},
                new TechTalkDTO {Id = 2, TechTalkName="Kubernetes", CategoryId = 2}
            };

            if(techTalks.Count() == 0)
            {
                techTalks.AddRange(result);
            }

            return View(techTalks);
        }

        public IActionResult Details(int id)
        {
            string url = "http://techtalksapi:8080/api/techtalks/" + id;
            string output = url;

            TechTalkDTO techTalk = null;
            try
            {
                var client = new WebClient();
                var response = client.DownloadString(url);
                Console.WriteLine($"Data returned from API call : {response}");
                techTalk  = JsonConvert.DeserializeObject<TechTalkDTO>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside exceptions block");
                Console.WriteLine(ex.Message);
            }

            if(techTalk != null)
            {
                return View(techTalk);
            }
            else
            {
                return NotFound();
            }            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]TechTalkDTO techTalk1)
        {
            string url = "http://techtalksapi:8080/api/techtalks/";
            string output = url;

            Console.WriteLine($"Talk name : {techTalk1.TechTalkName}");
            Console.WriteLine($"Category ID : {techTalk1.CategoryId}");

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Console.WriteLine($"Data is about to be sent to API call");
                string stringData = JsonConvert.SerializeObject(techTalk1);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8,"application/json");
                HttpResponseMessage response = client.PostAsync(url, contentData).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside exceptions block");
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            string url = "http://techtalksapi:8080/api/techtalks/" + id;
            string output = url;
            
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Console.WriteLine($"Data is about to be deleted using API call");
                HttpResponseMessage response = client.DeleteAsync(url).Result;

                if(response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully deleted Techalk with Id = {id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside exceptions block");
                Console.WriteLine(ex.Message);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult DisplayTechTalksResult(string searchText, string serviceUrl)
        {
            // string url = "http://techtalksapi:8080/api/keyvalue";
            string output = serviceUrl;

            List<TechTalkDTO> techTalks = new List<TechTalkDTO>();
            try
            {
                var client = new WebClient();
                var response = client.DownloadString(serviceUrl);
                Console.WriteLine($"Data returned from API call : {response}");
                techTalks.AddRange(JsonConvert.DeserializeObject<List<TechTalkDTO>>(response));

                Console.WriteLine($"Number of records in collecton : {techTalks.Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inside exceptions block");
                Console.WriteLine(ex.Message);
            }

            var result = new List<TechTalkDTO> 
            {
                new TechTalkDTO {Id = 1, TechTalkName="Docker", CategoryId = 1},
                new TechTalkDTO {Id = 2, TechTalkName="Kubernetes", CategoryId = 2}
            };

            if(techTalks.Count() == 0)
            {
                techTalks.AddRange(result);
            }

            return PartialView("SearchServiceResults", techTalks);

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
