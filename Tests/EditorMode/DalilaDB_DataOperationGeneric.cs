using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using U.DalilaDB;
using UnityEngine;
using UnityEngine.TestTools;

public class Dalila_DataOperationGeneric
{

    [Test]
    public void A_Successful_CanBeSetToSuccessOnlyOnce()
    {

        // When an altern operation is created
        var operation = new DataOperation<int>();
        var operationWithMessage = new DataOperation<int>();

        // You can set to Success
        operation.Successful(3);
        operationWithMessage.Successful(4, "A little error found");
        
        // And will be true and exception null and be set
        Assert.IsTrue(operation.IsDone); 
        Assert.IsTrue(operation); 
        Assert.IsTrue(operation.Data == 3); 
        Assert.Null(operation.Error); 
        Assert.IsTrue(operation.Message == ""); 

        Assert.IsTrue(operationWithMessage.IsDone); 
        Assert.IsTrue(operationWithMessage); 
        Assert.Null(operationWithMessage.Error); 
        Assert.IsTrue(operationWithMessage.Data == 4); 
        Assert.IsTrue(operationWithMessage.Message == "A little error found"); 


        // You cant set again the operation
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Successful(5));
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Fails(10, new System.Exception()));

        Assert.Throws<DataOparationAlreadySetException>(() => operationWithMessage.Successful(5));
        Assert.Throws<DataOparationAlreadySetException>(() => operationWithMessage.Fails(4, new System.Exception()));

    }


    [Test]
    public void B_Fails_CanBeSetToFailedOnlyOnce()
    {

        // When an altern operation is created
        var operation = new DataOperation<int>();

        // You can set to failed
        operation.Fails(4, new System.Exception());
        
        // And will be true and exception null and be set
        Assert.IsTrue(operation.IsDone); 
        Assert.IsFalse(operation); 
        Assert.IsTrue(operation.Data == 4); 
        Assert.NotNull(operation.Error); 
        Assert.IsTrue(operation.Message == "");
        

        // You cant set again the operation
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Successful(2));
        Assert.Throws<DataOparationAlreadySetException>(() => operation.Fails(8, new System.Exception()));
        
    }


    [Test]
    public void C_ImplicitOperatorBool()
    {

        // When an altern operation is created
        var successDataOperation = new DataOperation<int[]>();
        var successDataOperationNullData = new DataOperation<int[]>();
        var failedDataOperation = new DataOperation<int[]>();
        var uncompletedDataOperation = new DataOperation<int[]>();

        // You can set to failed or success
        successDataOperation.Successful(new int[2]);
        successDataOperationNullData.Successful(null);
        failedDataOperation.Fails(null, new System.Exception("Expected exception"));

        // And will be true and exception null and be set
        Assert.IsTrue(successDataOperation);
        Assert.IsFalse(successDataOperationNullData);
        Assert.IsFalse(failedDataOperation);
        Assert.IsFalse(uncompletedDataOperation);

    }


    [Test]
    public void D_ImplicitOperatorString()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation<int>();
        var successDataOperationWithMessage = new DataOperation<int>();
        var failedDataOperation = new DataOperation<int>();
        var uncompletedDataOperation = new DataOperation<int>();

        // You can set to failed
        successDataOperation.Successful(2);
        successDataOperationWithMessage.Successful(3, "A little error found");
        failedDataOperation.Fails(0, new System.Exception("Expected exception"));

        LogAssert.Expect(LogType.Assert, "The operation is: Completed successfully: ");
        LogAssert.Expect(LogType.Assert, "The operation is: Completed successfully: A little error found");
        LogAssert.Expect(LogType.Assert, "The operation is: Failed: System.Exception: Expected exception");
        LogAssert.Expect(LogType.Assert, "The operation is: In progress");

        Debug.LogAssertion("The operation is: " + successDataOperation);
        Debug.LogAssertion("The operation is: " + successDataOperationWithMessage);
        Debug.LogAssertion("The operation is: " + failedDataOperation);
        Debug.LogAssertion("The operation is: " + uncompletedDataOperation);

    }


    [Test]
    public void E_ImplicitOperatorInt()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation<bool>();
        var successDataOperationTwo = new DataOperation<bool>();
        var successDataOperationThree = new DataOperation<bool>();
        var successDataOperationFour = new DataOperation<bool>();
        var successDataOperationFive = new DataOperation<bool>();
        var successDataOperationSix = new DataOperation<bool>();
        var failedDataOperation = new DataOperation<bool>();
        var uncompletedDataOperation = new DataOperation<bool>();

        // You can set to failed
        successDataOperation.Successful(true, "0");
        successDataOperationTwo.Successful(false, "12");
        successDataOperationThree.Successful(true, "-22");
        successDataOperationFour.Successful(false);
        successDataOperationFive.Successful(true, "--12");
        successDataOperationSix.Successful(false, "Other text");
        failedDataOperation.Fails(true, new System.Exception("Expected exception"));


        Assert.AreEqual(0, (int)successDataOperation);
        Assert.AreEqual(12, (int)successDataOperationTwo);
        Assert.AreEqual(-22, (int)successDataOperationThree);
        Assert.AreEqual(0, (int)successDataOperationFour);
        Assert.AreEqual(0, (int)successDataOperationFive);
        Assert.AreEqual(0, (int)successDataOperationSix);
        Assert.AreEqual(0, (int)failedDataOperation);
        Assert.AreEqual(0, (int)uncompletedDataOperation);

        Assert.IsTrue(successDataOperation == 0);
        Assert.IsTrue(successDataOperationTwo == 12);
        Assert.IsTrue(successDataOperationThree == -22);

    }


    [Test]
    public void F_ImplicitOperatorFloat()
    {
        // When an altern operation is created
        var successDataOperation = new DataOperation<int>();
        var successDataOperationTwo = new DataOperation<int>();
        var successDataOperationThree = new DataOperation<int>();
        var successDataOperationFour = new DataOperation<int>();
        var successDataOperationFive = new DataOperation<int>();
        var successDataOperationSix = new DataOperation<int>();
        var failedDataOperation = new DataOperation<int>();
        var uncompletedDataOperation = new DataOperation<int>();

        // You can set to failed
        successDataOperation.Successful(3, "0");
        successDataOperationTwo.Successful(3, "12.22");
        successDataOperationThree.Successful(3, "-22.11");
        successDataOperationFour.Successful(3);
        successDataOperationFive.Successful(3, "--12.22");
        successDataOperationSix.Successful(3, "Other text");
        failedDataOperation.Fails(0, new System.Exception("Expected exception"));

        Assert.AreEqual(0f, (float)successDataOperation);
        Assert.AreEqual(12.22f, (float)successDataOperationTwo);
        Assert.AreEqual(-22.11f, (float)successDataOperationThree);
        Assert.AreEqual(0f, (float)successDataOperationFour);
        Assert.AreEqual(0f, (float)successDataOperationFive);
        Assert.AreEqual(0f, (float)successDataOperationSix);
        Assert.AreEqual(0f, (float)failedDataOperation);
        Assert.AreEqual(0f, (float)uncompletedDataOperation);

        Assert.IsTrue(successDataOperation == 0f);
        Assert.IsTrue(successDataOperationTwo == 12.22f);
        Assert.IsTrue(successDataOperationThree == -22.11f);

    }

}

