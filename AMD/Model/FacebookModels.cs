using System.Text.Json.Serialization;

namespace AMD.Model
{
    public class FacebookWebhookEvent
    {
        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("entry")]
        public List<Entry> Entry { get; set; }
    }

    public class Entry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("messaging")]
        public List<Messaging> Messaging { get; set; }
    }

    public class Messaging
    {
        [JsonPropertyName("sender")]
        public Sender Sender { get; set; }

        [JsonPropertyName("recipient")]
        public Recipient Recipient { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("postback")]
        public Postback Postback { get; set; }
    }

    public class Sender
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Recipient
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("mid")]
        public string Mid { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("quick_reply")]
        public QuickReplyPayload QuickReply { get; set; }

        [JsonPropertyName("attachments")]
        public List<Attachment> Attachments { get; set; }
    }

    public class QuickReplyPayload
    {
        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }

    public class Attachment
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("payload")]
        public AttachmentPayload Payload { get; set; }
    }

    public class AttachmentPayload
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Postback
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }

        [JsonPropertyName("referral")]
        public Referral Referral { get; set; }
    }

    public class Referral
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    // For sending buttons
    // For sending buttons
    public class Button
    {
        public string Type { get; set; } = "postback";
        public string Title { get; set; }
        public string Payload { get; set; }
        public string Url { get; set; }  // ✅ ITO ANG KULANG - para sa web_url buttons
    }

    // For quick replies
    public class QuickReply
    {
        public string ContentType { get; set; } = "text";
        public string Title { get; set; }
        public string Payload { get; set; }
        public string ImageUrl { get; set; }
    }
}
