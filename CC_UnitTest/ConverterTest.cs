using CC_UnitTest.TestModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CC_UnitTest
{
    public class ConverterTest : BaseServices
    {
        [SetUp]
        public void Setup()
        {
             _token = File.ReadAllText("token.txt");
        }

        [Test]
        [Order(2)]
        public void Convert()
        {
            using (var client = new HttpClient())
            {
                var request = new CurrencyConvert
                {
                    Base = "EUR",
                    Amount = 5
                };
                client.BaseAddress = new Uri(_baseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var httpReq = client.PostAsync(converterBase, data).Result;

                var resultContent = httpReq.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ConverterResonse>(resultContent);

                if (result != null)
                {
                    if (result.StatusCode == 100 && result.StatusCode == 100)
                    {
                        Assert.Pass(result.StatusCode.ToString());
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

            Assert.Fail();
        }

        [Test]
        [Order(3)]
        public void ConvertWithLimit()
        {
            using (var client = new HttpClient())
            {
                var request = new CurrencyConvertLimitSymbol
                {
                    Base = "EUR",
                    Amount = 5,
                    Symbols =
                    [
                        "USD",
                        "INR"
                    ]
                };

                client.BaseAddress = new Uri(_baseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var httpReq = client.PostAsync(converterWithLimit, data).Result;

                var resultContent = httpReq.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ConverterResonse>(resultContent);

                if (result != null)
                {
                    if (result.StatusCode == 100 && result.StatusCode == 100)
                    {
                        Assert.Pass(result.StatusCode.ToString());
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

            Assert.Fail();
        }

        [Test]
        [Order(4)]
        public void ConvertWithLimit_BadRequest()
        {
            using (var client = new HttpClient())
            {
                var request = new CurrencyConvertLimitSymbol
                {
                    Base = "EUR",
                    Amount = 5,
                    Symbols =
                    [
                        "THB",
                        "INR"
                    ]
                };

                client.BaseAddress = new Uri(_baseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var httpReq = client.PostAsync(converterWithLimit, data).Result;

                var resultContent = httpReq.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ConverterResonse>(resultContent);

                if (result != null)
                {
                    if (result.StatusCode == 104)
                    {
                        Assert.Pass(result.Message);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

            Assert.Fail();
        }
    }
}