﻿namespace UnityWeb.Models.Approval
{
  public class ApproveDocumentViewModel
  {
    public string DocumentId { get; set; }

    public string ManCo { get; set; }

    public string DocType { get; set; }

    public string SubDocType { get; set; }

    public bool Selected { get; set; }
  }
}