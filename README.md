# Entity framework testing helpers
This is a small set of helpers for more readable testing. Provides basic in memmory dbContext mocking. 

TestCase example:
```
 public static ExampleTestCase TestCase1 = 	new ExampleTestCase()
		.DescriptionIs($"Given existing debth of 30, when paid 40 more, expect total of 10")
		.WhenGiven(-30m)
		.WhenCalledWith(40m)
		.Expect(10m)
		.Asserts((expected, actuall) =>
		{
			Assert.Equal(expected, actuall);
		});
 ```
 
 DataContextBuilder example:
 ```
[Theory(DisplayName = "ClientPaymentService.MakePayment")]
[MemberData(nameof(TestCases))]
public void UnitTest(ExampleTestCase testcase)
{
	//SETUP
	//Creates a db context with in memmory Sets for all db entities;
	var dbContext = dbContextBuilder.EfDbContext;
	//Create a service with dbContext dependancy
	var sut = new ClientPaymentService(dbContext);

	//ACT
	var actual = sut.MakePayment(testcase.TestInput);

	//ASSERT
	testcase.DoAsserts(actual);
}
		
public static IEnumerable<IEnumerable<object>> TestCases
{
	get
	{
		return new[]
		{
			TestCase1,
			TestCases2,
			TestCase3
		}.ToMemberData();
	}
}
```

Caution, in memmory dbContext does not provide relation tracking.
```
public void InMemmoryRelationTrackingFlaws()
{
   var dbContext = dbContextBuilder.BuildIDataContext();
   var paymentItem = new PaymentItem
   {
	   Amount = 1
   };
   var payment = new Payment
   {
	   PaymentItems = new List<PaymentItem>
	   {
		   paymentItem
	   }
   };

   dbContext.Payments.Add(payment);
   //does not automatically do:
   dbContext.PaymentItems.Add(paymentItem);
   paymentItem.Payment = payment;
}
```
