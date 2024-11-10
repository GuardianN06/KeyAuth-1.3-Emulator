using System.Text;
using System.Net;
using System.Text.Json;
using System.Web;
using Sodium;
using System.Runtime.InteropServices;

namespace KeyAuthEmu1._3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/api/1.3/");
            listener.Start();
            Console.WriteLine("Server started. Listening on http://localhost:8080/api/1.3/");

            while (true)
            {
                var context = await listener.GetContextAsync();
                ProcessRequest(context);
            }
        }

        static void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nRequest URL: " + request.Url);
            Console.ResetColor();

            string requestBody = "";
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                requestBody = reader.ReadToEnd();
                Console.WriteLine("Request Body: " + requestBody);
                Console.ResetColor();
            }

            string responseBody = "";

            SodiumCore.Init();

            if (request.HttpMethod == "POST")
            {
                var query = request.QueryString;
                var parameters = new Dictionary<string, string>();

                if (query != null)
                {
                    foreach (string? keys in query.AllKeys)
                    {
                        if (keys != null)
                        {
                            parameters[keys] = query[keys] ?? "";
                        }
                    }
                }
                
                var bodyParams = ParseFormData(requestBody);
                foreach (var param in bodyParams)
                {
                    parameters[param.Key] = param.Value;
                }
                var check_paid = parameters.ContainsKey("check_paid") ? parameters["check_paid"] : "";
                var type = parameters.ContainsKey("type") ? parameters["type"] : "";
                var ownerid = parameters.ContainsKey("ownerid") ? parameters["ownerid"] : "";
                var name = parameters.ContainsKey("name") ? parameters["name"] : "";
                var version = parameters.ContainsKey("ver") ? parameters["ver"] : "";
                var sessionid = parameters.ContainsKey("sessionid") ? parameters["sessionid"] : "";
                var key = parameters.ContainsKey("key") ? parameters["key"] : "";
                var hwid = parameters.ContainsKey("hwid") ? parameters["hwid"] : "";

                if (type == "init")
                {
                    var randomSessionId = Guid.NewGuid().ToString("N").Substring(0, 8);
                    var nonce = Guid.NewGuid().ToString();

                    var initResponse = new
                    {
                        success = true,
                        code = 68,
                        message = "Initialized",
                        sessionid = randomSessionId,
                        appinfo = new
                        {
                            numUsers = "N/A - Use fetchStats() function in latest example",
                            numOnlineUsers = "N/A - Use fetchStats() function in latest example",
                            numKeys = "N/A - Use fetchStats() function in latest example",
                            version = version,
                            customerPanelLink = "https://keyauth.cc/panel"
                        },
                        newSession = true,
                        nonce = nonce,
                        ownerid = ownerid
                    };

                    responseBody = JsonSerializer.Serialize(initResponse);
                }
                else if (type == "license")
                {
                    var nonce = Guid.NewGuid().ToString();

                    long expiry = 88128416723;
                    long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    long timeleft = expiry - currentUnixTime;

                    var licenseResponse = new
                    {
                        success = true,
                        code = 68,
                        message = "Logged in!",
                        info = new
                        {
                            username = "github.com/GuardianN06",
                            subscriptions = new List<object>
                            {
                                new
                                {
                                    subscription = "default",
                                    key = key,
                                    expiry = expiry.ToString(),
                                    timeleft = timeleft
                                }
                            },
                            ip = "1.1.1.1",
                            hwid = hwid,
                            createdate = "1728416723",
                            lastlogin = "1730136359"
                        },
                        nonce = nonce,
                        ownerid = ownerid
                    };

                    responseBody = JsonSerializer.Serialize(licenseResponse);
                }
                else if (type == "login")
                {
                    var nonce = Guid.NewGuid().ToString();

                    long expiry = 88128416723;
                    long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    long timeleft = expiry - currentUnixTime;

                    var licenseResponse = new
                    {
                        success = true,
                        code = 68,
                        message = "Logged in!",
                        info = new
                        {
                            username = "github.com/GuardianN06",
                            subscriptions = new List<object>
                            {
                                new
                                {
                                    subscription = "default",
                                    key = key,
                                    expiry = expiry.ToString(),
                                    timeleft = timeleft
                                }
                            },
                            ip = "1.1.1.1",
                            hwid = hwid,
                            createdate = "1728416723",
                            lastlogin = "1730136359"
                        },
                        nonce = nonce,
                        ownerid = ownerid
                    };

                    responseBody = JsonSerializer.Serialize(licenseResponse);
                }
                else if (type == "log")
                {
                    var nonce = Guid.NewGuid().ToString();

                    long expiry = 88128416723;
                    long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    long timeleft = expiry - currentUnixTime;

                    var licenseResponse = new
                    {
                        success = true,
                        message = "Log sent",
                        nonce = nonce,
                        ownerid = ownerid
                    };

                    responseBody = JsonSerializer.Serialize(licenseResponse);
                }
                else if (type == "check")
                {
                    var nonce = Guid.NewGuid().ToString();

                    if (check_paid != "")
                    {
                        var checkResponse = new
                        {
                            success = true,
                            code = 68,
                            message = "Session is validated.",
                            nonce = nonce,
                            role = "seller",
                            ownerid = ownerid
                        };
                        responseBody = JsonSerializer.Serialize(checkResponse);
                    }
                    else
                    {
                        var checkResponse = new
                        {
                            success = true,
                            code = 68,
                            message = "Session is validated.",
                            nonce = nonce,
                            role = "not_checked",
                            ownerid = ownerid
                        };

                        responseBody = JsonSerializer.Serialize(checkResponse);
                    }
                }
                else if (type == "checkblacklist")
                {
                    var nonce = Guid.NewGuid().ToString();

                    var blacklistResponse = new
                    {
                        success = false,
                        code = 0,
                        message = "Client is not blacklisted",
                        nonce = nonce,
                        ownerid = ownerid
                    };

                    responseBody = JsonSerializer.Serialize(blacklistResponse);
                }
                else
                {
                    var errorResponse = new { success = false, message = "Invalid type parameter" };
                    responseBody = JsonSerializer.Serialize(errorResponse);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                var errorResponse = new { success = false, message = "Only POST requests are allowed" };
                responseBody = JsonSerializer.Serialize(errorResponse);
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }

            long epochtime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var signature = SignMessage(responseBody, epochtime);
           
            response.Headers.Add("x-signature-ed25519", signature);
            response.Headers.Add("x-signature-timestamp", epochtime.ToString());

            var buffer = Encoding.UTF8.GetBytes(responseBody);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Spoofed response sent!");
            Console.ResetColor();
        }

        public static string SignMessage(string message, long epocht)
        {
            
            string hexPrivateKey = "36ed9c997b89ada0c8fe186cea69f9a1d2dc5dfc171bb2cfecff8344dee999082571268f1826934a28a9eaa365c0496ac1e5a08bd23c4df275adf388948fd497";
            byte[] privateKey = HexStringToByteArray(hexPrivateKey);

            message = epocht.ToString() + message;
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            byte[] signature = new byte[64];

            PublicKeyAuth.SignDetached(messageBytes, privateKey).CopyTo(signature, 0);

            string signatureHex = ByteArrayToHexString(signature);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Spoofed Signature: " + signatureHex);
            Console.ResetColor();

            return signatureHex;
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static byte[] HexStringToByteArray(string hex)
        {
                int NumberChars = hex.Length;
                byte[] bytes = new byte[NumberChars / 2];
                for (int i = 0; i < NumberChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
        }

        static Dictionary<string, string> ParseFormData(string formData)
        {
            var parameters = new Dictionary<string, string>();
            var parsedData = HttpUtility.ParseQueryString(formData);

            if (parsedData != null)
            {
                foreach (string key in parsedData)
                {
                    if (key != null)
                    {
                        parameters[key] = parsedData[key] ?? "";
                    }
                }
                return parameters;
            }
            return parameters;
        }
    }
}
