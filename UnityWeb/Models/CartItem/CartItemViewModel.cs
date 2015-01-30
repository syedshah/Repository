namespace UnityWeb.Models.CartItem
{
  using System;

  public class CartItemViewModel
  {
    public CartItemViewModel(Entities.CartItem cartItem)
    {
      Id = cartItem.Id;
      CartId = cartItem.CartId;
      DocumentId = cartItem.Document.DocumentId;
      DocType = cartItem.Document.DocType.Description;
      SubDocType = cartItem.Document.SubDocType.Description;
      ManCo = cartItem.Document.ManCo.Description;
      ManCoDisplay = string.Format("{0} - {1}", cartItem.Document.ManCo.Code, cartItem.Document.ManCo.Description);

      if (cartItem.Document.Approval != null)
      {
        ApprovalStatus = "Approved";
        ApprovedBy = cartItem.Document.Approval.ApprovedBy;
        ApprovedDate = cartItem.Document.Approval.ApprovedDate;
      }
      else if (cartItem.Document.Rejection != null)
      {
        ApprovalStatus = "Rejected";
        RejectedBy = cartItem.Document.Rejection.RejectedBy;
        RejectedDate = cartItem.Document.Rejection.RejectionDate;
      }
      else
      {
        ApprovalStatus = "Unapproved";
      }
    }

    public int Id { get; set; }

    public string CartId { get; set; }

    public string DocumentId { get; set; }

    public string DocType { get; set; }

    public string SubDocType { get; set; }

    public string ManCo { get; set; }

    public string ManCoDisplay { get; set; }

    public bool Selected { get; set; }

    public string ApprovalStatus { get; set; }

    public string ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string RejectedBy { get; set; }

    public DateTime? RejectedDate { get; set; }
  }
}