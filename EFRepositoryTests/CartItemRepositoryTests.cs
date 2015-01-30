namespace EFRepositoryTests
{
  using System.Collections.Generic;
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class CartItemRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();

      _domicile = BuildMeA.Domicile("code", "description");
      _manCo1 = BuildMeA.ManCo("description1", "code1").WithDomicile(_domicile);
      _docType = BuildMeA.DocType("code", "description");
      _subDocType1 = BuildMeA.SubDocType("code 1", "description 1").WithDocType(_docType);

      _document1 = BuildMeA.Document("id").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1); 
      _document2 = BuildMeA.Document("id2").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);
      _document3 = BuildMeA.Document("id3").WithDocType(_docType).WithSubDocType(_subDocType1).WithManCo(_manCo1);

      _cartItem1 = BuildMeA.CartItem("Key1")
            .WithDocument(_document1);

      _cartItem2 = BuildMeA.CartItem("Key1")
            .WithDocument(_document2);

      _cartItem3 = BuildMeA.CartItem("Key2")
            .WithDocument(_document3);

      _cartItem4 = BuildMeA.CartItem("Key1")
            .WithDocument(_document3);

      _cartItem5 = BuildMeA.CartItem("Key1")
            .WithDocument(_document3);

      _cartItemRepository = new CartItemRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);

      _cartItemRepository.Create(_cartItem1);
      _cartItemRepository.Create(_cartItem2);
      _cartItemRepository.Create(_cartItem3);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private DocType _docType;
    private SubDocType _subDocType1;
    private Domicile _domicile;
    private ManCo _manCo1;
    private CartItemRepository _cartItemRepository;
    private TransactionScope _transactionScope;
    private CartItem _cartItem1;
    private CartItem _cartItem2;
    private CartItem _cartItem3;
    private CartItem _cartItem4;
    private CartItem _cartItem5;
    private Document _document1;
    private Document _document2;
    private Document _document3;
    private const string _cartId = "Key1";

    [Test]
    public void GivenACartItem_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _cartItemRepository.Entities.Count();
      _cartItemRepository.Create(_cartItem1);

      _cartItemRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenCartItems_WhenIAskForCartItemsForADocumentId_IGetTheCartItemsForThatDocument()
    {
      CartItem cartItem = _cartItemRepository.GetCart(_document1.DocumentId, _cartId);
      cartItem.CartId.Should().Be(_cartId);
    }

    [Test]
    public void GivenCartItems_WhenIAskForACountOfTheTotalNumberOfItemsInTheCart_IGetTheTotalNumberOfItemsInTheCart()
    {
      int cartItems = _cartItemRepository.GetNumberOfItemsInCart(_cartId);
      cartItems.Should().Be(2);
    }

    [Test]
    public void GivenCartItems_WhenIAskForCartItemsForACartId_IGetTheCartItemsForThatCart()
    {
      IEnumerable<CartItem> cartItems = _cartItemRepository.GetCart(_cartId);
      cartItems.Should().HaveCount(2);
    }

    [Test]
    public void WhenIAddCartITemsToTheDatabase_ThenICanRetrieveTheFirstPage()
    {
      _cartItemRepository.Create(_cartItem1);
      _cartItemRepository.Create(_cartItem2);
      _cartItemRepository.Create(_cartItem3);
      _cartItemRepository.Create(_cartItem4);
      _cartItemRepository.Create(_cartItem5);

      PagedResult<CartItem> retrievedCartItems = _cartItemRepository.GetCart("Key1", 1, 4);
      retrievedCartItems.ItemsPerPage.Should().Be(4);
      retrievedCartItems.Results.Should().NotContain(_cartItem3).And.HaveCount(4);
    }

    [Test]
    public void WhenIAddCartItemsToTheDatabase_ThenICanRetrieveTheSecondPage()
    {
      _cartItemRepository.Create(_cartItem1);
      _cartItemRepository.Create(_cartItem2);
      _cartItemRepository.Create(_cartItem3);
      _cartItemRepository.Create(_cartItem4);
      _cartItemRepository.Create(_cartItem5);

      PagedResult<CartItem> retrievedCartItems = _cartItemRepository.GetCart("Key1", 2, 2);
      retrievedCartItems.ItemsPerPage.Should().Be(2);
      retrievedCartItems.Results.Should().NotContain(_cartItem3).And.HaveCount(2);
    }

    [Test]
    public void WhenIDeleteACartItem_ThenItIsDeletedFromTheDatabase()
    {
      _cartItemRepository.Create(_cartItem1);
      CartItem retrievedCartItem = _cartItemRepository.GetCart(_cartItem1.Document.DocumentId, _cartItem1.CartId);
      _cartItemRepository.Delete(retrievedCartItem);
      CartItem deletedCartItem = _cartItemRepository.Entities.FirstOrDefault(c => c.Id == retrievedCartItem.Id);
      deletedCartItem.Should().BeNull();
    }
  }
}
