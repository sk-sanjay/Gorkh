﻿using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotyfService _notyf;
        public HttpClientService(
            IConfiguration config,
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            INotyfService notyf)
        {
            _config = config;
            httpClient.BaseAddress = new Uri(_config["BaseUrl"]);
            //httpClient.BaseAddress = new Uri(_config["SiteUrl"]);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _notyf = notyf;
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader)
        {
            if (addAuthHeader == false)
                return await GetResponse(path).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, int cid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, int? cid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, string cid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, int cid, int? did)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid, did).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid, did).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, string bid, string cid, int? did)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid, did).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid, did).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, int cid, string str1)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid, str1).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid, str1).ConfigureAwait(false);
            return "unauthorized";
        }
        //na
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, int bid, int cid, int did, string str1, int eid, string str2)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid, cid, did, str1, eid, str2).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid, cid, did, str1, eid, str2).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int? aid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int? aid, int? bid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, bid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, bid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, string str1)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, str1).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, str1).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, string str1, int aid)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, str1, aid).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, str1, aid).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, string str1)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, str1).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, str1).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, string str1, string str2)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, str1, str2).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, str1, str2).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, int aid, string str1, string str2, string str3)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, aid, str1, str2, str3).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, aid, str1, str2, str3).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, string str1, string str2)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, str1, str2).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, str1, str2).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, string str1, string str2, string str3)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, str1, str2, str3).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, str1, str2, str3).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> GetAsync(string path, bool addAuthHeader, string str1, string str2, string str3, string str4)
        {
            if (addAuthHeader == false)
                return await GetResponse(path, str1, str2, str3, str4).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await GetResponse(path, str1, str2, str3, str4).ConfigureAwait(false);
            return "unauthorized";
        }

        public async Task<string> PutAsync(string path, bool addAuthHeader, int id, object model)
        {
            var content = GenerateContent(model);
            if (addAuthHeader == false)
                return await PutResponse(path, id, content).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await PutResponse(path, id, content).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> PutAsync(string path, bool addAuthHeader, string id, object model)
        {
            var content = GenerateContent(model);
            if (addAuthHeader == false)
                return await PutResponse(path, id, content).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await PutResponse(path, id, content).ConfigureAwait(false);
            return "unauthorized";
        }

        public async Task<string> PostAsync(string path, bool addAuthHeader, object model)
        {
            var content = GenerateContent(model);
            if (addAuthHeader == false)
                return await PostResponse(path, content).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await PostResponse(path, content).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> PostMultipartAsync(string path, bool addAuthHeader, object model)
        {
            var content = GenerateMultipartContent(model);
            if (addAuthHeader == false)
                return await PostResponse(path, content).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await PostResponse(path, content).ConfigureAwait(false);
            return "unauthorized";
        }

        public async Task<string> DeleteAsync(string path, bool addAuthHeader, int id)
        {
            if (addAuthHeader == false)
                return await DeleteResponse(path, id).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await DeleteResponse(path, id).ConfigureAwait(false);
            return "unauthorized";
        }
        public async Task<string> DeleteAsync(string path, bool addAuthHeader, string id)
        {
            if (addAuthHeader == false)
                return await DeleteResponse(path, id).ConfigureAwait(false);
            var authHeaderAdded = await AddAuthHeader().ConfigureAwait(false);
            if (authHeaderAdded)
                return await DeleteResponse(path, id).ConfigureAwait(false);
            return "unauthorized";
        }

        //Get Response
        private async Task<string> GetResponse(string path)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid, int cid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid, int? cid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid, string cid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int? aid, int? bid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int? aid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid, int cid, int? did)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}/{did}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, string bid, string cid, int? did)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}/{did}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        //na
        private async Task<string> GetResponse(string path, int aid, int bid, int cid, int did,string str1,int eid, string str2)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}/{did}/{str1}/{eid}/{str2}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, int bid, int cid, string str1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{bid}/{cid}/{str1}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, string str1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{str1}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, string str1, int aid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{str1}/{aid}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, string str1)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{str1}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, string str1, string str2)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{str1}/{str2}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, int aid, string str1, string str2, string str3)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{aid}/{str1}/{str2}/{str3}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, string str1, string str2)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{str1}/{str2}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, string str1, string str2, string str3)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{str1}/{str2}/{str3}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> GetResponse(string path, string str1, string str2, string str3, string str4)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/{path}/{str1}/{str2}/{str3}/{str4}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }

        private async Task<string> PutResponse(string path, int id, StringContent content)
        {
            try
            {
                var response = await _httpClient.PutAsync($"/{path}/{id}", content).ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> PutResponse(string path, string id, StringContent content)
        {
            try
            {
                var response = await _httpClient.PutAsync($"/{path}/{id}", content).ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }

        private async Task<string> PostResponse(string path, StringContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync($"/{path}", content).ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> PostResponse(string path, MultipartContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync($"/{path}", content).ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }

        private async Task<string> DeleteResponse(string path, int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/{path}/{id}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }
        private async Task<string> DeleteResponse(string path, string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/{path}/{id}").ConfigureAwait(false);
                return await ResponseHandler(response).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return null;
            }
        }

        //Authentication Helpers
        private async Task<bool> AddAuthHeader()
        {
            //get RefreshToken from the cookie
            var refreshtoken = _httpContextAccessor.HttpContext.Request.Cookies[".SURP.RefreshToken"];
            //No RefreshToken -> Need to login again
            if (string.IsNullOrEmpty(refreshtoken))
            {
                //sign out of cookie authentication
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
                ClearSessionCookies();
                return false;
            }
            //Get AccessToken from the cookie
            var accesstoken = _httpContextAccessor.HttpContext.Request.Cookies[".SURP.AuthToken"];
            //AccessToken is null -> GetRefreshedToken
            if (string.IsNullOrEmpty(accesstoken))
            {
                var newTokenVmStr = await GetRefreshedToken(refreshtoken).ConfigureAwait(false);
                var newTokenVm = !string.IsNullOrEmpty(newTokenVmStr) ? JsonConvert.DeserializeObject<TokenVM>(newTokenVmStr) : null;
                if (newTokenVm == null)
                {
                    //sign out of cookie authentication
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
                    ClearSessionCookies();
                    return false;
                }

                UtilizeNewToken(newTokenVm);
                return true;
            }
            //AccessToken is not null -> Check for validity
            var TokenHandler = new JwtSecurityTokenHandler();
            var payload = TokenHandler.ReadJwtToken(accesstoken);
            if (payload.ValidTo < DateTime.UtcNow) //Token has expired
            {
                var newTokenVmStr = await GetRefreshedToken(refreshtoken).ConfigureAwait(false);
                var newTokenVm = !string.IsNullOrEmpty(newTokenVmStr) ? JsonConvert.DeserializeObject<TokenVM>(newTokenVmStr) : null;
                if (newTokenVm == null)
                {
                    //sign out of cookie authentication
                    await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
                    ClearSessionCookies();
                    return false;
                }
                UtilizeNewToken(newTokenVm);
                return true;
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
            return true;
        }
        private async Task<string> GetRefreshedToken(string refreshtoken)
        {
            var response = await _httpClient.GetAsync($"Auth/RefreshToken/{refreshtoken}").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                return null;
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private void UtilizeNewToken(TokenVM newTokenVm)
        {
            //add AuthHeader
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokenVm.AccessToken);
            //store new AccessToken into the cookie
            var cookieOptions = new CookieOptions
            {
                Domain = _config["Domain"],
                Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_config["CookieExpiry"])),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURP.AuthToken", newTokenVm.AccessToken, cookieOptions);
        }
        private void ClearSessionCookies()
        {
            //clear the session data
            _httpContextAccessor.HttpContext.Session.Clear();
            //delete the session cookie from client  (browser)
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURP.Session");
            //remove the token cookie from response
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURP.AuthToken");
            //remove the refresh token cookie from response
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURP.RefreshToken");
            //delete the token cookie from client (browser)
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                Domain = _config["Domain"]
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURP.AuthToken", "token", options);
        }

        //Content Generators
        private StringContent GenerateContent(object model)
        {
            var jsonContent = JsonConvert.SerializeObject(model, Formatting.Indented);
            var content = new StringContent(jsonContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
        private MultipartFormDataContent GenerateMultipartContent<T>(T data)
        {
            var content = new MultipartFormDataContent();
            foreach (var prop in data.GetType().GetProperties())
            {
                var value = prop.GetValue(data);
                if (value == null) continue;
                if (value is List<IFormFile> files)
                {
                    foreach (var file in files)
                    {
                        content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = file.FileName };
                    }
                }
                else if (value is IFormFile file)
                {
                    content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = file.FileName };
                }
                else
                {
                    content.Add(new StringContent(value.ToString()), prop.Name);
                }
            }
            return content;
        }

        //Response Handler
        private async Task<string> ResponseHandler(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                _notyf.Error("An internal error occured. Please try again later.");
                return null;
            }
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
                return result;
            //_notyf.Information(result);
            return null;
        }

        //Disposer
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
