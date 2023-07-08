using Application.Dtos;
using Application.ServiceInterfaces;
using Application.Helpers;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace WebSite.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductDetailsModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public string role => DataHelper.GetUserRole(User);
        [BindProperty] public BuyerSellerRegistrationsVM BuyerSellervm { get; set; }
        public string BuyerId => DataHelper.GetBuyerId(User);
        public string SellerId => DataHelper.GetSellerId(User);
        [FromRoute] public int? id { get; set; }
        public ProductsDetailsVM ProductsDetail { get; set; }
        public BuyerOffersDTO buyeroffersdto { get; set; }
        public List<RelatedProductsVM> RelatedProduct { get; set; } 
        [BindProperty] public ProductsBuyerIntrestsDTO ProductsBuyerIntrests { get; set; }
        [BindProperty] public BuyerOffersDTO BuyerOffers { get; set; }
        [BindProperty] public ProductsBuyerFavoritesDTO ProductsBuyerFavorites { get; set; }

        [BindProperty] public ProductsPurchasesDTO ProductsPurchase { get; set; }

        [BindProperty] public PaymentsDTO Payment { get; set; }
        public List<ProductsBySellerVM> ModelVms { get; set; }
        public List<ProductsBreadCrumbsVM> ProductsBreadCrumb { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // Get Visitor Counts
            var modelvisitors = await _httpClient.GetAsync("Products/GetProductVisitor", false, id).ConfigureAwait(false);
            ModelVms = !string.IsNullOrEmpty(modelvisitors) ? JsonConvert.DeserializeObject<List<ProductsBySellerVM>>(modelvisitors) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            
            // Get Product Details
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsByIdwebsite", false, id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            if (ProductsDetail == null)
            {
                _notyf.Error("Data not found");
                return null;
            }
            else
            {
                var model = new ProductsVisitorsDTO();
                model.ProductId = ProductsDetail.Id;
                if (role == "Buyer" && BuyerId != "0")
                {
                    model.UserId = Convert.ToInt32(BuyerId); //it means buyerid
                    model.UserType = "B"; //Buyer
                }
                else
                {
                    model.UserId = 0;
                    model.UserType = "A"; //Anonymus
                }
                model.CreatedDate = DateTime.Now;
                var result = await _httpClient.PostAsync("ProductsVisitors/Create", false, model).ConfigureAwait(false);
            }
            var RelatedPro = await _httpClient.GetAsync("Products/GetRelatedProducts", false,ProductsDetail.SubSubCatId).ConfigureAwait(false);
            if (RelatedPro == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            RelatedProduct = !string.IsNullOrEmpty(RelatedPro) ? JsonConvert.DeserializeObject<List<RelatedProductsVM>>(RelatedPro) : null;
            if (RelatedProduct == null)
            {
                _notyf.Error("Data not found");
                return null;
            }
           
            

            ////bind products purchase by buyer
            //int buyerid = Convert.ToInt32(BuyerId);
            //int productid = Convert.ToInt32(id);
            //var modelResponse2 = await _httpClient.GetAsync("ProductsPurchases/GetProductsPurchasesByBuyerandPid", true, buyerid, productid).ConfigureAwait(false);
            //if (modelResponse2 == "unauthorized")
            //{
            //    //_notyf.Information("Please login/register");
            //    //return RedirectToPage("/Account/Login");
            //    return null;
            //}
            //ProductsPurchase = !string.IsNullOrEmpty(modelResponse2) ? JsonConvert.DeserializeObject<ProductsPurchasesDTO>(modelResponse2) : null;
            //if (ProductsPurchase == null)
            //    _notyf.Error("Data not found");

            //bind products payments by buyer
            int buyerid = Convert.ToInt32(BuyerId);
            int productid = Convert.ToInt32(id);
            var modelResponse2 = await _httpClient.GetAsync("Payments/GetProductsPaymentsByBuyerandPid", true, buyerid, productid).ConfigureAwait(false);
            if (modelResponse2 == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            Payment = !string.IsNullOrEmpty(modelResponse2) ? JsonConvert.DeserializeObject<PaymentsDTO>(modelResponse2) : null;
            if (Payment == null)
                //_notyf.Error("Data not found");
                return null;

            //bind interest by buyer
            //int buyerid = Convert.ToInt32(BuyerId);
            //int productid = Convert.ToInt32(id);
            var modelResponse1 = await _httpClient.GetAsync("ProductsBuyerIntrests/GetProductsBuyerIntrestsByBuyerandPid", true, buyerid, productid).ConfigureAwait(false);
            if (modelResponse1 == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ProductsBuyerIntrests = !string.IsNullOrEmpty(modelResponse1) ? JsonConvert.DeserializeObject<ProductsBuyerIntrestsDTO>(modelResponse1) : null;
            if (ProductsBuyerIntrests == null)
                //_notyf.Error("Data not found");
                return null;

            //Get Favourite Products
           
            var modelfav = await _httpClient.GetAsync("ProductsBuyerFavorites/GetFavoutiteProductsbybuyerid", false, buyerid).ConfigureAwait(false);
            if (modelfav == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            //ProductsBuyerFavorites = !string.IsNullOrEmpty(modelfav) ? JsonConvert.DeserializeObject<ProductsBuyerFavoritesDTO>(modelfav) : null;
            //if (ProductsBuyerFavorites == null)
            //    //_notyf.Error("Data not found");
            //    return null;


            return Page();
        }
        public async Task<PartialViewResult> OnGetProductAsync(int id)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            //ProductsDetail.BuyerOffersInsert = new BuyerOffersInsertDTO();
            return Partial("_BuyerOfferPartial", ProductsDetail);
        }

        public async Task<IActionResult> OnPostInterest()
        {
            var model = new ProductsBuyerIntrestsDTO();
            model.ProductId = id.Value;
            if (role == "Buyer" && BuyerId != "0")
            {
                model.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                model.CreatedDate = DateTime.Now;
                var result = await _httpClient.PostAsync("ProductsBuyerIntrests/Create", true, model).ConfigureAwait(false);
                ProductsBuyerIntrests = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ProductsBuyerIntrestsDTO>(result) : null;

                if (ProductsBuyerIntrests == null)
                    _notyf.Warning("You have already shown interest.");
                else
                {
                    //MailMessage mail = new MailMessage();
                    //SmtpClient smtp = new SmtpClient();
                    //smtp.UseDefaultCredentials = false;

                    //mail.To.Add(VisitorRegistrationsdto.Email);

                    //mail.From = new MailAddress("helpdesk@surplusplatform.com");
                    //mail.Subject = "Account Credentials";
                    //mail.Body = "Visitor Registration";
                    //string strBody = "<html><body><div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Subject- Successful registration as Visitor</b><br>Dear Sir,<br/>Congratulations! You have successfully signed up.<br/>Surplus Platform always provides quality services to Seller/Buyer for surplus materials & assets.<br/>The dedicated modules are offered for the equitable redistribution, sale and disposal process.<br/><b>Counter Sale:</b> Retail functionality for selling surplus materials to qualified buyers.<br/><b>Online Auction:</b> Functionality for selling surplus materials through onsite auction platform.<br/><b>Web Surplus:</b> Enables eligible organizations to view contents of the surplus warehouse and submit waitlist requests online.<br/><br/>The system generated log in credentials of your registration are shared below:<br/>Username : " + VisitorRegistrationsdto.Email + "<br/>Password : " + VisitorRegistrationsdto.Password + "<br/><br/><b>Please note:</b><br/>a) The username is (not changeable) and the password (changeable)<br/>b) This is one-time registration.<br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                    //mail.Body = strBody;
                    //mail.IsBodyHtml = true;
                    //smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                    //smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                    //smtp.Port = 587;
                    ////Or your Smtp Email ID and Password
                    //smtp.EnableSsl = false;

                    //smtp.Send(mail);
                    _notyf.Success("Your request has been received. Please check your email for further details");

                }
                    
            }
            else
                _notyf.Error("Please Login as a buyer to show interest!");

            return RedirectToPage("ProductDetails", new { id = id.Value });
        }


        public async Task<IActionResult> OnPostPurchase()
        {
            var model = new ProductsPurchasesDTO();
            model.ProductId = id.Value;
            if (role == "Buyer" && BuyerId != "0")
            {
                model.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                model.CreatedDate = DateTime.Now;
                var result = await _httpClient.PostAsync("ProductsPurchases/Create", false, model).ConfigureAwait(false);
                ProductsPurchase = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ProductsPurchasesDTO>(result) : null;

                if (ProductsPurchase == null)
                    _notyf.Warning("You have already paid reserve price.");
                else
                    _notyf.Success("Reserve price paid successfully.");
            }
            else
                _notyf.Error("Please Login as a buyer to pay reserve price.");

            return RedirectToPage("ProductDetails", new { id = id.Value });
        }

        public async Task<IActionResult> OnPostDetailPrint()
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            return new Rotativa.AspNetCore.ViewAsPdf("_ProductDetailsPartial", ProductsDetail) { FileName = ProductsDetail.SubSubCategoriesName + ".pdf", CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8" };


        }
        public async Task<JsonResult> OnPostProductPrint()
        {

            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            return new JsonResult("_ProductDetailsPartial");


        }
        public async Task<PartialViewResult> OnGetProductsBreadCrumbs(int catId, int subcatId)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GetProductsBreadCrumbs", false, catId, subcatId).ConfigureAwait(false);
            ProductsBreadCrumb = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBreadCrumbsVM>>(modelResponse) : null;
            return Partial("_SearchProductBreadCrumbsPartial", ProductsBreadCrumb);
        }

        public async Task<IActionResult> OnPostBuyerOffer(int id , decimal price)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
           if(price ==0 || price > ProductsDetail.EstimatePrice)
            {
                _notyf.Warning("Invalid Offer Price.");
                return RedirectToPage("ProductDetails", new { id = id });
            }
            if (role == "Buyer" && BuyerId != "0")
            {
                var model = new BuyerOffersDTO();
                model.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                model.CreatedDate = DateTime.Now;
                model.EstimatePrice = ProductsDetail.EstimatePrice;
                model.OfferdPrice = price;
     
                //model.OfferdPrice = ProductsDetail.BuyerOffersInsert.OfferdPrice;
                model.ProductNumber = ProductsDetail.ProductNumber;
                model.Status = true;
                var result = await _httpClient.PostAsync("BuyerOffers/Create", true, model).ConfigureAwait(false);
                BuyerOffers = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerOffersDTO>(result) : null;

                var result1 = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyBuyerId", true, BuyerId).ConfigureAwait(false);
                if (result1 == "unauthorized") return RedirectToPage("/Account/Seller-Login");
                BuyerSellervm = !string.IsNullOrEmpty(result1) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(result1) : null;
                if (BuyerOffers == null)
                {
                    _notyf.Warning("You have Already Set Your Offer.");
                }

                else {
                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    mail.To.Add("support@surplusplatform.com");
                    //mail.To.Add("dharmkmr90@gmail.com");

                    mail.Bcc.Add("surplusplatforms@gmail.com");
                    mail.From = new MailAddress("helpdesk@surplusplatform.com");
                    mail.Subject = "Buyer Offer";
                    mail.Body = "Buyer Offer";
                    string strBody = "<html><body>";
                    strBody = strBody + "<p>Dear Admin,<br/>Greetings of the day!<br/> New Offer from Buyer has been received successfully !</p>";
                    strBody = strBody + "<p>Product Code : " + ProductsDetail.ProductNumber + "</p>";
                    strBody = strBody + "<p>Reserved Price : " + ProductsDetail.EstimatePrice + "</p>";
                    strBody = strBody + "<p>Offered Price : " + model.OfferdPrice + "</p>";
                    strBody = strBody + "<p>Buyer Name : " + BuyerSellervm.FirstName + " " + BuyerSellervm.LastName + " </p>";
                    strBody = strBody + "<p>Mobile/Phone No. :" + BuyerSellervm.Mobile + "</p>";
                    strBody = strBody + "<p>Email : " + BuyerSellervm.Email + "</p>";
                    mail.Body = strBody;
                    mail.IsBodyHtml = true;
                    smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                    smtp.Port = 587;
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = false;

                    smtp.Send(mail);
                    _notyf.Success("Your Offer Saved Successfully.");
                }
                    
               
            }
            else
                _notyf.Error("Please Login as a buyer to Set Offer!");

            return RedirectToPage("ProductDetails", new { id = id });
        }

        public async Task<IActionResult> OnPostFavorites(int id)
        {
            var model = new ProductsBuyerFavoritesDTO();
            model.ProductId = id;
            if (role == "Buyer" && BuyerId != "0")
            {
                model.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                model.CreatedDate = DateTime.Now;
                var result = await _httpClient.PostAsync("ProductsBuyerFavorites/Create", true, model).ConfigureAwait(false);
                ProductsBuyerFavorites = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ProductsBuyerFavoritesDTO>(result) : null;

                if (ProductsBuyerFavorites == null)
                    _notyf.Warning("You have already mark this product as Favorites.");
               // return new JsonResult("You have already mark this product as Favorites.");
                else
                     _notyf.Success("You have  mark this product as Favorites");
                    //return new JsonResult("You have  mark this product as Favorites");
            }
            else
                 _notyf.Error("Please Login as a buyer to mark this product as Favorites.!");
            //return new JsonResult("Please Login as a buyer to mark this product as Favorites.!");
            return RedirectToPage("ProductDetails", new { id = id });

        }


    }
    }

