namespace UnityWeb.Models
{
  public class ErrorViewModel
  {
    public ErrorCode ErrorCode { get; set; }
    public ErrorSeverity Severity { get; set; }
    public string ErrorMessage { get; set; }
    public string DisplayMessage { get; set; }
  }

  public enum ErrorSeverity
  {
    Error,
    Warning,
    Information
  }

  public enum ErrorCode
  {
    Unknown,
    GetMeeting,
    GetMeetingList,
    CreateMeeting,
    ReferenceData,
    AddMeetingAlternateOrganizers,
    DeleteMeetingAlternateOrganizers,
    ChangeMeetingPrimaryOrganizer,
    AddMeetingParticipants,
    DeleteMeetingParticipants,
    DeleteMeeting
  }
}