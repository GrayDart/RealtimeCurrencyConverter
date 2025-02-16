using CC_UnitTest.TestModel;
using Newtonsoft.Json;
using System.Text;

namespace CC_UnitTest
{
    public class LoginTest : BaseServices
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Order(1)]
        public void Login()
        {
            using (var client = new HttpClient())
            {
                var loginReq = new Login
                {
                    Email = "admin@graydart.com",
                    Password = "Qwerty123!"
                };
                client.BaseAddress = new Uri(_baseUrl);

                var json = JsonConvert.SerializeObject(loginReq);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var httpReq = client.PostAsync(api, data).Result;

                var resultContent = httpReq.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<LoginResult>(resultContent);

                if (result != null)
                {
                    if (result.StatusCode == 100 && !string.IsNullOrEmpty(result.Data.Token))
                    {
                        File.WriteAllText("token.txt", result.Data.Token ?? "");

                        Assert.Pass(result.Data.Token);
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