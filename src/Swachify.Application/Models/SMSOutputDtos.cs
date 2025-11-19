namespace Swachify.Application.DTOs;
public class SMSOutputDtos
{
    public string status { get; set; }
    public int statuscode { get; set; }
    public string statustext { get; set; }
    public MessageAck messageack { get; set; }
}

public class MessageAck
{
    public List<GuidItem> guids { get; set; }
}

public class GuidItem
{
    public string guid { get; set; }
    public string submitdate { get; set; }
    public string id { get; set; }
    public List<ErrorItem> errors { get; set; }
}

public class ErrorItem
{
    public int errorcode { get; set; }
    public string errortext { get; set; }
    public string seq { get; set; }
}
