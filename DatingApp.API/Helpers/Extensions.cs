using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response , string message){
            response.Headers.Add("Application-Error" , message);
            response.Headers.Add("Access-Control-Expose-Headers","Application Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");
        }

        public static void AddPagination(this HttpResponse response , int currentPage , int itemsperPage,int totalItems ,int totalPages){
            var PaginationHeader = new PaginationHeader(currentPage , itemsperPage , totalPages , totalItems);
            var camelcaseFormatter = new JsonSerializerSettings();
            camelcaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
            response.Headers.Add("Pagination" , JsonConvert.SerializeObject(PaginationHeader , camelcaseFormatter));
        }

        public static int ageFromDateofBirth(this DateTime dateofBirth){
            var ageyear = DateTime.Today.Year - dateofBirth.Year;
            if(dateofBirth.AddYears(ageyear)>DateTime.Today){
                ageyear--;
            }
            return ageyear ;
        }
    }
}