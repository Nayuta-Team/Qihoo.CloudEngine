using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Qihoo.CloudEngine
{
    /// <summary>
    /// 使用奇虎360云查杀引擎检测文件健康状态
    /// </summary>
    public static class FileHealth
    {
        private const string QihooCloudEngineUrl = "http://qup.qh-lb.com/file_health_info.php";
        private static readonly HttpClientHandler Handler = new HttpClientHandler() { ServerCertificateCustomValidationCallback = (a, b, c, d) => true };

        /// <summary>
        /// 检查指定MD5文件的健康状态
        /// </summary>
        /// <param name="md5String">MD5 字符串, 不区分大小写</param>
        /// <returns>文件健康状态实体类 FileHealthResult</returns>
        public static async Task<FileHealthResult?> CheckAsync(string md5String)
        {
            try
            {
                using var client = new HttpClient(Handler);
                using var response = await client.PostAsync(QihooCloudEngineUrl, GetContent(md5String));
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var xmlInstance = content.DeserializeXml();
                    if (xmlInstance != null)
                    {
                        if (xmlInstance.Softs != null)
                        {
                            if (xmlInstance.Softs.Soft != null)
                            {
                                return new FileHealthResult(
                                    xmlInstance.Softs.Soft.Malware ?? "Unknow Type",
                                    xmlInstance.Softs.Soft.IsMalwareDetected(),
                                    xmlInstance.Softs.Soft.Upload >= 1,
                                    xmlInstance.Softs.Soft.Age,
                                    xmlInstance.Softs.Soft.Pop,
                                    xmlInstance.Softs.Soft.ELevel,
                                    xmlInstance.IsOperationSuccess()
                                    );
                            }
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static MultipartFormDataContent GetContent(string md5String)
        {
            return new MultipartFormDataContent("---------------------------")
            {
                { new StringContent(md5String.ToLower()), "\"md5s\"" },
                { new StringContent("360zip"), "\"product\"" },
                { new StringContent("360zip_main"), "\"combo\"" },
                { new StringContent("2"), "\"v\"" }
            };
        }

        private static CloudScanResult.ScanResult? DeserializeXml(this string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(CloudScanResult.ScanResult));

                using var reader = new StringReader(xml);
                return (CloudScanResult.ScanResult?)serializer.Deserialize(reader);
            }
            catch
            {
                return default;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

        public static class CloudScanResult

        {
            [XmlRoot("ret")]
            public class ScanResult
            {
                [XmlAnyElement]
                public System.Xml.XmlElement[]? UnknownElements = null;

                [XmlElement("retinfo")]
                public RetInfo? RetInfo { get; set; }

                [XmlElement("softs")]
                public Softs? Softs { get; set; }

                public bool IsOperationSuccess()
                {
                    return RetInfo != null && RetInfo.Code == 0 && RetInfo.Success == 1 && Softs != null && Softs.Soft != null;
                }
            }

            public class RetInfo
            {
                [XmlAttribute("code")]
                public int Code { get; set; }

                [XmlAttribute("msg")]
                public string? Message { get; set; }

                [XmlAttribute("total")]
                public int Total { get; set; }

                [XmlAttribute("success")]
                public int Success { get; set; }

                [XmlAttribute("empty")]
                public int Empty { get; set; }

                [XmlAttribute("cost")]
                public int Cost { get; set; }
            }

            public class Softs
            {
                [XmlElement("soft")]
                public Soft? Soft { get; set; }
            }

            public class Soft
            {
                [XmlElement("md5")]
                public string? Md5 { get; set; }

                [XmlElement("sha1")]
                public string? Sha1 { get; set; }

                [XmlElement("sha256")]
                public string? Sha256 { get; set; }

                [XmlElement("e_level")]
                public float ELevel { get; set; }

                [XmlElement("size")]
                public string? Size { get; set; }

                [XmlElement("file_desc")]
                public string? FileDesc { get; set; }

                [XmlElement("upload")]
                public int Upload { get; set; }

                [XmlElement("is_sys_file")]
                public int IsSysFile { get; set; }

                [XmlElement("is_rep")]
                public int IsRep { get; set; }

                [XmlElement("age")]
                public int Age { get; set; }

                [XmlElement("pop")]
                public int Pop { get; set; }

                [XmlElement("describ")]
                public string? Describ { get; set; }

                [XmlElement("extinfo")]
                public string? Extinfo { get; set; }

                [XmlElement("malware")]
                public string? Malware { get; set; }

                [XmlElement("class")]
                public string? Class { get; set; }

                public bool IsMalwareDetected()
                {
                    return !string.IsNullOrEmpty(Malware);
                }
            }
        }

#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}