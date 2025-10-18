using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Shared;

public static class EmailValidator
{
    // RFC 5322 simplified regex, safe for most emails
    private static readonly Regex EmailRegex = new Regex(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static async Task<EmailValidationResult> ValidateAsync(string email)
    {
        var result = new EmailValidationResult();

        if (string.IsNullOrWhiteSpace(email)) 
        {
            result.Message = "Email is empty";
            return result;
        }

        email = email.Trim();

        if (email.Length > 254)
        {
            result.Message = "Email is too long";
            return result;
        }

        var parts = email.Split('@');
        if (parts.Length != 2)
        {
            result.Message = "Email must contain exactly one '@' symbol";
            return result;
        }

        var localPart = parts[0];
        var domainPart = parts[1];

        if (localPart.Length > 64)
        {
            result.Message = "Local part of email is too long";
            return result;
        }

        if (!EmailRegex.IsMatch(email))
        {
            result.Message = "Email format is invalid";
            return result;
        }

        result.IsValidFormat = true;

        // Check domain existence via MX records
        try
        {
            var lookup = new LookupClient();
            var dnsResult = await lookup.QueryAsync(domainPart, QueryType.MX);
            result.DomainExists = dnsResult.Answers.MxRecords().Any();

            if (!result.DomainExists)
                result.Message = "Domain does not exist or has no MX records";
            else
                result.Message = "Email format is valid and domain exists";
        }
        catch (Exception ex)
        {
            result.DomainExists = false;
            result.Message = $"Error checking domain: {ex.Message}";
        }

        return result;
    }


    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}


public class EmailValidationResult
{
    public bool IsValidFormat { get; set; }
    public bool DomainExists { get; set; }
    public string Message { get; set; } = string.Empty;
}