using Ghasedak.Core.Exceptions;
using AniGoldShop.Domain.Common.Exceptions.SMSService;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using RestSharp;
using System;
using System.Threading.Tasks; 

namespace AniGoldShop.Infrastructure.SMSService.GhasedakSMSService
{
    public class IPPanelSMS : ISmsService
    {
        private string _userName;
        private string _password;
        private string _pattern;

        public IPPanelSMS(string usern, string pass, string pattern)
        {
            _userName = usern;
            _password = pass;
            _pattern = pattern;
         }

        public async Task SendSMS(string phoneNumber,string docNumber,string cutting,string name)
        {
            if (_userName == null || _password == string.Empty)
                throw new SMSServiceAPIKeyEmptyException("SMS API Key is empty");

            try
            {
                var client = new RestClient("http://188.0.240.110/api/select");
                var request = new RestRequest("",Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("undefined", "{\"op\" : \"patternV2\"" +
                    ",\"user\" : \""+_userName+"\"" +
                    ",\"pass\":  \""+_password+"\"" +
                    ",\"fromNum\" : \"+9850001040898990\"" +
                    ",\"toNum\": \""+phoneNumber+"\"" +
                    ",\"patternCode\": \""+_pattern+"\"" +
                    ",\"inputData\" : {\"tracking_code\": \" "+ docNumber + " \",\"purity\": \" " + cutting + " \",\"workspace_name\": \" " + name + "\"}}"
                    , ParameterType.RequestBody);
                var response =await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);
            }
            catch (ApiException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
            catch (ConnectionException ex)
            {
                throw new SMSServiceException(ex.Message);
            }
        }

        public Task SendSMS(string message, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task SendSMS(string phoneNumber, string docNumber, string cutting, string name, int priot, string workId,bool call)
        {
            throw new NotImplementedException();
        }

        public async Task SendVerifySMS(string phoneNumber, string verifyCode)
        {
            throw new Exception();
        }

        public void SetAPIKey(string apiKey)
        {
            throw new NotImplementedException();
        }

        public void SetAPIKey(string usern, string pass, string pattern)
        {
            throw new NotImplementedException();
        }

        public void SetVerifySMSTemplate(string apiKey)
        {
            throw new NotImplementedException();
        }
    }
}
