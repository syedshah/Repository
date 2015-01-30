namespace UnityWeb.Models.Shared
{
  public class DocumentWarningsViewModel
  {
    public string BasketContainsDocumentFromAnotherManCo { get; set; }

    public string DocumentsAlreadyCheckedOut { get; set; }

    public string DocumentsAlreadyApproved { get; set; }

    public string DocumentsCheckedOutByOtherUser { get; set; }

    public string DocumentAddToBasketErrors { get; set; }

    public string DocumentsAlreadyRejected { get; set; }

    public int DocumentsRejected { get; set; }

    public int DocumentsApproved { get; set; }
  }
}