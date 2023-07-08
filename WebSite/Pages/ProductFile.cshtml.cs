using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "Seller")]
    public class ProductFileModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public ProductFileModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? ProductId { get; set; }
        [BindProperty] public ProductsFileUploadsDTO ProductsFileUpload { get; set; }
        public bool IsNew => ProductsFileUpload == null;
        public List<ProductsFileUploadsVM> ModelVms { get; set; }
        public List<ProductsFileUploadsVM> ModelVms1 { get; set; }
        public ProductsLocationsVM ProductsLocationsVM { get; set; }
        public ProductsSpecificationsVM ProductsSpecificationsVM { get; set; }
        public ProductsDescriptionsVM ProductsDescriptionsVM { get; set; }
        public async Task<IActionResult> OnGet()
        {
            //get video
            //ProductId = HttpContext.Session.GetInt32("ProductId");
            //productid = 100;
            TempData["Pid"] = (int)ProductId;
            if (!ProductId.HasValue) return Page();
            var modelResponse = await _httpClient.GetAsync("ProductsFileUploads/GetProductsFileUploadsByProductId", false, (int)ProductId, 0).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsVM>>(modelResponse) : null;

            //get image
            var modelResponse1 = await _httpClient.GetAsync("ProductsFileUploads/GetProductsFileUploadsByProductId", false, (int)ProductId, 1).ConfigureAwait(false);
            if (modelResponse1 == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms1 = !string.IsNullOrEmpty(modelResponse1) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsVM>>(modelResponse1) : null;

            // Get Locations
            var productlocation = await _httpClient.GetAsync("ProductsLocations/GetByProductId", false, ProductId);
            //if (getsubcategory == "unauthorized") return null;
            ProductsLocationsVM = !string.IsNullOrEmpty(productlocation) ? JsonConvert.DeserializeObject<ProductsLocationsVM>(productlocation) : null;

            // Get Descrition
            var productdescrition = await _httpClient.GetAsync("ProductsDescriptions/GetByProductId", false, ProductId);
            //if (getsubcategory == "unauthorized") return null;
            ProductsDescriptionsVM = !string.IsNullOrEmpty(productdescrition) ? JsonConvert.DeserializeObject<ProductsDescriptionsVM>(productdescrition) : null;

            

            return Page();



        }
        //public async Task<IActionResult> OnPost(List<IFormFile> UserImage)
        //{
        //    var MultipleFilesUploadDto = new MultipleFilesUploadDTO
        //    {
        //        ChangeName = true,
        //        ReturnValue = "name",
        //        //UploadedFile = Request.Form.Files.GetFile("UserImage"),
        //        UploadedFiles = UserImage,
        //        FilePath = "\\img\\products",
        //        //FileOldName = fileoldname,
        //        ChangeDimensions = true,
        //        Width = 128,
        //        Height = 128
        //    };
        //    var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFiles", false, MultipleFilesUploadDto).ConfigureAwait(false);

        //    var objResponse1 =  JsonConvert.DeserializeObject<List<string>>(FileUploadResult);
        //    var ProductsFileUploadsDTOs = new List<ProductsFileUploadsDTO>();
        //    foreach (var a in objResponse1)
        //    {
        //        var ProductsFileDto = new ProductsFileUploadsDTO
        //        {
        //            //ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId")),
        //            ProductId = 25,
        //            ProductImage =a
        //        };

        //        //ProductsDto = ModelAuditor<ProductsDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), ProductsDto);
        //        ProductsFileUploadsDTOs.Add(ProductsFileDto);
        //    }
        //    var response = string.Empty;
        //    response = await _httpClient.PostAsync("ProductsFileUploads/CreateRange", false, ProductsFileUploadsDTOs).ConfigureAwait(false);

        //    if (response == "unauthorized") return new JsonResult("unauthorized");
        //    ProductsFileUploadsDTOs = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsDTO>>(response) : null;
        //    if (ProductsFileUploadsDTOs == null)
        //        TempData["Message3"] = "Save Failed";
        //    else
        //        TempData["Message3"] = "Saved Succesfully";
        //    return RedirectToPage("ProductFile");
        //}

        public async Task<IActionResult> OnPost(List<IFormFile> UserImage)
        {
            var data = Request.Form;
            List<IFormFile> listValues = new List<IFormFile>();
            foreach (var b in data.Files)
            {
                listValues.Add(b);

            }
            var MultipleFilesUploadDto = new MultipleFilesUploadDTO
            {
                ChangeName = true,
                ReturnValue = "name",
                //UploadedFile = Request.Form.Files.GetFile("UserImage"),
                //UploadedFiles = UserImage,
                UploadedFiles = listValues,
                FilePath = "\\img\\products",
                //FileOldName = fileoldname,
                ChangeDimensions = true,
                Width = 500,
                Height = 500
            };
            //var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFiles", false, MultipleFilesUploadDto).ConfigureAwait(false);
            var FileUploadResult = await _fileService.SaveImageAsync(@"\img\products", listValues);

            //var objResponse1 = JsonConvert.DeserializeObject<List<string>>(FileUploadResult);
            var ProductsFileUploadsDTOs = new List<ProductsFileUploadsDTO>();
           // var prdid = TempData["Pid"];
            foreach (var a in FileUploadResult)
            {
                var ProductsFileDto = new ProductsFileUploadsDTO
                {
                    // ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId")),
                    ProductId = Convert.ToInt32(TempData["Pid"].ToString()),
                    //ProductId = 100,
                    ProductImage = a,
                    IsImageDefault = true,
                    FlagImage = 1

                };

                //ProductsDto = ModelAuditor<ProductsDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), ProductsDto);
                ProductsFileUploadsDTOs.Add(ProductsFileDto);
            }
            var response = string.Empty;
            response = await _httpClient.PostAsync("ProductsFileUploads/CreateRange", false, ProductsFileUploadsDTOs).ConfigureAwait(false);

            if (response == "unauthorized") return new JsonResult("unauthorized");
            ProductsFileUploadsDTOs = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsDTO>>(response) : null;
            if (ProductsFileUploadsDTOs == null)
                _notyf.Error("Save failed");
            else
                _notyf.Success("Saved successfully");
            //     return RedirectToPage("ProductFile");
            return RedirectToPage("ProductFile", new { ProductId });
        }

        public async Task<IActionResult> OnPostVideos(string productData)
        {
            var model = JsonConvert.DeserializeObject<List<ProductsFileUploadsDTO>>(productData);
            var ProductsFileUploadsDTOs = new List<ProductsFileUploadsDTO>();
            foreach (var a in model)
            {
                var ProductsFileDto = new ProductsFileUploadsDTO
                {
                    //ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId")),
                    ProductId = Convert.ToInt32(TempData["Pid"].ToString()),
                    //ProductId = 26,
                    ProductImage = a.ProductImage,
                    IsImageDefault = false, //when video then IsImageDefault is always false
                    FlagImage = 0 ////when video then FlagImage is always 0

                };

                //ProductsDto = ModelAuditor<ProductsDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), ProductsDto);
                ProductsFileUploadsDTOs.Add(ProductsFileDto);
            }
            var response = string.Empty;
            response = await _httpClient.PostAsync("ProductsFileUploads/CreateRange", false, ProductsFileUploadsDTOs).ConfigureAwait(false);
            if (response == "unauthorized") return new JsonResult("unauthorized");
            ProductsFileUploadsDTOs = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsDTO>>(response) : null;
            if (ProductsFileUploadsDTOs == null)
                return new JsonResult("Save failed");
            else
                return new JsonResult("Saved successfully");
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("ProductsFileUploads/Delete", false, id).ConfigureAwait(false);
            if (DeleteResult == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                return new JsonResult("unauthorized");
            }
            var RowsChanged = !string.IsNullOrEmpty(DeleteResult) && Convert.ToInt32(DeleteResult) > 0;
            if (RowsChanged)
            {
                //_notyf.Success("Deleted successfully");
                return new JsonResult("success");
            }
            //_notyf.Error("Delete failed. There might be active child records.");
            return new JsonResult("fail");
        }
    }
}
