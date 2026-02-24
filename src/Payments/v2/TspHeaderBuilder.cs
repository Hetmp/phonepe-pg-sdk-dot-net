/*
* Copyright (c) 2025 Original Author(s), PhonePe India Pvt. Ltd.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

namespace pg_sdk_dotnet.Payments.v2;

/// <summary>
/// TSP (Third Service Provider) header channel types.
/// Defines the integration platform for TSP payment flows.
/// </summary>
public enum TspSourceChannel
{
    Web,
    Android,
    Ios
}

/// <summary>
/// Indicates how the merchant is integrated with PhonePe for TSP flows.
/// This value is used when constructing TSP headers to determine whether
/// the primary account in the headers represents a partner/aggregator
/// platform or a directly integrated merchant.
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Use when the merchant is integrated with PhonePe via a partner,
    /// aggregator, or platform. TSP headers will identify the partner
    /// account as the primary TSP entity, and the end-merchant is treated
    /// as a sub-merchant under the partner.
    /// </summary>
    Partner,

    /// <summary>
    /// Use when the merchant is directly integrated with PhonePe without
    /// a partner/aggregator in between. TSP headers will identify the
    /// merchant account itself as the primary TSP entity.
    /// </summary>
    DirectMerchant,
}

/// <summary>
/// Metadata for TSP (Third Service Provider) headers.
/// Contains dynamic values required for TSP-specific payment processing.
/// All fields are optional—only populate fields relevant to your integration channel.
/// </summary>
public class TspHeaderMetadata
{
    /// <summary>
    /// The integration channel (Web, Android, iOS).
    /// Mandatory for TSP flows.
    /// </summary>
    public required TspSourceChannel SourceChannel { get; set; }

    /// <summary>
    /// End merchant's Merchant ID for TSP payment.
    /// Mandatory for all channels.
    /// </summary>
    public required string MerchantId { get; set; }

    /// <summary>
    /// Channel version:
    /// - Android: OS version (e.g., "11", "13")
    /// - iOS: iOS version (e.g., "17.1.2")
    /// - Web: Optional
    /// </summary>
    public string? ChannelVersion { get; set; }

    /// <summary>
    /// Browser fingerprint for fraud detection (Web/Android WebView).
    /// Example: "8357426ac73fcd60b17355ab7de60421"
    /// Required for: Web, Android (WebView), iOS (WebView)
    /// </summary>
    public string? BrowserFingerprint { get; set; }

    /// <summary>
    /// User-Agent header from the browser/app.
    /// Example: "Mozilla/5.0 (Linux; Android 13; ...) AppleWebKit/537.36..."
    /// Required for: Web, Android (WebView), iOS (WebView)
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Merchant's website/store URL.
    /// Example: "https://www.store.com"
    /// Required for: Web
    /// </summary>
    public string? MerchantDomain { get; set; }

    /// <summary>
    /// End-user's IP address making the request.
    /// Example: "11.123.123.212"
    /// Required for: All channels
    /// </summary>
    public string? MerchantIp { get; set; }

    /// <summary>
    /// Package name (Android) or Bundle ID (iOS).
    /// Example: "com.abc.pro.www"
    /// Required for: Android, iOS
    /// </summary>
    public string? MerchantAppId { get; set; }

    /// <summary>
    /// Type of redirection in TSP flow.
    /// Allowed values: "PARTNER_REDIRECTION", "MERCHANT_REDIRECTION"
    /// Default: "MERCHANT_REDIRECTION"
    /// Required for: Web
    /// </summary>
    public TspRedirectionType RedirectionType { get; set; } = TspRedirectionType.MerchantRedirection;
}

/// <summary>
/// TSP redirection type for payment flow.
/// Determines whether user is redirected to PhonePe from partner or merchant website.
/// </summary>
public enum TspRedirectionType
{
    PartnerRedirection,
    MerchantRedirection
}

/// <summary>
/// Builds TSP-specific HTTP headers based on integration channel and metadata.
/// Ensures only required headers for the specified channel are included.
/// Reference: https://developer.phonepe.com/tsp-integration/tsp-headers/http-headers-standard
/// </summary>
public static class TspHeaderBuilder
{
    private const string X_MERCHANT_ID = "X-MERCHANT-ID";
    private const string X_SOURCE = "X-SOURCE";
    private const string X_SOURCE_CHANNEL = "X-SOURCE-CHANNEL";
    private const string X_SOURCE_CHANNEL_VERSION = "X-SOURCE-CHANNEL-VERSION";
    private const string X_BROWSER_FINGERPRINT = "X-BROWSER-FINGERPRINT";
    private const string USER_AGENT = "USER-AGENT";
    private const string X_MERCHANT_DOMAIN = "X-MERCHANT-DOMAIN";
    private const string X_MERCHANT_IP = "X-MERCHANT-IP";
    private const string X_MERCHANT_APP_ID = "X-MERCHANT-APP-ID";
    private const string X_SOURCE_REDIRECTION_TYPE = "X-SOURCE-REDIRECTION-TYPE";

    /// <summary>
    /// Builds TSP headers based on the provided metadata and integration channel.
    /// Only includes headers relevant to the specified channel.
    /// </summary>
    public static Dictionary<string, string> BuildHeaders(TspHeaderMetadata? metadata)
    {
        if (metadata == null)
            throw new ArgumentNullException(nameof(metadata));

        ValidateMetadata(metadata);

        var headers = new Dictionary<string, string>
        {
            { X_MERCHANT_ID, metadata.MerchantId },
            { X_SOURCE, "API" }
        };

        // Add channel-specific headers
        return metadata.SourceChannel switch
        {
            TspSourceChannel.Web => BuildWebHeaders(headers, metadata),
            TspSourceChannel.Android => BuildAndroidHeaders(headers, metadata),
            TspSourceChannel.Ios => BuildIosHeaders(headers, metadata),
            _ => throw new ArgumentException($"Unknown source channel: {metadata.SourceChannel}")
        };
    }

    /// <summary>
    /// Builds headers for Web channel TSP integration.
    /// Required headers: Merchant ID, Source, Source Channel, Browser Fingerprint, 
    ///                   User Agent, Merchant Domain, Merchant IP, Redirection Type
    /// </summary>
    private static Dictionary<string, string> BuildWebHeaders(
        Dictionary<string, string> baseHeaders,
        TspHeaderMetadata metadata)
    {
        if (string.IsNullOrEmpty(metadata.BrowserFingerprint))
            throw new ArgumentException("BrowserFingerprint is required for Web channel", nameof(metadata.BrowserFingerprint));
        if (string.IsNullOrEmpty(metadata.UserAgent))
            throw new ArgumentException("UserAgent is required for Web channel", nameof(metadata.UserAgent));
        if (string.IsNullOrEmpty(metadata.MerchantDomain))
            throw new ArgumentException("MerchantDomain is required for Web channel", nameof(metadata.MerchantDomain));
        if (string.IsNullOrEmpty(metadata.MerchantIp))
            throw new ArgumentException("MerchantIp is required for Web channel", nameof(metadata.MerchantIp));

        baseHeaders[X_SOURCE_CHANNEL] = "web";
        baseHeaders[X_BROWSER_FINGERPRINT] = metadata.BrowserFingerprint;
        baseHeaders[USER_AGENT] = metadata.UserAgent;
        baseHeaders[X_MERCHANT_DOMAIN] = metadata.MerchantDomain;
        baseHeaders[X_MERCHANT_IP] = metadata.MerchantIp;
        baseHeaders[X_SOURCE_REDIRECTION_TYPE] = EnumToTspValue(metadata.RedirectionType);

        return baseHeaders;
    }

    /// <summary>
    /// Builds headers for Android channel TSP integration.
    /// Required headers: Merchant ID, Source, Source Channel, Source Channel Version, 
    ///                   Merchant App ID, Merchant IP
    /// </summary>
    private static Dictionary<string, string> BuildAndroidHeaders(
        Dictionary<string, string> baseHeaders,
        TspHeaderMetadata metadata)
    {
        if (string.IsNullOrEmpty(metadata.ChannelVersion))
            throw new ArgumentException("ChannelVersion is required for Android channel", nameof(metadata.ChannelVersion));
        if (string.IsNullOrEmpty(metadata.MerchantAppId))
            throw new ArgumentException("MerchantAppId is required for Android channel", nameof(metadata.MerchantAppId));
        if (string.IsNullOrEmpty(metadata.MerchantIp))
            throw new ArgumentException("MerchantIp is required for Android channel", nameof(metadata.MerchantIp));

        baseHeaders[X_SOURCE_CHANNEL] = "android";
        baseHeaders[X_SOURCE_CHANNEL_VERSION] = metadata.ChannelVersion;
        baseHeaders[X_MERCHANT_APP_ID] = metadata.MerchantAppId;
        baseHeaders[X_MERCHANT_IP] = metadata.MerchantIp;

        return baseHeaders;
    }

    /// <summary>
    /// Builds headers for iOS channel TSP integration.
    /// Required headers: Merchant ID, Source, Source Channel, Source Channel Version, 
    ///                   Merchant App ID, Merchant IP
    /// </summary>
    private static Dictionary<string, string> BuildIosHeaders(
        Dictionary<string, string> baseHeaders,
        TspHeaderMetadata metadata)
    {
        if (string.IsNullOrEmpty(metadata.ChannelVersion))
            throw new ArgumentException("ChannelVersion is required for iOS channel", nameof(metadata.ChannelVersion));
        if (string.IsNullOrEmpty(metadata.MerchantAppId))
            throw new ArgumentException("MerchantAppId is required for iOS channel", nameof(metadata.MerchantAppId));
        if (string.IsNullOrEmpty(metadata.MerchantIp))
            throw new ArgumentException("MerchantIp is required for iOS channel", nameof(metadata.MerchantIp));

        baseHeaders[X_SOURCE_CHANNEL] = "ios";
        baseHeaders[X_SOURCE_CHANNEL_VERSION] = metadata.ChannelVersion;
        baseHeaders[X_MERCHANT_APP_ID] = metadata.MerchantAppId;
        baseHeaders[X_MERCHANT_IP] = metadata.MerchantIp;

        return baseHeaders;
    }

    /// <summary>
    /// Validates that all required fields for the specified channel are provided.
    /// </summary>
    private static void ValidateMetadata(TspHeaderMetadata metadata)
    {
        if (string.IsNullOrEmpty(metadata.MerchantId))
            throw new ArgumentException("MerchantId is required for all TSP channels", nameof(metadata.MerchantId));
    }

    /// <summary>
    /// Converts enum value to TSP header format string.
    /// </summary>
    private static string EnumToTspValue(TspRedirectionType redirectionType)
    {
        return redirectionType switch
        {
            TspRedirectionType.PartnerRedirection => "PARTNER_REDIRECTION",
            TspRedirectionType.MerchantRedirection => "MERCHANT_REDIRECTION",
            _ => throw new ArgumentException($"Unknown redirection type: {redirectionType}")
        };
    }
}