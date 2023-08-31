using System;
using System.Configuration;
using QRCoder;

namespace QRCode
{
    static internal class Utils
    {
        /// <summary>
        /// Generate QRCode in PNG or PDF
        /// </summary>
        /// <param name="payload">qrcode string</param>
        /// <param name="fileType">png or pdf</param>
        /// <returns>byte array file</returns>
        public static byte[] GenerateQRCode(string payload, string fileType)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                try
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H);
                    dynamic qrCode;
                    if (fileType == "pdf")
                    {
                        qrCode = new PdfByteQRCode(qrCodeData);
                    }
                    else
                    {
                        qrCode = new PngByteQRCode(qrCodeData);
                    }

                    byte[] qrCodeAsByteArr = qrCode.GetGraphic(fileType == "pdf" ? 10 : 5);
                    qrCodeData.Dispose();
                    qrCode.Dispose();
                    return qrCodeAsByteArr;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get app settings
        /// </summary>
        /// <param name="key">Key to get setting</param>
        /// <returns>string if exists or null</returns>
        public static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Set appsetting
        /// </summary>
        /// <param name="key">key to set setting</param>
        /// <param name="value">string value</param>
        public static void SetSetting(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var keyValue = GetSetting(key);
            if (keyValue == null)
            {
                configuration.AppSettings.Settings.Add(key, value);
            }
            else
            {
                configuration.AppSettings.Settings[key].Value = value;
            }
            configuration.Save(ConfigurationSaveMode.Minimal, false);
            ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
        }
    }
}
