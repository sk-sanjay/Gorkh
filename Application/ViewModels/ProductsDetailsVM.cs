using Application.Dtos;
using System;
using System.Collections.Generic;

namespace Application.ViewModels
{
    public class ProductsDetailsVM
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public int SubCatId { get; set; }
        public int SubSubCatId { get; set; }
        public string ProductNumber { get; set; }
        public string SaleType { get; set; }
        public string SellerType { get; set; }
        public int QuantityAvl { get; set; }
        public string YearofProc { get; set; }
        public string InventoryId { get; set; }
        public string SerialNo { get; set; }
        public decimal EstimatePrice { get; set; }
        public decimal ReservePrice { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string Name { get; set; }
        public string SubCategoryName { get; set; }
        public string SubSubCategoriesName { get; set; }
        public string ConditionName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Descriptions { get; set; }
        public decimal OfferdPrice { get; set; }

        public int SellerId { get; set; }
        //List of images and youtube videos
        public BuyerOffersInsertDTO BuyerOffersInsert {get;set;}
        public List<ProductsFileUploadsVM> ProductsFileUpload { get; set; }
        public List<ProductsSpecificationsGetsVM> ProductsSpecificationsGet { get; set; }
    }
}
