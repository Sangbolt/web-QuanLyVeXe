using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

public class VNPayLibrary
{
    private Dictionary<string, string> _requestData = new Dictionary<string, string>();
    private Dictionary<string, string> _responseData = new Dictionary<string, string>();

    public void AddRequestData(string key, string value)
    {
        _requestData[key] = value;
    }

    public void AddResponseData(string key, string value)
    {
        _responseData[key] = value;
    }

    public string CreateRequestUrl(string vnpUrl, string vnpHashSecret)
    {
        // Tạo chuỗi tham số và hash cho VNPay request URL
        string queryString = string.Join("&", _requestData.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
        string secureHash = CalculateSecureHash(queryString, vnpHashSecret);
        return $"{vnpUrl}?{queryString}&vnp_SecureHash={secureHash}";
    }

    public bool ValidateSignature(string secureHash, string hashSecret)
    {
        // Kiểm tra tính hợp lệ của chữ ký
        string queryString = string.Join("&", _responseData.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
        string calculatedHash = CalculateSecureHash(queryString, hashSecret);
        return secureHash == calculatedHash;
    }

    private string CalculateSecureHash(string queryString, string secret)
    {
        string rawHash = queryString + "&" + secret;
        using (var sha256 = new System.Security.Cryptography.SHA256Managed())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}