using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using QRCoder;

namespace QRCode
{
    static internal class Utils
    {
        /// <summary>
        /// Generate QRCode in PNG or PDF
        /// </summary>
        /// <param name="payload">qrcode string</param>
        /// <param name="fileType">FileType enum (PDF or PNG)</param>
        /// <returns>byte array file</returns>
        public static byte[] GenerateQRCode(string payload, FileType fileType)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                try
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H);
                    dynamic qrCode;
                    if (fileType == FileType.PDF)
                    {
                        qrCode = new PdfByteQRCode(qrCodeData);
                    }
                    else
                    {
                        qrCode = new PngByteQRCode(qrCodeData);
                    }

                    byte[] qrCodeAsByteArr = qrCode.GetGraphic(fileType == FileType.PNG ? 5 : 10);
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

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }



        /*
        /// <summary>
        /// This attribute is used to represent a string value
        /// for a value in an enum.
        /// </summary>
        public class StringValueAttribute : Attribute
        {

            #region Properties
            /// <summary>
            /// Holds the stringvalue for a value in an enum.
            /// </summary>
            public string StringValue { get; protected set; }
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor used to init a StringValue Attribute
            /// </summary>
            /// <param name="value"></param>
            public StringValueAttribute(string value)
            {
                StringValue = value;
            }
            #endregion
        }*/

        public static class EnumList
        {
            public static IEnumerable<KeyValuePair<T, string>> Of<T>()
            {
                return Enum.GetValues(typeof(T))
                    .Cast<T>()
                    .Select(p => new KeyValuePair<T, string>(
                        p,
                        (p.GetType().GetField(p.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute)?.Description ?? p.ToString()
                        ))
                        .ToList();
            }
        }

        internal enum FileType
        {
            PDF,
            [Description("PDF en un archivo")]
            PDF_UNICO,
            PNG 
        }
    }
}
