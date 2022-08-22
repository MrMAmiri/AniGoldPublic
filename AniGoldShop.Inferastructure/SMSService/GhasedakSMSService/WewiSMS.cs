using Ghasedak.Core.Exceptions;
using AniGoldShop.Domain.Common.Exceptions.SMSService;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AniGoldShop.Infrastructure.SMSService.GhasedakSMSService
{
    public class WewiSMS : ISmsService
    {
        private string _userName;
        private string _password;
        private string _pattern;

        public WewiSMS(string usern, string pass, string pattern)
        {
            _userName = usern;
            _password = pass;
            _pattern = pattern;
        }

        public async Task SendSMS(string phoneNumber, string docNumber, string cutting, string name, int priot, string workId, bool call)
        {
            if (_userName == null || _password == string.Empty)
                throw new SMSServiceAPIKeyEmptyException("SMS API Key is empty");

            try
            {
                string message = "";
                switch (priot)
                {
                    case 0:
                        message = "شماره پاکت " + docNumber + " عیار " + cutting + " آزمایشگاه " + name + Environment.NewLine + "تلفن گویا" + Environment.NewLine + "03538423000" + Environment.NewLine + "https://irgoldshop.com";
                        break;
                    case 1:
                        message = docNumber + " عیار " + cutting + Environment.NewLine + name;
                        break;
                    case 2:
                        message = "شماره انگ " + docNumber + Environment.NewLine + name + " عیار " + cutting;
                        break;
                    case 3:
                        message = "پاکت " + docNumber + Environment.NewLine + "شماره انگ فوق متعلق به " + name + " به عیار " + cutting + " می باشد " + Environment.NewLine + " تلفن گویا 0353842300 " + Environment.NewLine + "https://irgoldshop.com";
                        break;
                    case 4:
                        message = docNumber + Environment.NewLine + name + " عیار " + cutting;
                        break;
                    case 5:
                        message = " عیار " + cutting + Environment.NewLine + name + Environment.NewLine + " انگ " + docNumber;
                        break;
                    case 6:
                        message = "پاکت " + docNumber + Environment.NewLine + "عیار " + cutting + Environment.NewLine + name + Environment.NewLine + "تلفن گویا " + Environment.NewLine + "0353842300";
                        break;
                    case 7:
                        message = docNumber + Environment.NewLine + "عیار " + cutting + Environment.NewLine + name + Environment.NewLine + "تلفن گویا " + Environment.NewLine + "0353842300" + Environment.NewLine + "https://irgoldshop.com";
                        break;
                }

                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Remove("Accept");
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                        var conten = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("username",_userName),
                        new KeyValuePair<string, string>("password",_password),
                        new KeyValuePair<string, string>("number",phoneNumber),
                        new KeyValuePair<string, string>("message",message),
                        new KeyValuePair<string, string>("port","gsm-1.3"),

                    });

                        var res = await client.PostAsync("http://api.wewi.ir/api/V1.0/send-sms", conten);

                        var resContetn = await res.Content.ReadAsStringAsync();

                    }

                }
                catch(Exception ex)
                {

                }

                if (call)
                {
                    using (var client = new HttpClient())
                    {

                        var conten = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("workspaceId",workId.ToString()),
                        new KeyValuePair<string, string>("packetId",docNumber),
                        new KeyValuePair<string, string>("number",phoneNumber),
                        new KeyValuePair<string, string>("ayar",cutting),
                    });

                        var res = await client.PostAsync("http://api.wewi.ir/api/V1.0/call-webservice", conten);

                        var resContetn = await res.Content.ReadAsStringAsync();

                    }
                }


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

        public Task SendSMS(string phoneNumber, string docNumber, string cutting, string name)
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
