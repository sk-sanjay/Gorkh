﻿using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
   public class OtherLinkDTO
    {
        public int Id { get; set; }
        public long? LoginUserId { get; set; }
        public long? AcceptedBy { get; set; }
        [Required(ErrorMessage = "English Heading required !")]
        [StringLength(250, ErrorMessage = "{1} characters max")]
        public string EnglishHeadingName { get; set; }
        public string URLHeadingEnglish { get; set; }
        public string URLHeadingHindi { get; set; }
        public string EnglishPageLink { get; set; }
        public string EnglishContentDesc { get; set; }
        public string EnglishAttachment { get; set; }
        public string Banner { get; set; }
        public string HindiBanner { get; set; }
        public int? ParentId { get; set; }
        
        public string HindiHeadingName { get; set; }
        public string HindiPageLink { get; set; }
        public string HindiContentDesc { get; set; }
        public string HindiAttachment { get; set; }
        public DateTime? ValidTill { get; set; }
        public long? ForReview { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string IsSubHeading { get; set; }
        public bool Show { get; set; }
        public IFormFile EnglishFile { get; set; }
        public IFormFile HindiFile { get; set; }
        public IFormFile EnglishImage { get; set; }
        public IFormFile HindiImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
    }
}
